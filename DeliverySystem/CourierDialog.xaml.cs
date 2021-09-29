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
    /// Interaction logic for CourierDialog.xaml
    /// </summary>
    public partial class CourierDialog : Window
    {
        public CourierDialog()
        {
            InitializeComponent();
        }
		
		//TabControl part, one tab is PickupDlg, one tab is DeliveryDlg
        private void TabItem_Initialized(object sender, EventArgs e)
        {
            CourierDlg.PickupDlg a = new CourierDlg.PickupDlg();
            this.PickupPage.Content = a;
            a.ParentWindow = this;
        }

        private void TabItem_Initialized_1(object sender, EventArgs e)
        {
            CourierDlg.DeliveryDlg a = new CourierDlg.DeliveryDlg();
            this.DeliveryPage.Content = a;
            a.ParentWindow = this;
        }

        private void BtAccount_Click(object sender, RoutedEventArgs e)
        {
            Courier_Change_Password_Dialog cdialog = new Courier_Change_Password_Dialog();
            cdialog.Owner = this;
            cdialog.ShowDialog();
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
