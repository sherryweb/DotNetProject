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

namespace DeliverySystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// The initial window -- the homepage of the project
    /// There are 3 types of users in this project.
    /// So we provide 3 entrances for the 3 types of users: The customer entrance , the courier entrance, the manager entrance
    ///    The Customer: can register a new package's information and then track the package with the package id.
    ///    The Courier: can read own package picking-up task list and package delivery list, then process the package picking-up and delivery task
    ///    The manager: can manage the staffs(Manager and courier in different processing center), manage the different processing center information
    ///                 can schedule the package picking-up tasks and package delivery tasks( to different couriers) automatically 
    ///                 can schedule package shipment tasks (from sendercity to recipientcity) manually
    ///                 can look the business statistic charts


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            try
            {
                Globals.ctx = new DeliverySystemDBConnection();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Database Connection failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                Environment.Exit(1); // fatal error
            }
        }

        private void BtCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerDialog cdialog = new CustomerDialog();
            cdialog.Owner = this;
            cdialog.ShowDialog();
        }

        private void BtCourier_Click(object sender, RoutedEventArgs e)
        {
            /*
            CourierDialog crdialog = new CourierDialog();
            crdialog.Owner = this;
            crdialog.ShowDialog();
            */

            // need login first 
            Login login = new Login();
            login.Owner = this;
            login.ShowDialog();
        }

        private void BtManager_Click(object sender, RoutedEventArgs e)
        {
            /*
            ManagerDialog mdialog = new ManagerDialog();
            mdialog.Owner = this;
            mdialog.ShowDialog();
            */
            // need login first
            ManagerLogin managerlogin = new ManagerLogin();
            managerlogin.Owner = this;
            managerlogin.ShowDialog();

        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
