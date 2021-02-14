using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using ACDTestTask.Data;
using ACDTestTask.Model;
using Microsoft.Win32;

namespace AcdTestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set data from table to TableGrid
        /// </summary>
        /// <param name="table"></param>
        private void SetTableToTableGrid(Table table)
        {
            var dataTable = new DataTable();

            //generate columns 
            var dataTableColumnCount = table.ColumnCount;
            for (var i = 0; i < dataTableColumnCount; i++)
            {
                dataTable.Columns.Add(new DataColumn((i + 1).ToString()));
            }

            //initial data
            foreach (var tableRow in table.Rows)
            {
                dataTable.Rows.Add(tableRow.ToArray<object>());
            }

            TableGrid.ItemsSource = dataTable.DefaultView;
        }

        /// <summary>
        /// get path to txt file by user
        /// </summary>
        /// <returns></returns>
        private string GetFilePathOrDefault()
        {
            var fileDialog = new OpenFileDialog() { Filter = "Text | *.txt" };
            return fileDialog.ShowDialog() == true ? fileDialog.FileName : null;
        }

        /// <summary>
        /// Open file and show sorted data to user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sourceFilePath = GetFilePathOrDefault();
            if (sourceFilePath != null)
            {
                var table = await TableFileReader.ReadTable(sourceFilePath);
                table.Sort();

                SetTableToTableGrid(table);
            }
        }

        /// <summary>
        /// Open file, sort data and write result data to result file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ProcessButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sourceFilePath = GetFilePathOrDefault();
            if (sourceFilePath != null)
            {
                var table = await TableFileReader.ReadTable(sourceFilePath);
                table.Sort();

                var resultFilePath = Path.ChangeExtension(sourceFilePath, ".result");
                await TableFileSaver.SaveToFile(resultFilePath, table);

                MessageBox.Show("Data was sorted and saved", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
