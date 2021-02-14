using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ACDTestTask.Model;

namespace ACDTestTask.Data
{
    /// <summary>
    /// Static class for saving table data into file
    /// </summary>
    public static class TableFileSaver
    {
        public static async Task SaveToFile(string filePath, Table table)
        {
            var resultBuilder = new StringBuilder();
            foreach (var row in table.Rows)
            {
                resultBuilder.AppendJoin(TableFileConstant.FieldSeparator, row);
                resultBuilder.Append(TableFileConstant.NewRowSeparator);
            }

            await using var writer = new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate));
            await writer.WriteAsync(resultBuilder.ToString());
        }
    }
}
