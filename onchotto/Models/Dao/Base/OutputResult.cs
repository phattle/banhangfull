using System;

namespace OnChotto.Models.Dao
{
    public class OutputResult
    {
        private OutputResult[] Collection = null;
        private int currIndex = 0;

        public OutputResult()
        {
        }

        public string Name { set; get; }
        public object Value { set; get; }

        public Int32 Length
        {
            get
            {
                if (Collection == null)
                    return 0;
                return Collection.Length;
            }
        }

        public Int64 LongLength
        {
            get
            {
                if (Collection == null)
                    return 0;
                return Collection.LongLength;
            }
        }

        public OutputResult[] ResultArray
        {
            get { return Collection; }
        }

        public void Add(OutputResult Info)
        {
            Collection[currIndex++] = Info;
        }

        public object GetValue(string itemName)
        {
            if (Collection == null || Collection.Length == 0 || string.IsNullOrEmpty(itemName))
                return null;
            for (int i = 0; i < Collection.Length; i++)
                if (Collection[i].Name.Trim().ToLower() == itemName.Trim().ToLower())
                    return Collection[i].Value;
            return null;
        }

        public object GetValue(Int32 itemIndex)
        {
            if (Collection == null || Collection.Length == 0 || itemIndex < 0 || itemIndex >= Collection.Length)
                return null;
            return Collection[itemIndex].Value;
        }

        //public object GetValue(Int64 index)
        //{
        //    if (Collection == null || Collection.LongLength == 0 || index < 0 || index >= Collection.LongLength)
        //        return null;
        //    return Collection[index].Value;
        //}

        public void Initialize(int count)
        {
            Collection = new OutputResult[count];
        }

        //public void Initialize(Int64 count)
        //{
        //    Collection = new OutputResult[count];
        //}
    
    }
}
