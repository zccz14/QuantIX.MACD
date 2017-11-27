using System;
using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    public class DIFFTendency : Indicator<DIFFTendency.Datum>
    {
        public DIFFTendency(IIndicator<double> source, double diffPlainEdge)
        {
            Source = source;
            DIFFPlainEdge = diffPlainEdge;
            Source.Update += SourceOnUpdate;
        }

        private void SourceOnUpdate()
        {
            Data.FillRange(Count, Source.Count, i =>
            {
                if (i == 0)
                {
                    return Source[0] > 0 ? Datum.Rise : Datum.Decline;
                }
                if (Source[i].Abs() < DIFFPlainEdge && Source[i - 1].Abs() < DIFFPlainEdge)
                {
                    return Datum.Plain;
                }
                return Source[i] > 0 ? Datum.Rise : Datum.Decline;
            });
            FollowUp();
        }

        public enum Datum
        {
            Plain, Rise, Decline//, RiseToPlain, DeclineToPlain, PlainToRise, PlainToDecline
        }


        public IIndicator<double> Source { get; }

        public double DIFFPlainEdge { get; }
    }
}