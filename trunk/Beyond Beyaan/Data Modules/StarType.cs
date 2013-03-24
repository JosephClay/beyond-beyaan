using System;
using System.Collections.Generic;

namespace Beyond_Beyaan.Data_Modules
{
	public class StarType
	{
		#region Graphics
		public float[] ShaderValue { get; set; }
		public GorgonLibrary.Graphics.FXShader Shader { get; set; }
		public string SpriteName { get; set; }
		#endregion

		#region Star Properties
		public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public Dictionary<string, int> MaxAmountForSectorType { get; set; }
		public Dictionary<SectorObjectType, string> SectorObjectTypesInSystem { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		#endregion

		public StarType()
		{
			MaxAmountForSectorType = new Dictionary<string, int>();
			SectorObjectTypesInSystem = new Dictionary<SectorObjectType, string>();
		}

		//This function generates a new star (or other galactic objects), complete with sector objects
		public List<SectorObjectType> GenerateSectorObjects(Random r)
		{
			List<SectorObjectType> typesToUse = new List<SectorObjectType>();
			Dictionary<SectorObjectType, int> amounts = new Dictionary<SectorObjectType, int>();
			Dictionary<string, int> amountAdded = new Dictionary<string, int>(); //To ensure we fall below the limit for this type (i.e. only 5 planets, or 2 starlanes max)

			List<SectorObjectType> typesToReturn = new List<SectorObjectType>();

			foreach (var sectorObjectType in SectorObjectTypesInSystem)
			{
				int amount = Utility.GetIntValue(sectorObjectType.Value, r);
				if (amount > 0)
				{
					amounts.Add(sectorObjectType.Key, amount);
					typesToUse.Add(sectorObjectType.Key); //For ease of randomization
				}
			}
			while (typesToUse.Count > 0)
			{
				//Add the next random object
				SectorObjectType type = typesToUse[r.Next(typesToUse.Count)];
				if (MaxAmountForSectorType.ContainsKey(type.Type) && amountAdded.ContainsKey(type.Type) && amountAdded[type.Type] >= MaxAmountForSectorType[type.Type])
				{
					//We've reached the limit for this type of sector objects
					typesToUse.Remove(type);
					continue;
				}
				typesToReturn.Add(type);
				amounts[type] -= 1;
				if (amounts[type] == 0)
				{
					//We've reached the limit for this particular sector object
					typesToUse.Remove(type);
				}
				if (amountAdded.ContainsKey(type.Type))
				{
					amountAdded[type.Type] += 1;
				}
				else
				{
					amountAdded[type.Type] = 1;
				}
			}
			return typesToReturn;
		}
	}
}
