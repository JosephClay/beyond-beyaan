using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorgonLibrary.InputDevices;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class GroundCombatScreen : WindowInterface
	{
		private class GroundUnit
		{
			public Point Location;
			public Race race;
			public GorgonLibrary.Graphics.Sprite sprite;
			public GorgonLibrary.Graphics.Sprite dyingSprite;
			public bool dying;
		}

		public delegate void DoneFunction();
		private DoneFunction doneFunction;

		private List<GroundUnit> defendingUnits;
		private List<GroundUnit> attackingUnits;

		private float landing;
		private float tickTilNextUnit;
		private float tickShowingEffect;
		private bool showingVictory;

		private int x;
		private int y;

		private Planet planetBeingInvaded;
		private Empire empireInvading;

		public GroundCombatScreen(int centerX, int centerY, GameMain gameMain, DoneFunction doneFunction)
			: base(centerX, centerY, 10, 10, string.Empty, gameMain, false)
		{
			this.gameMain = gameMain;
			this.doneFunction = doneFunction;

			backGroundImage = new StretchableImage(gameMain.ScreenWidth / 2 - 430, gameMain.ScreenHeight / 2 - 330, 860, 660, 200, 200, DrawingManagement.ScreenBorder);

			x = gameMain.ScreenWidth / 2 - 400;
			y = gameMain.ScreenHeight / 2 - 300;
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			base.DrawWindow(drawingManagement);

			planetBeingInvaded.PlanetType.GroundView.SetPosition(x, y);
			planetBeingInvaded.PlanetType.GroundView.Draw();

			//GorgonLibrary.Graphics.Sprite city = planetBeingInvaded.Owner.EmpireRace.GetCity();
			//city.SetPosition(x + 50, y + 400);
			//city.Draw();

			GorgonLibrary.Graphics.Sprite troopShip = empireInvading.EmpireRace.GetTroopShip();
			troopShip.SetPosition(x + 450, (int)(y + 400 - landing));
			troopShip.Draw();

			if (landing == 0)
			{
				foreach (GroundUnit unit in defendingUnits)
				{
					if (unit.dying)
					{
						unit.dyingSprite.HorizontalFlip = true;
						unit.dyingSprite.SetPosition(unit.Location.X, unit.Location.Y);
						unit.dyingSprite.Draw();
					}
					else
					{
						unit.sprite.HorizontalFlip = true;
						unit.sprite.SetPosition(unit.Location.X, unit.Location.Y);
						unit.sprite.Draw();
					}
				}
				foreach (GroundUnit unit in attackingUnits)
				{
					if (unit.dying)
					{
						unit.dyingSprite.HorizontalFlip = false;
						unit.dyingSprite.SetPosition(unit.Location.X, unit.Location.Y);
						unit.dyingSprite.Draw();
					}
					else
					{
						unit.sprite.HorizontalFlip = false;
						unit.sprite.SetPosition(unit.Location.X, unit.Location.Y);
						unit.sprite.Draw();
					}
				}
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			if (landing > 0)
			{
				landing -= frameDeltaTime * 100;
				if (landing < 0)
				{
					landing = 0;
				}
			}
			else
			{
				if (showingVictory)
				{
					return true;
				}
				if (tickTilNextUnit < 0.1f)
				{
					tickTilNextUnit += frameDeltaTime;
				}
				else if (tickShowingEffect < 0)
				{
					tickShowingEffect = 0;
					if (defendingUnits.Count == 0)
					{
						return true;
					}
					Random r = new Random();
					int result = r.Next(200);
					if (result < 100)
					{
						//defending unit lost
						defendingUnits[0].dying = true;
					}
					else
					{
						//attacking unit lost
						attackingUnits[0].dying = true;
					}
				}
				else
				{
					tickShowingEffect += frameDeltaTime;
					if (tickShowingEffect >= 0.25)
					{
						tickShowingEffect = -1.0f;
						tickTilNextUnit = 0;
						if (defendingUnits.Count == 0)
						{
							//Defenseless
							showingVictory = true;
							return true;
						}
						if (defendingUnits[0].dying)
						{
							defendingUnits.Remove(defendingUnits[0]);
							if (defendingUnits.Count == 0)
							{
								showingVictory = true;
							}
						}
						else
						{
							attackingUnits.Remove(attackingUnits[0]);
							if (attackingUnits.Count == 0)
							{
								showingVictory = true;
							}
						}
					}
				}
			}
			return true;
		}

		public override bool MouseDown(int x, int y)
		{
			return true;
		}

		public override bool MouseUp(int x, int y)
		{
			if (landing > 0)
			{
				landing = 0;
			}
			else if (showingVictory)
			{
				//exit the screen
				planetBeingInvaded.RemoveAllRaces();
				if (defendingUnits.Count > 0)
				{
					foreach (GroundUnit unit in defendingUnits)
					{
						planetBeingInvaded.AddRacePopulation(unit.race, 1);
					}
				}
				else
				{
					foreach (GroundUnit unit in attackingUnits)
					{
						planetBeingInvaded.AddRacePopulation(unit.race, 1);
					}
					//planetBeingInvaded.Owner.PlanetManager.Planets.Remove(planetBeingInvaded);
					//planetBeingInvaded.Owner = empireInvading;
					empireInvading.PlanetManager.Planets.Add(planetBeingInvaded);
					//planetBeingInvaded.SetMinimumFoodAndWaste();
					planetBeingInvaded.System.UpdateOwners();
				}
				doneFunction();
			}
			return true;
		}

		public void LoadScreen(Planet planetBeingInvaded, Empire invadingEmpire, Dictionary<Race, int> invaders)
		{
			/*if (planetBeingInvaded.Owner == invadingEmpire)
			{
				//Just add population since it's the same empire
				foreach (KeyValuePair<Race, int> race in invaders)
				{
					planetBeingInvaded.AddRacePopulation(race.Key, race.Value);
				}
				doneFunction();
			}*/

			this.planetBeingInvaded = planetBeingInvaded;
			this.empireInvading = invadingEmpire;

			//It's an actual invasion!
			landing = 400;
			showingVictory = false;
			defendingUnits = new List<GroundUnit>();
			attackingUnits = new List<GroundUnit>();

			int i = 0;
			int j = 0;

			foreach (Race race in planetBeingInvaded.Races)
			{
				for (int k = 0; k < (int)planetBeingInvaded.GetRacePopulation(race); k++)
				{
					GroundUnit unit = new GroundUnit();
					unit.race = race;
					unit.sprite = unit.race.GetGroundUnit();
					unit.dyingSprite = unit.race.GetGroundUnitDying();
					unit.Location = new Point(x + (352 - (16 * i)), y + 550 - (j * 32) + (i % 2 == 1 ? 16 : 0));
					i++;
					if (i >= 21)
					{
						i = 0;
						j++;
					}
					defendingUnits.Add(unit);
				}
			}

			i = 0;
			j = 0;
			foreach (KeyValuePair<Race, int> race in invaders)
			{
				for (int k = 0; k < race.Value; k++)
				{
					GroundUnit unit = new GroundUnit();
					unit.race = race.Key;
					unit.sprite = unit.race.GetGroundUnit();
					unit.dyingSprite = unit.race.GetGroundUnitDying();
					unit.Location = new Point(x + 398 + (16 * i), y + 550 - (j * 32) + (i % 2 == 1 ? 16 : 0));
					i++;
					if (i >= 21)
					{
						i = 0;
						j++;
					}
					attackingUnits.Add(unit);
				}
			}
			tickShowingEffect = -1.0f;
			tickTilNextUnit = 0.0f;
		}
	}
}
