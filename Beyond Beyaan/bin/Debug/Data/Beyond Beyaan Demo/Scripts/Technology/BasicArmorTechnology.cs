using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class BasicArmorTechnology
	{
		public static int GetTargetReticle(int shipSize)
		{
			return -1;
		}

		public Dictionary<string, object>[] Activate(int origX, int origY, int destX, int destY, int maxX, int maxY, Dictionary<string, object> itemValues)
		{
			if (itemValues.ContainsKey("baseDamageAbsorption"))
			{
				itemValues.Add("armorAbsorption", float.Parse((string)itemValues["baseDamageAbsorption"]));
			}
			return new Dictionary<string, object>[] { itemValues };
		}

		public Dictionary<string, object>[] OnHit(int impactX, int impactY, int shipX, int shipY, int shipSize, Dictionary<string, object> itemValues, Dictionary<string, object> particleValues)
		{
			List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();

			values.Add(new Dictionary<string, object>(particleValues));
			
			if (!values[0].ContainsKey("damage") || (float)values[0]["damage"] <= 0)
			{
				//No damage is being done, probably already applied, leave for visual effect
				return values.ToArray();
			}
			if (values.Count == 1)
			{
				values.Add(new Dictionary<string, object>());
			}
			float angle = (float)(Math.Atan2(shipY - impactY, shipX - impactX) - (Math.PI / 2));
			values[1].Add("Angle", angle);
			values[1].Add("LifeSpan", 2.0f);
			values[1].Add("ImpactX", impactX);
			values[1].Add("ImpactY", impactY);
			values[1].Add("ShipX", shipX);
			values[1].Add("ShipY", shipY);

			if (itemValues.ContainsKey("armorAbsorption") && particleValues.ContainsKey("damage"))
			{
				values[0]["damage"] = (float)particleValues["damage"] - (float)itemValues["armorAbsorption"];
				if ((float)values[0]["damage"] < 0)
				{
					values[0]["damage"] = 0;
				}
			}
			if (values[0].ContainsKey("PointAlreadyProcessed"))
			{
				values[0]["PointAlreadyProcessed"] = ((string)values[0]["PointAlreadyProcessed"] + impactX + "," + impactY + "|");
			}
			else
			{
				values[0].Add("PointAlreadyProcessed", impactX + "," + impactY + "|");
			}

			return values.ToArray();
		}

		public Dictionary<string, object> CompileEquipmentInfo(Dictionary<string, object> mainItemValues, Dictionary<string, object> mountItemValues, Dictionary<string, object>[] modifierItemValues, Dictionary<string, object> shipValues, Dictionary<string, object> modifableValues)
		{
			Dictionary<string, object> equipmentValues = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> keyValuePair in mainItemValues)
			{
				equipmentValues = HandleValue(keyValuePair, equipmentValues);
			}
			if (mountItemValues != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in mountItemValues)
				{
					equipmentValues = HandleValue(keyValuePair, equipmentValues);
				}
			}
			if (modifierItemValues != null)
			{
				foreach (Dictionary<string, object> modifierItem in modifierItemValues)
				{
					foreach (KeyValuePair<string, object> keyValuePair in modifierItem)
					{
						equipmentValues = HandleValue(keyValuePair, equipmentValues);
					}
				}
			}
			if (modifableValues != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in modifableValues)
				{
					equipmentValues = HandleValue(keyValuePair, equipmentValues);
				}
			}
			return equipmentValues;
		}

		private Dictionary<string, object> HandleValue(KeyValuePair<string, object> value, Dictionary<string, object> values)
		{
			switch (value.Key)
			{
				case "baseSpace": return AddValue("space", float.Parse((string)value.Value), values);
				case "baseCost": return AddValue("cost", float.Parse((string)value.Value), values);
				case "baseDamageAbsorption": return AddValue("armorAbsorption", float.Parse((string)value.Value), values);
				case "basePower": return AddValue("power", float.Parse((string)value.Value), values);
				case "baseRange": return AddValue("range", float.Parse((string)value.Value), values);
				case "baseSpaceMultiplier": return MultiplyValue("space", float.Parse((string)value.Value), values);
				case "baseCostMultiplier": return MultiplyValue("cost", float.Parse((string)value.Value), values);
				case "baseDamageAbsorptionMultiplier": return MultiplyValue("armorAbsorption", float.Parse((string)value.Value), values);
				case "basePowerMultiplier": return MultiplyValue("power", float.Parse((string)value.Value), values);
				case "baseRangeMultiplier": return MultiplyValue("range", float.Parse((string)value.Value), values);
				default: return values;
			}
		}

		private Dictionary<string, object> MultiplyValue(string key, float value, Dictionary<string, object> values)
		{
			if (values.ContainsKey(key))
			{
				values[key] = (float)values[key] * value;
			}
			else
			{
				values.Add(key, value * 100.0f);
			}
			return values;
		}

		private Dictionary<string, object> AddValue(string key, float value, Dictionary<string, object> values)
		{
			if (!values.ContainsKey(key))
			{
				values.Add(key, value);
				return values;
			}
			throw new Exception("Duplicate base value: " + key);
		}

		public Dictionary<string, object> UpdateShipInfo(Dictionary<string, object> shipValues, Dictionary<string, object> equipmentValues)
		{
			if (equipmentValues.ContainsKey("armorAbsorption"))
			{
				shipValues["armorAbsorption"] = (float)shipValues["armorAbsorption"] + (float)equipmentValues["armorAbsorption"];
			}
			if (equipmentValues.ContainsKey("space"))
			{
				shipValues["space"] = (float)shipValues["space"] + ((float)equipmentValues["space"] * (float)shipValues["maxSpace"]);
			}
			if (equipmentValues.ContainsKey("cost"))
			{
				shipValues["cost"] = (float)shipValues["cost"] + ((float)equipmentValues["cost"] * (float)shipValues["maxSpace"]);
			}
			return shipValues;
		}
	}
}