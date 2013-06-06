using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan
{
	class Technology
	{
		protected int baseResearchCost; //Base cost of researching an item
		protected float increaseRate; //How much cost increases to research the next level
		protected int levelLimit; //How many times this item can be researched, -1 means infinite
		protected string name; //Name of technology
		protected int startingLevel; //What level of technology this tech starts off (1 for having the tech researched, 0 for not researched, 2 or higher for base plus incremental effects)
		protected int currentLevel;
		protected float totalResearch; //How much points spent into researching this/next level
		protected int requiredFieldLevel; //How high this tech field must be reached before this tech is visible
		protected float nextLevelCost; //How much points the next level will require to achieve

		public int GetTotalResearchPoints()
		{
			return (int)totalResearch;
		}
		public int GetNextLevelCost()
		{
			return (int)nextLevelCost;
		}
		public string GetName()
		{
			return name;
		}
		public string GetNameWithCurrentLevel()
		{
			if (currentLevel > 1)
			{
				return name + " " + Utility.ConvertNumberToRomanNumberical(currentLevel);
			}
			return name;
		}
		public string GetNameWithNextLevel()
		{
			if (currentLevel > 0 && (currentLevel < levelLimit || levelLimit < 0))
			{
				return name + " " + Utility.ConvertNumberToRomanNumberical(currentLevel + 1);
			}
			return GetNameWithCurrentLevel();
		}
	}

	class Beam : Technology
	{
		private int baseDamage;
		private int accuracy;
		private int space;
		private int cost;
		public Beam()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["accuracy"], out accuracy))
			{
				reason = "accuracy value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"accuracy",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetAccuracy()
		{
			//this will factor in tech level increase
			return accuracy;
		}

		public int GetSpace()
		{
			//this will factor in tech level increase
			return space;
		}
		public int GetCost()
		{
			//this will factor in tech level increase
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Particle : Technology
	{
		private int baseDamage;
		private int accuracy;
		private int space;
		private int cost;
		public Particle()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["accuracy"], out accuracy))
			{
				reason = "accuracy value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"accuracy",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetAccuracy()
		{
			//this will factor in tech level increase
			return accuracy;
		}

		public int GetSpace()
		{
			//this will factor in tech level increase
			return space;
		}

		public int GetCost()
		{
			//this will factor in tech level increase
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Torpedo : Technology
	{
		private int baseDamage;
		private int space;
		private int cost;
		public Torpedo()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetSpace()
		{
			//this will factor in tech level increase
			return space;
		}

		public int GetCost()
		{
			//this will factor in tech level increase
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Missile : Technology
	{
		private int baseDamage;
		private int space;
		private int cost;
		public Missile()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetSpace()
		{
			//this will factor in tech level increase
			return space;
		}

		public int GetCost()
		{
			//this will factor in tech level increase
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Bomb : Technology
	{
		private int baseDamage;
		private int space;
		private int cost;
		public Bomb()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["basedamage"], out baseDamage))
			{
				reason = "basedamage value is not integer";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"basedamage",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetDamage()
		{
			//this will factor in tech level increase
			return baseDamage;
		}

		public int GetSpace()
		{
			return space;
		}

		public int GetCost()
		{
			return cost;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Armor : Technology
	{
		public int beamEfficiency { get; private set; }
		public int particleEfficiency { get; private set; }
		public int missileEfficiency { get; private set; }
		public int torpedoEfficiency { get; private set; }
		public int bombEfficiency { get; private set; }
		private int space;
		private int cost;
		private float baseHP;

		public Armor()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			int result;
			if (!int.TryParse(items["beameff"], out result))
			{
				reason = "beameff value is not int";
				return false;
			}
			beamEfficiency = result;
			if (!int.TryParse(items["particleeff"], out result))
			{
				reason = "particleeff value is not int";
				return false;
			}
			particleEfficiency = result;
			if (!int.TryParse(items["missileeff"], out result))
			{
				reason = "missileeff value is not int";
				return false;
			}
			missileEfficiency = result;
			if (!int.TryParse(items["torpedoeff"], out result))
			{
				reason = "torpedoeff value is not int";
				return false;
			}
			torpedoEfficiency = result;
			if (!int.TryParse(items["bombeff"], out result))
			{
				reason = "bombeff value is not int";
				return false;
			}
			bombEfficiency = result;
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not int";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!float.TryParse(items["basehp"], out baseHP))
			{
				reason = "basehp value is not float";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"beameff",
				"particleeff",
				"missileeff",
				"torpedoeff",
				"bombeff",
				"space",
				"cost",
				"basehp",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetHP(int size)
		{
			return (int)(size * (baseHP / 100.0f));
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Shield : Technology
	{
		public int beamEfficiency { get; private set; }
		public int particleEfficiency { get; private set; }
		public int missileEfficiency { get; private set; }
		public int torpedoEfficiency { get; private set; }
		public int bombEfficiency { get; private set; }
		private int space;
		private int cost;
		private int baseResistance;

		public Shield()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			int result;
			if (!int.TryParse(items["beameff"], out result))
			{
				reason = "beameff value is not int";
				return false;
			}
			beamEfficiency = result;
			if (!int.TryParse(items["particleeff"], out result))
			{
				reason = "particleeff value is not int";
				return false;
			}
			particleEfficiency = result;
			if (!int.TryParse(items["missileeff"], out result))
			{
				reason = "missileeff value is not int";
				return false;
			}
			missileEfficiency = result;
			if (!int.TryParse(items["torpedoeff"], out result))
			{
				reason = "torpedoeff value is not int";
				return false;
			}
			torpedoEfficiency = result;
			if (!int.TryParse(items["bombeff"], out result))
			{
				reason = "bombeff value is not int";
				return false;
			}
			bombEfficiency = result;
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["baseresistance"], out baseResistance))
			{
				reason = "baseresistance value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"beameff",
				"particleeff",
				"missileeff",
				"torpedoeff",
				"bombeff",
				"space",
				"cost",
				"baseresistance",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetResistance()
		{
			return baseResistance;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Computer : Technology
	{
		public int beamEfficiency { get; private set; }
		public int particleEfficiency { get; private set; }
		public int missileEfficiency { get; private set; }
		public int torpedoEfficiency { get; private set; }
		public int bombEfficiency { get; private set; }
		private int space;
		private int cost;

		public Computer()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			int result;
			if (!int.TryParse(items["beameff"], out result))
			{
				reason = "beameff value is not int";
				return false;
			}
			beamEfficiency = result;
			if (!int.TryParse(items["particleeff"], out result))
			{
				reason = "particleeff value is not int";
				return false;
			}
			particleEfficiency = result;
			if (!int.TryParse(items["missileeff"], out result))
			{
				reason = "missileeff value is not int";
				return false;
			}
			missileEfficiency = result;
			if (!int.TryParse(items["torpedoeff"], out result))
			{
				reason = "torpedoeff value is not int";
				return false;
			}
			torpedoEfficiency = result;
			if (!int.TryParse(items["bombeff"], out result))
			{
				reason = "bombeff value is not int";
				return false;
			}
			bombEfficiency = result;
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not int";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"beameff",
				"particleeff",
				"missileeff",
				"torpedoeff",
				"bombeff",
				"space",
				"cost",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Engine : Technology
	{
		private int space;
		private int cost;
		private int baseGalaxySpeed;
		private int baseCombatSpeed;

		public Engine()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["space"], out space))
			{
				reason = "space value is not integer";
				return false;
			}
			if (!int.TryParse(items["cost"], out cost))
			{
				reason = "cost value is not integer";
				return false;
			}
			if (!int.TryParse(items["basegalaxyspeed"], out baseGalaxySpeed))
			{
				reason = "basegalaxyspeed value is not integer";
				return false;
			}
			if (!int.TryParse(items["basecombatspeed"], out baseCombatSpeed))
			{
				reason = "basecombatspeed value is not integer";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"space",
				"cost",
				"basegalaxyspeed",
				"basecombatspeed",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetSpace(int size)
		{
			return (int)(size * (space / 100.0f));
		}

		public int GetCost(int size)
		{
			//this will factor in tech level increase
			return (int)(size * (cost / 100.0f));
		}

		public int GetGalaxySpeed()
		{
			return baseGalaxySpeed;
		}

		public int GetCombatSpeed()
		{
			return baseCombatSpeed;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}

	class Infrastructure : Technology
	{
		public Infrastructure()
		{
		}

		public bool Load(Dictionary<string, string> items, out string reason)
		{
			if (!VerifyItems(items, out reason))
			{
				return false;
			}

			name = items["techname"];
			if (!int.TryParse(items["baseresearchcost"], out baseResearchCost))
			{
				reason = "baseResearchCost value is not integer";
				return false;
			}
			if (!int.TryParse(items["techlevellimit"], out levelLimit))
			{
				reason = "techlevellimit value is not integer";
				return false;
			}
			if (!float.TryParse(items["researchexponentincrease"], out increaseRate))
			{
				reason = "researchexponentincrease value is not float";
				return false;
			}
			if (!int.TryParse(items["startlevel"], out startingLevel))
			{
				reason = "startlevel value is not integer";
				return false;
			}
			if (!int.TryParse(items["requiredfieldlevel"], out requiredFieldLevel))
			{
				reason = "requiredfieldlevel value is not integer";
				return false;
			}
			currentLevel = startingLevel;
			totalResearch = 0;
			nextLevelCost = baseResearchCost;

			return true;
		}

		private bool VerifyItems(Dictionary<string, string> items, out string reason)
		{
			string[] essentialTags = new string[] 
			{
				"techname",
				"baseresearchcost",
				"researchexponentincrease",
				"techlevellimit",
				"startlevel",
				"requiredfieldlevel",
			};
			foreach (string tag in essentialTags)
			{
				if (!items.ContainsKey(tag))
				{
					reason = "Missing " + tag + " tag";
					return false;
				}
			}
			reason = null;
			return true;
		}

		public int GetLevel()
		{
			return currentLevel;
		}

		public int GetRequiredLevel()
		{
			return requiredFieldLevel;
		}

		public bool UpdateResearch(float researchPoints)
		{
			totalResearch += researchPoints;
			if (totalResearch >= nextLevelCost)
			{
				totalResearch = 0;
				nextLevelCost = nextLevelCost *= increaseRate;
				currentLevel++;
				return true;
			}
			return false;
		}
	}
}
