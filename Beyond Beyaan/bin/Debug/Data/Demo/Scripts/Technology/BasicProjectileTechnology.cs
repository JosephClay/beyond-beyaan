using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class BasicProjectileTechnology
	{
		public static int GetTargetReticle(int shipSize)
		{
			return 0;
		}

		public Dictionary<string, object>[] Activate(int origX, int origY, int destX, int destY, int maxX, int maxY, Dictionary<string, object> itemValues)
		{
			Dictionary<string, object>[] currentValues = new Dictionary<string, object>[1];
			float angle = (float)Math.Atan2(destY - origY, destX - origX);
			float angle2 = (float)(angle - (Math.PI / 2));

			for (int i = 0; i < currentValues.Length; i++)
			{
				currentValues[i] = new Dictionary<string, object>();
				currentValues[i].Add("Angle", angle);
				currentValues[i].Add("OrigX", (float)(origX + (float)(Math.Cos(angle2) * ((4 * i) - (currentValues.Length / 2 * 4)))));
				currentValues[i].Add("OrigY", (float)(origY + (float)(Math.Sin(angle2) * ((4 * i) - (currentValues.Length / 2 * 4)))));
				currentValues[i].Add("DestX", destX + (int)(Math.Cos(angle2) * ((4 * i) - (currentValues.Length / 2 * 4))));
				currentValues[i].Add("DestY", destY + (int)(Math.Sin(angle2) * ((4 * i) - (currentValues.Length / 2 * 4))));
				currentValues[i].Add("MaxX", maxX);
				currentValues[i].Add("MaxY", maxY);

				if (itemValues.ContainsKey("damage"))
				{
					currentValues[i].Add("damage", itemValues["damage"]);
				}
				if (itemValues.ContainsKey("particle"))
				{
					currentValues[i].Add("SpawnParticle", itemValues["particle"]);
				}
			}
			
			return currentValues;
		}

		public Dictionary<string, object>[] OnHit(int impactX, int impactY, int shipX, int shipY, int shipSize, Dictionary<string, object> itemValues, Dictionary<string, object> particleValues)
		{
			return new Dictionary<string, object>[] { particleValues };
		}

		public Dictionary<string, object> CompileEquipmentInfo(Dictionary<string, object> mainItemValues, Dictionary<string, object> mountItemValues, Dictionary<string, object>[] modifierItemValues, Dictionary<string, object> shipValues, Dictionary<string, object> modifableValues)
		{
			Dictionary<string, object> equipmentValues = new Dictionary<string, object>();

			string equipmentName = string.Empty;

			foreach (KeyValuePair<string, object> keyValuePair in mainItemValues)
			{
				equipmentValues = HandleValue(keyValuePair, equipmentValues);
				if (keyValuePair.Key == "description")
				{
					equipmentName = (string)keyValuePair.Value;
				}
			}
			if (mountItemValues != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in mountItemValues)
				{
					equipmentValues = HandleValue(keyValuePair, equipmentValues);
					if (keyValuePair.Key == "description")
					{
						equipmentName = string.Format((string)keyValuePair.Value, equipmentName);
					}
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
			int mounts = 0;
			if (equipmentValues.ContainsKey("mounts"))
			{
				mounts = (int)equipmentValues["mounts"];
				equipmentValues.Add("_mounts", mounts.ToString());
			}
			if (equipmentValues.ContainsKey("damage"))
			{
				float value = (float)equipmentValues["damage"];
				equipmentValues.Add("_damage", value.ToString());
			}
			if (equipmentValues.ContainsKey("accuracy"))
			{
				float value = (float)equipmentValues["accuracy"] * 100.0f;
				equipmentValues.Add("_accuracy", string.Format("{0:0.0}", value.ToString()));
			}
			if (equipmentValues.ContainsKey("time"))
			{
				float value = (float)equipmentValues["time"];
				equipmentValues.Add("_time", value.ToString());
			}
			if (equipmentValues.ContainsKey("power"))
			{
				float value = (float)equipmentValues["power"];
				equipmentValues.Add("_power", value.ToString());
			}
			if (equipmentValues.ContainsKey("space"))
			{
				float value = (float)equipmentValues["space"] * mounts;
				equipmentValues.Add("_space", value.ToString());
			}
			if (equipmentValues.ContainsKey("ammo"))
			{
				int value = (int)equipmentValues["ammo"];
				equipmentValues.Add("_ammo", value.ToString());
			}
			if (!string.IsNullOrEmpty(equipmentName))
			{
				equipmentName = equipmentName + " x " + mounts;
				if (equipmentValues.ContainsKey("_name"))
				{
					equipmentValues["_name"] = equipmentName;
				}
				else
				{
					equipmentValues.Add("_name", equipmentName);
				}
			}
			return equipmentValues;
		}

		private Dictionary<string, object> HandleValue(KeyValuePair<string, object> value, Dictionary<string, object> values)
		{
			switch (value.Key)
			{
				case "particle": return AddValue("particle", value.Value, values);
				case "baseDamage": return AddValue("damage", float.Parse((string)value.Value), values);
				case "baseSpace": return AddValue("space", float.Parse((string)value.Value), values);
				case "baseAmmoSpace": return AddValue("ammoSpace", float.Parse((string)value.Value), values);
				case "baseCost": return AddValue("cost", float.Parse((string)value.Value), values);
				case "baseTime": return AddValue("time", float.Parse((string)value.Value), values);
				case "basePower": return AddValue("power", float.Parse((string)value.Value), values);
				case "baseAccuracy": return AddValue("accuracy", float.Parse((string)value.Value), values);
				case "baseRange": return AddValue("range", float.Parse((string)value.Value), values);
				case "baseDamageMultiplier": return MultiplyValue("damage", float.Parse((string)value.Value), values);
				case "baseSpaceMultiplier": return MultiplyValue("space", float.Parse((string)value.Value), values);
				case "baseAmmoSpaceMultiplier": return MultiplyValue("ammoSpace", float.Parse((string)value.Value), values);
				case "baseCostMultiplier": return MultiplyValue("cost", float.Parse((string)value.Value), values);
				case "baseTimeMultiplier": return MultiplyValue("shieldAbsorption", float.Parse((string)value.Value), values);
				case "basePowerMultiplier": return MultiplyValue("power", float.Parse((string)value.Value), values);
				case "baseAccuracyMultiplier": return MultiplyValue("accuracy", float.Parse((string)value.Value), values);
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

		private Dictionary<string, object> AddValue(string key, object value, Dictionary<string, object> values)
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
			int mounts = 1;
			if (equipmentValues.ContainsKey("mounts"))
			{
				mounts = (int)equipmentValues["mounts"];
			}
			if (equipmentValues.ContainsKey("space"))
			{
				shipValues["space"] = (float)shipValues["space"] + ((float)equipmentValues["space"] * mounts);
			}
			if (equipmentValues.ContainsKey("cost"))
			{
				shipValues["cost"] = (float)shipValues["cost"] + ((float)equipmentValues["cost"] * mounts);
			}
			return shipValues;
		}
	}
}