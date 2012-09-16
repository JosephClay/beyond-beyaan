using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Data_Modules
{
	public class Project
	{
		public StarSystem Location { get; private set; }
		public float Cost { get; private set; }
		public double AmountSoFar { get; private set; }
		public Planet PlanetToTerraform { get; private set; }
		public ShipDesign ShipToBuild { get; private set; }
		public int ProductionCapacityRequired { get; private set; }

		public Project(StarSystem location, Planet planetToTerraform)
		{
			Location = location;
			Cost = planetToTerraform.PlanetType.CostForTerraforming;
			PlanetToTerraform = planetToTerraform;
			AmountSoFar = 0;
			ProductionCapacityRequired = planetToTerraform.PlanetType.ProductionCapacityRequiredForTerraforming;
		}

		public Project(StarSystem location, ShipDesign shipToBuild)
		{
			Location = location;
			Dictionary<string, object> shipValues = shipToBuild.GetBasicValues();
			if (shipValues.ContainsKey("cost"))
			{
				Cost = (float)shipValues["cost"];
			}
			else
			{
				Cost = 1;
			}
			ShipToBuild = shipToBuild;
			AmountSoFar = 0;
			ProductionCapacityRequired = shipToBuild.ShipClass.Size;
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
			if (PlanetToTerraform != null)
			{
				return "Terraforming " + PlanetToTerraform.Name;
			}
			else
			{
				return "Building " + ShipToBuild.Name;
			}
		}

		public string GetPotentialProjectName()
		{
			if (PlanetToTerraform != null)
			{
				return "Terraform " + PlanetToTerraform.Name;
			}
			else
			{
				return "Build " + ShipToBuild.Name;
			}
		}
	}
}
