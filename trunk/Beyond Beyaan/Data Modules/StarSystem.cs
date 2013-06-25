using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan
{
	//public enum StarColor { RED, ORANGE, GREEN, PURPLE, BLUE, BROWN, WHITE, YELLOW }
	//public enum StarType { NORMAL, BLACK_HOLE }

	public class StarSystem
	{
		#region Member Variables
		//StarType type;
		private int x;
		private int y;
		private int size;
		private string name;

		private float[] color;
		private List<Empire> exploredBy;

		List<Planet> planets;
		#endregion

		#region Properties
		public int X 
		{ 
			get { return x; } 
			set { x = value; } 
		}
		public int Y
		{
			get { return y; }
			set { y = value; }
		}
		public string Name
		{
			get { return name; }
		}
		public BBSprite Sprite
		{
			get;
			private set;
		}
		public float[] StarColor
		{
			get { return color; }
		}
		public int Size
		{
			get { return size; }
		}
		public List<Planet> Planets
		{
			get { return planets; }
		}
		/*public StarType Type
		{
			get { return type; }
		}*/
		public Label StarName { get; set; }
		public Empire DominantEmpire { get; private set; }

		public List<Empire> EmpiresWithFleetAdjacentLastTurn { get; set; }
		public List<Empire> EmpiresWithFleetAdjacentThisTurn { get; set; }
		public List<Empire> EmpiresWithPlanetsInThisSystem { get; private set; }

		public Dictionary<Empire, float> OwnerPercentage;
		#endregion

		#region Constructor
		public StarSystem(string name, int x, int y, Color color, int minPlanets, int maxPlanets, SpriteManager spriteManager, Random r)
		{
			this.Sprite = spriteManager.GetSprite("Star", r);
			this.name = name;
			this.x = x;
			this.y = y;
			this.size = 1;

			this.color = new float[]
				{
					color.R / 255.0f,
					color.G / 255.0f,
					color.B / 255.0f,
					color.A / 255.0f
				};

			exploredBy = new List<Empire>();

			//type = StarType.NORMAL;

			int amountOfPlanets = r.Next(maxPlanets - minPlanets) + minPlanets;
			planets = new List<Planet>();
			for (int i = 0; i < amountOfPlanets; i++)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(this.name);
				sb.Append(" ");
				sb.Append(Utility.ConvertNumberToRomanNumberical(i + 1));
				planets.Add(new Planet(sb.ToString(), spriteManager, r, this));
			}
			StarName = new Label(name, 0, 0);
			EmpiresWithFleetAdjacentLastTurn = new List<Empire>();
			EmpiresWithFleetAdjacentThisTurn = new List<Empire>();
			EmpiresWithPlanetsInThisSystem = new List<Empire>();
			OwnerPercentage = new Dictionary<Empire, float>();
		}
		#endregion

		#region Public Functions
		public void SetHomeworld(Empire empire, SpriteManager spriteManager, out Planet homePlanet, Random r)
		{
			if (planets.Count == 0)
			{
				planets.Add(new Planet(name + " I", spriteManager, r, this));
				planets[0].SetHomeworld(empire, spriteManager, r);
				homePlanet = planets[0];
			}
			else
			{
				int whichPlanet = r.Next(planets.Count);
				planets[whichPlanet].SetHomeworld(empire, spriteManager, r);
				homePlanet = planets[whichPlanet];
			}
			exploredBy.Add(empire);
			UpdateOwners();
		}

		private void UpdateDominantEmpire()
		{
			Dictionary<Empire, int> count = new Dictionary<Empire, int>();
			foreach (Planet planet in planets)
			{
				if (planet.Owner != null)
				{
					if (count.ContainsKey(planet.Owner))
					{
						count[planet.Owner] = count[planet.Owner] + 1;
					}
					else
					{
						count.Add(planet.Owner, 1);
					}
				}
			}
			int biggestCount = 0;
			Empire biggestOwner = null;
			foreach (KeyValuePair<Empire, int> empire in count)
			{
				if (biggestCount < empire.Value)
				{
					biggestCount = empire.Value;
					biggestOwner = empire.Key;
				}
			}
			DominantEmpire = biggestOwner;
		}

		public bool IsThisSystemExploredByEmpire(Empire empire)
		{
			return exploredBy.Contains(empire);
		}
		public bool SystemHavePlanetOwnedByEmpire(Empire empire)
		{
			return EmpiresWithPlanetsInThisSystem.Contains(empire);
		}
		public void AddEmpireExplored(Empire empire)
		{
			exploredBy.Add(empire);
		}
		public void UpdateOwners()
		{
			EmpiresWithPlanetsInThisSystem = new List<Empire>();
			int amountOfPlanetsOwned = 0;
			Dictionary<Empire, int> planetsOwned = new Dictionary<Empire,int>();
			OwnerPercentage = new Dictionary<Empire, float>();
			foreach (Planet planet in planets)
			{
				if (planet.Owner != null)
				{
					if (!EmpiresWithPlanetsInThisSystem.Contains(planet.Owner))
					{
						EmpiresWithPlanetsInThisSystem.Add(planet.Owner);
						planetsOwned.Add(planet.Owner, 1);
					}
					else
					{
						planetsOwned[planet.Owner] += 1;
					}
					amountOfPlanetsOwned++;
				}
			}

			//Update the color percentage here
			foreach (KeyValuePair<Empire, int> keyValuePair in planetsOwned)
			{
				OwnerPercentage[keyValuePair.Key] = ((float)keyValuePair.Value / (float)amountOfPlanetsOwned);
			}

			UpdateDominantEmpire();
		}
		#endregion
	}
}
