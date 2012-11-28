using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan
{
	public class PlanetManager
	{
		#region Variables
		//private float totalResearch;
		//private float totalCommerce;
		private List<Planet> planets;
		#endregion

		#region Properties
		public List<Planet> Planets
		{
			get { return planets; }
		}
		#endregion

		#region Constructor
		public PlanetManager()
		{
			planets = new List<Planet>();
		}
		#endregion

		#region Functions
		public void CalculateFoodConsumption(Dictionary<Resource, float> foodConsumption)
		{
			foodConsumption.Clear();
			foreach (Planet planet in planets)
			{
				planet.CalculateFoodConsumption(foodConsumption);
			}
		}

		public void ConsumeFood(Dictionary<Resource, float> resources, Dictionary<Resource, float> shortages)
		{
			foreach (Planet planet in planets)
			{
				planet.ConsumeFood(resources, shortages);
			}
		}

		public void PoolResources(Dictionary<Resource, float> availableResources, Dictionary<Resource, float> shortages)
		{
			// TODO: Update star systems' available resources and shortages for display

			for (int i = 0; i < planets.Count; i++)
			{
				for (int j = i + 1; j < planets.Count; j++)
				{
					var planetA = planets[i];
					var planetB = planets[j];
					//quick check to see if a planet is in demand, and another have some resources
					if (planetA.Shortages.Count > 0 && planetB.AvailableResources.Count > 0)
					{
						foreach (KeyValuePair<Resource, float> shortage in planetA.Shortages)
						{
							if (planetB.AvailableResources.ContainsKey(shortage.Key))
							{
								if (planetB.AvailableResources[shortage.Key] >= shortage.Value)
								{
									planetA.AddSuppliedResources(shortage.Key, shortage.Value);
									planetB.AddSharedResources(shortage.Key, shortage.Value);
									availableResources[shortage.Key] -= shortage.Value;
									if (availableResources[shortage.Key] <= 0)
									{
										availableResources.Remove(shortage.Key);
									}
								}
								else
								{
									planetA.AddSuppliedResources(shortage.Key, planetB.AvailableResources[shortage.Key]);
									planetB.AddSharedResources(shortage.Key, planetB.AvailableResources[shortage.Key]);
									availableResources[shortage.Key] -= planetB.AvailableResources[shortage.Key];
									if (availableResources[shortage.Key] <= 0)
									{
										availableResources.Remove(shortage.Key);
									}
								}
							}
						}
					}
					//quick check to see if a planet is in demand, and another have some resources
					if (planetB.Shortages.Count > 0 && planetA.AvailableResources.Count > 0)
					{
						foreach (KeyValuePair<Resource, float> shortage in planetB.Shortages)
						{
							if (planetA.AvailableResources.ContainsKey(shortage.Key))
							{
								if (planetA.AvailableResources[shortage.Key] >= shortage.Value)
								{
									planetB.AddSuppliedResources(shortage.Key, shortage.Value);
									planetA.AddSharedResources(shortage.Key, shortage.Value);
									availableResources[shortage.Key] -= shortage.Value;
									if (availableResources[shortage.Key] <= 0)
									{
										availableResources.Remove(shortage.Key);
									}
								}
								else
								{
									planetB.AddSuppliedResources(shortage.Key, planetA.AvailableResources[shortage.Key]);
									planetA.AddSharedResources(shortage.Key, planetA.AvailableResources[shortage.Key]);
									availableResources[shortage.Key] -= planetA.AvailableResources[shortage.Key];
									if (availableResources[shortage.Key] <= 0)
									{
										availableResources.Remove(shortage.Key);
									}
								}
							}
						}
					}
				}
			}
		}

		/*public void UpdateProduction(Dictionary<Resource, float> productions, Dictionary<Resource, float> consumptions, Dictionary<Resource, float> shortages, Dictionary<Resource, float> resources)
		{
			consumptions.Clear();
			foreach (Planet planet in planets)
			{
				planet.CalculateOptimalConsumption(consumptions);
			}
			shortages.Clear();
			foreach (var consumption in consumptions)
			{
				if (resources.ContainsKey(consumption.Key))
				{
					if (resources[consumption.Key] < consumption.Value)
					{
						shortages[consumption.Key] = resources[consumption.Key] / consumption.Value;
					}
					else
					{
						shortages[consumption.Key] = 1.0f;
					}
				}
				else
				{
					//No resource, so nothing to produce
					shortages[consumption.Key] = 0.0f;
				}
			}
			consumptions.Clear();
			productions.Clear();
			foreach (Planet planet in planets)
			{
				planet.CalculateConsumptionAndProduction(shortages, consumptions, productions);
			}
		}*/

		public void UpdatePopGrowth(Dictionary<Resource, float> foodShortages)
		{
			//this function calculates regular pop growth plus any bonuses or negatives
			foreach (Planet planet in planets)
			{
				planet.UpdatePlanet(foodShortages);
				foreach (Race race in planet.Races)
				{
					if (planet.GetRacePopulation(race) <= 0.1f)
					{
						//This race died out
						planet.RemoveRace(race);
					}
				}
			}
		}
		#endregion
	}
}
