using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace DeliverySystem.CourierDlg
{
    /// <summary>
    /// Interaction logic for PickupDlg.xaml
    /// </summary>
    public partial class PickupDlg : Page
    {
        /* TabControl, define ParentWindow */
        private CourierDialog _parentWin;
        public CourierDialog ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        List<int> pIdList = new List<int>();

        public PickupDlg()
        {
            InitializeComponent();

            string name = (from s in Globals.ctx.Staffs where s.StaffId == Globals.curCourierId select s.Name).First().ToString();
            lblCourier.Content = name;
            FetchPackage(Globals.curCourierId);  
        }

        private void clearInputs()
        {
            lblName.Content = "";
            lblPhone.Content = "";
            lblAddress.Content = "";
            lblZipcode.Content = "";
            btSubmit.IsEnabled = false;
            comboReceived.IsChecked = false;
            comboFinished.IsChecked = false;
            comboReceived.IsEnabled = false;
            comboFinished.IsEnabled = false;

        }


        /*  when one pick-up package is selected, more information loaded */
        private void lvPickup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Package p = (Package)lvPickup.SelectedItem;

            /*
            if (lvPickup.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose one package to update");
                lblName.Content = "";
                lblPhone.Content = "";
                lblAddress.Content = "";
                lblZipcode.Content = "";
                btSubmit.IsEnabled = false;
                return;
            }
            */
            try
            {
                if ( p != null )
                {
                    btSubmit.IsEnabled = true;
                    lblName.Content = p.SenderName;
                    lblPhone.Content = p.SenderPhone;
                    lblAddress.Content = p.SenderAddress;
                    lblZipcode.Content = p.SenderZipcode;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package Management: Data Operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        /* when pick-up is done, update Package-Status to "PackageReceived "and DeliveryScheduel-Status to 1*/
        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Package p = (Package)lvPickup.SelectedItem;
                int pId = p.PackageId;
                var sd = (from s in Globals.ctx.ScheduledDeliveries where (s.PackageId == pId && s.StaffId == Globals.curCourierId && s.ScheduleType == 0) select s).First();

                if (p != null & comboReceived.IsChecked == true & comboFinished.IsChecked == true)
                {
                    p.Status = "PackageReceived";
                    sd.Status = 1;  //DeliveryScheduel.Status 0= task pending, 1=task finished
                    Globals.ctx.SaveChanges();
                    MessageBox.Show("Package info update successfully");
                    FetchPackage(Globals.curCourierId);

                    clearInputs();
                }
                else
                {
                    MessageBox.Show("Package Management: Please select status to update");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package Management: Data parsing error: ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /* fetch all packages need to be picked-up by login staff */
        public void FetchPackage(short id)
        {
            List<Package> pList = new List<Package>();

            try
            {            
                 pList = (from p in Globals.ctx.Packages
                         join sd in Globals.ctx.ScheduledDeliveries
                         on p.PackageId equals sd.PackageId
                         where sd.StaffId == id && sd.ScheduleType == 0 && sd.Status==0
                         select p).ToList<Package>();

                /*  ********************* another method *********************    
                pIdList = (from sd in Globals.ctx.ScheduledDeliveries where sd.StaffId == id && sd.ScheduleType == 0 && sd.Status == 0 select sd.PackageId).ToList<int>();

                foreach (int i in pIdList)
                {
                    Package package = (from p in Globals.ctx.Packages where p.PackageId == i select p).First();
                    pList.Add(package);
                }

                */
                lvPickup.ItemsSource = pList;
                lvPickup.Items.Refresh();
                lblPickup.Content = pList.Count() + "";
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "PickUp Dialog : Package data fetching failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}
