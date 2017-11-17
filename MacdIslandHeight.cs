using System;
using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    /// <summary>
    /// 这里用来记录一个岛中的最高值的绝对值
    /// Macd island height.
    /// </summary>
	public class MacdIslandHeight : Indicator<double>
	{
		public MacdIslandHeight(IIndicator<double> source, IIndicator<int> left)
		{
			Source = source;
			Left = left;
			Source.Update += Source_Update;
		}
        /// <summary>
        /// Left[i] == Left[i - 1] 表示是同一个岛，在同一个岛中，每一个high表示从开始位置到当前位置的最高值，并不是整个岛的最高值
        /// 只表示到当前位置的最高值。
        /// Sources the update.
        /// </summary>
		private void Source_Update()
		{
			Data.FillRange(Count, Source.Count, i =>
			{
				if (i > 0 && Left[i] == Left[i - 1])
				{
					return Math.Max(Math.Abs(Source[i]), Data[i - 1]);
				}
				return Math.Abs(Source[i]);
			});
			FollowUp();
		}

		private IIndicator<double> Source { get; }
		private IIndicator<int> Left { get; }
	}
}