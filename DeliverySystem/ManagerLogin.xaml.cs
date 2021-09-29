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

namespace DeliverySystem
{
    /// <summary>
    /// Interaction logic for ManagerLogin.xaml
    /// </summary>
    public partial class ManagerLogin : Window
    {
        public ManagerLogin()
        {
            InitializeComponent();
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                short managerid = short.Parse(tbUserId.Text);
                string password = tbPassword.Password;

                Staff manager = (from s in Globals.ctx.Staffs where (s.StaffId == managerid && s.Password == password && (s.Position == "Manager" || s.Position == "General Manager")) select s).ToList<Staff>().First();

                if (manager != null)
                {
                    Globals.ManagerId = manager.StaffId;

                    ManagerDialog mdialog = new ManagerDialog();
                    mdialog.Owner = this;
                    mdialog.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Username and password validation error.");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Manager login: Username and password validation error." + ex.Message);
                this.Close();
            }
        }

        
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
