using System;
using System.Collections.Generic;
using System.Text;

using CUODesktop.PatchLib;

namespace PatchUO
{
	public class MetaData
	{
		private SubscriberList<Patch> _patches;

		public SubscriberList<Patch> Patches { get { return _patches; } set { _patches = value; } }

		public MetaData()
		{
			_patches = new SubscriberList<Patch>();
		}
	}
}
