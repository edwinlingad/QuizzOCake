using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace L2L.WebApi.Helper
{
    public class IntArray
    {
        private List<int> _intList = new List<int>();
        public IntArray(string strArray)
        {
            if (strArray != null & string.IsNullOrEmpty(strArray) == false)
            {
                var strArrayTmp = strArray.Substring(0, strArray.Length - 1);
                _intList = strArrayTmp.Split(',').Select(s => Int32.Parse(s)).ToList();
            }
        }

        public void IncAtIndex(int index)
        {
            while (_intList.Count() - 1 < index)
                _intList.Add(0);
            _intList[index]++;
        }

        public void DecAtIndex(int index)
        {
            while (_intList.Count() - 1 < index)
                _intList.Add(0);
            _intList[index]--;

            if (_intList[index] < 0)
                _intList[index] = 0;
        }

        public int SubAtIndex(int index, int count)
        {
            
            while (_intList.Count() - 1 < index)
                _intList.Add(0);
            var diff = _intList[index] - count;
            var ret = _intList[index] < count ? _intList[index] : count;
            _intList[index] = diff > 0 ? diff : 0;

            return ret;
        }

        public void ResetAtIndex(int index)
        {
            while (_intList.Count() - 1 < index)
                _intList.Add(0);

            _intList[index] = 0;
        }

        public string GetStringArray()
        {
            StringBuilder str = new StringBuilder();
            foreach (var item in _intList)
                str.Append(item.ToString() + ",");

            return str.ToString();
        }

        public int GetAtIndex(int index)
        {
            if (index > _intList.Count() - 1)
                return 0;
            return _intList[index];
        }

        public int GetTotal()
        {
            return _intList.Sum();
        }
    }
}