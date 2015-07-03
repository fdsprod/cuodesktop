using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Microsoft.Win32;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;

namespace CUODesktop
{
	/// <summary>
	/// Enum for sorting the server list.
	/// </summary>
	public enum SortType
	{
		Id,
		MaxOnline,
		AvgOnline,
		CurOnline,
		Votes,
		TotalVotes,
		Rank,
		Name,
		Description,
	}	

	/// <summary>
	/// Provides methods for sorting, handling, updating, etc of the public servers list.
	/// </summary>
	public class ServerList
	{
		private static XmlDocument _serverList;
		private static SortType _sortType;
		private static List<ServerEntry> _publicServerList;


		/// <summary>
		/// Gets the serverlist, Sets the server list to the specified value.
		/// </summary>
		public static List<ServerEntry> PublicServerList
		{
			get
			{				
				return _publicServerList;
			}
			set
			{
				_publicServerList = value;
			}
		}

		/// <summary>
		/// Gets a XmlDocument version of the serverlist.
		/// </summary>
		public static XmlDocument XmlList
		{
			get
			{
				if (_serverList == null)
					LoadServerList();

				return _serverList;
			}
		}

		/// <summary>
		/// Loads the serverlist
		/// </summary>
		public static void LoadServerList()
		{
			string path = Path.Combine(Core.BaseDirectory, "list.xml");

			if (File.Exists(path))
			{
                string xmlList = File.ReadAllText(path);
                File.WriteAllText(path, xmlList.Replace("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", Path.Combine(Core.DataDirectory, "xhtml1-transitional.dtd")));

				_serverList = new XmlDocument();

				try
				{
					_serverList.Load(path);
				}
				catch
				{
					if (MessageBox.Show("An error occured while trying to load the serverlist.\nWould you like to load a backup list?",
						"ConnectUO Desktop",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        File.WriteAllText(path, Properties.Resources.DefaultList.Replace("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", Path.Combine(Core.DataDirectory, "xhtml1-transitional.dtd")));
                        _serverList.Load(path);
					}
					else
						_serverList = new XmlDocument();
				}
			}
			else
			{
				if (MessageBox.Show("A serverlist was not found.\nWould you like to load a backup list?",
						"ConnectUO Desktop",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question) == DialogResult.Yes)
				{
                    File.WriteAllText(path, Properties.Resources.DefaultList.Replace("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", Path.Combine(Core.DataDirectory, "xhtml1-transitional.dtd")));
                    _serverList.Load(path);
				}
				else
					_serverList = new XmlDocument();				
			}

			_publicServerList = GetSortedPublicServers();
		}

		/// <summary>
		/// Creates the serverlist from the XmlDocument
		/// </summary>
		/// <returns>Public Serverlist</returns>
		private static List<ServerEntry> CreateServerList()
		{
			List<ServerEntry> list = new List<ServerEntry>();
			XmlElement[] elems = GetElementArray();

			for( int i = 0; i < elems.Length; i++ )
				list.Add(new ServerEntry(elems[i]));

			return list;
		}

		internal static XmlElement[] GetElementArray()
		{
			if (_serverList == null)
				LoadServerList();

			XmlElement root = _serverList["Shards"];

			int total = root.ChildNodes.Count;
			XmlElement[] elements = new XmlElement[total];

			for( int i = 0; i < total; i++ )
				elements[i] = (XmlElement)root.ChildNodes[i];

			return elements;
		}

		/// <summary>
		/// Retrieves an array of server entries.
		/// </summary>
		/// <param name="page">page number</param>
		/// <returns>Array of server entries</returns>
		public static ServerEntry[] GetServersByPage(int page)
		{
			return GetServersByPage(page, AppSettings.Current.Get<int>("ServersPerPage"));
		}

		/// <summary>
		/// Retrieves an array of server entries.
		/// </summary>
		/// <param name="page">page number</param>
		/// <param name="maxPerPage">maximum number displayed per page</param>
		/// <returns>Array of server entries</returns>
		public static ServerEntry[] GetServersByPage(int page, int maxPerPage)
		{
			int maxPages = _publicServerList.Count / maxPerPage;

			if (maxPages > page)
				return null;

			if (page > 0)
				page--;

			int index = page * maxPerPage;
			int max = Math.Min(_publicServerList.Count - index, maxPerPage);

			ServerEntry[] entries = new ServerEntry[max];
			_publicServerList.CopyTo(index, entries, 0, entries.Length);

			return entries;
		}

		/// <summary>
		/// Retrieves a IEntry based on the id provided
		/// </summary>
		/// <param name="id">id of the server</param>
		/// <returns>IEntry or null if the id is not recognized</returns>
		public static IEntry GetServerById(string id)
		{
			for (int i = 0; i < Favorites.MyFavorites.Count; i++)
				if (Favorites.MyFavorites[i].ToString() == id)
					return Favorites.MyFavorites[i];

			for( int i = 0; i < PublicServerList.Count; i++ )
				if( PublicServerList[i].Id == id )
					return PublicServerList[i];

			for( int i = 0; i < Favorites.Customs.Count; i++ )
				if( Uri.EscapeUriString(Favorites.Customs[i].Id) == id )
					return Favorites.Customs[i];

			return null;
		}

		/// <summary>
		/// Retrieves a default sorted Generic List of all servers in the public list.
		/// </summary>
		/// <returns></returns>
		public static List<ServerEntry> GetSortedPublicServers()
		{
			return GetSortedPublicServers("Id");
		}	

		/// <summary>
		/// Retrieves a sorted Generic List of all servers in the public list.
		/// </summary>
		/// <param name="sortby"></param>
		/// <returns></returns>
		public static List<ServerEntry> GetSortedPublicServers(string sortby)
		{
			if( _publicServerList == null )
				_publicServerList = CreateServerList();

			List<ServerEntry> list = _publicServerList;

			try
			{
				_sortType = (SortType)Enum.Parse(typeof(SortType),
					(sortby == "default" || sortby.Length == 0) ? "Id" : sortby);
			}
			catch
			{
				_sortType = (SortType)Enum.Parse(typeof(SortType), "Id");
			}

			list.Sort( new Comparison<ServerEntry>( Compare ) );
			return list;
		}

		private static int Compare(ServerEntry one, ServerEntry two)
		{
			switch( _sortType )
			{
				default:
				{
					return int.Parse(one.Id).CompareTo(int.Parse(two.Id));
				}
				case SortType.Name:
				{
					return one.Name.CompareTo(two.Name);
				}
				case SortType.Description:
				{
					return one.Description.CompareTo(two.Description);
				}
				case SortType.Rank:
				{
					return int.Parse(one.Rank).CompareTo(int.Parse(two.Rank));
				}
				case SortType.AvgOnline:
				{
					return int.Parse(one.AvgOnline).CompareTo(int.Parse(two.AvgOnline)) * -1;
				}
				case SortType.CurOnline:
				{
					return int.Parse(one.CurOnline).CompareTo(int.Parse(two.CurOnline)) * -1;
				}
				case SortType.MaxOnline:
				{
					return int.Parse(one.MaxOnline).CompareTo(int.Parse(two.MaxOnline)) * -1;
				}
				case SortType.TotalVotes:
				{
					return int.Parse(one.TotalVotes).CompareTo(int.Parse(two.TotalVotes)) * -1;
				}
				case SortType.Votes:
				{
					return int.Parse(one.Votes).CompareTo(int.Parse(two.Votes)) * -1;
				}

			}
		}	
	}
}
