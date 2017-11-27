using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    public class DEATendency : Indicator<DEATendency.DEADatum>
    {
        public DEATendency(IIndicator<double> source, double deaPlainEdge)
        {
            Source = source;
            DEAPlainEdge = deaPlainEdge;
            Source.Update += SourceOnUpdate;
        }

        private void SourceOnUpdate()
        {
            Data.FillRange(Count, Source.Count, i =>
            {
                if (i == 0)
                {
                    return Source[0] > 0 ? DEADatum.Rise : DEADatum.Decline;
                }
                if (Source[i].Abs() < DEAPlainEdge && Source[i - 1].Abs() < DEAPlainEdge)
                {
                    return DEADatum.Plain;
                }
                return Source[i] > 0 ? DEADatum.Rise : DEADatum.Decline;
            });
            FollowUp();
        }

        public enum DEADatum
        {
            Plain, Rise, Decline//, RiseToPlain, DeclineToPlain, PlainToRise, PlainToDecline
        }


        public IIndicator<double> Source { get; }

        public double DEAPlainEdge { get; }
    }
}