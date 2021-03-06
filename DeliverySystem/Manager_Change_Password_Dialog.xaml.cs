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
    /// Interaction logic for Manager_Change_Password_Dialog.xaml
    /// Manager can change his password after login
    /// 

    public partial class Manager_Change_Password_Dialog : Window
    {
        public Manager_Change_Password_Dialog()
        {
            InitializeComponent();
        }

        private void btConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string oldpassword = tbOldPassword.Password;
                string newpassword = tbNewPassword.Password;
                string renewpassword = tbReNewPassword.Password;

                // validate whether the newpassword and reinputnew password are equal
                if (renewpassword.Equals(newpassword) != true)
                {
                    MessageBox.Show("ReInput new password should equal to New password");
                    return;
                }

                // validate the old password
                Staff manager = (from s in Globals.ctx.Staffs where (s.StaffId == Globals.ManagerId && s.Password == oldpassword && (s.Position == "Manager" || s.Position == "General Manager")) select s).ToList<Staff>().First();

                if (manager != null)
                {
                    // update the password
                    manager.Password = newpassword;
                    Globals.ctx.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Old password validation error.");
                }
                MessageBox.Show("Manager password changed.");
                this.Close();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Manager Change Password: Old password validation error." + ex.Message);
                this.Close();
            }

        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
