using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class BasicShip
	{
		public Dictionary<string, object>[] OnHit(int impactX, int impactY, int shipX, int shipY, int shipSize, Dictionary<string, object>[] currentValues, Dictionary<string, object> shipValues, Dictionary<string, object> particleValues)
		{
			List<Dictionary<string, object>> newValues = new List<Dictionary<string,object>>();
			newValues.Add(new Dictionary<string,object>(particleValues));
			newValues.Add(new Dictionary<string,object>(shipValues));
			if (!newValues[1].ContainsKey("RemoveShip") && newValues[0].ContainsKey("damage") && newValues[1].ContainsKey("HitPoints"))
			{
				float damageAmount = (float)newValues[1]["health"] - (float)newValues[0]["damage"];
				float damageReduction = (float)newValues[0]["damage"] - (float)newValues[1]["health"]; //If the damage exceeds the hitpoints, it continues on hitting

				newValues[1]["health"] = damageAmount;
				newValues[0]["damage"] = damageReduction;

				if (damageAmount <= 0)
				{
					//The ship died
					newValues[1].Add("RemoveShip", true);
					Dictionary<string, object> dyingEffect = new Dictionary<string, object>();
					dyingEffect.Add("_spawnTempEffect", "ShipDying");
					newValues.Add(dyingEffect);
				}
				else
				{
					Dictionary<string, object> explosion = new Dictionary<string,object>();
					explosion.Add("SpawnParticle", "SmallExplosion");
					explosion.Add("ImpactX", impactX);
					explosion.Add("ImpactY", impactY);
					explosion.Add("Angle", 0.0f);
					explosion.Add("LifeSpan", 0.5f);
					newValues.Add(explosion);
				}
				if (newValues[0].ContainsKey("PointAlreadyProcessed"))
				{
					newValues[0]["PointAlreadyProcessed"] = ((string)newValues[0]["PointAlreadyProcessed"] + impactX + "," + impactY + "|");
				}
				else
				{
					newValues[0].Add("PointAlreadyProcessed", impactX + "," + impactY + "|");
				}
			}
			return newValues.ToArray();
		}

		public Dictionary<string, object> UpdateShipInfo(Dictionary<string, object> shipValues)
		{
			//Resets the ship information
			if (shipValues.ContainsKey("time") && shipValues.ContainsKey("maxTime"))
			{
				shipValues["time"] = shipValues["maxTime"];
			}
			if (!shipValues.ContainsKey("armorAbsorption"))
			{
				shipValues.Add("armorAbsorption", 0.0f);
			}
			else
			{
				shipValues["armorAbsorption"] = 0.0f;
			}
			if (!shipValues.ContainsKey("shieldAbsorption"))
			{
				shipValues.Add("shieldAbsorption", 0.0f);
			}
			else
			{
				shipValues["shieldAbsorption"] = 0.0f;
			}
			if (shipValues.ContainsKey("maxPower"))
			{
				shipValues["maxPower"] = 0.0f;
			}
			else
			{
				shipValues.Add("maxPower", 0.0f);
			}
			if (shipValues.ContainsKey("combatSpeed"))
			{
				shipValues["combatSpeed"] = 0.0f;
			}
			else
			{
				shipValues.Add("combatSpeed", 0.0f);
			}
			if (shipValues.ContainsKey("galaxySpeed"))
			{
				shipValues["galaxySpeed"] = 0.0f;
			}
			else
			{
				shipValues.Add("galaxySpeed", 0.0f);
			}
			if (shipValues.ContainsKey("powerRechargeRate"))
			{
				shipValues["powerRechargeRate"] = 0.0f;
			}
			else
			{
				shipValues.Add("powerRechargeRate", 0.0f);
			}
			return shipValues;
		}

		public Dictionary<string, object> GetShipInfo(Dictionary<string, object> shipValues)
		{
			if (shipValues.ContainsKey("health"))
			{
				if (shipValues.ContainsKey("_health"))
				{
					shipValues["_health"] = ((float)shipValues["health"]).ToString();
				}
				else
				{
					shipValues.Add("_health", ((float)shipValues["health"]).ToString());
				}
			}
			if (shipValues.ContainsKey("maxHealth"))
			{
				if (shipValues.ContainsKey("_maxHealth"))
				{
					shipValues["_maxHealth"] = ((float)shipValues["maxHealth"]).ToString();
				}
				else
				{
					shipValues.Add("_maxHealth", ((float)shipValues["maxHealth"]).ToString());
				}
			}
			if (shipValues.ContainsKey("armorAbsorption"))
			{
				if (shipValues.ContainsKey("_armorAbsorption"))
				{
					shipValues["_armorAbsorption"] = ((float)shipValues["armorAbsorption"]).ToString();
				}
				else
				{
					shipValues.Add("_armorAbsorption", ((float)shipValues["armorAbsorption"]).ToString());
				}
			}
			if (shipValues.ContainsKey("shieldAbsorption"))
			{
				if (shipValues.ContainsKey("_shieldAbsorption"))
				{
					shipValues["_shieldAbsorption"] = ((float)shipValues["shieldAbsorption"]).ToString();
				}
				else
				{
					shipValues.Add("_shieldAbsorption", ((float)shipValues["shieldAbsorption"]).ToString());
				}
			}
			if (shipValues.ContainsKey("time"))
			{
				if (shipValues.ContainsKey("_time"))
				{
					shipValues["_time"] = ((float)shipValues["time"]).ToString();
				}
				else
				{
					shipValues.Add("_time", ((float)shipValues["time"]).ToString());
				}
			}
			if (shipValues.ContainsKey("maxTime"))
			{
				if (shipValues.ContainsKey("_maxTime"))
				{
					shipValues["_maxTime"] = ((float)shipValues["maxTime"]).ToString();
				}
				else
				{
					shipValues.Add("_maxTime", ((float)shipValues["maxTime"]).ToString());
				}
			}
			if (shipValues.ContainsKey("power"))
			{
				if (shipValues.ContainsKey("_power"))
				{
					shipValues["_power"] = ((float)shipValues["power"]).ToString();
				}
				else
				{
					shipValues.Add("_power", ((float)shipValues["power"]).ToString());
				}
			}
			if (shipValues.ContainsKey("maxPower"))
			{
				if (shipValues.ContainsKey("_maxPower"))
				{
					shipValues["_maxPower"] = ((float)shipValues["maxPower"]).ToString();
				}
				else
				{
					shipValues.Add("_maxPower", ((float)shipValues["maxPower"]).ToString());
				}
			}
			if (shipValues.ContainsKey("powerRechargeRate"))
			{
				if (shipValues.ContainsKey("_powerRechargeRate"))
				{
					shipValues["_powerRechargeRate"] = ((float)shipValues["powerRechargeRate"]).ToString();
				}
				else
				{
					shipValues.Add("_powerRechargeRate", ((float)shipValues["powerRechargeRate"]).ToString());
				}
			}
			if (shipValues.ContainsKey("maxSpace"))
			{
				if (shipValues.ContainsKey("_maxSpace"))
				{
					shipValues["_maxSpace"] = ((float)shipValues["maxSpace"]).ToString();
				}
				else
				{
					shipValues.Add("_maxSpace", ((float)shipValues["maxSpace"]).ToString());
				}
			}
			if (shipValues.ContainsKey("space"))
			{
				if (shipValues.ContainsKey("_space"))
				{
					shipValues["_space"] = ((float)shipValues["space"]).ToString();
				}
				else
				{
					shipValues.Add("_space", ((float)shipValues["space"]).ToString());
				}
			}
			if (shipValues.ContainsKey("maxSpace"))
			{
				if (shipValues.ContainsKey("_maxSpace"))
				{
					shipValues["_maxSpace"] = ((float)shipValues["maxSpace"]).ToString();
				}
				else
				{
					shipValues.Add("_maxSpace", ((float)shipValues["maxSpace"]).ToString());
				}
			}
			if (shipValues.ContainsKey("galaxySpeed"))
			{
				if (shipValues.ContainsKey("_galaxySpeed"))
				{
					shipValues["_galaxySpeed"] = ((float)shipValues["galaxySpeed"]).ToString();
				}
				else
				{
					shipValues.Add("_galaxySpeed", ((float)shipValues["galaxySpeed"]).ToString());
				}
			}
			if (shipValues.ContainsKey("combatSpeed"))
			{
				if (shipValues.ContainsKey("_combatSpeed"))
				{
					shipValues["_combatSpeed"] = ((float)shipValues["combatSpeed"]).ToString();
				}
				else
				{
					shipValues.Add("_combatSpeed", ((float)shipValues["combatSpeed"]).ToString());
				}
			}
			if (shipValues.ContainsKey("cost"))
			{
				if (shipValues.ContainsKey("_cost"))
				{
					shipValues["_cost"] = ((float)shipValues["cost"]).ToString();
				}
				else
				{
					shipValues.Add("_cost", ((float)shipValues["cost"]).ToString());
				}
			}
			return shipValues;
		}

		public Dictionary<string, object> Initialize(Dictionary<string, object> shipValues)
		{
			if (shipValues.ContainsKey("HitPoints"))
			{
				if (!shipValues.ContainsKey("_maxHealth"))
				{
					shipValues.Add("_maxHealth", shipValues["HitPoints"]);
				}
				if (!shipValues.ContainsKey("maxHealth"))
				{
					shipValues.Add("maxHealth", float.Parse((string)shipValues["HitPoints"]));
				}
				if (!shipValues.ContainsKey("health"))
				{
					//Haven't been damaged yet, first combat
					shipValues.Add("health", shipValues["maxHealth"]);
				}
				if (!shipValues.ContainsKey("_health"))
				{
					shipValues.Add("_health", shipValues["_maxHealth"]);
				}
			}
			if (!shipValues.ContainsKey("maxPower"))
			{
				shipValues.Add("maxPower", 0.0f);
			}
			if (!shipValues.ContainsKey("power"))
			{
				shipValues.Add("power", shipValues["maxPower"]);
			}
			if (!shipValues.ContainsKey("powerRechargeRate"))
			{
				shipValues.Add("powerRechargeRate", 0.0f);
			}
			if (!shipValues.ContainsKey("maxTime"))
			{
				shipValues.Add("maxTime", 100.0f);
			}
			if (!shipValues.ContainsKey("time"))
			{
				shipValues.Add("time", shipValues["maxTime"]);
			}
			if (!shipValues.ContainsKey("armorAbsorption"))
			{
				shipValues.Add("armorAbsorption", 0.0f);
			}
			if (!shipValues.ContainsKey("shieldAbsorption"))
			{
				shipValues.Add("shieldAbsorption", 0.0f);
			}
			if (!shipValues.ContainsKey("galaxySpeed"))
			{
				shipValues.Add("galaxySpeed", 0.0f);
			}
			if (!shipValues.ContainsKey("combatSpeed"))
			{
				shipValues.Add("combatSpeed", 0.0f);
			}
			if (!shipValues.ContainsKey("cost"))
			{
				if (shipValues.ContainsKey("_shipCost"))
				{
					shipValues.Add("cost", float.Parse((string)shipValues["_shipCost"]));
				}
				else
				{
					shipValues.Add("cost", 1.0f);
				}
			}
			if (!shipValues.ContainsKey("maxSpace"))
			{
				if (shipValues.ContainsKey("_maxSpace"))
				{
					shipValues.Add("maxSpace", float.Parse((string)shipValues["_maxSpace"]));
				}
				else
				{
					shipValues.Add("maxSpace", 0.0f);
				}
				if (shipValues.ContainsKey("space"))
				{
					shipValues["space"] = 0;
				}
				else
				{
					shipValues.Add("space", 0.0f);
				}
			}

			return shipValues;
		}

		public string IsShipDesignValid(Dictionary<string, object> shipValues)
		{
			if (shipValues.ContainsKey("maxSpace") && shipValues.ContainsKey("space"))
			{
				if ((float)shipValues["maxSpace"] < (float)shipValues["space"])
				{
					return "Space used exceeds the available space";
				}
			}
			else
			{
				return "Invalid ship configuration, missing either maxSpace or space values";
			}
			return null;
		}
	}
}