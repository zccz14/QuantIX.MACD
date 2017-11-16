using System;
using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
	public class MacdIsland
	{
		public MacdIsland(IIndicator<double> source, int islandMinCount)
		{
			Source = source;
			IslandMinCount = islandMinCount;
			Bars = new MacdIslandBars(source);
			Left = new MacdIslandLeft(source, islandMinCount, Bars);
			Height = new MacdIslandHeight(source, Left);
			ShrinkingCount = new MacdIslandShrinkingCount(Source, Left, Height);
			IsGood = new MacdIslandGood(Source, 3, Left, ShrinkingCount);
			RangeTuple = new MacdIslandRangeTuple(Left);
			IslandCounter = new MacdIslandCounter(Left);
		}

		private IIndicator<double> Source { get; }
		public int IslandMinCount { get; }
		public IIndicator<int> Bars { get; }
		public MacdIslandLeft Left { get; }
		/// <summary>
		/// The Height of Current Island (always >= 0)
		/// </summary>
		public IIndicator<double> Height { get; }
		public IIndicator<int> ShrinkingCount { get; }
		public IIndicator<bool> IsGood { get; }
		public IIndicator<Tuple<int, int>> RangeTuple { get; }
		/// <summary>
		/// Count the islands in the past
		/// </summary>
		public IIndicator<int> IslandCounter { get; }
	}

	public class MacdIslandCounter : Indicator<int>
	{
		public MacdIslandCounter(IIndicator<int> left)
		{
			Left = left;
			Left.Update += Left_Update;
		}

		private void Left_Update()
		{
			Data.FillRange(Count, Left.Count, i => i > 0 ? (Left[i - 1] != Left[i] ? Data[i - 1] + 1 : Data[i - 1]) : 1);
			FollowUp();
		}

		private IIndicator<int> Left { get; }
	}

	public class MacdIslandRangeTuple : Indicator<Tuple<int, int>>
	{
		public MacdIslandRangeTuple(IIndicator<int> left)
		{
			Left = left;
			Left.Update += Left_Update;
		}

		private void Left_Update()
		{
			Functions.Range(Prev, Left.Count).ForEach(i =>
			{
				if (i == 0)
				{
					Data.Add(Tuple.Create(0, 1));
				}
				else if (Left[i] != Left[i - 1])
				{
					Data[Count - 1] = Tuple.Create(Left[i - 1], Left[i]);
					Data.Add(Tuple.Create(Left[i], Left.Count));
				}
				else
				{
					Data[Count - 1] = Tuple.Create(Left[i], Left.Count);
				}
			});
			Prev = Left.Count;
			FollowUp();
		}

		private IIndicator<int> Left { get; }
		private int Prev { get; set; }
	}
}