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

namespace AdministrationProgram.wpf.ConfigurationWizard
{
    public partial class Wizard : Window
    {
        public Button priButton { get { return _priButton; } }
        public Button nextButten { get { return _nextButten; } }
        public Label topLebel { get { return _topLebel; } }
        public Frame frame { get { return _frame; } }

        public List<Page> pageList;

        public AdministrationProgram.Settings settings;

        public event System.EventHandler onSave;

        public Wizard()
        {
            InitializeComponent();
        }

        public Wizard(AdministrationProgram.Settings settings, System.EventHandler onSave)
            : this()
        {
            this.settings = settings; 
            
            pageList = new List<Page>();
            pageList.Add(new WelcomePage(this));
            pageList.Add(new SetSqlStrPage(this));
            pageList.Add(new InstalSqlTablesPage(this));
            pageList.Add(new SaveAndClosePage());

            this.onSave += onSave;
            next();
        }

        void _frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            topLebel.Content = (frame.NavigationService.Content as Page).Title;
            int index = pageList.IndexOf(frame.NavigationService.Content as Page);

            priButton.Visibility = (index != -1) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            nextButten.Visibility = (index != -1) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

            if (index != -1)
            {
                priButton.IsEnabled = (index != 0);
                nextButten.IsEnabled = true;
                nextButten.Content = (index != pageList.Count-1)?"Next":"save";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (nextButten == sender)
                next(frame.NavigationService.Content as Page);
            else
                pri(frame.NavigationService.Content as Page);
        }

        public void next(Page p = null)
        {
            if (pageList.IndexOf((p != null) ? p : (frame.NavigationService.Content as Page)) + 1 == pageList.Count)
            {
                if (onSave != null)
                    onSave(this, EventArgs.Empty);
                this.Close();
                return;
            }

            _frame.Navigate(pageList[pageList.IndexOf((p != null) ? p : (frame.NavigationService.Content as Page)) + 1]);
        }

        public void pri(Page p = null)
        {
            _frame.Navigate(pageList[pageList.IndexOf((p != null) ? p : (frame.NavigationService.Content as Page)) - 1]);
        }

    }
}
