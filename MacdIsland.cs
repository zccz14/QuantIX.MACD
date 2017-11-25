using System;
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
}