using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data;
using System.Data.SqlClient;
using Common.DatabaseObjects;

namespace AdministrationProgram
{
    public partial class AddUser : Window
    {
        Settings settings;
        System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
        Random myRandom = new Random();

        class workOrder
        {
            public string OrgName { get; set; } 
            public string Phone { get; set; } 
            public string ConfirmEmail { get; set; } 
            public string Subject { get; set; } 
            public string TextBody { get; set; } 
        }

        public AddUser()
        {
            InitializeComponent();
            bw = new System.ComponentModel.BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
        }

        void bw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void bw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.IsEnabled = true;

            if(e.Result != null)
            {
                Exception ex = e.Result as Exception;
                MessageBox.Show(ex.Message);   
            }
            else
            {
                MessageBox.Show("Success!\n An Email Has Been Sent To: " + TextBox_Email.Text);
                Close();
            }
        }

        void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            workOrder work = e.Argument as workOrder;

            bw.ReportProgress(0);

            int test = work.TextBody.IndexOf("(-=-_key_-=-)");
            if (test == -1)
            {
                e.Result = new Exception("keunde inte hitta (-=-_key_-=-)");
                return;
            }

            bw.ReportProgress(1);

            long id;
            SqlTransaction transaction = null;
            using (SqlConnection connection = new SqlConnection(settings.SqlConString))
            {
                connection.Open();
                transaction = connection.BeginTransaction("Begin");
                using (SqlCommand command = new SqlCommand("INSERT INTO [dbo].[User](OrgName,Tele,UserName,Activated,Salt,Epost) OUTPUT INSERTED.ID VALUES (@OrgName,@Tele,@UserName,@Activated,@Salt,@Epost);", connection, transaction))
                {
                    try
                    {
                        command.Parameters.AddWithValue("OrgName", work.OrgName);
                        command.Parameters.AddWithValue("Tele", work.Phone);
                        command.Parameters.AddWithValue("UserName", work.OrgName);
                        command.Parameters.AddWithValue("Activated", 0);
                        command.Parameters.AddWithValue("Salt", myRandom.Next());
                        command.Parameters.AddWithValue("Epost", work.ConfirmEmail);

                        id = (long)command.ExecuteScalar();

                        bw.ReportProgress(2);

                        ActivationKey newKey = new ActivationKey(id);

                        command.Parameters.Clear();

                        command.CommandText = "INSERT INTO [dbo].[ActivationKey](Typ,User_ID,[Key]) VALUES (@Typ,@User_ID,@Key);";

                        command.Parameters.AddWithValue("Typ", newKey.Typ);
                        command.Parameters.AddWithValue("User_ID", newKey.User_ID);
                        command.Parameters.AddWithValue("Key", newKey.Key);

                        command.ExecuteNonQuery();

                        transaction.Commit();

                        bw.ReportProgress(3);

                        work.TextBody = work.TextBody.Replace("(-=-_key_-=-)", "http://localhost:62170/accountcreation?anActivationKey=" + newKey.Key);

                        EmailSender.SendEmail("ljusochmiljo@gmail.com", "projectlight", work.ConfirmEmail, work.Subject, work.TextBody);
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        e.Result = ex;
                    }
                }
            }
        }

        public AddUser(Settings settings)
            : this()
        {
            this.settings = settings;
        }

        private void TextBox_Edit(object sender, TextChangedEventArgs e)
        {
            bool test = true;

            if (TextBox_OrgName.Text == "")
                test = false;

            if (TextBox_Phone.Text == "")
                test = false;

            if (TextBox_ConfirmEmail.Text == "" && TextBox_Email.Text == "")
            {
                test = false;
            }
            else if (TextBox_ConfirmEmail.Text == TextBox_Email.Text)
            {
                Label_ConfirmEmail.Content = "Correct";
                Label_ConfirmEmail.Foreground = Brushes.Green;
            }
            else
            {
                Label_ConfirmEmail.Content = "Invalid";
                Label_ConfirmEmail.Foreground = Brushes.Red;
                test = false;
            }

            Button_Send.IsEnabled = test;
        }

        private void RadioButton(object sender, RoutedEventArgs e)
        {
            if (RadioButton_Default.IsChecked.Value)
            {
                TextBox_Subject.IsEnabled = false;
                TextBox_Subject.Background = Brushes.Gray;
                TextBox_Subject.Text = "Account Activation";

                TextBox_Body.IsEnabled = false;
                TextBox_Body.Background = Brushes.Gray;

                TextBox_Body.Text = "Welcome To Ljus &amp; Miljö!&#xA;Press The Link Below To Activate Your Account:(-=-_key_-=-)";
            }
            else
            {
                TextBox_Subject.IsEnabled = true;
                TextBox_Subject.Background = Brushes.White;
                TextBox_Subject.Text = "Account Activation";

                TextBox_Body.IsEnabled = true;
                TextBox_Body.Background = Brushes.White;
            }
        }

        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            workOrder work = new workOrder()
            {
                OrgName = TextBox_OrgName.Text,
                Phone = TextBox_Phone.Text,
                ConfirmEmail = TextBox_ConfirmEmail.Text,

                Subject = TextBox_Subject.Text,
                TextBody = TextBox_Body.Text
            };
            bw.RunWorkerAsync(work);
        }

    }

}