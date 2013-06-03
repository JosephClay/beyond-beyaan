using System.Collections.Generic;

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

		public float EstimatedTurns(Dictionary<Resource, float> amount, float percentage)
		{
			Dictionary<Resource, float> amountSoFar = new Dictionary<Resource, float>(AmountSoFar);
			foreach (var resource in amount)
			{
				if (Cost.ContainsKey(resource.Key))
				{
					if (amountSoFar.ContainsKey(resource.Key))
					{
						amountSoFar[resource.Key] += resource.Value * percentage;
					}
					else
					{
						amountSoFar[resource.Key] = resource.Value * percentage;
					}
				}
			}
			float lowestAmount = float.MaxValue;
			foreach (var resource in amountSoFar)
			{
				if (resource.Value == AmountSoFar[resource.Key])
				{
					//No change, so this won't ever get completed
					return 0;
				}
				if (resource.Value < Cost[resource.Key])
				{
					//More than a turn
					float time = amount[resource.Key] / (Cost[resource.Key] - AmountSoFar[resource.Key]);
					if (lowestAmount > time)
					{
						lowestAmount = percentage;
					}
				}
				else
				{
					float value = resource.Value;
					float time = 0;
					while (value > Cost[resource.Key])
					{
						time += 1;
						value -= Cost[resource.Key];
					}
					if (time < lowestAmount)
					{
						lowestAmount = time;
					}
				}
			}
			return lowestAmount;
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
