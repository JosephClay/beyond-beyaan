using System.Collections.Generic;

namespace Beyond_Beyaan.Data_Modules
{
	public class RegionType
	{
		public float[] Color { get; set; }

		public string RegionTypeName { get; set; }
		
		public Dictionary<Resource, float> Consumptions { get; set; }
		public Dictionary<Resource, float> Productions { get; set; }

		public Dictionary<Resource, float> DevelopmentCost { get; set; }

		public RegionType()
		{
			Consumptions = new Dictionary<Resource, float>();
			Productions = new Dictionary<Resource, float>();
			DevelopmentCost = new Dictionary<Resource, float>();
		}
	}
}
