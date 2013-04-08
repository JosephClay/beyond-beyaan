using System;
using System.Collections.Generic;
using System.Text;

namespace Beyond_Beyaan
{
	public class BasicInstantBeamParticle
	{
		public Dictionary<string, object> Spawn(Dictionary<string, object> itemValues)
		{
			float length = (int)itemValues["MaxX"] + (int)itemValues["MaxY"];

			itemValues.Add("PosX", (float)itemValues["OrigX"]);
			itemValues.Add("PosY", (float)itemValues["OrigY"]);
			itemValues.Add("LastX", (float)itemValues["OrigX"]);
			itemValues.Add("LastY", (float)itemValues["OrigY"]);
			itemValues.Add("Width", length);
			itemValues.Add("Height", 4.0f);
			return itemValues;
		}

		public Dictionary<string, object>[] Update(Dictionary<string, object> itemValues, float frameDeltaTime)
		{
			float angle = (float)itemValues["Angle"];
			float lifeSpan = (float)itemValues["LifeSpan"];
			lifeSpan -= frameDeltaTime;
			itemValues["LifeSpan"] = lifeSpan;

			if (lifeSpan <= 0)
			{
				itemValues.Add("RemoveParticle", true);
			}

			return new Dictionary<string, object>[] { itemValues };
		}

		public Dictionary<string, object>[] PostHit(Dictionary<string, object> newValues, int impactX, int impactY)
		{
			Dictionary<string, object> result = new Dictionary<string, object>(newValues);
			if ((!newValues.ContainsKey("damage") || (float)newValues["damage"] <= 0) && !result.ContainsKey("StopProcessing")) //Beam got terminated at this impact
			{
				//End the beam
				float x = (float)newValues["PosX"];
				float y = (float)newValues["PosY"];

				float width = (float)(Math.Sqrt((x - impactX) * (x - impactX) + (y - impactY) * (y - impactY))) - 1;
				result["Width"] = width;
				result.Add("StopProcessing", true);
			}
			return new Dictionary<string, object>[] { result };
		}

	}
}