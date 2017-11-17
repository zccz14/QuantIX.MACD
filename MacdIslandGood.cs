using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    /// <summary>
    /// 这个用bool类型来标记当前这个岛是不是好岛，和high函数也是一样，只保存到当前位置是不是好岛如果要取得这个岛是不是好岛，就要找到这个
    /// 岛的右边的结束位置，去取他的值。
    /// Macd island good.
    /// </summary>
	public class MacdIslandGood : Indicator<bool>
	{
		public MacdIslandGood(IIndicator<double> source, int maxCount, IIndicator<int> left, IIndicator<int> shrinkingCount)
		{
			Source = source;
			MaxCount = maxCount;
			Left = left;
			ShrinkingCount = shrinkingCount;
			Source.Update += Source_Update;
		}
        /// <summary>
        /// 这个地方也是只保留到当前位置这个岛是不是个好岛。
        /// Left[i] == Left[i - 1] 判定是同一个岛，如果是新岛的开始，就直接是true
        /// ShrinkingCount[i] == 0 && ShrinkingCount[i - 1] > MaxCoun 用来判定只有当重新出现ShrinkingCount[i] == 0
        /// 的时候才能只当当前的岛是不好的，之后的所有的位置都是false。
        /// Sources the update.
        /// </summary>
		private void Source_Update()
        {
			Data.FillRange(Count, Source.Count, i =>
			{
				if (i > 0 && Left[i] == Left[i - 1])
				{
					if (!Data[i - 1] || ShrinkingCount[i] == 0 && ShrinkingCount[i - 1] > MaxCount)
					{
						return false;
					}
				}
				return true;
			});
		}

		private IIndicator<double> Source { get; }
		public int MaxCount { get; }
		private IIndicator<int> Left { get; }
		private IIndicator<int> ShrinkingCount { get; }
	}
}