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

		/*public int Update(double amount, PlanetTypeManager planetTypeManager, Random r)
		{
			AmountSoFar += amount;
			int totalProduced = 0;
			while (AmountSoFar > Cost)
			{
				totalProduced++;
				AmountSoFar -= Cost;
				if (PlanetToTerraform != null)
				{
					//Update the cost
					PlanetToTerraform.SetPlanetType(planetTypeManager.GetPlanet(PlanetToTerraform.PlanetType.PlanetTerraformsTo), r);
					if (string.IsNullOrEmpty(PlanetToTerraform.PlanetType.PlanetTerraformsTo))
					{
						return -1; //End this project
					}
					Cost = PlanetToTerraform.PlanetType.CostForTerraforming;
				}
			}
			return totalProduced;
		}*/

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
