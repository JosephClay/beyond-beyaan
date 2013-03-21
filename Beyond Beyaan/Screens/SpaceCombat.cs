using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Windows.Forms;
using Beyond_Beyaan.Data_Modules;
using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	class SpaceCombat : ScreenInterface
	{
		//const int COMBATSIZE = 200;
		const double HalfPI = Math.PI / 2;

		//bool loaded;
		private GameMain gameMain;
		private Camera camera;
		private BackgroundStars backgroundStars;

		private List<Empire> empires;
		private List<CombatFleet> fleetsInCombat;
		private CombatFleet currentFleetTurn;

		private Button[] actionButtons;
		/*private StretchableImage passiveSystemsBackground;
		private StretchableImage activeSystemsBackground;
		private StretchableImage statusBackground;*/

		private ScrollBar passiveScrollBar;
		private ScrollBar activeScrollBar;
		private int maxPassiveVisible;
		private int maxActiveVisible;

		private InvisibleStretchButton[] passiveButtons;
		private InvisibleStretchButton[] activeButtons;
		private Label[] passiveLabels;
		private Label[] activeLabels;
		//private Label[] passivePowerLabels;
		//private Label[] activePowerLabels;
		//private Label[] activeTimeLabels;
//		private ProgressBar[] passiveHealthBars;
//		private ProgressBar[] activeHealthBars;

		private StretchableImage nameBackground;

		private Label shipNameLabel;
//		private Label timeUnitsLabel;
//		private Label powerUnitsLabel;
		private Label empireNameLabel;

		private int size;
		private Point taskBarArea;
		/*StarSystem system;
		private int combatIter;
		private int whichEmpireTurn;
		private int selectedShipIter;
		//private List<CombatShip> retreatingShips;
		private bool processRetreating;
		private float retreatProcess;*/
		private ShipInstance selectedShip;
		private EquipmentInstance selectedEquipment;

		private List<EquipmentInstance> passiveEquipments;
		private List<EquipmentInstance> activeEquipments;

		private List<ParticleInstance> particles;
		private List<EffectInstance> tempEffects;

		private List<ParticleInstance> particlesToAdd;
		private List<ParticleInstance> particlesToRemove;
		private List<EffectInstance> effectsToAdd;
		private List<EffectInstance> effectsToRemove;

		private int selectionSize;
		private Point selectionPoint;
		private int mouseX;
		private int mouseY;

		private ShipInstance SelectedShip
		{
			get { return selectedShip; }
			set
			{
				actionButtons[0].Active = false;
				actionButtons[1].Active = false;
				actionButtons[3].Active = false;
				actionButtons[4].Active = false;
				selectedShip = value;
				SelectedEquipment = null;
				if (selectedShip == null)
				{
					return;
				}
				shipNameLabel.SetText(selectedShip.BaseShipDesign.Name);
				empireNameLabel.SetText(selectedShip.Owner.EmpireName);
				empireNameLabel.SetColor(selectedShip.Owner.EmpireColor);

				passiveEquipments = new List<EquipmentInstance>();
				activeEquipments = new List<EquipmentInstance>();

				bool controlsEnabled = selectedShip.Owner == currentFleetTurn.Empire;

				Dictionary<string, object> shipValues = selectedShip.BaseShipDesign.ShipClass.ShipScript.GetInformation(selectedShip.Values);

				foreach (Icon icon in selectedShip.BaseShipDesign.ShipClass.DisplayIcons)
				{
					icon.UpdateText(shipValues);
				}

				foreach (EquipmentInstance equipment in selectedShip.Equipments)
				{
					/*if (equipment.IsPassive())
					{
						passiveEquipments.Add(equipment);
					}
					else
					{
						activeEquipments.Add(equipment);
					}*/
				}
				if (passiveEquipments.Count > 6)
				{
					passiveScrollBar.SetEnabledState(true);
					passiveScrollBar.SetAmountOfItems(passiveEquipments.Count);
					passiveScrollBar.TopIndex = 0;
					maxPassiveVisible = 6;
				}
				else
				{
					passiveScrollBar.SetEnabledState(false);
					passiveScrollBar.SetAmountOfItems(6);
					passiveScrollBar.TopIndex = 0;
					maxPassiveVisible = passiveEquipments.Count;
				}
				if (activeEquipments.Count > 6)
				{
					activeScrollBar.SetEnabledState(true);
					activeScrollBar.SetAmountOfItems(activeEquipments.Count);
					activeScrollBar.TopIndex = 0;
					maxPassiveVisible = 6;
				}
				else
				{
					activeScrollBar.SetEnabledState(false);
					activeScrollBar.SetAmountOfItems(6);
					activeScrollBar.TopIndex = 0;
					maxActiveVisible = activeEquipments.Count;
				}
				for (int i = 0; i < maxPassiveVisible; i++)
				{
					passiveLabels[i].SetText(passiveEquipments[i].GetName());
					passiveButtons[i].Active = controlsEnabled;
					//long power = passiveEquipments[i].GetPower(selectedShip.BaseShipDesign.ShipClass.Size);
					//passivePowerLabels[i].SetText(power != 0 ? passiveEquipments[i].GetPower(selectedShip.Size) + " MW" : string.Empty);
				}
				for (int i = 0; i < maxActiveVisible; i++)
				{
					activeLabels[i].SetText(activeEquipments[i].GetName());
					activeButtons[i].Active = controlsEnabled;
					activeButtons[i].SetInfoTip(activeEquipments[i].GetName(), activeEquipments[i].CombatIcons, activeEquipments[i].GetEquipmentInfo(selectedShip.Values), DrawingManagement.BoxBorder, 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
					//activePowerLabels[i].SetText(activeEquipments[i].GetPower(selectedShip.Size) + " MW");
					//activeTimeLabels[i].SetText(activeEquipments[i].GetTime(selectedShip.Size) + " TU");
				}
				if (controlsEnabled)
				{
					actionButtons[0].Active = false;
					actionButtons[1].Active = false;
					actionButtons[3].Active = true;
					actionButtons[4].Active = true;
				}
			}
		}
		private EquipmentInstance SelectedEquipment
		{
			get { return selectedEquipment; }
			set
			{
				selectedEquipment = value;
				if (selectedEquipment == null)
				{
					selectionSize = -1;
					return;
				}
				selectionSize = selectedEquipment.GetSelectionSize(selectedShip.BaseShipDesign.ShipClass.Size);
			}
		}

		public void Initialize(GameMain gameMain)
		{
			this.gameMain = gameMain;
			camera = new Camera(this.gameMain.ScreenWidth, this.gameMain.ScreenHeight);

			int x = (gameMain.ScreenWidth / 2) - 140;
			int y = gameMain.ScreenHeight - 40;
			actionButtons = new Button[7];
			actionButtons[0] = new Button(SpriteName.BattleAutoBG, SpriteName.BattleAutoFG, string.Empty, x, y, 40, 40);
			actionButtons[1] = new Button(SpriteName.BattleRetreatBG, SpriteName.BattleRetreatFG, string.Empty, x + 40, y, 40, 40);
			actionButtons[2] = new Button(SpriteName.BattlePrevShipBG, SpriteName.BattlePrevShipFG, string.Empty, x + 80, y, 40, 40);
			actionButtons[3] = new Button(SpriteName.BattlePrevShipDoneBG, SpriteName.BattlePrevShipDoneFG, string.Empty, x + 120, y, 40, 40);
			actionButtons[4] = new Button(SpriteName.BattleNextShipDoneBG, SpriteName.BattleNextShipDoneFG, string.Empty, x + 160, y, 40, 40);
			actionButtons[5] = new Button(SpriteName.BattleNextShipBG, SpriteName.BattleNextShipFG, string.Empty, x + 200, y, 40, 40);
			actionButtons[6] = new Button(SpriteName.BattleNextTurnBG, SpriteName.BattleNextTurnFG, string.Empty, x + 240, y, 40, 40);

			taskBarArea = new Point(x, x + 280);

			/*passiveSystemsBackground = new StretchableImage(x - 300, gameMain.ScreenHeight - 150, 300, 170, 30, 13, DrawingManagement.BoxBorderBG);
			activeSystemsBackground = new StretchableImage(x + 280, gameMain.ScreenHeight - 150, 300, 170, 30, 13, DrawingManagement.BoxBorderBG);
			statusBackground = new StretchableImage(x, y - 100, 280, 100, 30, 13, DrawingManagement.BoxBorderBG);*/

			passiveScrollBar = new ScrollBar(x - 33, gameMain.ScreenHeight - 160, 16, 128, 6, 6, false, false, DrawingManagement.VerticalScrollBar);
			activeScrollBar = new ScrollBar(x + 557, gameMain.ScreenHeight - 160, 16, 128, 6, 6, false, false, DrawingManagement.VerticalScrollBar);
			passiveScrollBar.SetEnabledState(false);
			activeScrollBar.SetEnabledState(false);

			passiveButtons = new InvisibleStretchButton[6];
			passiveLabels = new Label[6];
			//passivePowerLabels = new Label[4];
			activeButtons = new InvisibleStretchButton[6];
			activeLabels = new Label[6];
			//activePowerLabels = new Label[4];
			//activeTimeLabels = new Label[4];

			x = gameMain.ScreenWidth / 2;
			for (int i = 0; i < passiveButtons.Length; i++)
			{
				int tempY = gameMain.ScreenHeight - 165 + (i * 25);
				passiveButtons[i] = new InvisibleStretchButton(DrawingManagement.TinyButtonBackground, DrawingManagement.TinyButtonForeground, string.Empty, x - 440, tempY, 265, 25, 10, 10);
				activeButtons[i] = new InvisibleStretchButton(DrawingManagement.TinyButtonBackground, DrawingManagement.TinyButtonForeground, string.Empty, x + 152, tempY, 265, 25, 10, 10);

				passiveLabels[i] = new Label(x - 435, tempY + 2);
				activeLabels[i] = new Label(x + 157, tempY + 2);
				//passivePowerLabels[i] = new Label(x - 280, tempY + 25);
				//activePowerLabels[i] = new Label(x + 300, tempY + 25);
				//activeTimeLabels[i] = new Label(x + 445, tempY + 25);
			}

			nameBackground = new StretchableImage(gameMain.ScreenWidth / 2 - 175, gameMain.ScreenHeight - 215, 350, 45, 30, 13, DrawingManagement.BoxBorderBG);

			shipNameLabel = new Label(gameMain.ScreenWidth / 2 - 168, gameMain.ScreenHeight - 205);
			empireNameLabel = new Label(gameMain.ScreenWidth / 2 + 168, gameMain.ScreenHeight - 205);
			empireNameLabel.SetAlignment(true);

			selectionSize = 0;
			selectionPoint = new Point();
			SelectedShip = null;
			/*retreatingShips = new List<CombatShip>();

			shipNameLabel = new Label(string.Empty, 25, gameMain.ScreenHeight - 150);
			computerLabel = new Label(string.Empty, 25, gameMain.ScreenHeight - 125);
			engineLabel = new Label(string.Empty, 25, gameMain.ScreenHeight - 100);
			shieldLabel = new Label(string.Empty, 25, gameMain.ScreenHeight - 75);
			armorLabel = new Label(string.Empty, 25, gameMain.ScreenHeight - 50);
			hitPointsLabel = new Label(string.Empty, 25, gameMain.ScreenHeight - 25);

			weaponButtons = new Button[6];
			for (int i = 0; i < weaponButtons.Length; i++)
			{
				weaponButtons[i] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, string.Empty, 150, gameMain.ScreenHeight - (150 - (i * 25)), 180, 25);
			}

			weaponScrollBar = new ScrollBar(330, gameMain.ScreenHeight - 150, 16, 118, 6, 6, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton, SpriteName.ScrollDownBackgroundButton,
				SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton, SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);*/
		}

		public void SetupScreen()
		{
			//combatIter = 0;
			//whichEmpireTurn = 0;
			/*SetupBattle(gameMain.empireManager.CombatsToProcess[combatIter].fleetsInCombat, null);*/
		}

		public void ResetScreen()
		{
			//whichEmpireTurn = 0;
		}

		public void DrawScreen(DrawingManagement drawingManagement)
		{
			backgroundStars.Draw(camera.CameraX, camera.CameraY, camera.GetViewSize().X, camera.GetViewSize().Y, camera.XOffset, camera.YOffset, camera.Scale, drawingManagement);
			GorgonLibrary.Graphics.Sprite shipSprite;

			foreach (CombatFleet fleet in fleetsInCombat)
			{
				GorgonLibrary.Gorgon.CurrentShader = gameMain.ShipShader;
				gameMain.ShipShader.Parameters["EmpireColor"].SetValue(fleet.Empire.ConvertedColor);
				foreach (ShipInstance ship in fleet.combatShips)
				{
					shipSprite = ship.BaseShipDesign.ShipClass.Sprites[ship.BaseShipDesign.WhichStyle];
					shipSprite.Axis = new GorgonLibrary.Vector2D(shipSprite.Width / 2, shipSprite.Height / 2);
					shipSprite.Rotation = (float)ship.Values["Angle"];
					shipSprite.SetPosition(((int)ship.Values["XPos"] - (camera.CameraX * 16 + camera.XOffset)) * camera.Scale, ((int)ship.Values["YPos"] - (camera.CameraY * 16 + camera.YOffset)) * camera.Scale);
					/*if (processRetreating && ship == SelectedShip)
					{
						if (ship.Size % 2 == 0)
						{
							shipSprite.SetScale(camera.Scale * retreatProcess, camera.Scale * retreatProcess);
						}
						else
						{
							shipSprite.SetScale(((shipSprite.Width - 16) / shipSprite.Width) * camera.Scale * retreatProcess, ((shipSprite.Height - 16) / shipSprite.Height) * camera.Scale * retreatProcess);
						}
					}
					else
					{*/
						if (ship.BaseShipDesign.ShipClass.Size % 2 == 0)
						{
							shipSprite.SetScale(camera.Scale, camera.Scale);
						}
						else
						{
							shipSprite.SetScale(((shipSprite.Width - 16) / shipSprite.Width) * camera.Scale, ((shipSprite.Height - 16) / shipSprite.Height) * camera.Scale);
						}
					//}*/
					if (ship == SelectedShip)
					{
						drawingManagement.SetSpriteScale(SpriteName.ShipSelectionTL, camera.Scale * 16, camera.Scale * 16);
						drawingManagement.SetSpriteScale(SpriteName.ShipSelectionTR, camera.Scale * 16, camera.Scale * 16);
						drawingManagement.SetSpriteScale(SpriteName.ShipSelectionBL, camera.Scale * 16, camera.Scale * 16);
						drawingManagement.SetSpriteScale(SpriteName.ShipSelectionBR, camera.Scale * 16, camera.Scale * 16);
						drawingManagement.SetSpriteScale(SpriteName.ShipSelectionBG, camera.Scale * 16, camera.Scale * 16);

						int amount = (int)ship.Values["ShipRadius"] / 8;
						for (int i = 0; i < amount; i++)
						{
							for (int j = 0; j < amount; j++)
							{
								drawingManagement.DrawSprite(SpriteName.ShipSelectionBG, (int)((((int)ship.Values["XPos"] - (int)ship.Values["ShipRadius"] + (i * 16)) - (camera.CameraX * 16 + camera.XOffset)) * camera.Scale), (int)((((int)ship.Values["YPos"] - (int)ship.Values["ShipRadius"] + (j * 16)) - (camera.CameraY * 16 + camera.YOffset)) * camera.Scale));
							}
						}
						drawingManagement.DrawSprite(SpriteName.ShipSelectionTL, (int)((((int)ship.Values["XPos"] - (int)ship.Values["ShipRadius"]) - (camera.CameraX * 16 + camera.XOffset)) * camera.Scale), (int)((((int)ship.Values["YPos"] - (int)ship.Values["ShipRadius"]) - (camera.CameraY * 16 + camera.YOffset)) * camera.Scale));
						drawingManagement.DrawSprite(SpriteName.ShipSelectionTR, (int)((((int)ship.Values["XPos"] + (int)ship.Values["ShipRadius"] - 16) - (camera.CameraX * 16 + camera.XOffset)) * camera.Scale), (int)((((int)ship.Values["YPos"] - (int)ship.Values["ShipRadius"]) - (camera.CameraY * 16 + camera.YOffset)) * camera.Scale));
						drawingManagement.DrawSprite(SpriteName.ShipSelectionBL, (int)((((int)ship.Values["XPos"] - (int)ship.Values["ShipRadius"]) - (camera.CameraX * 16 + camera.XOffset)) * camera.Scale), (int)((((int)ship.Values["YPos"] + (int)ship.Values["ShipRadius"] - 16) - (camera.CameraY * 16 + camera.YOffset)) * camera.Scale));
						drawingManagement.DrawSprite(SpriteName.ShipSelectionBR, (int)((((int)ship.Values["XPos"] + (int)ship.Values["ShipRadius"] - 16) - (camera.CameraX * 16 + camera.XOffset)) * camera.Scale), (int)((((int)ship.Values["YPos"] + (int)ship.Values["ShipRadius"] - 16) - (camera.CameraY * 16 + camera.YOffset)) * camera.Scale));
					}
					shipSprite.Draw();
				}
				GorgonLibrary.Gorgon.CurrentShader = null;
			}

			if (particles.Count == 0)
			{
				if (selectionSize == 0)
				{
					//Target reticle
					drawingManagement.SetSpriteAxis(SpriteName.TargetReticle, 7, 7);
					//drawingManagement.SetSpriteScale(SpriteName.TargetReticle, 16 * camera.Scale, 16 * camera.Scale);
					drawingManagement.DrawSprite(SpriteName.TargetReticle, mouseX, mouseY);
				}
				//drawingManagement.SetSpriteScale(SpriteName.ShipSelection32, selectionSize * 16 * camera.Scale, selectionSize * 16 * camera.Scale);
				//drawingManagement.DrawSprite(SpriteName.ShipSelection32, (int)(selectionPoint.X - ((camera.CameraX * 16) + camera.XOffset - 8) * camera.Scale), (int)(selectionPoint.Y - ((camera.CameraY * 16) + camera.YOffset - 8) * camera.Scale), 255, System.Drawing.Color.White);
			}
			else
			{
				int xOffset = (int)(((camera.CameraX * 16) + camera.XOffset) * camera.Scale);
				int yOffset = (int)(((camera.CameraY * 16) + camera.YOffset) * camera.Scale);
				for (int i = 0; i < particles.Count; i++)
				{
					particles[i].Draw(xOffset, yOffset, camera.Scale);
				}
			}

			/*if (!processRetreating)
			{
				int empireY = gameMain.ScreenHeight - (fleetsInCombat.Count * 25);
				for (int i = 0; i < fleetsInCombat.Count; i++)
				{
					int empireX = (int)(gameMain.ScreenWidth - (fleetsInCombat[i].EmpireNameLabel.GetWidth() + 5));
					fleetsInCombat[i].EmpireNameLabel.MoveTo(empireX, empireY);
					fleetsInCombat[i].EmpireNameLabel.Draw();
					if (i == whichEmpireTurn)
					{
						drawingManagement.DrawSprite(SpriteName.EmpireTurnArrow, empireX - 30, empireY - 4, 255, System.Drawing.Color.White);
					}
					empireY += 25;
				}*/

			if (selectedShip != null)
			{
				/*passiveSystemsBackground.Draw(drawingManagement);
				activeSystemsBackground.Draw(drawingManagement);
				statusBackground.Draw(drawingManagement);*/
				nameBackground.Draw(drawingManagement);
				drawingManagement.DrawSprite(SpriteName.BattleBackground, gameMain.ScreenWidth / 2 - 450, gameMain.ScreenHeight - 175);
				passiveScrollBar.Draw(drawingManagement);
				activeScrollBar.Draw(drawingManagement);

				shipNameLabel.Draw();
				empireNameLabel.Draw();

				for (int i = 0; i < selectedShip.BaseShipDesign.ShipClass.DisplayIcons.Count; i++)
				{
					selectedShip.BaseShipDesign.ShipClass.DisplayIcons[i].Draw((gameMain.ScreenWidth / 2) - 135 + (140 * (i % 2)), gameMain.ScreenHeight - 165 + ((i / 2) * 25), 140, 25, drawingManagement);
				}

				for (int i = 0; i < maxPassiveVisible; i++)
				{
					passiveButtons[i].Draw(drawingManagement);
					passiveLabels[i].Draw(drawingManagement);
				}
				for (int i = 0; i < maxActiveVisible; i++)
				{
					activeButtons[i].Draw(drawingManagement);
					activeLabels[i].Draw(drawingManagement);
				}
				for (int i = 0; i < maxActiveVisible; i++)
				{
					activeButtons[i].DrawTips(drawingManagement);
				}
			}

			foreach (Button button in actionButtons)
			{
				button.Draw(drawingManagement);
			}

				/*drawingManagement.DrawSprite(SpriteName.ControlBackground, 0, gameMain.ScreenHeight - 160, 100, 350, 160, System.Drawing.Color.White);
				shipNameLabel.Draw();
				computerLabel.Draw();
				shieldLabel.Draw();
				hitPointsLabel.Draw();
				armorLabel.Draw();
				engineLabel.Draw();
				for (int i = 0; i < maxVisible; i++)
				{
					weaponButtons[i].Draw(drawingManagement);
				}
				weaponScrollBar.Draw(drawingManagement);
			}*/
		}

		public void UpdateBackground(float frameDeltaTime)
		{
			backgroundStars.Update(frameDeltaTime);
		}

		public void Update(int mouseX, int mouseY, float frameDeltaTime)
		{
			this.mouseX = mouseX;
			this.mouseY = mouseY;

			UpdateBackground(frameDeltaTime);

			particlesToRemove = new List<ParticleInstance>();
			particlesToAdd = new List<ParticleInstance>();
			effectsToAdd = new List<EffectInstance>();
			effectsToRemove = new List<EffectInstance>();

			if (particles.Count > 0 || tempEffects.Count > 0)
			{
				for (int i = 0; i < tempEffects.Count; i++)
				{
					Dictionary<string, object>[] result = tempEffects[i].Update(frameDeltaTime);
					if (result[0].ContainsKey("_remove") && (bool)result[0]["_remove"])
					{
						effectsToRemove.Add(tempEffects[i]);
					}
					ProcessResults(result, 1, false, tempEffects[i].ShipAffected);
					if (result[0].ContainsKey("_removeShip") && (bool)result[0]["_removeShip"]) //Dying is an effect
					{
						for (int j = 0; j < fleetsInCombat.Count; j++)
						{
							if (fleetsInCombat[j].Empire == tempEffects[i].ShipAffected.Owner && fleetsInCombat[j].combatShips.Contains(tempEffects[i].ShipAffected))
							{
								fleetsInCombat[j].combatShips.Remove(tempEffects[i].ShipAffected);
								break;
							}
						}
					}
				}
				for (int i = 0; i < particles.Count; i++)
				{
					Dictionary<string, object>[] result = particles[i].Update(frameDeltaTime);
					if (result[0].ContainsKey("RemoveParticle") && (bool)result[0]["RemoveParticle"])
					{
						particlesToRemove.Add(particles[i]);
						continue;
					}
					ProcessResults(result, 1, false, null);
					if (result[0].ContainsKey("ProcessCollision") && !(bool)result[0]["ProcessCollision"])
					{
						//Don't process collisions for this particle, just visual effect
						continue;
					}
					//Add collision detection/handling here
					//Line vs circle collision, find all intersections, process from closest to farthest
					List<Collision> collisions = CheckForCollisions(result[0], particles[i].ShipToIgnore);
					foreach (Collision collision in collisions)
					{
						if (collision.EquipmentCollision != null)
						{
							Dictionary<string, object>[] collisionResult = collision.EquipmentCollision.OnHit(collision.Position, new Point((int)collision.ShipCollision.Values["XPos"], (int)collision.ShipCollision.Values["YPos"]), collision.ShipCollision.BaseShipDesign.ShipClass.Size, collision.EquipmentCollision.GetEquipmentInfo(collision.ShipCollision.Values), particles[i].Values);
							if (collisionResult != null)
							{
								ProcessResults(collisionResult, 1, false, null);
								Dictionary<string, object>[] postHitResults = particles[i].PostHit(collisionResult[0], collision.Position.X, collision.Position.Y);
								ProcessResults(postHitResults, 1, false, null);
								if (particles[i].Values.ContainsKey("StopProcessing") && (bool)particles[i].Values["StopProcessing"])
								{
									//Particle either ended or died, stop processing for speed and to avoid triggering scripts
									break;
								}
							}
						}
						else
						{
							//Collision with the ship itself
							Dictionary<string, object>[] results = collision.ShipCollision.BaseShipDesign.ShipClass.ShipScript.OnHit(collision.Position.X, collision.Position.Y, (int)collision.ShipCollision.Values["XPos"], (int)collision.ShipCollision.Values["YPos"], collision.ShipCollision.BaseShipDesign.ShipClass.Size, new Dictionary<string, object>[]{}, collision.ShipCollision.Values, particles[i].Values);
							ProcessResults(results, 2, false, collision.ShipCollision);
							collision.ShipCollision.Values = results[1];
							Dictionary<string, object>[] postHitResults = particles[i].PostHit(results[0], collision.Position.X, collision.Position.Y);
							ProcessResults(postHitResults, 1, false, null);
							if (particles[i].Values.ContainsKey("StopProcessing") && (bool)particles[i].Values["StopProcessing"])
							{
								//Particle either ended or died, stop processing for speed and to avoid triggering scripts
								break;
							}
						}
					}
				}
				foreach (ParticleInstance particle in particlesToRemove)
				{
					particles.Remove(particle);
				}
				foreach (ParticleInstance particle in particlesToAdd)
				{
					particles.Add(particle);
				}
				foreach (EffectInstance effect in effectsToRemove)
				{
					tempEffects.Remove(effect);
				}
				foreach (EffectInstance effect in effectsToAdd)
				{
					tempEffects.Add(effect);
				}
				return;
			}
			/*if (processRetreating)
			{
				if (retreatProcess <= 0) //This ship has finished retreating
				{
					foreach (CombatFleet fleet in fleetsInCombat)
					{
						if (fleet.combatShips.Contains(retreatingShips[0]))
						{
							fleet.combatShips.Remove(retreatingShips[0]);
							break;
						}
					}
					retreatingShips.Remove(retreatingShips[0]);
					//Since the ships has left battlefield successfully, no need to reduce ships.
					if (retreatingShips.Count > 0)
					{
						selectedShip = retreatingShips[0];
						camera.CenterCamera(selectedShip.X, selectedShip.Y);
						retreatProcess = 1.0f;
					}
					else
					{
						processRetreating = false;
					}
				}
				retreatProcess -= frameDeltaTime;
				if (retreatProcess < 0)
				{
					retreatProcess = 0;
				}
				return;
			}*/

			foreach (Button button in actionButtons)
			{
				button.MouseHover(mouseX, mouseY, frameDeltaTime);
			}
			if (selectedShip != null)
			{
				for (int i = 0; i < maxActiveVisible; i++)
				{
					activeButtons[i].MouseHover(mouseX, mouseY, frameDeltaTime);
				}
			}
			//TaskBarArea.Y is actually rightmost X pos
			if (mouseX >= taskBarArea.X && mouseX < taskBarArea.Y && mouseY >= gameMain.ScreenHeight - 40)
			{
				return;
			}
			selectionPoint = GetMousePoint(mouseX, mouseY);
			camera.HandleUpdate(mouseX, mouseY, frameDeltaTime);
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			if (particles.Count > 0)
			{
				//wait til particles expires
				return;
			}
			foreach (Button button in actionButtons)
			{
				button.MouseDown(x, y);
			}
			if (selectedShip != null)
			{
				for (int i = 0; i < maxActiveVisible; i++)
				{
					activeButtons[i].MouseDown(x, y);
				}
			}
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			if (particles.Count > 0)
			{
				//wait til particles expires
				return;
			}
			for (int i = 0; i < actionButtons.Length; i++)
			{
				if (actionButtons[i].MouseUp(x, y))
				{
					switch (i)
					{
						case 1:
							{
								//SelectedShip.Retreating = true;
								//actionButtons[i].Active = false;
							} break;
						case 2:
							{
								if (SelectedShip == null || SelectedShip.Owner != currentFleetTurn.Empire)
								{
									int iter = 0;
									bool found = false;
									while (iter < currentFleetTurn.combatShips.Count - 1)
									{
										if (!currentFleetTurn.combatShips[iter].Values.ContainsKey("_doneTurn"))
										{
											SelectedShip = currentFleetTurn.combatShips[iter];
											found = true;
											break;
										}
										iter++;
									}
									if (!found)
									{
										SelectedShip = currentFleetTurn.combatShips[0];
									}
								}
								else
								{
									int shipIter = currentFleetTurn.combatShips.IndexOf(SelectedShip);
									int amountOfShipsToCheck = currentFleetTurn.combatShips.Count;
									int shipsChecked = 0;
									while (shipsChecked < amountOfShipsToCheck)
									{
										shipIter--;
										if (shipIter < 0)
										{
											shipIter = amountOfShipsToCheck - 1;
										}
										if (!currentFleetTurn.combatShips[shipIter].Values.ContainsKey("_doneTurn"))
										{
											SelectedShip = currentFleetTurn.combatShips[shipIter];
											camera.CenterCamera((int)SelectedShip.Values["XPos"] / 16, (int)SelectedShip.Values["YPos"] / 16);
											break;
										}
										shipsChecked++;
									}
								}
								//actionButtons[1].Active = !SelectedShip.Retreating;
							} break;
						case 3:
							{
								//This button is active only if the selected ship's owner is the current fleet's empire
								if (!SelectedShip.Values.ContainsKey("_doneTurn"))
								{
									SelectedShip.Values.Add("_doneTurn", true);
								}
								int amountOfShipsToCheck = currentFleetTurn.combatShips.Count;
								int nextShipToCheck = currentFleetTurn.combatShips.IndexOf(SelectedShip);
								int shipsChecked = 0;
								while (shipsChecked < amountOfShipsToCheck)
								{
									nextShipToCheck--;
									if (nextShipToCheck < 0)
									{
										nextShipToCheck = amountOfShipsToCheck - 1;
									}
									if (!currentFleetTurn.combatShips[nextShipToCheck].Values.ContainsKey("_doneTurn"))
									{
										SelectedShip = currentFleetTurn.combatShips[nextShipToCheck];
										camera.CenterCamera((int)SelectedShip.Values["XPos"] / 16, (int)SelectedShip.Values["YPos"] / 16);
										break;
									}
									shipsChecked++;
								}
								//actionButtons[1].Active = !SelectedShip.Retreating;
							} break;
						case 4:
							{
								//This button is active only if the selected ship's owner is the current fleet's empire
								if (!SelectedShip.Values.ContainsKey("_doneTurn"))
								{
									SelectedShip.Values.Add("_doneTurn", true);
								}
								int amountOfShipsToCheck = currentFleetTurn.combatShips.Count;
								int nextShipToCheck = currentFleetTurn.combatShips.IndexOf(SelectedShip);
								int shipsChecked = 0;
								while (shipsChecked < amountOfShipsToCheck)
								{
									nextShipToCheck++;
									if (nextShipToCheck >= amountOfShipsToCheck)
									{
										nextShipToCheck = 0;
									}
									if (!currentFleetTurn.combatShips[nextShipToCheck].Values.ContainsKey("_doneTurn"))
									{
										SelectedShip = currentFleetTurn.combatShips[nextShipToCheck];
										camera.CenterCamera((int)SelectedShip.Values["XPos"] / 16, (int)SelectedShip.Values["YPos"] / 16);
										break;
									}
									shipsChecked++;
								}
								//actionButtons[1].Active = !SelectedShip.Retreating;
							} break;
						case 5:
							{
								if (SelectedShip == null || SelectedShip.Owner != currentFleetTurn.Empire)
								{
									int iter = currentFleetTurn.combatShips.Count - 1;
									bool found = false;
									while (iter >= 0)
									{
										if (!currentFleetTurn.combatShips[iter].Values.ContainsKey("_doneTurn"))
										{
											SelectedShip = currentFleetTurn.combatShips[iter];
											found = true;
											break;
										}
										iter--;
									}
									if (!found)
									{
										SelectedShip = currentFleetTurn.combatShips[currentFleetTurn.combatShips.Count - 1];
									}
								}
								else
								{
									int amountOfShipsToCheck = currentFleetTurn.combatShips.Count;
									int nextShipToCheck = currentFleetTurn.combatShips.IndexOf(SelectedShip);
									int shipsChecked = 0;
									while (shipsChecked < amountOfShipsToCheck)
									{
										nextShipToCheck++;
										if (nextShipToCheck >= amountOfShipsToCheck)
										{
											nextShipToCheck = 0;
										}
										if (!currentFleetTurn.combatShips[nextShipToCheck].Values.ContainsKey("_doneTurn"))
										{
											SelectedShip = currentFleetTurn.combatShips[nextShipToCheck];
											camera.CenterCamera((int)SelectedShip.Values["XPos"] / 16, (int)SelectedShip.Values["YPos"] / 16);
											break;
										}
										shipsChecked++;
									}
								}
								//actionButtons[1].Active = !SelectedShip.Retreating;
							} break;
						case 6:
							{
								MoveToNextEmpireTurn();
							} break;
					}
					return;
				}
			}
			if (selectedShip != null)
			{
				for (int i = 0; i < maxActiveVisible; i++)
				{
					if (activeButtons[i].MouseUp(x, y))
					{
						SelectedEquipment = activeEquipments[i + activeScrollBar.TopIndex];
						foreach (InvisibleStretchButton button in activeButtons)
						{
							button.Selected = false;
						}
						activeButtons[i].Selected = true;
						return;
					}
				}
			}
			if (SelectedEquipment != null)
			{
				Point cell = GetMousePoint(x, y);
				Dictionary<string, object>[] result = SelectedEquipment.Activate(cell, new Point((int)selectedShip.Values["XPos"], (int)selectedShip.Values["YPos"]), new Point(size * 16, size * 16), SelectedEquipment.Values);
				if (result != null && result.Length > 0)
				{
					for (int i = 0; i < result.Length; i++)
					{
						if (result[i].ContainsKey("SpawnParticle"))
						{
							string value = (string)result[i]["SpawnParticle"];
							Dictionary<string, object> newValues = new Dictionary<string, object>(result[i]);
							newValues.Remove("SpawnParticle");
							ParticleInstance particleToAdd = gameMain.particleManager.SpawnParticle(value, selectedShip, newValues);
							if (particleToAdd != null)
							{
								particles.Add(particleToAdd);
							}
						}
					}
				}
				SelectedEquipment = null;
				foreach (InvisibleStretchButton button in activeButtons)
				{
					button.Selected = false;
				}
				return;
			}
			if (selectedShip != null && y >= gameMain.ScreenHeight - 175 && x >= (gameMain.ScreenWidth / 2) - 450 && x < (gameMain.ScreenWidth / 2) + 450)
			{
				return;
			}
			IsClickOnShip(x, y);
		}

		public void Resize()
		{
			//camera.ResizeScreen(gameMain.ScreenWidth, gameMain.ScreenHeight);
		}

		public void MouseScroll(int direction, int x, int y)
		{
			//camera.MouseWheel(direction, x, y);
		}

		public void KeyDown(KeyboardInputEventArgs e)
		{
		}

		private Point GetMousePoint(int x, int y)
		{
			int X = (int)((x / camera.Scale) + camera.XOffset + (camera.CameraX * 16));
			int Y = (int)((y / camera.Scale) + camera.YOffset + (camera.CameraY * 16));
			return new Point(X, Y);
		}

		private void ProcessResults(Dictionary<string, object>[] result, int startingIndex, bool onlyCosmetic, ShipInstance shipAffected)
		{
			if (result.Length > startingIndex)
			{
				//Spawns new particles
				for (int i = startingIndex; i < result.Length; i++)
				{
					if (!onlyCosmetic && result[i].ContainsKey("SpawnParticle"))
					{
						string value = (string)result[i]["SpawnParticle"];
						Dictionary<string, object> newValues = new Dictionary<string, object>(result[i]);
						newValues.Remove("SpawnParticle");
						ParticleInstance particleToAdd = gameMain.particleManager.SpawnParticle(value, null, newValues);
						if (particleToAdd != null)
						{
							particlesToAdd.Add(particleToAdd);
						}
					}
					else if (!onlyCosmetic && result[i].ContainsKey("_spawnTempEffect"))
					{
						string value = (string)result[i]["_spawnTempEffect"];
						Dictionary<string, object> newValues = new Dictionary<string, object>(result[i]);
						newValues.Remove("_spawnTempEffect");
						EffectInstance tempEffectToAdd = gameMain.effectManager.SpawnEffect(value, shipAffected, newValues);
						if (tempEffectToAdd != null)
						{
							effectsToAdd.Add(tempEffectToAdd);
						}
					}
				}
			}
		}

		private void IsClickOnShip(int x, int y)
		{
			Point point = GetMousePoint(x, y);

			foreach (CombatFleet fleet in fleetsInCombat)
			{
				for (int i = 0; i < fleet.combatShips.Count; i++)
				{
					if ((int)fleet.combatShips[i].Values["XPos"] - (int)fleet.combatShips[i].Values["ShipRadius"] <= point.X && 
						(int)fleet.combatShips[i].Values["XPos"] + (int)fleet.combatShips[i].Values["ShipRadius"] > point.X && 
						(int)fleet.combatShips[i].Values["YPos"] - (int)fleet.combatShips[i].Values["ShipRadius"] <= point.Y && 
						(int)fleet.combatShips[i].Values["YPos"] + (int)fleet.combatShips[i].Values["ShipRadius"] > point.Y)
					{
						SelectedShip = fleet.combatShips[i];
						/*if (fleet == fleetsInCombat[whichEmpireTurn])
						{
							selectedShipIter = i;
							actionButtons[1].Active = !SelectedShip.Retreating;
						}
						else
						{
							actionButtons[1].Active = false;
						}*/
						return;
					}
				}
			}
			//At this point, no ship is selected, clear the current selected ship
			SelectedShip = null;
		}

		public void LoadBattle(string filePath)
		{
			//This is a battle simulator, so we need to create fake empires and original fleets
			empires = new List<Empire>();
			fleetsInCombat = new List<CombatFleet>();
			particles = new List<ParticleInstance>();
			tempEffects = new List<EffectInstance>();

			try
			{
				XDocument doc = XDocument.Load(filePath);
				XElement root = doc.Element("Battle");

				XElement layout = root.Element("Layout");
				size = int.Parse(layout.Attribute("size").Value);

				XElement empireNodes = root.Element("Empires");
				int i = 0;
				foreach (XElement empire in empireNodes.Elements())
				{
					string name = empire.Attribute("name").Value;
					string raceName = empire.Attribute("race").Value;
					Race realRace = null;
					foreach (Race race in gameMain.raceManager.Races)
					{
						if (race.RaceName == raceName)
						{
							realRace = race;
							break;
						}
					}
					bool isCPU = empire.Attribute("player").Value == "CPU";
					AI realAI = null;
					if (isCPU)
					{
						string aiName = empire.Attribute("ai").Value;
						foreach (AI ai in gameMain.aiManager.AIs)
						{
							if (ai.AIName == aiName)
							{
								realAI = ai;
							}
						}
					}
					string[] colorValues = empire.Attribute("color").Value.Split(new[] { ',' });
					Empire newEmpire = new Empire(name, i, realRace, isCPU ? PlayerType.CPU : PlayerType.HUMAN, realAI, System.Drawing.Color.FromArgb(255, int.Parse(colorValues[0]), int.Parse(colorValues[1]), int.Parse(colorValues[2])));
					empires.Add(newEmpire);
					CombatFleet fleet = new CombatFleet(null);
					fleet.Empire = newEmpire;
					foreach (XElement ship in empire.Elements())
					{
						StartingShip combatShip = new StartingShip();
						combatShip.name = ship.Attribute("name").Value;
						combatShip.shipClass = ship.Attribute("class").Value;
						combatShip.style = int.Parse(ship.Attribute("style").Value);
						combatShip.equipment = new List<StartingEquipment>();
						foreach (XElement equipment in ship.Elements())
						{
							StartingEquipment newEquipment = new StartingEquipment();
							newEquipment.modifiableValues = new List<string>();
							newEquipment.modifiers = new List<string>();
							newEquipment.mainItem = equipment.Attribute("mainItem").Value;
							if (equipment.Attribute("mountItem") != null)
							{
								newEquipment.mountItem = equipment.Attribute("mountItem").Value;
								if (equipment.Attribute("modifierItems") != null)
								{
									newEquipment.modifiers.AddRange(equipment.Attribute("modifierItems").Value.Split(new[] { ',' }));
								}
							}
							if (equipment.Attribute("modifiableValues") != null)
							{
								foreach (string value in equipment.Attribute("modifiableValues").Value.Split(new[] { '|' }))
								{
									if (!string.IsNullOrEmpty(value))
									{
										newEquipment.modifiableValues.Add(value);
									}
								}
							}
							combatShip.equipment.Add(newEquipment);
						}
						/*ShipInstance convertedShip = new ShipInstance(gameMain.masterTechnologyList.ConvertStartingShipToRealShip(combatShip, newEmpire.EmpireRace, gameMain.iconManager), newEmpire);
						convertedShip.Values.Add("ShipRadius", (convertedShip.BaseShipDesign.ShipClass.Size * 16) / 2);
						convertedShip.Values.Add("XPos", (int.Parse(ship.Attribute("posX").Value) * 16 + (int)convertedShip.Values["ShipRadius"]));
						convertedShip.Values.Add("YPos", (int.Parse(ship.Attribute("posY").Value) * 16 + (int)convertedShip.Values["ShipRadius"]));
						convertedShip.Values.Add("Angle", (float.Parse(ship.Attribute("rotation").Value, System.Globalization.CultureInfo.InvariantCulture)));
						convertedShip.Values = convertedShip.BaseShipDesign.ShipClass.ShipScript.Initialize(convertedShip.Values);
						fleet.combatShips.Add(convertedShip);*/
					}
					fleet.UpdateShipInfo();
					fleetsInCombat.Add(fleet);
					i++;
				}
				currentFleetTurn = fleetsInCombat[0];
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

			camera.InitCamera(size, 16);
			backgroundStars = new BackgroundStars(size, gameMain.r, 40);
		}

		public void SetupBattle(List<Squadron> fleets, StarSystem system)
		{
			/*fleetsInCombat = new List<CombatFleet>();
			
			int totalCircumferenceNeeded = 0;

			//To do: randomize order of empires
			foreach (Fleet fleet in fleets)
			{
				CombatFleet combatFleet = new CombatFleet(fleet);
				totalCircumferenceNeeded += combatFleet.Length;
				combatFleet.Empire = fleet.Empire;
				fleetsInCombat.Add(combatFleet);
			}
			totalCircumferenceNeeded *= 5;

			if (totalCircumferenceNeeded < 50)
			{
				totalCircumferenceNeeded = 50;
			}

			float radius = totalCircumferenceNeeded / 2.0f;
			radius = (float)(radius / Math.PI);

			float angle = 0;

			foreach (CombatFleet fleet in fleetsInCombat)
			{
				fleet.SetStartingPosition(radius, angle);
				angle += (float)((Math.PI * 2) / fleetsInCombat.Count);
			}

			camera.InitCamera((int)(radius * 3), 16);

			this.system = system;

			selectedShipIter = 0;
			SelectedShip = fleetsInCombat[0].combatShips[selectedShipIter];
			camera.CenterCamera(SelectedShip.X, SelectedShip.Y);*/
		}

		private void MoveToNextEmpireTurn()
		{
			foreach (ShipInstance ship in currentFleetTurn.combatShips)
			{
				ship.Values.Remove("_doneTurn");
			}
			int iter = fleetsInCombat.IndexOf(currentFleetTurn);
			if (iter == fleetsInCombat.Count - 1)
			{
				iter = 0;
			}
			else
			{
				iter++;
			}
			currentFleetTurn = fleetsInCombat[iter];
			currentFleetTurn.UpdateShipInfo();
			/*if (whichEmpireTurn >= fleetsInCombat.Count)
			{
				whichEmpireTurn = 0;
				//Since all empires are done, reset all ships' "Done" to false
				foreach (CombatFleet fleet in fleetsInCombat)
				{
					foreach (CombatShip ship in fleet.combatShips)
					{
						ship.DoneThisTurn = false;
						if (ship.DoneRetreatWait)
						{
							retreatingShips.Add(ship);
						}
						if (ship.Retreating)
						{
							ship.DoneRetreatWait = true;
						}
					}
				}
			}*/
			SelectedShip = currentFleetTurn.combatShips[0];
			camera.CenterCamera((int)SelectedShip.Values["XPos"] / 16, (int)SelectedShip.Values["YPos"] / 16);
			/*selectedShipIter = 0;
			SelectedShip = fleetsInCombat[whichEmpireTurn].combatShips[selectedShipIter];
			actionButtons[1].Active = !selectedShip.Retreating;
			camera.CenterCamera(SelectedShip.X, SelectedShip.Y);
			if (retreatingShips.Count > 0)
			{
				processRetreating = true;
				retreatProcess = 1.0f;
				selectedShip = retreatingShips[0];
				camera.CenterCamera(selectedShip.X, selectedShip.Y);
			}*/
		}

		private List<Collision> CheckForCollisions(Dictionary<string, object> particleValues, ShipInstance shipToIgnore)
		{
			float x = (float)particleValues["PosX"];
			float y = (float)particleValues["PosY"];
			float width = (float)particleValues["Width"];
			float height = (float)particleValues["Height"];
			float angle = (float)particleValues["Angle"];

			List<Point> pointsAlreadyProcessed = new List<Point>();
			if (particleValues.ContainsKey("PointAlreadyProcessed"))
			{
				string value = (string)particleValues["PointAlreadyProcessed"];
				string[] points = value.Split(new[] { '|' });
				foreach (string point in points)
				{
					string[] pointParts = point.Split(new[] { ',' });
					if (pointParts.Length == 2)
					{
						try
						{
							pointsAlreadyProcessed.Add(new Point(int.Parse(pointParts[0]), int.Parse(pointParts[1])));
						}
						catch
						{
							throw new Exception("Tried to parse PointAlreadyProcessed, but the values were invalid, using the particle \"" + particleValues["name"] + "\"");
						}
					}
				}
			}

			PointF pos = new PointF(x, y);
			PointF dimensions = new PointF(width, height);

			List<Collision> collisions = new List<Collision>();

			foreach (CombatFleet fleet in fleetsInCombat)
			{
				foreach (ShipInstance ship in fleet.combatShips)
				{
					if (ship == shipToIgnore)
					{
						continue;
					}
					foreach (EquipmentInstance equipment in ship.Equipments)
					{
						float range = -1;
						if (equipment.Values.ContainsKey("range"))
						{
							range = (float)equipment.Values["range"];
						}
						if (range < 0)
						{
							continue;
						}
						//Add the ship's size to get the radius
						range += (int)ship.Values["ShipRadius"];
						List<Point> areCollisions = LineVsCircleCollision(new Point((int)ship.Values["XPos"], (int)ship.Values["YPos"]), range, pos, dimensions, angle);
						if (areCollisions != null)
						{
							foreach (Point point in areCollisions)
							{
								bool inList = false;
								foreach (Point pointAlreadyProcessed in pointsAlreadyProcessed)
								{
									//Add a bit of fudge due to floating point rounding errors
									if (point.X >= pointAlreadyProcessed.X - 2 && point.X <= pointAlreadyProcessed.X + 2 && point.Y >= pointAlreadyProcessed.Y - 2 && point.Y >= pointAlreadyProcessed.Y - 2)
									{
										inList = true;
										break;
									}
								}
								if (!inList)
								{
									collisions.Add(new Collision(point, equipment, ship));
								}
							}
						}
					}
					//Check the ship itself for collision, minus 1 so that equipment will be sorted in teh correct order
					List<Point> shipCollision = LineVsCircleCollision(new Point((int)ship.Values["XPos"], (int)ship.Values["YPos"]), (int)ship.Values["ShipRadius"] - 1, pos, dimensions, angle);
					if (shipCollision != null)
					{
						foreach (Point point in shipCollision)
						{
							bool inList = false;
							foreach (Point pointAlreadyProcessed in pointsAlreadyProcessed)
							{
								if (point.X == pointAlreadyProcessed.X && point.Y == pointAlreadyProcessed.Y)
								{
									inList = true;
									break;
								}
							}
							if (!inList)
							{
								collisions.Add(new Collision(point, ship));
							}
						}
					}
				}
			}

			//Sort the list here based on distance from xPos and yPos of the particle (the origin)
			collisions.Sort((Collision pointA, Collision pointB) => 
			{
				return (((x - pointA.Position.X) * (x - pointA.Position.X)) + ((y - pointA.Position.Y) * (y - pointA.Position.Y))).CompareTo(((x - pointB.Position.X) * (x - pointB.Position.X)) + ((y - pointB.Position.Y) * (y - pointB.Position.Y)));
			});

			return collisions;
		}

		private static List<Point> LineVsCircleCollision(Point circlePos, float circleRadius, PointF position, PointF dimensions, float angle)
		{
			//First, get the corner positions
			PointF pos1 = new PointF(position.X, position.Y); //The first corner
			PointF pos2 = new PointF(position.X + (float)(Math.Cos(angle) * dimensions.X), position.Y + (float)(Math.Sin(angle) * dimensions.X));

			//Get the rectangle area of collision, since it uses an infinite line
			PointF topLeft = new PointF();
			PointF bottomRight = new PointF();
			if (pos1.X < pos2.X)
			{
				topLeft.X = pos1.X;
				bottomRight.X = pos2.X;
			}
			else
			{
				topLeft.X = pos2.X;
				bottomRight.X = pos1.X;
			}
			if (pos1.Y < pos2.Y)
			{
				topLeft.Y = pos1.Y;
				bottomRight.Y = pos2.Y;
			}
			else
			{
				topLeft.Y = pos2.Y;
				bottomRight.Y = pos1.Y;
			}

			double dx = pos2.X - pos1.X;
			double dy = pos2.Y - pos1.Y;

			double A = dx * dx + dy * dy;
			double B = 2 * (dx * (pos1.X - circlePos.X) + dy * (pos1.Y - circlePos.Y));
			double C = (pos1.X - circlePos.X) * (pos1.X - circlePos.X) + (pos1.Y - circlePos.Y) * (pos1.Y - circlePos.Y) - (circleRadius * circleRadius);

			double det = B * B - 4 * A * C;

			if (A <= 0 || det < 0)
			{
				//No point
				return null;
			}
			double t;
			if (det == 0)
			{
				//One point
				t = -B / (2.0f * A);
				Point collision = new Point();
				collision.X = (int)(pos1.X + t * dx);
				collision.Y = (int)(pos1.Y + t * dx);
				if (collision.X >= topLeft.X && collision.X <= bottomRight.X && collision.Y >= topLeft.Y && collision.Y <= bottomRight.Y)
				{
					return new List<Point>(new[] { collision });
				}
				//Outside the collision area
				return null;
			}
			//Two points
			t = (float)((-B + Math.Sqrt(det)) / (2 * A));
			Point pointA = new Point();
			pointA.X = (int)(pos1.X + t * dx);
			pointA.Y = (int)(pos1.Y + t * dy);
			t = (float)((-B - Math.Sqrt(det)) / (2 * A));
			Point pointB = new Point();
			pointB.X = (int)(pos1.X + t * dx);
			pointB.Y = (int)(pos1.Y + t * dy);
			List<Point> impacts = new List<Point>();
			if (pointA.X >= topLeft.X && pointA.X <= bottomRight.X && pointA.Y >= topLeft.Y && pointA.Y <= bottomRight.Y)
			{
				impacts.Add(pointA);
			}
			if (pointB.X >= topLeft.X && pointB.X <= bottomRight.X && pointB.Y >= topLeft.Y && pointB.Y <= bottomRight.Y)
			{
				impacts.Add(pointB);
			}
			if (impacts.Count == 0)
			{
				return null;
			}
			return impacts;
		}

		private class Collision
		{
			public Point Position;
			public ShipInstance ShipCollision;
			public EquipmentInstance EquipmentCollision;

			public Collision(Point position, ShipInstance shipCollision)
			{
				this.Position = position;
				this.ShipCollision = shipCollision;
				this.EquipmentCollision = null;
			}

			public Collision(Point position, EquipmentInstance equipmentCollision, ShipInstance collidingShip)
			{
				this.Position = position;
				this.EquipmentCollision = equipmentCollision;
				this.ShipCollision = collidingShip;
			}
		}
	}
}
