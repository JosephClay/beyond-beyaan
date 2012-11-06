using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class BasicReactorTechnology
	{
		public static int GetTargetReticle(int shipSize)
		{
			return -1;
		}

		public Dictionary<string, object>[] Activate(int origX, int origY, int destX, int destY, int maxX, int maxY, Dictionary<string, object> itemValues)
		{
			return new Dictionary<string, object>[] { itemValues };
		}

		public Dictionary<string, object>[] OnHit(int impactX, int impactY, int shipX, int shipY, int shipSize, Dictionary<string, object> itemValues, Dictionary<string, object> particleValues)
		{
			return new Dictionary<string, object>[] { particleValues };
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
				case "basePowerRechargeRate": return AddValue("powerRechargeRate", float.Parse((string)value.Value), values);
				case "basePowerCapacity": return AddValue("powerCapacity", float.Parse((string)value.Value), values);
				case "baseRange": return AddValue("range", float.Parse((string)value.Value), values);
				case "baseSpaceMultiplier": return MultiplyValue("space", float.Parse((string)value.Value), values);
				case "baseCostMultiplier": return MultiplyValue("cost", float.Parse((string)value.Value), values);
				case "basePowerRechargeRateMultiplier": return MultiplyValue("powerRechargeRate", float.Parse((string)value.Value), values);
				case "basePowerCapacityMultiplier": return MultiplyValue("powerCapacity", float.Parse((string)value.Value), values);
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
			if (equipmentValues.ContainsKey("powerCapacity"))
			{
				shipValues["maxPower"] = (float)shipValues["maxPower"] + (float)equipmentValues["powerCapacity"];
			}
			if (equipmentValues.ContainsKey("powerRechargeRate"))
			{
				shipValues["powerRechargeRate"] = (float)shipValues["powerRechargeRate"] + (float)equipmentValues["powerRechargeRate"];
			}
			if (equipmentValues.ContainsKey("space"))
			{
				shipValues["space"] = (float)shipValues["space"] + (float)equipmentValues["space"];
			}
			if (equipmentValues.ContainsKey("cost"))
			{
				shipValues["cost"] = (float)shipValues["cost"] + (float)equipmentValues["cost"];
			}
			return shipValues;
		}
	}
}