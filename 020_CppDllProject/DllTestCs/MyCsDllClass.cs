using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllTestCs
{
    public class MyCsDllClass
    {
        public List<MyData> DataList = new List<MyData>();

        /// <summary>
        /// staticなメソッド。ただの足し算を行う。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Add(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// データリストに引数のデータを追加する。
        /// 追加後のデータリストの件数を返す。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int AddMyData(MyData data)
        {
            if (data != null)
            {
                DataList.Add(data);
            }

            return DataList.Count();
        }

        /// <summary>
        /// 引数で指定したindexを持つデータがリスト内にあればそれを返す。
        /// 無ければnullを返す。
        /// </summary>
        /// <param name="serachIndex"></param>
        /// <returns></returns>
        public MyData DataSearch(int serachIndex)
        {
            var result = DataList.Where(x => x.index == serachIndex);
            return result.Any() ? result.First() : null;
        }
    }

    /// <summary>
    /// お試しデータのクラス。
    /// </summary>
    public class MyData
    {
        public int index;
        public double data;
    }
}
