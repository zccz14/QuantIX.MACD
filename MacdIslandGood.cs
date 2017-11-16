using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
	public class MacdIslandGood : Indicator<bool>
	{
		public MacdIslandGood(IIndicator<double> source, int maxCount, IIndicator<int> left, IIndicator<int> shinkingCount)
		{
			Source = source;
			MaxCount = maxCount;
			Left = left;
			ShinkingCount = shinkingCount;
			Source.Update += Source_Update;
		}

		private void Source_Update()
		{
			Data.FillRange(Count, Source.Count, i =>
			{
				if (i > 0 && Left[i] == Left[i - 1])
				{
					if (!Data[i - 1] || ShinkingCount[i] == 0 && ShinkingCount[i - 1] > MaxCount)
					{
						return false;
					}
				}
				return true;
			});
		}

		private IIndicator<double> Source { get; }
		public int MaxCount { get; }
		private IIndicator<int> Left { get; }
		private IIndicator<int> ShinkingCount { get; }
	}
}