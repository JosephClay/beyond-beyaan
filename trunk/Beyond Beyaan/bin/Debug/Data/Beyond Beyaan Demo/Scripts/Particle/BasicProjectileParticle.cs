using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class BasicProjectileParticle
	{
		public Dictionary<string, object> Spawn(Dictionary<string, object> itemValues)
		{
			itemValues.Add("PosX", (float)itemValues["OrigX"]);
			itemValues.Add("PosY", (float)itemValues["OrigY"]);
			itemValues.Add("EndX", (float)itemValues["OrigX"]);
			itemValues.Add("EndY", (float)itemValues["OrigY"]);
			itemValues.Add("LastX", (float)itemValues["OrigX"]);
			itemValues.Add("LastY", (float)itemValues["OrigY"]);
			itemValues.Add("Width", 1.0f);
			itemValues.Add("Height", 4.0f);
			itemValues.Add("Speed", float.Parse((string)itemValues["speed"], System.Globalization.CultureInfo.InvariantCulture));

			return itemValues;
		}

		public Dictionary<string, object>[] Update(Dictionary<string, object> itemValues, float frameDeltaTime)
		{
			float angle = (float)itemValues["Angle"];

			float speed = (float)itemValues["Speed"];
			float oldX = (float)itemValues["PosX"];
			float oldY = (float)itemValues["PosY"];
			float spawnLength = (float)itemValues["SpawnLength"];
			if (spawnLength > 0)
			{
				spawnLength -= frameDeltaTime;
				itemValues["SpawnLength"] = spawnLength;
			}
			else
			{
				itemValues["PosX"] = (float)itemValues["PosX"] + (float)(Math.Cos(angle) * speed * frameDeltaTime);
				itemValues["PosY"] = (float)itemValues["PosY"] + (float)(Math.Sin(angle) * speed * frameDeltaTime);
				itemValues["LastX"] = itemValues["PosX"];
				itemValues["LastY"] = itemValues["PosY"];
			}

			if (!itemValues.ContainsKey("ImpactX"))
			{
				itemValues["EndX"] = (float)itemValues["EndX"] + (float)(Math.Cos(angle) * speed * frameDeltaTime);
				itemValues["EndY"] = (float)itemValues["EndY"] + (float)(Math.Sin(angle) * speed * frameDeltaTime);
			}
			else
			{
				if ((oldX < (float)itemValues["EndX"] && (float)itemValues["PosX"] >= (float)itemValues["EndX"]) ||
					(oldX > (float)itemValues["EndX"] && (float)itemValues["PosX"] <= (float)itemValues["EndX"]) ||
					(oldY < (float)itemValues["EndY"] && (float)itemValues["PosY"] >= (float)itemValues["EndY"]) ||
					(oldY > (float)itemValues["EndY"] && (float)itemValues["PosY"] <= (float)itemValues["EndY"]))
				{
					//It crossed the impact point, remove
					itemValues.Add("RemoveParticle", true);
				}
			}

			float x = (float)itemValues["EndX"] - (float)itemValues["PosX"];
			float y = (float)itemValues["EndY"] - (float)itemValues["PosY"];

			itemValues["Width"] = (float)Math.Sqrt((x * x) + (y * y));			
			
			return new Dictionary<string, object>[] { itemValues };
		}

		public Dictionary<string, object>[] PostHit(Dictionary<string, object> newValues, int impactX, int impactY)
		{
			Dictionary<string, object> result = new Dictionary<string, object>(newValues);
			if ((!newValues.ContainsKey("damage") || (float)newValues["damage"] <= 0) && !result.ContainsKey("ImpactX")) //Beam got terminated at this impact
			{
				//End the beam
				result.Add("ImpactX", impactX);
				result.Add("ImpactY", impactY);				
			}
			return new Dictionary<string, object>[] { result };
		}

	}
}