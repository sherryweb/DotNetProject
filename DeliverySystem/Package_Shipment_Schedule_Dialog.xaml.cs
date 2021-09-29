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
    /// Interaction logic for Package_Shipment_Schedule_Dialog.xaml
    /// With this dialog, manager can shedule the package shipment from sender city to recipient city
    
    public partial class Package_Shipment_Schedule_Dialog : Window
    {
        List<string> ProcCenterCityList = new List<string>();
        Package curPackage = null;

        public Package_Shipment_Schedule_Dialog(Package p, string currentposition)
        {
            InitializeComponent();
            curPackage = p;

            // display the package destination address information for manager's reference to decide which city can be next target city
            lblId.Content = p.PackageId;
            tbRecName.Text = p.RecipientName;
            tbRecAddress.Text = p.RecipientAddress;
            tbRecCity.Text = p.RecipientCity;
            tbRecProvince.Text = p.RecipientProvince;
            tbRecZipCode.Text = p.RecipientZipCode;
            tbRecPhone.Text = p.RecipientPhone;

            tbCurrentPosition.Text = currentposition;

            FetchProcCenterCities();

        }

        // Provide the destination city list for manager's choice
        public void FetchProcCenterCities()
        {
            try
            {
                ProcCenterCityList = (from p in Globals.ctx.ProcCenters select p.City).ToList<string>();
                comboProcCenterCity.ItemsSource = ProcCenterCityList;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package Shipment: Fetch ProcCenter City failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ComboProcCenterCity_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                string city = (string)comboProcCenterCity.SelectedItem;
                short proccenterid = (short)(from p in Globals.ctx.ProcCenters where p.City == city select p.ProcCenterId).First();
                tbProcCenterId.Text = proccenterid + "";
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package Shipment: ProcCenter name fetching failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public bool IsFieldsValid()
        {
            if (comboProcCenterCity.SelectedIndex == -1)
            {
                MessageBox.Show("You should choose a Proc Center", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            try
            {
                string city = comboProcCenterCity.SelectedItem.ToString();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("ProcCenterId should be integer" + ex.Message, "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        private void BtSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldsValid()) { return; }
            try
            {
                PackageHistory ph = new PackageHistory
                {
                    PackageId = curPackage.PackageId,
                    ProcCenterId = short.Parse(tbProcCenterId.Text),
                    ProcDate = System.DateTime.Today,
                };
                Globals.ctx.PackageHistories.Add(ph);
                Globals.ctx.SaveChanges();
                
                if ( cbArrive.IsChecked == true )
                {
                    curPackage.Status = "OutForDeliver";
                    Globals.ctx.SaveChanges();
                }
                else
                {
                    curPackage.Status = "Routed";
                    Globals.ctx.SaveChanges();
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package Shipment: short interger parsing error: ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            this.Close();
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
