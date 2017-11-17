using System;
using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    /// <summary>
    /// 保存反向收缩的个数，就是说一个岛本来是下降了，但是下降了之后有上涨了，表明这个岛不是一个好岛
    /// 这个时候需要记录这个反向收缩的数据的个数，
    /// Macd island shrinking count.
    /// </summary>
	public class MacdIslandShrinkingCount : Indicator<int>
	{
		public MacdIslandShrinkingCount(IIndicator<double> source, IIndicator<int> left, IIndicator<double> height)
		{
			Source = source;
			Left = left;
			Height = height;
			Source.Update += Source_Update;
		}
        /// <summary>
        /// i > 0 && Left[i] == Left[i - 1] 表示在同一个岛中
        /// Height[i] > Math.Abs(Source[i]) 表示到当前位置，岛的最高点比当前点要高，表示这个岛开始下降了
        /// 上升的时候data里面的值为0，下降的时候开始增加，如果又上升了又清零，这样这个位置就可以记录这个收缩的数据的个数
        /// Sources the update.
        /// </summary>
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