using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
	public class MacdIslandLeft: Indicator<int>
    {
	    public MacdIslandLeft(IIndicator<double> source, int ignore, IIndicator<int> bars)
	    {
		    Source = source;
		    Ignore = ignore;
		    Bars = bars;
		    Source.Update += Macd_Update;
	    }

		private void Macd_Update()
		{
			Data.FillRange(Count, Source.Count, i =>
			{
				if (i == 0) return 0;
				if (Source[Data[i - 1]].IsDiffSigned(Source[i]) && Bars[i] > Ignore)
				{
					return i - Ignore;
				}
				return Data[i - 1];
			});
			FollowUp();
		}

	    public int this[int index, int times] => times > 0 ? this[this[index], times - 1] : this[index];

		private IIndicator<double> Source { get; }
		public IIndicator<int> Bars { get; }
		public int Ignore { get; }
    }
}
