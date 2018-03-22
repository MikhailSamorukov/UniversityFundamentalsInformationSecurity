namespace Encryption
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Helpers;
    using Interfaces;

    public class EncryptionBySimpleSubstitution : IEncryptorByKey<List<int>>
    {
        public EncryptionBySimpleSubstitution()
        {
            _indexHelper = new IndexHelper();
        }

        private const char _delimiter = '_';
        private readonly IndexHelper _indexHelper;

        public List<int> GenerateRandomKey(string text) {
            var newKey = new List<int>();
            Random random = new Random();
            var matrixSize = Convert.ToInt32(Math.Round(Math.Sqrt(text.Length)));
            for (int i = 1; i < matrixSize + 1; i++)
            {
                newKey.Add(i);
            }
            for (int i = newKey.Count() - 1; i >= 1; i--)
            {
                int j = random.Next(i);
                var temp = newKey[j];
                newKey[j] = newKey[i];
                newKey[i] = temp;
            }
            return newKey;
        }

        public string Decrypt(string text, List<int> key)
        {
            var result = new StringBuilder();
            ValidateText(text);
            ValidateKey(text, key);
            var matrix = GetSplitedMatrixByRows(text);
            var changedMatrix = ChangeIndexesByKey(matrix, key);
            foreach (var listItem in changedMatrix)
            {
                result.Append(listItem.ToArray());
            }
            return result.ToString().Replace('_', ' ').Trim();
        }

        public string Encrypt(string text, List<int> key)
        {
            var result = new StringBuilder();
            ValidateText(text);
            ValidateKey(text, key);
            var matrix = GetSplitedMatrixByColumn(text);
            var modifiedKeys = key.Select(item => --item);
            foreach (var keyItem in modifiedKeys)
            {
                for (int i = 0; i < key.Count(); i++)
                {
                    result.Append(matrix[i][keyItem]);
                }
            }
            return result.ToString().Trim();
        }

        private void ValidateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text can't be null");
        }

        private void ValidateKey(string text, List<int> key)
        {
            var keySize = Convert.ToInt32(Math.Round(Math.Sqrt(text.Length)));

            if (key == null)
                throw new ArgumentException("Key can't be null");

            if (key.Count() != keySize)
                throw new ArgumentException($"The key length should be: {keySize}");

            if (key.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key).Any())
                throw new ArgumentException("The key doesn't should contain repeatable value");
        }

        private List<List<char>> GetSplitedMatrixByColumn(string text)
        {
            StringBuilder sb = new StringBuilder(text);
            var matrixSize = Convert.ToInt32(Math.Round(Math.Sqrt(text.Length)));
            var _initialMatrix = new List<List<char>>();
            for (int i = 0; i < matrixSize; i++)
            {
                _initialMatrix.Add(new List<char>());
                foreach (var item in sb.ToString().Take(matrixSize))
                {
                    if (item == ' ')
                        _initialMatrix[i].Add(_delimiter);
                    else
                        _initialMatrix[i].Add(item);
                }
                sb.Remove(0, sb.Length > matrixSize ? matrixSize : sb.Length);
                _initialMatrix[i].AddRange(_indexHelper.Fill(_delimiter, matrixSize - _initialMatrix[i].Count()));
            }
            return _initialMatrix;
        }

        private List<List<char>> GetSplitedMatrixByRows(string text)
        {
            StringBuilder sb = new StringBuilder(text);
            var matrixSize = Convert.ToInt32(Math.Round(Math.Sqrt(text.Length)));
            var _initialMatrix = new List<List<char>>();

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (i == 0) _initialMatrix.Add(new List<char>());
                    if (sb.Length > 0)
                    {
                        _initialMatrix[i].Add(sb[0]);
                        sb.Remove(0, 1);
                    }
                    else _initialMatrix[i].Add(_delimiter);
                }
            }
            return _initialMatrix;
        }

        private List<List<char>> ChangeIndexesByKey(List<List<char>> matrix, List<int> encryptionKey)
        {
            var result = new List<List<char>>();
            var size = encryptionKey.Count();
            var modifiedKeys = encryptionKey.Select(item => --item).ToList();
            for (int i = 0; i < size; i++)
            {
                result.Add(new List<char>());
                result[i].AddRange(_indexHelper.Fill(_delimiter, size));
            }
            var counter = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i][counter] = matrix[modifiedKeys.FindIndex(element => element == counter)][i];
                    counter++;
                }
                counter = 0;
            }
            return result;
        }
    }
}
