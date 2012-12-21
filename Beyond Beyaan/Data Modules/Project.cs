using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Data_Modules
{
	public enum PROJECT_TYPE { REGION, REGION_BUILDING, PLANET_BUILDING, PLANET_TERRAFORM, SHIP, SYSTEM_BUILDING, RESEARCH }

	public class Project
	{
		public PROJECT_TYPE ProjectType { get; private set; }
		public StarSystem WhichSystem { get; private set; }
		public Planet WhichPlanet { get; private set; }
		public Region WhichRegion { get; private set; }
		public Dictionary<Resource, float> Cost { get; private set; }
		public Dictionary<Resource, float> AmountSoFar { get; private set; }
		//public Planet PlanetToTerraform { get; private set; }
		public ShipDesign ShipToBuild { get; private set; }
		public RegionType RegionTypeToBuild { get; private set; }
		//public int ProductionCapacityRequired { get; private set; }

		public Project(PROJECT_TYPE projectType, StarSystem whichSystem = null, Planet whichPlanet = null, Region whichRegion = null,
			ShipDesign shipToBuild = null, RegionType regionTypeToBuild = null)
		{
			ProjectType = projectType;
			WhichSystem = whichSystem;
			WhichPlanet = whichPlanet;
			WhichRegion = whichRegion;

			AmountSoFar = new Dictionary<Resource, float>();

			switch (ProjectType)
			{
				case PROJECT_TYPE.REGION:
					RegionTypeToBuild = regionTypeToBuild;
					Cost = new Dictionary<Resource, float>(RegionTypeToBuild.DevelopmentCost);
					break;
				case PROJECT_TYPE.SHIP:
					ShipToBuild = shipToBuild;
					Cost = new Dictionary<Resource, float>(ShipToBuild.DevelopmentCost);
					break;
			}
		}

		public int Update(Dictionary<Resource, float> amount, float percentage)
		{
			//Just add the values, even if it exceeds the cost
			foreach (var resource in Cost)
			{
				if (amount.ContainsKey(resource.Key))
				{
					if (AmountSoFar.ContainsKey(resource.Key))
					{
						AmountSoFar[resource.Key] += (amount[resource.Key] * percentage);
					}
					else
					{
						AmountSoFar[resource.Key] = (amount[resource.Key] * percentage);
					}
				}
			}

			int totalProduced = 0;
			bool sufficientFunds = true;
			while (sufficientFunds)
			{
				foreach (var resource in Cost)
				{
					if (!AmountSoFar.ContainsKey(resource.Key) || AmountSoFar[resource.Key] < resource.Value)
					{
						sufficientFunds = false;
						break;
					}
				}
				if (sufficientFunds)
				{
					totalProduced++;
					foreach (var resource in Cost)
					{
						AmountSoFar[resource.Key] -= resource.Value;
					}
				}
			}
			return totalProduced;
		}

		public override string ToString()
		{
			switch (ProjectType)
			{
				case PROJECT_TYPE.REGION:
					return "Building " + RegionTypeToBuild.RegionTypeName;
				case PROJECT_TYPE.SHIP:
					return "Building " + ShipToBuild.Name;
				default:
					return "Error - Unknown";
			}
		}
	}
}
