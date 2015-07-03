using System;
using System.Collections.Generic;
using System.Xml;

namespace CUODesktop
{
	public class ServerEntry : IEntry
	{
		private XmlElement _node;

		#region IEntry Members
		/// <summary>
		/// Gets a boolean base on the server up status.
		/// </summary>
		public bool Status
		{
			get { return XmlUtility.GetBool(XmlUtility.GetText(_node["status"], "true"), true); } 
		}
		/// <summary>
		/// Gets the XmlElement for the server
		/// </summary>
		public XmlElement Element
		{
			get { return _node; } 
		}
		/// <summary>
		/// Gets the id of the server
		/// </summary>
		public string Id
		{
			get { return XmlUtility.GetAttributeInt32(_node, "id").ToString(); }
		}
		/// <summary>
		/// Gets the name of the server
		/// </summary>
		public string Name
		{
			get { return XmlUtility.GetText(_node["name"], string.Empty); }
		}
		/// <summary>
		/// Gets the desciption of the server
		/// </summary>
		public string Description
		{
			get { return XmlUtility.GetText(_node["description"], string.Empty); }
		}
		/// <summary>
		/// Gets the url of the server
		/// </summary>
		public string Url
		{ 
			get { return XmlUtility.GetText(_node["url"], string.Empty); }
		}
		/// <summary>
		/// Gets the patch url of the server
		/// </summary>
		public string PatchUrl 
		{ 
			get { return XmlUtility.GetText(_node["patchurl"], string.Empty); }
		}
		/// <summary>
		/// Gets the update url of the server
		/// </summary>
		public string UpdateUrl
		{
			get { return XmlUtility.GetText(_node["updateurl"], string.Empty); }
		}
		/// <summary>
		/// Gets the server era
		/// </summary>
		public string Era 
		{ 
			get { return XmlUtility.GetText(_node["era"], "--"); }
		}
		/// <summary>
		/// Gets the server type
		/// </summary>
		public string Type 
		{ 
			get { return XmlUtility.GetText(_node["type"], "--"); }
		}
		/// <summary>
		/// Gets the server language
		/// </summary>
		public string Lang 
		{ 
			get { return XmlUtility.GetText(_node["lang"], "--"); }
		}
		/// <summary>
		/// Gets a boolean based on whether the encryption for the client is to be removed
		/// </summary>
		public bool RemoveEncryption 
		{ 
			get { return XmlUtility.GetBool(XmlUtility.GetText(_node["patch_enc"], "true"), true); }
		}
		/// <summary>
		/// Gets a boolean based on whether razor is allowed on the server
		/// </summary>
		public bool AllowRazor 
		{
			get { return XmlUtility.GetBool(XmlUtility.GetText(_node["razor"], "true"), true); }
		}
		/// <summary>
		/// Gets the host address/ip for the server
		/// </summary>
		public string HostAddress 
		{ 
			get { return XmlUtility.GetText(_node["address"], "--"); }
		}
		/// <summary>
		/// Gets the port for the server
		/// </summary>
		public int Port 
		{
			get { return XmlUtility.GetInt32(XmlUtility.GetText(_node["port"], "2593"), 2593); }
		}
		/// <summary>
		/// Gets the peak amount of players ever online
		/// </summary>
		public string MaxOnline 
		{ 
			get { return XmlUtility.GetText(_node["max_online"], "--"); }
		}
		/// <summary>
		/// Gets the current amount of player online
		/// </summary>
		public string CurOnline 
		{ 
			get { return XmlUtility.GetText(_node["cur_online"], "--"); }
		}
		/// <summary>
		/// Gets the overall average number of players online
		/// </summary>
		public string AvgOnline 
		{ 
			get { return XmlUtility.GetText(_node["avg_online"], "--"); }
		}
		/// <summary>
		/// Gets the number of votes for the server 
		/// </summary>
		public string Votes 
		{ 
			get { return XmlUtility.GetText(_node["votes"], "--"); }
		}
		/// <summary>
		/// Gets the number of total votes the server has recieved
		/// </summary>
		public string TotalVotes 
		{ 
			get { return XmlUtility.GetText(_node["total_votes"], "--"); }
		}
		/// <summary>
		/// Gets the number of rejected votes for the server
		/// </summary>
		public string Out 
		{ 
			get { return XmlUtility.GetText(_node["out"], "--"); }
		}
		/// <summary>
		/// Gets the number of total rejected votes the server has recieved
		/// </summary>
		public string TotalOut 
		{ 
			get { return XmlUtility.GetText(_node["total_out"], "--"); }
		}
		/// <summary>
		/// Gets the server's rank within the list.
		/// </summary>
		public string Rank 
		{ 
			get { return XmlUtility.GetText(_node["rank"], "--"); }
		}
		/// <summary>
		/// Gets a link to the server's banner
		/// </summary>
		public string Banner
		{
			get { return "http://www.connectuo.com/banner.php?id=" + Id; }
		}
		/// <summary>
		/// Gets a link to display moreinfo about the server
		/// </summary>
		public string MoreInfoLink
		{
			get { return "http://localhost.:1980/moreinfo.html?id=" + Id; }
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
			get { return "http://localhost.:1980/addfavorite?id=" + Id; }
		}
		/// <summary>
		/// Gets a link to play the server.
		/// </summary>
		public virtual string PlayLink
		{
			get { return "http://localhost.:1980/play?id=" + Id + "&auth=" + Authentication.AuthCode.ToString() + "&type=public"; }
		}
		/// <summary>
		/// Gets a link to the servers website.
		/// </summary>
		public string WebsiteLink
		{
			get { return XmlUtility.GetText(_node["url"], "http://www.connectuo.com"); }
		}
		/// <summary>
		/// Gets a link to vote for the server
		/// </summary>
		public string VoteLink
		{
			get { return "http://www.connectuo.com/index.php?page=shards&do=vote&id=" + Id; }
		}
		/// <summary>
		/// Gets an address to show a online player count for one day
		/// </summary>
		public string OneDayGraph
		{
			get { return "http://www.connectuo.com/graph.php?id=" + Id + "&mode=1"; }
		}
		/// <summary>
		/// Gets an address to show a online player count for seven days
		/// </summary>
		public string SevenDayGraph
		{
			get { return "http://www.connectuo.com/graph.php?id=" + Id + "&mode=7"; }
		}
		/// <summary>
		/// Gets an address to show a online player count for fourteen days
		/// </summary>
		public string FourteenDayGraph
		{
			get { return "http://www.connectuo.com/graph.php?id=" + Id + "&mode=14"; }
		}
		/// <summary>
		/// Gets an address to show a online player count for thirty days
		/// </summary>
		public string ThirtyDayGraph
		{
			get { return "http://www.connectuo.com/graph.php?id=" + Id + "&mode=30"; }
		}
		/// <summary>
		/// Gets an address to show a online player count for 180 days
		/// </summary>
		public string OneEightDayGraph
		{
			get { return "http://www.connectuo.com/graph.php?id=" + Id + "&mode=180"; }
		}
		/// <summary>
		/// Gets an address to show a online player count for one year
		/// </summary>
		public string YearDayGraph
		{
			get { return "http://www.connectuo.com/graph.php?id=" + Id + "&mode=365"; }
		}
		#endregion

		public ServerEntry( XmlElement node ) 
		{
			_node = node;
		}
	}
}
