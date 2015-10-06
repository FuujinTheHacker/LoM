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
using System.Security;

namespace AdministrationProgram.wpf.ConfigurationWizard
{
    public partial class SetSqlStrPage : Page
    {
        Wizard wizard;

        public SetSqlStrPage()
        {
            InitializeComponent();
        }

        public SetSqlStrPage(Wizard wizard)
            : this()
        {
            this.wizard = wizard;
            comboBox.Text = wizard.settings.SqlConString;
            
            Binding bb;

            bb = new Binding("SqlConString");
            bb.Source = wizard.settings;
            bb.Mode = BindingMode.TwoWay;
            comboBox.SetBinding(ComboBox.TextProperty, bb);

#if DEBUG
            ComboBoxItem i = new ComboBoxItem();
            string dir = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Database.mdf");
            i.Content = "Server=(localdb)\\v11.0;Integrated Security=true;AttachDbFileName=" + dir + ";";
            comboBox.Items.Add(i);

            i = new ComboBoxItem();
            i.Content = "Server=tcp:ljus-och-miljo-ab-sql-server.database.windows.net,1433;Database=Ljus_och_Miljo_DB;User ID=LaAdmin@ljus-och-miljo-ab-sql-server;Password=1mgtA45m4pkKK34HHIwfAW;Trusted_Connection=False;Encrypt=True;";
            comboBox.Items.Add(i);

            if (comboBox.Text == "")
                comboBox.Text = i.Content.ToString();
#endif
        }

        void butten_Click(object sender, RoutedEventArgs e)
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

            LoadingPage loadingPage = new LoadingPage(true, true);
            loadingPage.Title = "Testig SQL connection string";
            loadingPage.backgroundWorker.DoWork += backgroundWorker_DoWork;
            loadingPage.backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            loadingPage.cancelButton.Click += cancelButton_Click;

            wizard.frame.Navigate(loadingPage);
            loadingPage.backgroundWorker.RunWorkerAsync(con);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (testButton != null)
                wizard.nextButten.IsEnabled = testButton.IsEnabled = (comboBox.Text != "");
        }

        void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            wizard.frame.Navigate(this);
        }

        void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker backgroundWorker = sender as BackgroundWorker;
                backgroundWorker.ReportProgress(0, "Testig SQL connection string");
                using (SqlConnection sqlcon = e.Argument as SqlConnection)
                {
                    sqlcon.Open();
                    if (sqlcon.State != System.Data.ConnectionState.Open)
                        throw new Exception("feld");
                }
            }
            catch (Exception ex)
            {
                e.Result = new Tuple<string, bool>("failed Test\n" + ex.Message, false);
                return;
            }
            e.Result = new Tuple<string, bool>("successfully Test", true);
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           wizard.nextButten.IsEnabled = testButton.IsEnabled = (comboBox.Text != "");
        }

    }
}
