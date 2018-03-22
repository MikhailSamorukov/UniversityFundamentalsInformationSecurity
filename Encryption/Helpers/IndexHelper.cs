namespace Encryption.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Encryption.POCO;

    class IndexHelper
    {
        internal int GetMaxRowIndex(List<List<char>> array) => array.Count() - 1;

        internal int GetMaxColumnIndex(List<List<char>> array) => array.First().Count() - 1;

        internal ArrayIndexes GetIndexes(List<List<char>> array, char symbol)
        {
            return new ArrayIndexes()
            {
                ColumnIndex = array[array.FindIndex(list => list.Any(item => item == symbol))].FindIndex(item => item == symbol),
                RowIndex = array.FindIndex(list => list.Any(item => item == symbol))
            };
        }
        internal List<T> Fill<T>(T FillVal, int length)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < length; i++)
                result.Add(FillVal);
            return result;
        }
    }
}
