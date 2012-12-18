using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	//This is for storing a list of systems with planets owned by current empire
	public class StarSystemManager
	{
		//This will always be sorted based on number of population per system
		public List<StarSystem> StarSystems { get; private set; }
		private Empire empire;

		public StarSystemManager(Empire empire)
		{
			StarSystems = new List<StarSystem>();
			this.empire = empire;
		}

		public void SortSystems()
		{
			StarSystems.Sort((a, b) => (b.GetPopulation(empire).CompareTo(a.GetPopulation(empire))));
		}

		public void AddSystem(StarSystem starSystem)
		{
			if (!StarSystems.Contains(starSystem))
			{
				StarSystems.Add(starSystem);
				SortSystems();
			}
		}

		public void RemoveSystem(StarSystem starSystem)
		{
			if (StarSystems.Contains(starSystem))
			{
				StarSystems.Remove(starSystem);
				SortSystems();
			}
		}

		public void TallyConsumptions(out Dictionary<Resource, float> consumptions)
		{
			consumptions = new Dictionary<Resource, float>();
			foreach (StarSystem starSystem in StarSystems)
			{
				starSystem.TallyConsumptions(empire, consumptions);
			}
		}

		public void TallyResources(out Dictionary<Resource, float> resources)
		{
			resources = new Dictionary<Resource, float>();
			foreach (StarSystem starSystem in StarSystems)
			{
				starSystem.TallyResources(empire, resources);
			}
		}

		public void TallyAvailableResourcesAndShortages(out Dictionary<Resource, float> availableResources, out Dictionary<Resource, float> shortages)
		{
			availableResources = new Dictionary<Resource, float>();
			shortages = new Dictionary<Resource, float>();
			foreach (StarSystem starSystem in StarSystems)
			{
				starSystem.TallyAvailableResourcesAndShortages(empire, availableResources, shortages);
			}
		}

		public void SetSharedResources(Dictionary<Resource, float> sharedAvailable, Dictionary<Resource, float> sharedConsumped)
		{
			foreach (StarSystem starSystem in StarSystems)
			{
				starSystem.SetSharedResources(empire, sharedAvailable, sharedConsumped);
			}
		}

		public void CalculatePopGrowth()
		{
			foreach (StarSystem starSystem in StarSystems)
			{
				starSystem.CalculatePopGrowth(empire);
			}
		}

		public void TallyProductions(out Dictionary<Resource, float> productions)
		{
			productions = new Dictionary<Resource, float>();
			foreach (StarSystem starSystem in StarSystems)
			{
				starSystem.TallyProductions(empire, productions);
			}
		}
	}
}
