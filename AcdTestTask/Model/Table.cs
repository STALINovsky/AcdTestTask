using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ACDTestTask.Model
{
    /// <summary>
    /// Class to work with data in table format
    /// </summary>
    public class Table
    {
        private List<List<string>> rowsList;
        /// <summary>
        /// Create new Table by collection of rows
        /// </summary>
        /// <param name="rowsList">collection of rows (every row is collection)</param>
        public Table(List<List<string>> rowsList)
        {
            this.rowsList = rowsList;
        }

        /// <summary>
        /// Collection of rows
        /// </summary>
        public IReadOnlyCollection<IReadOnlyCollection<string>> Rows => rowsList;

        /// <summary>
        /// Count of table rows
        /// </summary>
        public int RowsCount => rowsList.Count;
        /// <summary>
        /// Count of table columns
        /// </summary>
        public int ColumnCount => rowsList.Max(row => row.Count);

        /// <summary>
        /// Check is all elements in column same
        /// </summary>
        /// <param name="columnIndex">index of column to check</param>
        /// <returns></returns>
        private bool IsAllItemsOfColumnSame(int columnIndex)
        {
            var firstItem = rowsList.First().First();
            return rowsList.All(row => row.ElementAtOrDefault(columnIndex) == firstItem);
        }

        /// <summary>
        /// Sort collection by specific logic:
        /// numbers higher than non numbers
        /// numbers sorted in ascending order
        /// non numbers sorted by alphabet 
        /// </summary>
        public void Sort()
        {
            for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
            {
                if (!IsAllItemsOfColumnSame(columnIndex))
                {
                    rowsList = rowsList.OrderBy(row => row.ElementAtOrDefault(columnIndex) ?? "", new TableItemComparer()).ToList();
                    break;
                }
            }
        }

        /// <summary>
        /// Comparer for table elements
        /// </summary>
        private class TableItemComparer : IComparer<string>
        {
            private const char DecimalSeparator = '.';
            private static readonly Regex DecimalNumberRegex = new Regex(@$"^-?\d(\{DecimalSeparator}\d+)?$");

            public int Compare(string left, string right)
            {
                var isLeftNumber = DecimalNumberRegex.IsMatch(left ?? string.Empty);
                var isRightNumber = DecimalNumberRegex.IsMatch(right ?? string.Empty);

                return (isLeftNumber, isRightNumber) switch
                {
                    //comparing elements which are numbers 
                    (true, true) => decimal.Parse(left).CompareTo(decimal.Parse(right)),
                    //numbers higher than non numbers
                    (true, false) => -1,
                    //non numbers below numbers
                    (false, true) => +1,
                    //comparing non numeric values 
                    (false, false) => String.Compare(left, right)
                };
            }
        }
    }
}
