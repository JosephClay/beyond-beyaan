using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class ProjectManager
	{
		private Empire empire;
		private List<Project> projects;
		private List<int> percentageAmounts;
		private List<bool> locked;
		private bool isQueue; //If planet, it's queue, otherwise, apply development to all projects

		public List<Project> Projects { get { return projects; } }
		public List<int> PercentageAmounts { get { return percentageAmounts; } }
		public List<bool> Locked { get { return locked; } }
		public bool IsQueue { get { return isQueue; } }

		public ProjectManager(Empire empire, bool isQueue)
		{
			projects = new List<Project>();
			percentageAmounts = new List<int>();
			locked = new List<bool>();
			this.empire = empire;
			this.isQueue = isQueue;
		}

		public void AddProject(Project project)
		{
			projects.Add(project);
			percentageAmounts.Add(projects.Count == 1 ? 100 : 0);
			locked.Add(false);
		}

		public void CancelProject(int project)
		{
			bool added = false;
			for (int i = 0; i < projects.Count; i++)
			{
				if (!locked[i] && i != project)
				{
					percentageAmounts[i] += percentageAmounts[project];
					added = true;
					break;
				}
			}
			if (!added)
			{
				//All projects are locked or no projects
				if (projects.Count > 0)
				{
					percentageAmounts[0] += percentageAmounts[project];
				}
			}
			percentageAmounts.RemoveAt(project);
			locked.RemoveAt(project);
			projects.RemoveAt(project);
		}

		public void UpdateProjects(Dictionary<Resource, float> amount, PlanetTypeManager planetTypeManager, Random r)
		{
			List<Project> projectsCompleted = new List<Project>();
			for (int i = 0; i < projects.Count; i++)
			{
				int amountProduced = projects[i].Update(amount, percentageAmounts[i]);
				if (amountProduced > 0)
				{
					switch (projects[i].ProjectType)
					{
						case PROJECT_TYPE.REGION:
							projects[i].WhichRegion.RegionType = projects[i].RegionTypeToBuild;
							projectsCompleted.Add(projects[i]);
							break;
						case PROJECT_TYPE.SHIP:
							//TODO: Finish this
							//projects[i].WhichSystem.ShipReserves[empire].Add(ships)
							break;
					}

					/*if (projects[i].ShipToBuild != null)
					{
						//Built ship
						Squadron newFleet = new Squadron(projects[i].Location);
						for (int j = 0; j < amountProduced; j++)
						{
							newFleet.AddShipFromDesign(projects[i].ShipToBuild);
						}
						newFleet.Empire = empire;
						empire.FleetManager.AddFleet(newFleet);
						empire.SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, projects[i].Location, null, new Point(projects[i].Location.X, projects[i].Location.Y), "Your empire has produced " + amountProduced + " " + projects[i].ShipToBuild.Name + " at " + projects[i].Location.Name));
					}
					else
					{
						empire.SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, projects[i].Location, projects[i].PlanetToTerraform, new Point(projects[i].Location.X, projects[i].Location.Y), "Your empire has terraformed " + projects[i].PlanetToTerraform.Name + " to " + projects[i].PlanetToTerraform.PlanetType.Name + " in " + projects[i].Location.Name + " system"));
					}*/
				}
			}
			for (int i = 0; i < projectsCompleted.Count; i++)
			{
				//Projects that are done, such as terraforming planets, need to be removed
				projects.Remove(projectsCompleted[i]);
				// TODO: Add to sitrep whatever projects were completed.
				//empire.SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, projectsCompleted[i].Location, projectsCompleted[i].PlanetToTerraform, new Point(projectsCompleted[i].Location.X, projectsCompleted[i].Location.Y), projectsCompleted[i].PlanetToTerraform.Name + " is fully terraformed, this project is completed."));
			}
		}

		public void SetLocked(int project, bool locked)
		{
			this.locked[project] = locked;
		}

		public void SetPercentage(int project, int amount)
		{
			int remainingPercentile = 100;
			for (int i = 0; i < projects.Count; i++)
			{
				if (locked[i])
				{
					remainingPercentile -= percentageAmounts[i];
				}
			}

			if (amount >= remainingPercentile)
			{
				for (int i = 0; i < locked.Count; i++)
				{
					if (!locked[i])
					{
						percentageAmounts[i] = 0;
					}
				}
				amount = remainingPercentile;
			}

			//Now scale

			int totalPointsExcludingSelectedProject = 0;
			percentageAmounts[project] = amount;
			remainingPercentile -= percentageAmounts[project];
			totalPointsExcludingSelectedProject = GetTotalPercentageExcludingProjectAndLocked(project);

			if (remainingPercentile < totalPointsExcludingSelectedProject)
			{
				int amountToDeduct = totalPointsExcludingSelectedProject - remainingPercentile;
				int prevValue;
				for (int i = 0; i < locked.Count; i++)
				{
					if (amountToDeduct <= 0)
					{
						break;
					}
					if (!locked[i] && project != i)
					{
						prevValue = percentageAmounts[i];
						percentageAmounts[i] -= percentageAmounts[i] >= amountToDeduct ? amountToDeduct : percentageAmounts[i];
						amountToDeduct -= (prevValue - percentageAmounts[i]);
					}
				}
			}

			if (remainingPercentile > totalPointsExcludingSelectedProject)
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedProject;
				for (int i = 0; i < locked.Count; i++)
				{
					if (amountToAdd <= 0)
					{
						break;
					}
					if (!locked[i] && project != i)
					{
						percentageAmounts[i] += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					//All projects are already checked, so have to add teh remaining back to the current project
					percentageAmounts[project] += amountToAdd;
				}
			}
		}

		private int GetTotalPercentageExcludingProjectAndLocked(int whichProject)
		{
			int total = 0;
			for (int i = 0; i < percentageAmounts.Count; i++)
			{
				if (!locked[i] && i != whichProject)
				{
					total += percentageAmounts[i];
				}
			}
			return total;
		}
	}
}
