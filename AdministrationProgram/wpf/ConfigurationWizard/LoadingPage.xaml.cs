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
using System.ComponentModel;

namespace AdministrationProgram.wpf.ConfigurationWizard
{
    public partial class LoadingPage : Page
    {
        public BackgroundWorker backgroundWorker;
        public LoadingPage()
        {
            InitializeComponent();
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
        }

        public LoadingPage(bool IsIndeterminate, bool WorkerSupportsCancellation)
            : this()
        {
            progressBar.IsIndeterminate = IsIndeterminate;
            cancelButton.IsEnabled = WorkerSupportsCancellation;
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
                label.Content = e.UserState.ToString();
            progressBar.Value = e.ProgressPercentage;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
