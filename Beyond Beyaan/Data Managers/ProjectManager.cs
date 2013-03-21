using System;
using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class ProjectManager
	{
		public List<Project> Projects { get; private set; }
		public List<int> PercentageAmounts { get; private set; }
		public List<bool> Locked { get; private set; }
		public bool IsQueue { get; private set; }

		public ProjectManager(bool isQueue)
		{
			Projects = new List<Project>();
			PercentageAmounts = new List<int>();
			Locked = new List<bool>();
			this.IsQueue = isQueue;
		}

		public void AddProject(Project project)
		{
			Projects.Add(project);
			PercentageAmounts.Add(Projects.Count == 1 ? 100 : 0);
			Locked.Add(false);
		}

		public void CancelProject(int project)
		{
			bool added = false;
			if (!IsQueue)
			{
				for (int i = 0; i < Projects.Count; i++)
				{
					if (!Locked[i] && i != project)
					{
						PercentageAmounts[i] += PercentageAmounts[project];
						added = true;
						break;
					}
				}
				if (!added)
				{
					//All projects are locked or no projects
					if (Projects.Count > 0)
					{
						PercentageAmounts[0] += PercentageAmounts[project];
					}
				}
				PercentageAmounts.RemoveAt(project);
				Locked.RemoveAt(project);
				Projects.RemoveAt(project);
			}
			Projects.RemoveAt(project);
		}

		public void UpdateProjects(Dictionary<Resource, float> amount, PlanetTypeManager planetTypeManager, Random r)
		{
			List<Project> projectsCompleted = new List<Project>();
			for (int i = 0; i < Projects.Count; i++)
			{
				int amountProduced = 0;
				if (IsQueue)
				{
					if (i == 0)
					{
						amountProduced = Projects[i].Update(amount, 1.0f);
					}
				}
				else
				{
					amountProduced = Projects[i].Update(amount, PercentageAmounts[i] / 100f);
				}
				if (amountProduced > 0)
				{
					switch (Projects[i].ProjectType)
					{
						case PROJECT_TYPE.REGION:
							Projects[i].WhichRegion.RegionType = Projects[i].RegionTypeToBuild;
							projectsCompleted.Add(Projects[i]);
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
				Projects.Remove(projectsCompleted[i]);
				// TODO: Add to sitrep whatever projects were completed.
				//empire.SitRepManager.AddItem(new SitRepItem(Screen.Galaxy, projectsCompleted[i].Location, projectsCompleted[i].PlanetToTerraform, new Point(projectsCompleted[i].Location.X, projectsCompleted[i].Location.Y), projectsCompleted[i].PlanetToTerraform.Name + " is fully terraformed, this project is completed."));
			}
		}

		public void SetLocked(int project, bool locked)
		{
			this.Locked[project] = locked;
		}

		public void SetPercentage(int project, int amount)
		{
			int remainingPercentile = 100;
			for (int i = 0; i < Projects.Count; i++)
			{
				if (Locked[i])
				{
					remainingPercentile -= PercentageAmounts[i];
				}
			}

			if (amount >= remainingPercentile)
			{
				for (int i = 0; i < Locked.Count; i++)
				{
					if (!Locked[i])
					{
						PercentageAmounts[i] = 0;
					}
				}
				amount = remainingPercentile;
			}

			//Now scale

			int totalPointsExcludingSelectedProject;
			PercentageAmounts[project] = amount;
			remainingPercentile -= PercentageAmounts[project];
			totalPointsExcludingSelectedProject = GetTotalPercentageExcludingProjectAndLocked(project);

			if (remainingPercentile < totalPointsExcludingSelectedProject)
			{
				int amountToDeduct = totalPointsExcludingSelectedProject - remainingPercentile;
				for (int i = 0; i < Locked.Count; i++)
				{
					if (amountToDeduct <= 0)
					{
						break;
					}
					if (!Locked[i] && project != i)
					{
						int prevValue = PercentageAmounts[i];
						PercentageAmounts[i] -= PercentageAmounts[i] >= amountToDeduct ? amountToDeduct : PercentageAmounts[i];
						amountToDeduct -= (prevValue - PercentageAmounts[i]);
					}
				}
			}

			if (remainingPercentile > totalPointsExcludingSelectedProject)
			{
				int amountToAdd = remainingPercentile - totalPointsExcludingSelectedProject;
				for (int i = 0; i < Locked.Count; i++)
				{
					if (amountToAdd <= 0)
					{
						break;
					}
					if (!Locked[i] && project != i)
					{
						PercentageAmounts[i] += amountToAdd;
						amountToAdd = 0;
					}
				}
				if (amountToAdd > 0)
				{
					//All projects are already checked, so have to add teh remaining back to the current project
					PercentageAmounts[project] += amountToAdd;
				}
			}
		}

		private int GetTotalPercentageExcludingProjectAndLocked(int whichProject)
		{
			int total = 0;
			for (int i = 0; i < PercentageAmounts.Count; i++)
			{
				if (!Locked[i] && i != whichProject)
				{
					total += PercentageAmounts[i];
				}
			}
			return total;
		}
	}
}
