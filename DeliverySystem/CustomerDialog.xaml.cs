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
    /// Interaction logic for CustomerDialog.xaml
    /// </summary>
    public partial class CustomerDialog : Window
    {
        public CustomerDialog()
        {
            InitializeComponent();
        }

        private void TabItem_Initialized(object sender, EventArgs e)
        {
            CustomerDlg.CustShippingDlg a = new CustomerDlg.CustShippingDlg();
            this.ShippingPage.Content = a;
            a.ParentWindow = this;
        }

        private void TabItem_Initialized_1(object sender, EventArgs e)
        {
            CustomerDlg.CustTrackingDlg a = new CustomerDlg.CustTrackingDlg();
            this.TrackingPage.Content = a;
            a.ParentWindow = this;
        }

        private void TabItem_Initialized_2(object sender, EventArgs e)
        {
            CustomerDlg.Contact a = new CustomerDlg.Contact();
            this.ContactPage.Content = a;
            a.ParentWindow = this;
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
