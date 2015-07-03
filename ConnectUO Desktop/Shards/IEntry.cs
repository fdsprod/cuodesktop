using System;
using System.Collections.Generic;
using System.Xml;

namespace CUODesktop
{
	public interface IEntry
	{
		XmlElement Element { get; }

		bool Status { get; }
		string Id { get; }
		string Name { get; }
		string Description { get; }
		string Url { get; }
		string PatchUrl { get; }
		string UpdateUrl { get; }
		string Era { get; }
		string Type { get; }
		string Lang { get; }
		bool RemoveEncryption { get; }
		bool AllowRazor { get; }
		string HostAddress { get; }
		int Port { get; }
		string MaxOnline { get; }
		string CurOnline { get; }
		string AvgOnline { get; }
		string Votes { get; }
		string TotalVotes { get; }
		string Out { get; }
		string TotalOut { get; }
		string Rank { get; }
		string ResetPatchesLink { get; }

		string Banner { get; }
		string MoreInfoLink { get; }
		string AddToFavoritesLink { get; }
		string PlayLink { get; }
		string WebsiteLink { get; }
		string VoteLink { get; }
		string OneDayGraph { get; }
		string SevenDayGraph { get; }
		string FourteenDayGraph { get; }
		string ThirtyDayGraph { get; }
		string OneEightDayGraph { get; }
		string YearDayGraph { get; }		
	}
}
