namespace Encryption
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using POCO;
    using Interfaces;
    using Helpers;

    public class EncryptionByPleyfer : IEncryptor
    {
        public EncryptionByPleyfer()
        {
            _indexHelper = new IndexHelper();
        }

        private IndexHelper _indexHelper;

        private List<List<char>> _key => new List<List<char>>
        {
            new List<char>(){ 'а', 'б', 'в', 'г', 'д', 'е', 'ж', 'з' },
            new List<char>(){ 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п' },
            new List<char>(){ 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч' },
            new List<char>(){ 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' }
        };

        public string Encrypt(string text)
        {
            ValidateText(text);
            return RunAlgorithmWithShift(text, 1);
        }

        public string Decrypt(string text)
        {
            ValidateText(text);
            return RunAlgorithmWithShift(text, -1);
        }

        private string RunAlgorithmWithShift(string text, int shift)
        {
            var sb = new StringBuilder();
            foreach (var item in Split(text))
            {
                var firstLetterIndexes = _indexHelper.GetIndexes(_key, item.FirstLetter);
                var secondLetterIndexes = _indexHelper.GetIndexes(_key, item.LastLetter);
                if (firstLetterIndexes.RowIndex == secondLetterIndexes.RowIndex)
                {
                    firstLetterIndexes.ColumnIndex = ShiftIndex(firstLetterIndexes.ColumnIndex, shift, true);
                    secondLetterIndexes.ColumnIndex = ShiftIndex(secondLetterIndexes.ColumnIndex, shift, true);
                }
                else if (firstLetterIndexes.ColumnIndex == secondLetterIndexes.ColumnIndex)
                {
                    firstLetterIndexes.RowIndex = ShiftIndex(firstLetterIndexes.RowIndex, shift, false);
                    secondLetterIndexes.RowIndex = ShiftIndex(secondLetterIndexes.RowIndex, shift, false);
                }
                else
                {
                    SwapRows(firstLetterIndexes, secondLetterIndexes);
                }
                sb.Append(GetCharePareByIndex(firstLetterIndexes, secondLetterIndexes).ToString());
            }
            return sb.ToString();
        }

        private void SwapRows(ArrayIndexes firstLetterIndexes, ArrayIndexes secondLetterIndexes)
        {
            var columnIndexToTransfer = firstLetterIndexes.ColumnIndex;
            firstLetterIndexes.ColumnIndex = secondLetterIndexes.ColumnIndex;
            secondLetterIndexes.ColumnIndex = columnIndexToTransfer;
        }

        private int ShiftIndex(int index, int shift, bool isColumnIndex)
        {
            var incrementCandidate = index;
            incrementCandidate += shift;
            var max = isColumnIndex ? _indexHelper.GetMaxColumnIndex(_key) : _indexHelper.GetMaxRowIndex(_key);
            if (incrementCandidate > max)
            {
                index = 0;
            }
            else if (incrementCandidate < 0)
            {
                index = max;
            }
            else
            {
                index += shift;
            }
            return index;
        }

        private CharPare GetCharePareByIndex(ArrayIndexes firstLetterIndexes, ArrayIndexes secondLetterIndexes)
        {
            return new CharPare
            {
                FirstLetter = _key[firstLetterIndexes.RowIndex][firstLetterIndexes.ColumnIndex],
                LastLetter = _key[secondLetterIndexes.RowIndex][secondLetterIndexes.ColumnIndex],
            };
        }

        private void ValidateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text can't be null");

            if (text.Length % 2 != 0)
                throw new ArgumentException("The length of the text should be divided into two");

            if (!(text.ToCharArray().Any(element => _key.Any(keys => keys.Contains(element)))))
                throw new ArgumentException("Unsupported symbol");

            if (Split(text).Any(element => element.FirstLetter == element.LastLetter))
                throw new ArgumentException("Found equal elements in pare");
        }

        private List<CharPare> Split(string text)
        {
            var sb = new StringBuilder(text);
            var result = new List<CharPare>();
            while (sb.Length > 0)
            {
                result.Add(
                    new CharPare
                    {
                        FirstLetter = sb[0],
                        LastLetter = sb[1]
                    });
                sb.Remove(0, 2);
            }
            return result;
        }
    }
}
