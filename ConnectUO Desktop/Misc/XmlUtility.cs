using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Net;
using System.Runtime.InteropServices;

using Microsoft.Win32;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;

namespace CUODesktop
{
	public class XmlUtility
	{
		/// <summary>
		/// Converts string to IPAdress, returns null if the operation fails
		/// </summary>
		/// <param name="IP"></param>
		/// <returns></returns>
		public static IPAddress GetIPAddress(string IP)
		{
			try
			{
				return IPAddress.Parse(IP.Equals("localhost.") ? "localhost." : IP);
			}
			catch( System.FormatException e )
			{
				Trace.HandleException(e);
				IPHostEntry entry1 = Dns.GetHostEntry(IP);
				if( entry1.AddressList.Length == 0 )
				{
					return null;
				}
				return entry1.AddressList[0];
			}
		}

		/// <summary>
		/// Converts string to Int32, returns defaultValue if the operation fails
		/// </summary>
		/// <param name="intString"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int GetInt32(string intString, int defaultValue)
		{
			try
			{
				return XmlConvert.ToInt32(intString);
			}
			catch( Exception e )
			{
				Trace.HandleException(e);

				try
				{
					return System.Convert.ToInt32(intString);
				}
				catch( Exception err )
				{
					Trace.HandleException(err);
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Converts XmlElement InnerText to string, returns defaultValue if the operation fails
		/// </summary>
		/// <param name="node"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetText(XmlElement node, string defaultValue)
		{
			if( node == null )
				return defaultValue;

			return node.InnerText == string.Empty ? defaultValue : node.InnerText;
		}

		/// <summary>
		/// Converts string to bool, returns defaultValue if the operation fails
		/// </summary>
		/// <param name="boolString"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool GetBool(string boolString, bool defaultValue)
		{
			if( boolString == "ACTIVE" || boolString == "DOWN" )
				return boolString == "ACTIVE";

			try
			{
				return XmlConvert.ToBoolean(boolString);
			}
			catch( Exception e )
			{
				Trace.HandleException(e);

				try
				{
					return System.Convert.ToBoolean(boolString);
				}
				catch( Exception err )
				{
					Trace.HandleException(err);
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Converts the specified XmlElement Attribute to Int32
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attributeName"></param>
		/// <returns></returns>
		public static int GetAttributeInt32(XmlElement node, string attributeName)
		{
			XmlAttribute attr = node.Attributes[attributeName];
			string intString = attr.Value;

			try
			{
				return XmlConvert.ToInt32(intString);
			}
			catch( Exception e )
			{
				Trace.HandleException(e);

				try
				{
					return System.Convert.ToInt32(intString);
				}
				catch( Exception err )
				{
					Trace.HandleException(err);
					return 0;
				}
			}
		}	
	}
}
