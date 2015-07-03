using System;
using System.Collections.Generic;
using System.Net;

namespace CUODesktop
{
	public class ProxyInfo
	{
		private string _server;
		private int _port;
		private string _username;
		private string _password;

		public string Server { get { return _server; } set { _server = value; } }
		public int Port { get { return _port; } set { _port = value; } }
		public string Username { get { return _username; } set { _username = value; } }
		public string Password { get { return _password; } set { _password = value; } }

		public ProxyInfo(string server, int port) : 
			this( server, port, "", "" ) { }

		public ProxyInfo(string username, string password) : 
			this("", 0, username, password) 
		{ }

		public ProxyInfo(string server, int port, string username, string password) 
		{
			Server = server;
			Port = port;
			Username = username;
			Password = password;
		}
	}
}
