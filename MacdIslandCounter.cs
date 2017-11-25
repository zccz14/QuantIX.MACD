using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    /// <summary>
    /// 保存到当前位置岛的个数
    /// Macd island counter.
    /// </summary>
    public class MacdIslandCounter : Indicator<int>
    {
        public MacdIslandCounter(IIndicator<int> left)
        {
            Left = left;
            Left.Update += Left_Update;
        }
        /// <summary>
        /// 如果发现新岛就data[i] = data[i - 1] + 1 
        /// 否则就是和data[i - 1]相同
        /// Lefts the update.
        /// </summary>
        private void Left_Update()
        {
            Data.FillRange(Count, Left.Count, i => i > 0 ? (Left[i - 1] != Left[i] ? Data[i - 1] + 1 : Data[i - 1]) : 1);
            FollowUp();
        }

        private IIndicator<int> Left { get; }
    }
}