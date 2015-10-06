using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data;

namespace AdministrationProgram.wpf
{
    class DataGridPWColumn : DataGridColumn
    {
        string PropertyName;

        public DataGridPWColumn(string PropertyName, string Header)
        {
            this.PropertyName = PropertyName;
            this.Header = Header;
        }

        protected override System.Windows.FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            if (dataItem.GetType() == typeof(DataRowView))
            {

                Button b = new Button();
                b.Content = "set";
                b.Click += b_Click;
                return b;
            }
            return null;
        }

        void b_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            DataRowView dataRowView = b.DataContext as DataRowView;
            if (dataRowView.IsNew || dataRowView.Row.RowState == DataRowState.Detached || dataRowView.Row.RowState == DataRowState.Added)
            {
                MessageBox.Show("du måste spara användaren först");
            }
            else if (dataRowView != null)
            {
                object salt = dataRowView.Row.Field<int>("Salt");

                if (salt != null)
                {
                    SetPwWindow win = new SetPwWindow();
                    win.ShowDialog();
                    if (win.password != null)
                    {
                        dataRowView.BeginEdit();
                        byte[] binPw = Common.UserLoginHelper.ComputeHash(win.password, (int)salt);
                        dataRowView[PropertyName] = binPw;
                        dataRowView.EndEdit();
                    }
                }
            }
        }
 
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return null;
        }
    }
}
