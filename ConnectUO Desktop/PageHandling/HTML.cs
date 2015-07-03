using System;
using System.Collections.Generic;
using System.Text;

namespace CUODesktop
{
	public class HTML
	{
		#region _sortForm
		private static string[] _sortForm = new string[] 
        {
			"<form id=\"form1\" name=\"form1\" method=\"get\"  style=\"display:inline\" action=\"http://localhost:1980/sort\">",
			"      <select name=\"sortby\">",
			"        <option value=\"default\">Default</option>",
			"        <option value=\"Rank\">Rank</option>",
			"        <option value=\"CurOnline\">Online Count</option>",
			"        <option value=\"MaxOnline\">Online Peak</option>",
			"        <option value=\"AvgOnline\">Online Avg</option>",
			"        <option value=\"Votes\">Votes</option>",
			"        <option value=\"TotalVotes\">Total Votes</option>",
			"        <option value=\"Name\">Name</option>",
			"        <option value=\"Description\">Description</option>",
		    "      </select>",
			"      <input type=\"submit\" value=\"Sort\" />",
            "    </form>"	
	    };
		#endregion	

		internal static string SortForm
		{
			get
			{
				string s = "";
				for( int i = 0; i < _sortForm.Length; i++ )
					s += _sortForm[i];
				return s;
			}
		}

		internal static string ParseUrl(string url, string key)
		{
			string[] split = url.Split('&');
			string value = string.Empty;

			for( int i = 0; i < split.Length; i++ )
				if( split[i].Contains(key) )
				{
					split = split[i].Split('=');
					value = split[1];
					break;
				}

			return value;
		}

		internal static Dictionary<string, string> ParseUrl(string request)
		{
			string[] split = request.Split('&');
			Dictionary<string, string> requests = new Dictionary<string, string>();

			for( int i = 0; i < split.Length; i++ )
			{
				string[] keyPair = split[i].Split('=');

				if( keyPair.Length == 2 )
					requests.Add(keyPair[0], keyPair[1]);
			}

			return requests;
		}
		
	}
}
