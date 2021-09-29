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

namespace DeliverySystem
{
    /// <summary>
    /// Interaction logic for ProcCenter_AddUpdate_Dialog.xaml
    /// The window for Processing center's maintanance
    /// Processing adding
    /// Processing updating
    /// Processing deleting
    public partial class ProcCenter_AddUpdate_Dialog : Window
    {
        List<ProcCenter> ProcCenterList = new List<ProcCenter>();

        public ProcCenter_AddUpdate_Dialog()
        {
            InitializeComponent();
            FetchProcCenterRecords();
            comboCity.ItemsSource = Globals.CityList; 
            comboProvince.ItemsSource = Globals.ProvinceList;
        }

        public void ClearInputs()
        {
            lblId.Content = "...";
            tbName.Text = "";
            tbAddress.Text = "";
            comboCity.SelectedIndex = -1;
            comboProvince.SelectedIndex = -1;
            tbZipcode.Text = "";
            tbPhone.Text = "";
            BtProcDelete.IsEnabled = false;
            BtProcUpdate.IsEnabled = false;
        }

        public void FetchProcCenterRecords()
        {
            try
            { 
                ProcCenterList = (from p in Globals.ctx.ProcCenters select p).ToList<ProcCenter>();
                lvProcCenters.ItemsSource = ProcCenterList;
                lvProcCenters.Items.Refresh();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "ProcCenter Management: Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Validate all input data's format and value
        public bool IsFieldsValid()
        {
            InputValidation ivalidation = new InputValidation();

            if (!ivalidation.ValidateName(tbName.Text))
            {
                MessageBox.Show("Name format should be 1-50 uppercase or lowercase letters and -_  and 0 - 9 numbers, Please input again");
                return false;
            }

            if (!ivalidation.ValidateAddress(tbAddress.Text))
            {
                MessageBox.Show("Address format error, Please input again.");
                return false;
            }

            if (comboCity.SelectedIndex == -1)
            {
                MessageBox.Show("City should be choosed", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (comboProvince.SelectedIndex == -1)
            {
                MessageBox.Show("Province Should be choosed", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!ivalidation.ValidateZipcode(tbZipcode.Text))
            {
                MessageBox.Show("Zipcode format error, Please input again.");
                return false;
            }

            if (!ivalidation.ValidatePhone(tbPhone.Text))
            {
                MessageBox.Show("Phone format error, Please input again.");
                return false;
            }


            return true;
        }

        // Add a new processing center
        private void BtProcAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldsValid()) { return; }
            try
            {
                ProcCenter p = new ProcCenter
                {
                    Name = tbName.Text,
                    Address = tbAddress.Text,
                    City = comboCity.SelectedItem.ToString(),
                    Province = comboProvince.SelectedItem.ToString(),
                    Zipcode = tbZipcode.Text,
                    Phone = tbPhone.Text,
                };
                Globals.ctx.ProcCenters.Add(p);
                Globals.ctx.SaveChanges();
                ClearInputs();
                FetchProcCenterRecords();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "ProcCenter Management: Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // update a existing processing center's information
        private void BtProcUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldsValid()) { return; }
            try
            {
                ProcCenter pc = (ProcCenter)lvProcCenters.SelectedItem;
                if (pc != null)
                {
                    pc.Name = tbName.Text;
                    pc.Address = tbAddress.Text;
                    pc.City = comboCity.SelectedItem.ToString();
                    pc.Province = comboProvince.SelectedItem.ToString();
                    pc.Zipcode = tbZipcode.Text;
                    pc.Phone = tbPhone.Text;

                    Globals.ctx.SaveChanges();
                    ClearInputs();
                    FetchProcCenterRecords();
                }
                else
                {
                    MessageBox.Show("ProcCenter Management: Unable to find record to update");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("ProcCenter Management: Database operation failed: " + ex.Message);
            }
        }

        // delete a existing processing center
        private void BtProcDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcCenter pc = (ProcCenter)lvProcCenters.SelectedItem;
                if (pc != null)
                {
                    var result = MessageBox.Show("ProcCenter Management: Are you sure to delete this item?", "confirm", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    Globals.ctx.ProcCenters.Remove(pc);
                    Globals.ctx.SaveChanges();
                    ClearInputs();
                    FetchProcCenterRecords();
                }
                else
                {
                    MessageBox.Show("ProcCenter Management: Unable to find record to delete");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("ProcCenter Management: Database operation failed: " + ex.Message);
            }
        }

        private void LvProcCenters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProcCenter pc = (ProcCenter)lvProcCenters.SelectedItem;
            if (lvProcCenters.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose one item to update or delete....");
                ClearInputs();
                return;
            }
            try
            {
                BtProcDelete.IsEnabled = true;
                BtProcUpdate.IsEnabled = true;
                lblId.Content = pc.ProcCenterId;
                tbName.Text = pc.Name;
                tbAddress.Text = pc.Address;
                for ( int index = 0; index < Globals.CityList.Count; index++ )
                {
                    if ( Globals.CityList[index].Equals(pc.City))
                    comboCity.SelectedIndex = index;
                }
                for (int index = 0; index < Globals.ProvinceList.Count; index++)
                {
                    if (Globals.ProvinceList[index].Equals(pc.Province))
                        comboProvince.SelectedIndex = index;
                }
                tbZipcode.Text = pc.Zipcode;
                tbPhone.Text = pc.Phone;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "ProcCenter Management: Data Operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
