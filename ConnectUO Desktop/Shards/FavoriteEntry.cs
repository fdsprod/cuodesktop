using System;
using System.Collections.Generic;
using System.Xml;

namespace CUODesktop
{
	public class FavoriteEntry : ServerEntry
	{	
		public FavoriteEntry(XmlElement node) : base( node )
		{
			
		}

		public override string PlayLink
		{
			get { return "http://localhost.:1980/play?id=" + Id + "&auth=" + Authentication.AuthCode.ToString() + "&type=favorite"; }
		}
	}
}
