using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace L2L.WebApi.Utilities
{
    public static class HelperUtil
    {
        public static int[] GetIntArrayFromString(string strArray)
        {
            int[] intArr = new int[0];
            if (strArray != null & string.IsNullOrEmpty(strArray) == false)
            {
                var strArrayTmp = strArray.Substring(0, strArray.Length - 1);
                intArr = strArrayTmp.Split(',').Select(s => Int32.Parse(s)).ToArray();
            }

            return intArr;
        }

        public static int[] GetIntArrayFromString(string strArray, int size)
        {
            var list = new List<int>();
            for (int i = 0; i < size; i++)
                list.Add(0);
            var intArr = list.ToArray();
            var intArrTmp = GetIntArrayFromString(strArray);

            for (int i = 0; i < intArrTmp.Count(); i++)
                intArr[i] = intArrTmp[i];

            return intArr;
        }

        public static string GetStrFromIntArray(int[] intArray)
        {
            StringBuilder str = new StringBuilder();
            foreach (var item in intArray)
                str.Append(item.ToString() + ",");

            return str.ToString();
        }
    }
}