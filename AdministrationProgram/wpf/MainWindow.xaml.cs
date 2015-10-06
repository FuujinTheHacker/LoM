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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;

namespace AdministrationProgram.wpf
{
    public partial class MainWindow : Window
    {
        // hello word
        Settings settings;
        BackgroundWorker backgroundWorker;

        DataSet dataSet;
        SqlDataAdapter sqlDataAdapter;
        SqlCommandBuilder sqlCommandBuilder;

        Random random = new Random();

        MenuItem dbMenuItem;
        string dbName;

        bool advancedMode = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Settings settings)
            : this()
        {
            this.settings = settings;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            MenuItem_Click(selectDatabase.Items[selectDatabase.Items.Count-1], null);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsEnabled = true;
            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(e.Error.Message);
            }
            else
            {
                string comad = ((string)((object[])e.Result)[0]);
                object backData = (((object[])e.Result)[1]);

                if (comad == "getDb")
                {
                    dataSet.Tables[0].RowChanged += MainWindow_RowChanged;
                    dataSet.Tables[0].RowDeleted += MainWindow_RowChanged;
                    dataGrid.ItemsSource = dataSet.Tables[0].DefaultView;

                    setLcok();

                    dataEditControl.init(dataSet.Tables[0]);

                }

                if (comad == "accept")
                {
                    object[] objArr = (object[])backData;
                    MessageBox.Show((string)objArr[0]);
                    if ((bool)objArr[1])
                        MenuItem_Click(dbMenuItem, null);
                }

            }
        }


        void MainWindow_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add)
            {
                if (dbName == "User")
                {
                    e.Row.SetField<int>("Salt", random.Next());
                }
            }
            acceptButton.IsEnabled = rejectButton.IsEnabled = dataEditControl.eidt();
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string comad = ((string)((object[])e.Argument)[0]);
            object argData = ((object[])e.Argument)[1];
            object backData = null;

            if (comad == "getDb")
            {
                string db = argData as string;
                try
                {
                    using (SqlConnection con = new SqlConnection(settings.SqlConString))
                    {

                        if (dataSet != null)
                            dataSet.Dispose();
                        dataSet = new DataSet();

                        if (sqlDataAdapter != null)
                            sqlDataAdapter.Dispose();
                        sqlDataAdapter = new SqlDataAdapter("Select * from [dbo].[" + db + "];", con);

                        if (sqlCommandBuilder != null)
                            sqlCommandBuilder.Dispose();
                        sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);

                        sqlDataAdapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
                        sqlDataAdapter.UpdateCommand = sqlCommandBuilder.GetUpdateCommand();
                        sqlDataAdapter.DeleteCommand = sqlCommandBuilder.GetDeleteCommand();

                        sqlDataAdapter.Fill(dataSet);

                        dataSet.Tables[0].TableName = db;
                    }
                }
                catch (Exception)
                {
                    dataSet = null;
                    throw;
                }
            }

            if (comad == "accept")
            {
                using (SqlConnection con = new SqlConnection(settings.SqlConString))
                {
                    con.Open();
                    SqlTransaction trans = con.BeginTransaction();

                    dataSet.Tables[0].RowChanged -= MainWindow_RowChanged;
                    dataSet.Tables[0].RowDeleted -= MainWindow_RowChanged;

                    try
                    {
                        sqlDataAdapter.DeleteCommand.Transaction = sqlDataAdapter.UpdateCommand.Transaction = sqlDataAdapter.InsertCommand.Transaction = trans;
                        int row = sqlDataAdapter.Update(dataSet.Tables[0]);
                        trans.Commit();
                        backData = new object[] { "You have succesfully changed " + row + " rows!", true };
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        backData = new object[] { "ex\n" + ex.Message, false };
                    }

                    dataSet.Tables[0].RowChanged += MainWindow_RowChanged;
                    dataSet.Tables[0].RowDeleted += MainWindow_RowChanged;

                }
            }

            e.Result = new object[] { comad, backData };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender == acceptButton)
            {
                IsEnabled = false;
                backgroundWorker.RunWorkerAsync(new object[] { "accept", null });
            }
            if(sender == rejectButton)
            {
                dataSet.RejectChanges();
            }
            if(sender == repeatButton)
            {
                MenuItem_Click(dbMenuItem, null);
            }
            if(sender == menuItemAddUser)
            {
                new AddUser(settings).ShowDialog();
                MenuItem_Click(dbMenuItem, null);
            }
            if (sender == advancedMode_ && advancedMode != advancedMode_.IsChecked)
            {
                advancedMode = advancedMode_.IsChecked;
                setLcok();
            }
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (dbName == "User")
            {
                if (e.PropertyName == "Pw" && e.PropertyType == typeof(byte[]))
                    e.Column = new DataGridPWColumn(e.PropertyName, e.PropertyName);
            }
        }

        void setLcok()
        {
            if (advancedMode)
                foreach (var item in dataGrid.Columns)
                {
                    item.IsReadOnly = false;
                    item.Visibility = System.Windows.Visibility.Visible;
                }
            else
                foreach (var item in dataGrid.Columns)
                {
                    if (item.Header.Equals("ID"))
                    {
                        item.IsReadOnly = true;
                        item.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    if (item.Header.Equals("TS"))
                    {
                        item.IsReadOnly = true;
                    }

                    if (dbName == "User")
                    {
                        if (item.Header.Equals("Salt"))
                        {
                            item.IsReadOnly = true;
                            item.Visibility = System.Windows.Visibility.Collapsed;
                        }
                            
                    }
                }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (dbMenuItem != null)
                dbMenuItem.IsChecked = false;

            dbMenuItem = (MenuItem)sender;

            dbName = (string)dbMenuItem.Tag;

            dbMenuItem.IsChecked = true;
            selectDatabase.Header = "Select Database (" + dbMenuItem.Header + ")";
            Title = "Administration Program (" + dbMenuItem.Header + ")";

            DataTable t;
            if (dataSet == null ||
                ( t = dataSet.Tables[0].GetChanges()) == null ||
                t.Rows.Count == 0 ||
                MessageBox.Show("Efter detta kommer dina förändringar gå förlorade!\nVill du forsätta?", "Varning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsEnabled = false;
                backgroundWorker.RunWorkerAsync(new object[] { "getDb", dbName });
            }
            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DataTable t;
            if (dataSet != null &&
                (t = dataSet.Tables[0].GetChanges()) != null &&
                t.Rows.Count != 0 &&
                MessageBox.Show("Vill du avsluta?\nDina förändringar kommer gå förlorade", "Varning", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

    }
}
