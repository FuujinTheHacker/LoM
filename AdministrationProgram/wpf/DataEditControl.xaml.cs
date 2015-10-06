    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace AdministrationProgram.wpf
{
    public partial class DataEditControl : UserControl
    {
        DataTable tables;

        Geometry repeatGeometry;
        Geometry uploadGeometry;
        Geometry plusGeometry;
        Geometry minusGeometry;
        Geometry rewindGeometry;

        public DataEditControl()
        {
            InitializeComponent();

            repeatGeometry = repeat.Data;
            grid.Children.Remove(repeat);

            uploadGeometry = upload.Data;
            grid.Children.Remove(upload);

            plusGeometry = plus.Data;
            grid.Children.Remove(plus);

            minusGeometry = minus.Data;
            grid.Children.Remove(minus);

            rewindGeometry = rewind.Data;
            grid.Children.Remove(rewind);
        }

        public void init(DataTable tables)
        {
            this.tables = tables;

            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.Children.Clear();

            addRowDefinition(GridUnitType.Auto);

            addColumnDefinitions(GridUnitType.Star, 1);

            addColumnDefinitions(GridUnitType.Auto);

            addPath(uploadGeometry, 1, 0);

            for (int i = 0; i < tables.Columns.Count; i++)
            {
                addColumnDefinitions(GridUnitType.Auto);

                Label newLabel = new Label();
                newLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                newLabel.Content = tables.Columns[i].ColumnName;
                Grid.SetColumn(newLabel, 2 + i);
                Grid.SetRow(newLabel, 0);
                grid.Children.Add(newLabel);
            }

            addColumnDefinitions(GridUnitType.Auto);

            addColumnDefinitions(GridUnitType.Star, 1);

            addRowDefinition(GridUnitType.Pixel,1);

            Grid newGrid = new Grid();
            newGrid.Background = new SolidColorBrush(Colors.Black);
            Grid.SetColumnSpan(newGrid, int.MaxValue);
            Grid.SetColumn(newGrid, 0);
            Grid.SetRow(newGrid, 1);
            grid.Children.Add(newGrid);

        }

        public bool eidt()
        {
            for (int i = 0; i < grid.Children.Count; i++)
                if (Grid.GetRow(grid.Children[i]) > 1)
                {
                    grid.Children.RemoveAt(i);
                    if (i != 0)
                        i--;
                }

            if (grid.RowDefinitions.Count != 2)
                grid.RowDefinitions.RemoveRange(2, grid.RowDefinitions.Count - 2);

            int rowIndex = 2;

            bool edit = false;

            for (int i = 0; i < tables.Rows.Count; i++)
                if (tables.Rows[i].RowState != DataRowState.Unchanged)
                {
                    DataRow dataRow = tables.Rows[i];
                    edit = true;

                    addRowDefinition(GridUnitType.Auto);

                    if (dataRow.RowState == DataRowState.Deleted)
                    {
                        addPath(minusGeometry, 1, rowIndex);
                        fillrow(DataRowVersion.Original, dataRow, rowIndex);
                    }

                    if (dataRow.RowState == DataRowState.Modified)
                    {
                        addPath(repeatGeometry, 1, rowIndex);
                        fillrow(DataRowVersion.Original, dataRow, rowIndex);
                    }

                    if (dataRow.RowState == DataRowState.Added)
                    {
                        addPath(plusGeometry, 1, rowIndex);
                        fillrow(DataRowVersion.Current, dataRow, rowIndex);
                    }

                    Path p = new Path();
                    p.Data = rewindGeometry;
                    p.Fill = new SolidColorBrush(Colors.Black);
                    p.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    p.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    p.Width = 25;
                    p.Height = 25;
                    p.Stretch = Stretch.Uniform;

                    Button button = new Button();
                    button.Content = p;
                    button.Click += button_Click;
                    button.ToolTip = "Reject this change";
                    button.Tag = dataRow;
                    button.BorderBrush = null;
                    button.Background = null;

                    Grid.SetColumn(button, 2 + tables.Columns.Count);
                    Grid.SetRow(button, rowIndex);
                    if (dataRow.RowState == DataRowState.Modified)
                        Grid.SetRowSpan(button,2);
                    grid.Children.Add(button);

                    if (dataRow.RowState == DataRowState.Modified)
                    {
                        addRowDefinition(GridUnitType.Auto);
                        rowIndex++;
                        fillrow(DataRowVersion.Current, dataRow, rowIndex);
                    }

                    rowIndex++;

                    addRowDefinition(GridUnitType.Pixel,1);

                    Grid newGrid = new Grid();
                    newGrid.Background = new SolidColorBrush(Colors.Black);
                    Grid.SetColumnSpan(newGrid, int.MaxValue);
                    Grid.SetColumn(newGrid, 0);
                    Grid.SetRow(newGrid, rowIndex);
                    grid.Children.Add(newGrid);

                    rowIndex++;
                }

            return edit;
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            ((DataRow)((Button)sender).Tag).RejectChanges();
        }

        void addRowDefinition(GridUnitType gridUnitType , double d = 0 )
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(d, gridUnitType);
            grid.RowDefinitions.Add(row);
        }

        void addColumnDefinitions(GridUnitType gridUnitType , double d = 0 )
        {
            ColumnDefinition column = new ColumnDefinition();
            column.Width = new GridLength(d, gridUnitType);
            grid.ColumnDefinitions.Add(column);
        }

        void fillrow(DataRowVersion dataRowVersion, DataRow dataRow, int rowIndex)
        {
            for (int i = 0; i < tables.Columns.Count; i++)
            {
                object o = dataRow.Field<object>(i, dataRowVersion);
                Label newLabel = new Label();
                if (o != null && o.GetType() == typeof(byte[]))
                    newLabel.Content = "byte[]";
                else
                    newLabel.Content = (o != null) ? o.ToString() : "NULL";
                newLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                Grid.SetColumn(newLabel, 2 + i);
                Grid.SetRow(newLabel, rowIndex);
                grid.Children.Add(newLabel);
            }
        }

        void addPath(Geometry g, int column, int row)
        {
            Path p = new Path();
            p.Data = g;
            p.Fill = new SolidColorBrush(Colors.Black);
            p.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            p.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            p.Width = 25;
            p.Height = 25;
            p.Stretch = Stretch.Uniform;
            if (g == minusGeometry)
                p.Fill = new SolidColorBrush(Color.FromRgb(191,44,44));
            if (g == plusGeometry)
                p.Fill = new SolidColorBrush(Color.FromRgb(111,209,111));
            if (g == repeatGeometry)
                p.Fill = new SolidColorBrush(Color.FromRgb(0,170,255));
            Grid.SetColumn(p, column);
            Grid.SetRow(p, row);
            if (g == repeatGeometry)
                Grid.SetRowSpan(p, 2);
            grid.Children.Add(p);
        }

    }
}
