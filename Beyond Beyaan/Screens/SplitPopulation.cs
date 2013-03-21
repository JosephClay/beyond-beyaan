using System.Collections.Generic;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	class SplitPopulation : WindowInterface
	{
		public delegate void DoneFunction(Dictionary<Race, int> races);
		private DoneFunction doneFunction;

		//Right is the fleet to split, left is the unselected ships left behind

		private Button splitButton;
		private ScrollBar[] popScrollBarsLeft;
		private Label[] popLabelsLeft;
		private ScrollBar[] popScrollBarsRight;
		private Label[] popLabelsRight;
		private ScrollBar raceScrollBar;
		private ProgressBar leftTotalBar;
		private ProgressBar rightTotalBar;
		private Label leftCapacityLabel;
		private Label rightCapacityLabel;

		private int leftCapacity;
		private int rightCapacity;

		private int maxRaceVisible;
		private List<Race> races;
		private Dictionary<Race, int> populationToSplit;
		private List<int> totalEach;

		public SplitPopulation(int centerX, int centerY, GameMain gameMain, DoneFunction doneFunction)
			: base(centerX, centerY, 10, 10, string.Empty, gameMain, false)
		{
			this.doneFunction = doneFunction;

			popScrollBarsLeft = new ScrollBar[5];
			popScrollBarsRight = new ScrollBar[popScrollBarsLeft.Length];
			popLabelsLeft = new Label[popScrollBarsLeft.Length];
			popLabelsRight = new Label[popScrollBarsLeft.Length];

			for (int i = 0; i < popScrollBarsLeft.Length; i++)
			{
				popScrollBarsLeft[i] = new ScrollBar(centerX - 175, centerY + 40 + (45 * i), 16, 128, 1, 100, true, true, DrawingManagement.HorizontalSliderBar);
				popScrollBarsRight[i] = new ScrollBar(centerX + 25, centerY + 40 + (45 * i), 16, 128, 1, 100, true, true, DrawingManagement.HorizontalSliderBar);

				popLabelsLeft[i] = new Label(centerX - 175, centerY + 15 + (45 * i));
				popLabelsRight[i] = new Label(centerX + 25, centerY + 15 + (45 * i));
			}

			windowHeight = 400;
			windowWidth = 440;

			backGroundImage.MoveTo(xPos - windowWidth / 2, yPos);
			backGroundImage.SetDimensions(windowWidth, windowHeight);

			splitButton = new Button(SpriteName.TransferButtonBG, SpriteName.TransferButtonFG, string.Empty, centerX - 38, centerY + 350, 75, 35);
			splitButton.SetToolTip(DrawingManagement.BoxBorderBG, DrawingManagement.GetFont("Computer"), "Confirm population split", "confirmPopulationSplitToolTip", 30, 13, gameMain.ScreenWidth, gameMain.ScreenHeight);
			raceScrollBar = new ScrollBar(centerX + 183, yPos + 15, 16, 253, 5, 5, false, false, DrawingManagement.VerticalScrollBar);

			leftTotalBar = new ProgressBar(xPos - 175, centerY + 330, 150, 16, 100, 100, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
			rightTotalBar = new ProgressBar(xPos + 25, centerY + 330, 150, 16, 100, 100, SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
			leftCapacityLabel = new Label(xPos - 175, centerY + 301);
			rightCapacityLabel = new Label(xPos + 25, centerY + 301);
		}

		public override void DrawWindow(DrawingManagement drawingManagement)
		{
			backGroundImage.Draw(drawingManagement);

			for (int i = 0; i < maxRaceVisible; i++)
			{
				popLabelsLeft[i].Draw();
				popScrollBarsLeft[i].Draw(drawingManagement);
				popLabelsRight[i].Draw();
				popScrollBarsRight[i].Draw(drawingManagement);
			}
			splitButton.Draw(drawingManagement);
			leftTotalBar.Draw(drawingManagement);
			rightTotalBar.Draw(drawingManagement);
			raceScrollBar.Draw(drawingManagement);
			leftCapacityLabel.Draw();
			rightCapacityLabel.Draw();
			splitButton.DrawToolTip(drawingManagement);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			for (int i = 0; i < maxRaceVisible; i++)
			{
				if (popScrollBarsLeft[i].MouseHover(x, y, frameDeltaTime))
				{
					populationToSplit[races[i + raceScrollBar.TopIndex]] = totalEach[i + raceScrollBar.TopIndex] - popScrollBarsLeft[i].TopIndex;
					UpdateLabelsAndCapacityAndValidate();
				}
				if (popScrollBarsRight[i].MouseHover(x, y, frameDeltaTime))
				{
					populationToSplit[races[i + raceScrollBar.TopIndex]] = popScrollBarsRight[i].TopIndex;
					UpdateLabelsAndCapacityAndValidate();
				}
			}
			if (raceScrollBar.MouseHover(x, y, frameDeltaTime))
			{
				UpdateLabelsAndCapacityAndValidate();
			}
			splitButton.MouseHover(x, y, frameDeltaTime);
			return true;
		}

		public override bool MouseDown(int x, int y)
		{
			for (int i = 0; i < maxRaceVisible; i++)
			{
				popScrollBarsLeft[i].MouseDown(x, y);
				popScrollBarsRight[i].MouseDown(x, y);
			}
			raceScrollBar.MouseDown(x, y);
			splitButton.MouseDown(x, y);
			return true;
		}

		public override bool MouseUp(int x, int y)
		{
			for (int i = 0; i < maxRaceVisible; i++)
			{
				if (popScrollBarsLeft[i].MouseUp(x, y))
				{
					populationToSplit[races[i + raceScrollBar.TopIndex]] = totalEach[i + raceScrollBar.TopIndex] - popScrollBarsLeft[i].TopIndex;
					UpdateLabelsAndCapacityAndValidate();
				}
				if (popScrollBarsRight[i].MouseUp(x, y))
				{
					populationToSplit[races[i + raceScrollBar.TopIndex]] = popScrollBarsRight[i].TopIndex;
					UpdateLabelsAndCapacityAndValidate();
				}
			}
			if (raceScrollBar.MouseUp(x, y))
			{
				UpdateLabelsAndCapacityAndValidate();
			}
			if (splitButton.MouseUp(x, y))
			{
				doneFunction(populationToSplit);
			}
			return true;
		}

		public void LoadSplitPopulation(Squadron fleetToSplit, Squadron fleetToLeave)
		{
			// No people to transport
			if (fleetToLeave.TotalPeopleInTransit == 0)
			{
				doneFunction(new Dictionary<Race, int>());
				return;
			}
			//New fleet don't have transport capacity
			if (fleetToSplit.TransportCapacity == 0)
			{
				doneFunction(new Dictionary<Race, int>());
				return;
			}

			Squadron remainingShipsFleet = new Squadron(fleetToLeave.System);
			for (int i = 0; i < fleetToLeave.Ships.Count; i++)
			{
				if (!fleetToSplit.Ships.Contains(fleetToLeave.Ships[i]))
				{
					//This ship won't be in the new fleet
					remainingShipsFleet.AddShipItself(fleetToLeave.Ships[i]);
				}
			}

			if (remainingShipsFleet.TransportCapacity == 0)
			{
				//The ships left behind don't have any transport capacity, move all population to the new fleet
				doneFunction(new Dictionary<Race, int>(fleetToLeave.PopulationInTransit));
				return;
			}
			//At this point, we will need to split the population
			populationToSplit = new Dictionary<Race, int>();

			if (fleetToLeave.PopulationInTransit.Count == 1)
			{
				int total = fleetToSplit.TransportCapacity + remainingShipsFleet.TransportCapacity;
				foreach (KeyValuePair<Race, int> race in fleetToLeave.PopulationInTransit)
				{
					if (race.Value == total)
					{
						populationToSplit.Add(race.Key, fleetToSplit.TransportCapacity);
						//Split the population automatically
						doneFunction(populationToSplit);
						return;
					}
				}
			}

			//Can't resolve automatically, so set up the data
			races = new List<Race>();
			totalEach = new List<int>();

			//FleetToSplit will be the right column, fleetToLeave on left, and fleetToLeave is the fleet before splitting
			if (fleetToLeave.PopulationInTransit.Count > 5)
			{
				raceScrollBar.SetEnabledState(true);
				raceScrollBar.SetAmountOfItems(fleetToLeave.PopulationInTransit.Count);
				raceScrollBar.TopIndex = 0;
				maxRaceVisible = 5;
			}
			else
			{
				raceScrollBar.SetEnabledState(false);
				raceScrollBar.SetAmountOfItems(5);
				raceScrollBar.TopIndex = 0;
				maxRaceVisible = fleetToLeave.PopulationInTransit.Count;
			}

			foreach (KeyValuePair<Race, int> population in fleetToLeave.PopulationInTransit)
			{
				populationToSplit.Add(population.Key, population.Value); //This will be modified, the below two won't be
				races.Add(population.Key);
				totalEach.Add(population.Value);
			}

			for (int i = 0; i < maxRaceVisible; i++)
			{
				int maxAmount = populationToSplit[races[i]];
				popScrollBarsLeft[i].SetAmountOfItems(maxAmount + 1);
				popScrollBarsLeft[i].TopIndex = 0;
				popScrollBarsRight[i].SetAmountOfItems(maxAmount + 1);
				popScrollBarsRight[i].TopIndex = maxAmount;
			}

			rightCapacity = fleetToSplit.TransportCapacity;
			leftCapacity = remainingShipsFleet.TransportCapacity;

			leftTotalBar.SetMaxProgress(leftCapacity);
			rightTotalBar.SetMaxProgress(rightCapacity);

			UpdateLabelsAndCapacityAndValidate();
		}

		private void UpdateLabelsAndCapacityAndValidate()
		{
			for (int i = 0; i < maxRaceVisible; i++)
			{
				popScrollBarsLeft[i].TopIndex = totalEach[i + raceScrollBar.TopIndex] - populationToSplit[races[i + raceScrollBar.TopIndex]];
				popScrollBarsRight[i].TopIndex = populationToSplit[races[i + raceScrollBar.TopIndex]];
				popLabelsLeft[i].SetText(popScrollBarsLeft[i].TopIndex + " " + races[raceScrollBar.TopIndex + i].RaceName);
				popLabelsRight[i].SetText(popScrollBarsRight[i].TopIndex + " " + races[raceScrollBar.TopIndex + i].RaceName);
			}

			int leftTotal = 0;
			int rightTotal = 0;
			for (int i = 0; i < races.Count; i++)
			{
				rightTotal += populationToSplit[races[i]];
				leftTotal += totalEach[i] - populationToSplit[races[i]];
			}

			leftTotalBar.SetProgress(leftTotal);
			leftCapacityLabel.SetText(leftTotal + " / " + leftCapacity);
			leftTotalBar.SetColor(leftTotal > leftCapacity ? System.Drawing.Color.Red : System.Drawing.Color.Green);

			rightTotalBar.SetProgress(rightTotal);
			rightCapacityLabel.SetText(rightTotal + " / " + rightCapacity);
			rightTotalBar.SetColor(rightTotal > rightCapacity ? System.Drawing.Color.Red : System.Drawing.Color.Green);

			if (leftTotal > leftCapacity || rightTotal > rightCapacity)
			{
				splitButton.Active = false;
			}
			else
			{
				splitButton.Active = true;
			}
		}
	}
}
