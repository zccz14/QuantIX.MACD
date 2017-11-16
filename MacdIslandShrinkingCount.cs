using System;
using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
	public class MacdIslandShrinkingCount : Indicator<int>
	{
		public MacdIslandShrinkingCount(IIndicator<double> source, IIndicator<int> left, IIndicator<double> height)
		{
			Source = source;
			Left = left;
			Height = height;
			Source.Update += Source_Update;
		}

		private void Source_Update()
		{
			Data.FillRange(Count, Source.Count, i =>
			{
				if (i > 0 && Left[i] == Left[i - 1] && Height[i] > Math.Abs(Source[i]))
				{
					return Data[i - 1] + 1;
				}
				return 0;
			});
		}

		private IIndicator<double> Source { get; }
		private IIndicator<int> Left { get; }
		private IIndicator<double> Height { get; }
	}
}