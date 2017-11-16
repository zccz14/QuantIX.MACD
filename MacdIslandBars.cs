using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
	public class MacdIslandBars : Indicator<int>
	{
		public MacdIslandBars(IIndicator<double> source)
		{
			Source = source;
			Source.Update += Source_Update;
		}

		private void Source_Update()
		{
			Data.FillRange(Count, Source.Count, i => i <= 0 || !Source[i].IsSameSigned(Source[i - 1]) ? 1 : Data[i - 1] + 1);
			FollowUp();
		}

		private IIndicator<double> Source { get; }
	}
}