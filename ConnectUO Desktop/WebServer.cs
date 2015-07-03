using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace CUODesktop
{
	public class WebServer
	{
		
		public const string STATUS_CODE_OK = " 200 OK";
		public const string STATUS_CODE_NOT_FOUND = " 404 Not Found";
		public const string STATUS_CODE_CONTINUE = " 100 Continue";
		public const string STATUS_CODE_SWITCHING_PROTICOLS = " 101 Switching Protocols";
		public const string STATUS_CODE_NON_AUTHORITATIVE_INFO = " 203 Non-Authoritative Information";
		public const string STATUS_CODE_NO_CONENT = " 204 No Content";
		public const string STATUS_CODE_RESET_CONENT = " 205 Reset Content";
		public const string STATUS_CODE_TEMPORARY_REDIRECT = " 307 Temporary Redirect";
		public const string STATUS_CODE_BAD_REQUEST = " 400 Bad Request";
		public const string STATUS_CODE_UNAUTHORIZED = " 401 Unauthorized";
		public const string STATUS_CODE_FORBIDDEN = " 403 Forbidden";

		private byte[] _buffer;
		private TcpListener _listener;
		private string _lastPageRequest;
		private Dictionary<string, string> _mimeTypes;

		/// <summary>
		/// The last requested url
		/// </summary>
		public string LastPageRequest 
		{ 
			get 
			{ 
				if( _lastPageRequest == string.Empty ) 
					_lastPageRequest = "http://localhost.:1980/home.html?page=1"; 
				
				return _lastPageRequest; 
			} 
		}

		public WebServer()
		{
			_mimeTypes = LoadMimeTypes();

			try
			{
				_listener = new TcpListener(IPAddress.Loopback, 1980);
				_listener.Start(5000);

				Thread th = new Thread(new ThreadStart(StartListen));
				th.Start();
			}
			catch (Exception e)
			{
				Trace.HandleException(e);
			}
		}

		private Dictionary<string, string> LoadMimeTypes()
		{
			//Create the dictionary for the mimetypes
			Dictionary<string, string> mimeTypes = new Dictionary<string, string>();

			//Check if our mimetypes.dat file exists
			if (File.Exists(Path.Combine(Core.BaseDirectory, "mimetypes.dat")))
			{
				//Create a reader for the mimetype data
				StreamReader reader = File.OpenText(Path.Combine(Core.BaseDirectory, "mimetypes.dat"));

				//While the reader has not reached the end of the file.
				while (!reader.EndOfStream)
				{
					//Read a line of data
					string line = reader.ReadLine();
					//Split it into 2 strings
					string[] pair = line.Split('|');

					//If it isnt 2 strings we dont want to use it
					if (pair.Length != 2)
						continue;

					//If the mimeType dictionary doesnt contain the key for we add it.
					if( !mimeTypes.ContainsKey(pair[0]) )
						mimeTypes.Add(pair[0], pair[1]);
				}

				//Close the reader
				reader.Close();
			}

			//Return the mimetypes
			return mimeTypes;
		}

		public string GetMimeType(string fileName)
		{
			if (_mimeTypes.ContainsKey(Path.GetExtension(fileName).Trim()))
				return _mimeTypes[Path.GetExtension(fileName)];
			else
				return "";
		}

		/// <summary>
		/// Closes the listener socket
		/// </summary>
		public void Close()
		{
			if( _listener != null )
				_listener.Stop();
		}

		/// <summary>
		/// Sends the specified message to the browser
		/// </summary>
		/// <param name="message"></param>
		/// <param name="socket"></param>
		public void SendToBrowser(string message, ref Socket socket)
		{
			SendToBrowser(Encoding.ASCII.GetBytes(message), ref socket);
		}

		/// <summary>
		/// Sends the specified message to the browser
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="socket"></param>
		public void SendToBrowser(Byte[] buffer, ref Socket socket)
		{
			int resp = 0;

			try
			{
				if( socket.Connected )
				{
					if( ( resp = socket.Send(buffer, buffer.Length, 0) ) == -1 )
						socket.Close();					
				}
			}
			catch
			{
				socket.Close();	
			}
		}

		public void SendHeader(string httpVersion, string mimeHeader, int length, string statusCode, ref Socket socket)
		{
			string header = "";

			// if Mime type is not provided set default to text/html
			if (mimeHeader.Length == 0)
				mimeHeader = "text/html";  // Default Mime Type is text/html

			//Build the header to send to the browser
			header += httpVersion + statusCode + "\r\n";
			header += "Server: ConnectUO Desktop-b\r\n";
			header += "Content-Type: " + mimeHeader + "\r\n";
			header += "Accept-Ranges: bytes\r\n";
			header += "Content-Length: " + length + "\r\n\r\n";

			//Send the header to the browser
			SendToBrowser(header, ref socket);
		}

		private void StartListen()
		{
			int startPos = 0;
			string request;

			while( true )
			{
				Socket socket = null;

				try
				{
					socket = _listener.AcceptSocket();

					if (!socket.RemoteEndPoint.ToString().Contains("127.0.0.1"))
						socket.Close();

					if (socket.Connected)
					{
						if (_buffer == null)
							_buffer = new byte[1024];

						int i = socket.Receive(_buffer, _buffer.Length, 0);

						string httpRequest = Encoding.ASCII.GetString(_buffer);
						startPos = httpRequest.IndexOf("HTTP", 1);
						string ver = httpRequest.Substring(startPos, 8);
						request = httpRequest.Substring(5, startPos - 5);
						request = request.Trim();

						if (request.Length <= 1)
							request = "home.html?page=1";
						
						if (IsTemplateFileRequest(request))
						{	
							string[] split = request.Split('?');
							string ext = Path.GetExtension(split[0]);

							string getData = split.Length > 1 ? split[1].Trim() : "";
                            getData = Uri.UnescapeDataString(getData);						
                             
                            ext.Trim();

							if (ext.Contains(".html"))
							{
								string output = PageCompiler.CompileCode(Path.Combine(Templates.CurrentTemplate.RootDirectory, split[0]), getData);

								SendHeader(ver, GetMimeType(split[0]), output.Length, STATUS_CODE_OK, ref socket);
								SendToBrowser(output, ref socket);
							}
							else
							{
								FileStream fs = new FileStream(Path.Combine(Templates.CurrentTemplate.RootDirectory, split[0]), FileMode.Open);
								byte[] send = new byte[fs.Length];

								fs.Read(send, 0, send.Length);
								fs.Close();

								SendHeader(ver, GetMimeType(split[0]), send.Length, STATUS_CODE_OK, ref socket);
								SendToBrowser(send, ref socket);
							}
						}
						else
						{
							HandleRequest(request, ref socket);
						}

						_lastPageRequest = "http://localhost.:1980/" + request;
						socket.Close();
					}
				}
				catch
				{
					if (socket != null && socket.Connected)
						socket.Close();
				}
			}
		}

        private bool IsTemplateFileRequest(string request)
        {
            return Templates.CurrentTemplate.ContainsFile(request);
        }

		private void HandleRequest(string request, ref Socket socket)
		{
			string orig = request;
			string[] strings = request.Split(new char[] { '?' }, 2);

			if( strings.Length > 1 )
				request = strings[1].Trim();
			else
				request = "";

			strings[0] = strings[0].Trim();

			PageHandler handler;

			if( PageHandlers.Handlers.TryGetValue( strings[0], out handler ) )
			{				
				if( handler.Validate( request ) )
					handler.OnRequest( request, handler.Compiler, ref socket );
			}
		}
	}

	public class Authentication
	{
		private static uint _authCode;

		internal static uint AuthCode
		{
			get
			{
				return _authCode;
			}
		}

		/// <summary>
		/// Updates the current Authentication number used to authenticate requests.
		/// </summary>
		public static void UpdateAuth()
		{
			_authCode = (uint)DateTime.Now.GetHashCode();
		}

		internal static bool ValidateAuth(uint auth)
		{
			return _authCode == auth;
		}
	}
}