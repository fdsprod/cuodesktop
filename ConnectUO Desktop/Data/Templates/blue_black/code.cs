using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

using CUODesktop;

namespace CUOCompiledCode
{
	public class CUOCompiledPage
	{
		//GET data dictionary
		private static Dictionary<string, string> m_GetData;
		private static string _getDataString;
		private static string _output;

		public static string Output(string[] args)
		{
			//Update the authentication codes
			Authentication.UpdateAuth();
			//Convert HTTP GET data into a dictionary collection

			//Set _getDataString with the HTTP GET data from the server
			if( args.Length > 0 )
				_getDataString = args[0];
			else
				_getDataString = "";

			//Build a Dictionary with the GET data for easier access
			m_GetData = Utility.RetrieveGetData(args);

			//Check if sortby is in the GET data
			string sortBy = RequestData("sortby");

			//If it is, Sort the list.
			if( sortBy.Length > 0 )
				ServerList.PublicServerList = ServerList.GetSortedPublicServers(sortBy);
			
			/*START PAGE DATA*/
			{0}//DO NOT REMOVE THIS
			/*END PAGE DATA*/

			return _output;
		}

		/// <summary>
		/// Inserts an HTML sort form into the page
		/// </summary>
		/// <returns>HTML code</returns>
		public static string GetSortForm()
		{
			//Get the form code.
			string form = Utility.SortForm;
			//Replace %GETDATA% with the getdata from the server
			return form.Replace("%GETDATA%", _getDataString.Length > 0 ? "?" + _getDataString : "");
		}

		/// <summary>
		/// Retrieves the value of the key requested from the GET data
		/// </summary>
		/// <param name="key"></param>
		/// <returns>GET request information, or "" if the key does not exist</returns>
		public static string RequestData(string key)
		{
			//If the key is not present return ""
			if (!m_GetData.ContainsKey(key))
				return "";

			return m_GetData[key];
		}

		/// <summary>
		/// Writes the string to the page
		/// </summary>
		/// <param name="echo"></param>
		public static void Echo(string echo)
		{
			_output += echo;
		}

		/// <summary>
		/// Writes the formatted string to the page
		/// </summary>
		/// <param name="echo"></param>
		/// <param name="args"></param>
		public static void Echo(string echo, params object[] args)
		{
			_output += String.Format(echo, args);
		}

		/// <summary>
		/// Replaces all occurances of the specified string, with a specified string.
		/// </summary>
		/// <param name="text">string to search for occurances</param>
		/// <param name="find">string to find</param>
		/// <param name="replace">string to replace</param>
		public static void Replace(ref string text, string find, string replace)
		{
			text = text.Replace(find, replace);
		}
	}
}
