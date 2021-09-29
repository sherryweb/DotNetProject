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
    /// Interaction logic for Staff_AddUpdate_Dialog.xaml
    /// </summary>
    /// The window for Staffs( Managers and Couriers) maintainance
    /// Staffs adding
    /// Staffs updating
    /// Staffs deleting
    /// 
    public partial class Staff_AddUpdate_Dialog : Window
    {
        List<Staff> StaffList = new List<Staff>();
        List<short> ProcCenterIdList = new List<short>();
        bool isClearInputs = false;

        public Staff_AddUpdate_Dialog()
        {
            InitializeComponent();
            FetchStaffRecords();
            FetchProcCenterIds();
            comboPosition.ItemsSource = Globals.PositionList;
        }

        public void ClearInputs()
        {
            isClearInputs = true;
            lblId.Content = "...";
            tbName.Text = "";
            comboProcCenterId.SelectedIndex = -1;
            tbProcCenterName.Text = "";
            tbPhone.Text = "";
            tbEmail.Text = "";
            comboPosition.SelectedIndex = -1;
            BtStaffDelete.IsEnabled = false;
            BtStaffUpdate.IsEnabled = false;
            
        }

        public void FetchProcCenterIds()
        {
            try
            {
                ProcCenterIdList = (from p in Globals.ctx.ProcCenters select p.ProcCenterId).ToList<short>();
                comboProcCenterId.ItemsSource = ProcCenterIdList;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Staff Management: ProcCenter failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void FetchStaffRecords()
        {
            try
            {
                StaffList = (from s in Globals.ctx.Staffs select s).ToList<Staff>();
                lvStaffs.ItemsSource = StaffList;
                lvStaffs.Items.Refresh();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Staff Management: Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // validate the input data fromat and value
        public bool IsFieldsValid()
        {
            InputValidation ivalidation = new InputValidation();

            if (!ivalidation.ValidateName(tbName.Text))
            {
                MessageBox.Show("Name format should be 1-50 uppercase or lowercase letters and -_  and 0 - 9 numbers, Please input again");
                return false;
            }

            if (comboProcCenterId.SelectedIndex == -1)
            {
                MessageBox.Show("You should choose a Proc Center", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            try
            {
                short id = (short)comboProcCenterId.SelectedItem;
            }
            catch ( FormatException ex )
            {
                MessageBox.Show("ProcCenterId should be integer"+ex.Message, "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!ivalidation.ValidatePhone(tbPhone.Text))
            {
                MessageBox.Show("Phone format error, Please input again.");
                return false;
            }

            if (!ivalidation.ValidateEmail(tbEmail.Text))
            {
                MessageBox.Show("Email format error, Please input again.");
                return false;
            }

            if (comboPosition.SelectedIndex == -1)
            {
                MessageBox.Show("Position should be choosed", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        // add a new staff
        private void BtStaffAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldsValid()) { return; }
            try
            {
                Staff staff = new Staff
                {
                    Name = tbName.Text,
                    ProcCenterId = (short)comboProcCenterId.SelectedItem,
                    Phone = tbPhone.Text,
                    Email = tbEmail.Text,
                    Position = comboPosition.SelectedItem.ToString(),
                    Password = "12345678",
                };
                Globals.ctx.Staffs.Add(staff);
                Globals.ctx.SaveChanges();
                ClearInputs();
                isClearInputs = false;
                FetchStaffRecords();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Staff Management: Interger parsing error: ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // update a existing staff's information
        private void BtStaffUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldsValid()) { return; }
            try
            {
                Staff staff = (Staff)lvStaffs.SelectedItem;
                if (staff != null)
                {
                    staff.Name = tbName.Text;
                    staff.ProcCenterId = (short)comboProcCenterId.SelectedItem;
                    staff.Phone = tbPhone.Text;
                    staff.Email = tbEmail.Text;
                    staff.Position = comboPosition.SelectedItem.ToString();

                    Globals.ctx.SaveChanges();
                    ClearInputs();
                    isClearInputs = false;
                    FetchStaffRecords();
                }
                else
                {
                    MessageBox.Show("Staff Management: Unable to find record to update");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Staff Management: Interger parsing error: " + ex.Message);
            }
        }

        // delete a existing staff
        private void BtStaffDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Staff staff = (Staff)lvStaffs.SelectedItem;
                if (staff != null)
                {
                    var result = MessageBox.Show("Staff Management: Are you sure to delete this item?", "confirm", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    Globals.ctx.Staffs.Remove(staff);
                    Globals.ctx.SaveChanges();
                    ClearInputs();
                    isClearInputs = false;
                    FetchStaffRecords();
                }
                else
                {
                    MessageBox.Show("Staff Management: Unable to find record to delete");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Staff Management: Database operation failed: " + ex.Message);
            }
        }

        private void LvStaffs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Staff staff = (Staff)lvStaffs.SelectedItem;
            if (lvStaffs.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose one item to update or delete....");
                ClearInputs();
                isClearInputs = false;
                return;
            }
            try
            {
                BtStaffDelete.IsEnabled = true;
                BtStaffUpdate.IsEnabled = true;
                BtStaffAdd.IsEnabled = false;
                lblId.Content = staff.StaffId;
                tbName.Text = staff.Name;
                for (int index = 0; index < ProcCenterIdList.Count; index++)
                {
                    if (ProcCenterIdList[index] == (staff.ProcCenterId))
                        comboProcCenterId.SelectedIndex = index;
                }
                tbPhone.Text = staff.Phone;
                tbEmail.Text = staff.Email;
                for (int index = 0; index < Globals.PositionList.Count; index++)
                {
                    if (Globals.PositionList[index].Equals(staff.Position))
                        comboPosition.SelectedIndex = index;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "ProcCenter Management: Data Operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ComboProcCenterId_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                if ( isClearInputs == false)
                {
                    short id = (short)comboProcCenterId.SelectedItem;
                    string proccentername = (from p in Globals.ctx.ProcCenters where p.ProcCenterId == id select p.Name).First().ToString();
                    tbProcCenterName.Text = proccentername;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Staff Management: ProcCenter name fetching failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
