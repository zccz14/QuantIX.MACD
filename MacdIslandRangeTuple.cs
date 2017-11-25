using System;
using QuantTC;
using QuantTC.Indicators.Generic;

namespace QuantIX.MACD
{
    /// <summary>
    /// 这里保存所有岛的起始位置，这个数据和source的个数是不同的
    /// Macd island range tuple.
    /// </summary>
    public class MacdIslandRangeTuple : Indicator<Tuple<int, int>>
    {
        public MacdIslandRangeTuple(IIndicator<int> left)
        {
            Left = left;
            Left.Update += Left_Update;
        }
        /// <summary>
        /// Prev初始值为0，到当前的Left.Count Range返回一个迭代器，从Prev，迭代到Left.Count
        /// i = 0表示开始位置，这个要新建一个Tuple 开始的位置为0，结束的位置为1 左闭右开
        /// 不然在迭代的时候就要不断的更新data的最后一个数据的闭合位置
        /// 如果发现一个新岛，则将当前的data的最后一个数据的右端确定，然后创建一个新的Tuple
        /// 如果不是新岛，就不断更新data的最后一个数据的闭合位置就好了，每次更新完一段数据，更新Prev的值
        /// Lefts the update.
        /// </summary>
        private void Left_Update()
        {
            Functions.Range(Prev, Left.Count).ForEach(i =>
            {
                if (i == 0)
                {
                    Data.Add(Tuple.Create(0, 1));
                }
                else if (Left[i] != Left[i - 1])
                {
                    Data[Count - 1] = Tuple.Create(Left[i - 1], Left[i]);
                    Data.Add(Tuple.Create(Left[i], Left.Count));
                }
                else
                {
                    Data[Count - 1] = Tuple.Create(Left[i], Left.Count);
                }
            });
            Prev = Left.Count;
            FollowUp();
        }

        private IIndicator<int> Left { get; }
        private int Prev { get; set; }
    }
}