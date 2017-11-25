using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    /// <summary>
    /// Macd island bars.
    /// 这里记录每个数据是在它所在岛的第几个数据
    /// </summary>
	public class MacdIslandBars : Indicator<int>
	{
        /// <summary>
        /// 这里保存source 数据 并挂载监听
        /// Initializes a new instance of the <see cref="T:QuantIX.MACD.MacdIslandBars"/> class.
        /// </summary>
        /// <param name="source">Source.</param>
		public MacdIslandBars(IIndicator<double> source)
		{
			Source = source;
			Source.Update += Source_Update;
		}
        /// <summary>
        /// 这里更新数据，每个source数据比较当前位置与前一个位置的符号是否相同，如果相同就把当前data[i] = data[i - 1] + 1
        /// 否则就是一个新岛，此时data[i] = 1
        /// Sources the update.
        /// </summary>
		private void Source_Update()
		{
			Data.FillRange(Count, Source.Count, i => i <= 0 || !Source[i].IsSameSigned(Source[i - 1]) ? 1 : Data[i - 1] + 1);
			FollowUp();
		}

		private IIndicator<double> Source { get; }
	}
}