using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DeliverySystem
{
    /// <summary>
    /// Interaction logic for CustPrintDlg.xaml
    /// </summary>
    public partial class CustPrintDlg : Window
    {
        int pid;
        public CustPrintDlg(int id)
        {
            InitializeComponent();
            pid = id;
            FetchInfo(pid);

        }

        public void FetchInfo(int id)
        {
            var p = (from pi in Globals.ctx.Packages where pi.PackageId == id select pi).First();

            lblId.Content = id;
            lblSenderName.Content = p.SenderName;
            lblSenderAddress.Content = p.SenderAddress;
            lblSenderCity.Content = p.SenderCity;
            lblSenderCode.Content = p.SenderZipcode;
            lblSenderPhone.Content = p.SenderPhone;

            lblRepName.Content = p.RecipientName;
            lblRepAddress.Content = p.RecipientAddress;
            lblRepCity.Content = p.RecipientCity;
            lblRepProv.Content = p.RecipientProvince;
            lblRepCode.Content = p.RecipientZipCode;
            lblRepPhone.Content = p.RecipientPhone;

        }

        private void printer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(printArea, "orderDetail");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
