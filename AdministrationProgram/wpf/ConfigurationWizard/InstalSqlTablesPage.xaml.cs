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
using System.Data.SqlClient;
using System.ComponentModel;

namespace AdministrationProgram.wpf.ConfigurationWizard
{
    public partial class InstalSqlTablesPage : Page
    {
        Wizard wizard;

        public InstalSqlTablesPage()
        {
            InitializeComponent();
        }

        public InstalSqlTablesPage(Wizard wizard)
            : this()
        {
            this.wizard = wizard;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SqlConnection con = null;
                try
                {
                    con = new SqlConnection(wizard.settings.SqlConString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                LoadingPage loadingPage = new LoadingPage(true, false);
                loadingPage.Title = "instaling Sql TablesPage";
                loadingPage.backgroundWorker.DoWork += backgroundWorker_DoWork;
                loadingPage.backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

                wizard.frame.Navigate(loadingPage);

                loadingPage.backgroundWorker.RunWorkerAsync(con);
            }
        }

        void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker backgroundWorker = sender as BackgroundWorker;
                backgroundWorker.ReportProgress(0, "instaling");
                using (SqlConnection con = e.Argument as SqlConnection)
                {
                    string comad1 = System.IO.File.ReadAllText("KillSQL.sql");
                    string comad2  = System.IO.File.ReadAllText("DeploySQL.sql");
                    int r;

                    SqlCommand sqlCommand = new SqlCommand(comad1, con);
                    
                    con.Open();
                    r = sqlCommand.ExecuteNonQuery();

                    sqlCommand.CommandText = comad2;
                    r = sqlCommand.ExecuteNonQuery();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                e.Result = new Tuple<string, bool>("failed " + ex.Message, false);
                return;
            }
            e.Result = new Tuple<string,bool>("success", true );
        }

        void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Tuple<string, bool> r = e.Result as Tuple<string, bool>;
            MessageBox.Show(r.Item1);

            if (r.Item2)
                wizard.next(this);
            else
                wizard.frame.Navigate(this);
        }
    }
}
