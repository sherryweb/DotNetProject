using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeliverySystem.CustomerDlg
{
    /// <summary>
    /// Interaction logic for CustShippingDlg.xaml
    /// </summary>
    public partial class CustShippingDlg : Page
    {
        private CustomerDialog _parentWin;
        public CustomerDialog ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        public CustShippingDlg()
        {
            InitializeComponent();
            comboSenderCity.ItemsSource = Globals.CityList;
            comboRecCity.ItemsSource = Globals.CityList;
            comboSenderProv.ItemsSource = Globals.ProvinceList;
            comboRecProv.ItemsSource = Globals.ProvinceList;
        }

        //validate the field of Package information
        public bool IsFieldsValid()
        {
            InputValidation ivalidation = new InputValidation();

            if (!ivalidation.ValidateName(tbSenderName.Text))
            {
                MessageBox.Show("Sender name format should be 1-50 uppercase or lowercase letters and -_  and 0 - 9 numbers, Please input again");
                return false;
            }
            if (!ivalidation.ValidateName(tbRecName.Text))
            {
                MessageBox.Show("Recipient name format should be 1-50 uppercase or lowercase letters and -_  and 0 - 9 numbers, Please input again");
                return false;
            }

            if (!ivalidation.ValidateAddress(tbSenderAddress.Text))
            {
                MessageBox.Show("Sender address format error, Please input again.");
                return false;
            }
            if (!ivalidation.ValidateAddress(tbRecAddress.Text))
            {
                MessageBox.Show("Recipient address format error, Please input again.");
                return false;
            }


            if (comboSenderCity.SelectedItem == null || comboRecCity.SelectedItem == null)
            {
                MessageBox.Show("City should be choosed", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (comboSenderProv.SelectedItem == null || comboRecProv.SelectedItem == null)
            {
                MessageBox.Show("Province should be choosed", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!ivalidation.ValidateZipcode(tbSenderCode.Text))
            {
                MessageBox.Show("Sender zipcode format error, Please input again.");
                return false;
            }
            if (!ivalidation.ValidateZipcode(tbRecCode.Text))
            {
                MessageBox.Show("Recipient zipcode format error, Please input again.");
                return false;
            }

            if (!ivalidation.ValidatePhone(tbSenderPhone.Text))
            {
                MessageBox.Show("Sender phone format error, Please input again.");
                return false;
            }
            if (!ivalidation.ValidatePhone(tbRecPhone.Text))
            {
                MessageBox.Show("Recipient phone format error, Please input again.");
                return false;
            }

            return true;
        }

 /* Pick-up request submit, Package status is "InfoReceived", user has an id to tracking package, another dialog to save/print info  */
        private void btConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldsValid()) { return; }
            try
            {
                Package package = new Package
                {
                    SenderName = tbSenderName.Text,
                    SenderAddress = tbSenderAddress.Text,
                    SenderCity = comboSenderCity.SelectedItem.ToString(),
                    SenderProvince = comboSenderProv.SelectedItem.ToString(),
                    SenderZipcode = tbSenderCode.Text,
                    SenderPhone = tbSenderPhone.Text,

                    RecipientName = tbRecName.Text,
                    RecipientAddress = tbRecAddress.Text,
                    RecipientCity = comboRecCity.SelectedItem.ToString(),
                    RecipientProvince = comboRecProv.SelectedItem.ToString(),
                    RecipientZipCode = tbRecCode.Text,
                    RecipientPhone = tbRecPhone.Text,

                    DeliveredImage = null,

                    Status = "InfoReceived",
                };
                Globals.ctx.Packages.Add(package);
                Globals.ctx.SaveChanges();

                int id = package.PackageId;

                MessageBox.Show("Package submit successfully, please remember your tracking number: " + id);
                CustPrintDlg a = new CustPrintDlg(id);
                a.ShowDialog();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package Submit Error: ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
