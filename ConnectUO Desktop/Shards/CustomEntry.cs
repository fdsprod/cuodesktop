using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CUODesktop
{
	public class CustomEntry : IEntry
	{
		private string _name;
		private string _description;
		private string _patchURL;
		private string _updateURL;
		private string _host;
		private string _port;
		private bool _patchEnc;

		#region IEntry Members
		/// <summary>
		/// Gets a boolean based on the servers up status
		/// </summary>
		public bool Status
		{
			get { return true; }
		}
		/// <summary>
		/// Gets the XmlElement of the server
		/// </summary>
		public XmlElement Element
		{
			get { return null; }
		}
		/// <summary>
		/// Gets the id of the server
		/// </summary>
		public string Id
		{
			get { return _name; }
		}
		/// <summary>
		/// Gets the name of the server
		/// </summary>
		public string Name
		{		
			get { return _name; }
			set { _name = value; }
		}
		/// <summary>
		/// Gets the desciption of the server
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
		/// <summary>
		/// Gets the url of the server
		/// </summary>
		public string Url
		{
			get { return ""; }
		}
		/// <summary>
		/// Gets the update url of the server
		/// </summary>
		public string UpdateUrl
		{
			get { return _updateURL; }
			set { _updateURL = value; }
		}
		/// <summary>
		/// Gets the patch url of the server
		/// </summary>
		public string PatchUrl
		{
			get { return _patchURL; }
			set { _patchURL = value; }
		}
		/// <summary>
		/// Gets the server era
		/// </summary>
		public string Era
		{
			get { return "--"; ; }
		}
		/// <summary>
		/// Gets the server type
		/// </summary>
		public string Type
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets the server language
		/// </summary>
		public string Lang
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets a boolean based on whether the encryption for the client is to be removed
		/// </summary>
		public bool RemoveEncryption
		{
			get { return _patchEnc; }
			set { _patchEnc = value; }
		}
		/// <summary>
		/// Gets a boolean based on whether razor is allowed on the server
		/// </summary>
		public bool AllowRazor
		{
			get { return true; }
		}
		/// <summary>
		/// Gets the host address/ip for the server
		/// </summary>
		public string HostAddress
		{
			get { return _host; }
			set { _host = value; }
		}
		/// <summary>
		/// Gets the port for the server
		/// </summary>
		public int Port
		{
			get
			{
				int port;

				if( int.TryParse(_port, out port) )
					return port;

				return 0;
			}
			set { _port = value.ToString(); }
		}
		/// <summary>
		/// Gets the peak amount of players ever online
		/// </summary>
		public string MaxOnline
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets the current amount of player online
		/// </summary>
		public string CurOnline
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets the overall average number of players online
		/// </summary>
		public string AvgOnline
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets the number of votes for the server 
		/// </summary>
		public string Votes
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets the number of total votes the server has recieved
		/// </summary>
		public string TotalVotes
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets the number of rejected votes for the server
		/// </summary>
		public string Out
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets the number of total rejected votes the server has recieved
		/// </summary>
		public string TotalOut
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets the server's rank within the list.
		/// </summary>
		public string Rank
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets a link to the server's banner
		/// </summary>
		public string Banner
		{
			get { return "--"; }
		}
		/// <summary>
		/// Gets a link to display moreinfo about the server
		/// </summary>
		public string MoreInfoLink
		{
			get { return "http://localhost.:1980/addlocal?id=" + Id + "&mode=edit"; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string ResetPatchesLink
		{
			get { return "http://localhost.:1980/resetpatches?id=" + Id; }
		}
		/// <summary>
		/// Gets a link to add the server to the favorites list.
		/// </summary>
		public string AddToFavoritesLink
		{
			get { return "http://localhost.:1980/removefavorite?id=" + Id; }
		}
		/// <summary>
		/// Gets a link to play the server.
		/// </summary>
		public string PlayLink
		{
			get { return "http://localhost.:1980/play?id=" + Id + "&type=custom&auth=" + Authentication.AuthCode.ToString(); }
		}
		/// <summary>
		/// Gets a link to the servers website.
		/// </summary>
		public string WebsiteLink
		{
			get { return "http://www.connectuo.com"; }
		}
		/// <summary>
		/// Gets a link to vote for the server
		/// </summary>
		public string VoteLink
		{
			get { return ""; }
		}
		/// <summary>
		/// Gets an address to show a online player count for one day
		/// </summary>
		public string OneDayGraph
		{
			get { return ""; }
		}
		/// <summary>
		/// Gets an address to show a online player count for seven days
		/// </summary>
		public string SevenDayGraph
		{
			get { return ""; }
		}
		/// <summary>
		/// Gets an address to show a online player count for fourteen days
		/// </summary>
		public string FourteenDayGraph
		{
			get { return ""; }
		}
		/// <summary>
		/// Gets an address to show a online player count for thirty days
		/// </summary>
		public string ThirtyDayGraph
		{
			get { return ""; }
		}
		/// <summary>
		/// Gets an address to show a online player count for 180 days
		/// </summary>
		public string OneEightDayGraph
		{
			get { return ""; }
		}
		/// <summary>
		/// Gets an address to show a online player count for one year
		/// </summary>
		public string YearDayGraph
		{
			get { return ""; }
		}
		#endregion

		public CustomEntry()
		{}

		public CustomEntry(string name, string desc, string patchUrl, string updateUrl, 
			string host, string port, bool patchEnc)
		{
			_name = name;
			_description = desc;
			_patchURL = patchUrl;
			_updateURL = updateUrl;
			_host = host;
			_port = port;
			_patchEnc = patchEnc;
		}

		internal void Serialize(BinaryWriter writer)
		{
			writer.Write((int)0);

			writer.Write((string)_name);
			writer.Write((string)_description);
			writer.Write((string)_patchURL);
			writer.Write((string)_updateURL);
			writer.Write((string)_host);
			writer.Write((string)_port);
			writer.Write((bool)_patchEnc);
		}

		internal void Deserialize(BinaryReader reader)
		{
			int ver = reader.ReadInt32();

			switch( ver )
			{
				case 0:
				{
					_name = reader.ReadString();
					_description = reader.ReadString();
					_patchURL = reader.ReadString();
					_updateURL = reader.ReadString();
					_host = reader.ReadString();
					_port = reader.ReadString();
					_patchEnc = reader.ReadBoolean();
					break;
				}
			}
		}		
	}
}
