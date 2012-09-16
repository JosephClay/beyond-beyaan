using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Beyond_Beyaan.Data_Modules
{
	public class Region
	{
		public RegionType RegionType { get; set; }

		//Below is used for UI display in list of regions.  They are affected by shortages/population count, and is different from the Region's output/consumption list
		public Dictionary<Resource, float> CurrentConsumptions { get; set; }
		public Dictionary<Resource, float> CurrentProductions { get; set; }
	}
}
