using System;
using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    public class DIFFSlope : Indicator<double>
    {
        public DIFFSlope(IIndicator<double> source)
        {
            Source = source;
            Source.Update += SourceOnUpdate;
        }

        private void SourceOnUpdate()
        {
            Data.FillRange(Count, Source.Count, i => i > 0 ? Source[i] - Source[i - 1] :  Source[0]);
            FollowUp();
        }


        public IIndicator<double> Source { get; }
    }
}