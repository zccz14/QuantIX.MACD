using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    /// <summary>
    /// Macd island left.
    /// 这里用来保存每个岛的起始位置，就是如果source中的数据是同一个岛，那么这些data都指向这个岛的第一个数据的下标。
    /// </summary>
	public class MacdIslandLeft: Indicator<int>
    {
        /// <summary>
        /// 这里保存source，同时Ignore表示忽略多少个数据，这里表示如果这个岛的数据小于Ignore个，则不把它看作一个岛
        /// Initializes a new instance of the <see cref="T:QuantIX.MACD.MacdIslandLeft"/> class.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="ignore">Ignore.</param>
        /// <param name="bars">Bars.</param>
	    public MacdIslandLeft(IIndicator<double> source, int ignore, IIndicator<int> bars)
	    {
		    Source = source;
		    Ignore = ignore;
		    Bars = bars;
		    Source.Update += Macd_Update;
	    }
        /// <summary>
        /// Source[Data[i - 1]]表示前一个数据指向的那个岛的符号，如果与当前的数据不同，同时当前Bars[i] （当前数据是这个岛的第几个数据）大于忽略的数据的大小则表示一个新岛的left诞生
        /// 则用当前的i 减去 Ignore 得到新岛的开始下标
        /// Macds the update.
        /// </summary>
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
        /// <summary>
        /// this 如果返回的是当前的 this[index]表示当前的data[index]，这里是用的迭代，times表示迭代几次
        /// times 大于0 就把this[index] 变成index 传入this函数，就是data[index] 的值传入this函数
        /// 就是当前index这个岛的left，即当前岛的左端点的下标，然后再取this[this[index]]表示当前岛的前一个岛的左端点的下标
        /// Gets the <see cref="T:QuantIX.MACD.MacdIslandLeft"/> at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="times">Times.</param>
	    public int this[int index, int times] => times > 0 ? this[this[index], times - 1] : this[index];

		private IIndicator<double> Source { get; }
		public IIndicator<int> Bars { get; }
		public int Ignore { get; }
    }
}
