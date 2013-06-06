using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	class PlanetManager
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
		public void UpdatePopGrowth()
		{
			//this function calculates regular pop growth plus any bonuses or negatives
			List<Planet> planetsToRemove = new List<Planet>();
			foreach (Planet planet in planets)
			{
				planet.UpdatePlanet();
				foreach (Race race in planet.Races)
				{
					if (planet.GetRacePopulation(race) < 0.1)
					{
						//This race died out
						planet.RemoveRace(race);
					}
				}
				if (planet.TotalPopulation == 0.0)
				{
					//Planet died out
					planet.Owner = null;
					planetsToRemove.Add(planet);
				}
			}
			foreach (Planet planet in planetsToRemove)
			{
				//those planets died out
				planets.Remove(planet);
			}
		}
		#endregion
	}
}
