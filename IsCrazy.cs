using System;
using QuantTC;
using QuantTC.Data;
using QuantTC.Indicators.Generic;
using static QuantTC.Functions;
using System.Linq;

namespace QuantIX.MACD
{
    public class IsCrazy : Indicator<bool>
    {
        public IsCrazy(IIndicator<IMacd> source, int length, double edge)
        {
            Source = source;
            Length = length;
            Edge = edge;
            Source.Update += SourceOnUpdate;
        }

        private void SourceOnUpdate()
        {
            Data.FillRange(Count, Source.Count, 
                i => i > Length && (RangeRight(0, i + 1).Take(Length).All(j => Source[j].Diff.Abs() > Edge)));
            FollowUp();
        }


        public IIndicator<IMacd> Source { get; }

        public int Length { get; }

        public double Edge { get; }
    }
}