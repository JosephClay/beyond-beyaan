using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Modules
{
	public class SectorObject
	{
		public SectorObjectType Type { get; private set; }
		public string Name { get; set; }

		public Empire ClaimedBy { get; set; }

		public SectorObject(SectorObjectType type)
		{
			Type = type;
		}
	}
}
