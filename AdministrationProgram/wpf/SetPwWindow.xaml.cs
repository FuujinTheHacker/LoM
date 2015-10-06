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

namespace AdministrationProgram.wpf
{
    /// <summary>
    /// Interaction logic for SetPwWindowxaml.xaml
    /// </summary>
    public partial class SetPwWindow : Window
    {
        public string password;

        public SetPwWindow()
        {
            InitializeComponent();
            test();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            textBox2.Visibility = textBox1.Visibility = (checkBox.IsChecked.Value) ? Visibility.Visible : Visibility.Collapsed;
            passwordBox2.Visibility = passwordBox1.Visibility = (!checkBox.IsChecked.Value) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            passwordBox1.PasswordChanged -= passwordBox1_PasswordChanged;
            passwordBox1.Password = textBox1.Text;
            passwordBox1.PasswordChanged += passwordBox1_PasswordChanged;
            test();
            e.Handled = true;
        }

        private void passwordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            textBox1.TextChanged -= textBox1_TextChanged;
            textBox1.Text = passwordBox1.Password;
            textBox1.TextChanged += textBox1_TextChanged;
            test();
            e.Handled = true;
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            passwordBox2.PasswordChanged -= passwordBox2_PasswordChanged;
            passwordBox2.Password = textBox2.Text;
            passwordBox2.PasswordChanged += passwordBox2_PasswordChanged;
            test();
        }

        private void passwordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            textBox2.TextChanged -= textBox2_TextChanged;
            textBox2.Text = passwordBox2.Password;
            textBox2.TextChanged += textBox2_TextChanged;
            test();
        }

        private void test()
        {
            int antalTeken = 5;
            saveButton.IsEnabled = ((textBox1.Text != "") && (textBox1.Text == textBox2.Text) && (textBox1.Text.Length >= antalTeken));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            password = (sender == this.saveButton) ? textBox1.Text : null;
            Close();
        }
    }
}
