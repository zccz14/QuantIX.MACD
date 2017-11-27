using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    public class DEASlope : Indicator<double>
    {
        public DEASlope(IIndicator<double> source)
        {
            Source = source;
            Source.Update += SourceOnUpdate;
        }

        private void SourceOnUpdate()
        {
            Data.FillRange(Count, Source.Count, i => i > 0 ?  Source[i] - Source[i - 1] : Source[0]);
            FollowUp();
        }


        public IIndicator<double> Source { get; }
    }
}