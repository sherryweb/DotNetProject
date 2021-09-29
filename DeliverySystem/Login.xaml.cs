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
    /// Interaction logic for Login.xaml
    /// The dialog for courier login and courier information validation
    
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        //login for staff
        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                short staffid = short.Parse(tbUserId.Text);
                string password = tbPassword.Password;

                Staff staff = (from s in Globals.ctx.Staffs where (s.StaffId == staffid && s.Password == password && s.Position == "Courier") select s).ToList<Staff>().First();

                if (staff != null)
                {
                    Globals.curCourierId = staff.StaffId;

                    CourierDialog courierdlg = new CourierDialog();
                    courierdlg.Owner = this;
                    courierdlg.Show();
                    //this.Close();
                }
                else
                {
                    MessageBox.Show("Username and password validation error.");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Courier login: Username and password validation error." + ex.Message);
                this.Close();
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
