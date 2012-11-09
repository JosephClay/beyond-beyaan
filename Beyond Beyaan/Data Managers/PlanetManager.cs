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
