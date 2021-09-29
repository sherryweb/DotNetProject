using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace DeliverySystem.CourierDlg
{
    /// <summary>
    /// Interaction logic for DeliveryDlg.xaml
    /// </summary>
    public partial class DeliveryDlg : Page
    {
        /* TabControl, define ParentWindow */
        private CourierDialog _parentWin;
        public CourierDialog ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        string imageLocation = string.Empty;
        List<int> pIdList = new List<int>();

        public DeliveryDlg()
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
            btSubmit.IsEnabled = false;
            btPhoto.IsEnabled = false;
            photoPackage.Source = null;
            imageLocation = string.Empty;
            tbPhoto.Text = "Click to take photo";
            comboDelivered.IsChecked = false;
            comboFinished.IsChecked = false;
            comboDelivered.IsEnabled = false;
            comboFinished.IsEnabled = false;

        }

        /*  when one delivery package is selected, more information loaded */
        private void lvDelivery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Package p = (Package)lvDelivery.SelectedItem;
            /*
            if (lvDelivery.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose one package to update");
                lblName.Content = "";
                lblPhone.Content = "";
                lblAddress.Content = "";
                btSubmit.IsEnabled = false;
                btPhoto.IsEnabled = false;
                photoPackage.Source = null;
                imageLocation = string.Empty;
                tbPhoto.Text= "Click to take photo";
                return;
            }
            */
            try
            {
                if (p != null )
                {
                    btSubmit.IsEnabled = true;
                    lblName.Content = p.RecipientName;
                    lblPhone.Content = p.RecipientPhone;
                    lblAddress.Content = p.RecipientAddress;
                    tbPhoto.Text = "Click to take photo";
                    //photoPackage.Source = (ImageSource)((new ImageSourceConverter()).ConvertFrom(p.DeliveredImage));
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package Management: Data Operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //upload photo when the package is delivered
        private void btPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg;*.gif;*.png)|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    string fileName = dlg.FileName;
                    if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                    {
                        photoPackage.Source = new BitmapImage(new Uri(fileName));
                        tbPhoto.Text = string.Empty;
                        imageLocation = fileName;
                    }
                }
                catch (Exception ex) when (ex is SystemException || ex is IOException)
                {
                    MessageBox.Show(ex.Message, "File reading failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /* when delivery package is done, update Package-Status to "Delivered "and DeliveryScheduel-Status to 1*/
        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Package p = (Package)lvDelivery.SelectedItem;
                int pId = p.PackageId;
          
                byte[] image = File.ReadAllBytes(imageLocation);

                if (string.IsNullOrEmpty(imageLocation))
                {
                    MessageBox.Show("Please choose image");
                }

                if (p != null & comboDelivered.IsChecked == true & comboFinished.IsChecked == true )
                {
                    p.DeliveredImage = File.ReadAllBytes(imageLocation);
                    p.Status = "Delivered";
                    Globals.ctx.SaveChanges();

                    ScheduledDelivery sd = (from s in Globals.ctx.ScheduledDeliveries where (s.PackageId == pId && s.StaffId == Globals.curCourierId && s.ScheduleType == 1) select s).First();
                    sd.Status = 1;          //DeliveryScheduel.Status 0= task pending, 1=task finished
                    Globals.ctx.SaveChanges();

                    MessageBox.Show("Package info update successfully");

                    FetchPackage(Globals.curCourierId);
                    lvDelivery.Items.Refresh();

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

        /* fetch all packages need to be delivered by login staff */
        public void FetchPackage(short id)
        {
            List<Package> pList = new List<Package>();

            try
            {
                pList = (from p in Globals.ctx.Packages
                         join sd in Globals.ctx.ScheduledDeliveries
                         on p.PackageId equals sd.PackageId
                         where sd.StaffId == id && sd.ScheduleType == 1 && sd.Status == 0
                         select p).ToList<Package>();

                lvDelivery.ItemsSource = pList;
                //lvDelivery.Items.Refresh();
                lblDelivery.Content = pList.Count()+"";
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Delivery Dialog : Delivery data fetching failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
