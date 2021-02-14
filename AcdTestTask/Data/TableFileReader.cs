using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACDTestTask.Model;

namespace ACDTestTask.Data
{
    /// <summary>
    /// Static class for read table from file
    /// </summary>
    public static class TableFileReader
    {
        public static async Task<Table> ReadTable(string pathToFile)
        {
            using var dataReader = File.OpenText(pathToFile);
            var dataText = await dataReader.ReadToEndAsync();

            var rows = dataText.Split(TableFileConstant.NewRowSeparator, StringSplitOptions.RemoveEmptyEntries);

            var data = new List<List<string>>();
            foreach (var row in rows)
            {
                data.Add(row.Split(TableFileConstant.FieldSeparator).ToList());
            }

            return new Table(data);
        }
    }
}
