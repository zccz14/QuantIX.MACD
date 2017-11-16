using System;
using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
	public class MacdIslandHeight : Indicator<double>
	{
		public MacdIslandHeight(IIndicator<double> source, IIndicator<int> left)
		{
			Source = source;
			Left = left;
			Source.Update += Source_Update;
		}

		private void Source_Update()
		{
			Data.FillRange(Count, Source.Count, i =>
			{
				if (i > 0 && Left[i] == Left[i - 1])
				{
					return Math.Max(Math.Abs(Source[i]), Data[i - 1]);
				}
				return Math.Abs(Source[i]);
			});
			FollowUp();
		}

		private IIndicator<double> Source { get; }
		private IIndicator<int> Left { get; }
	}
}