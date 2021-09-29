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

namespace DeliverySystem.CustomerDlg
{
    /// <summary>
    /// Interaction logic for CustTrackingDlg.xaml
    /// </summary>
    public partial class CustTrackingDlg : Page
    {
        private CustomerDialog _parentWin;
        public CustomerDialog ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        List<int> packageIdList = new List<int>();

        public CustTrackingDlg()
        {
            InitializeComponent();
        }

        /* fetch package history base on id, show process date, and location to user */
        public void FetchHistory(int id)
        {
            try
            {
                var Result = Globals.ctx.PackageHistories
                    .Join(
                        Globals.ctx.ProcCenters,
                        ph => ph.ProcCenterId,
                        pk => pk.ProcCenterId,
                        (ph, pk) => new
                        {
                            PackageId = ph.PackageId,
                            ProcCenterId = ph.ProcCenterId,
                            ProcDate = ph.ProcDate,
                            City = pk.City,
                        }
                    ).Where(list => list.PackageId == id).
                    ToList().OrderBy(ph => ph.ProcDate);

                lvHistory.ItemsSource = Result;

                /*   another method , not lambda   
                var joinList = from ph in Globals.ctx.PackageHistories
                join pc in Globals.ctx.ProcCenters
                on new { ph.ProcCenterId } equals new { pc.ProcCenterId }
                where ph.PackageId == id
                select new
                {
                    PackageId = ph.PackageId,
                    ProcCenterId = ph.ProcCenterId,
                    ProcDate = ph.ProcDate,
                    City = pc.City,
                };
                var join = joinList.ToList();
                */
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package History: Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btTrack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbTrackingNumber.Text))
                {
                    MessageBox.Show("Please input your tracking number", "Information");
                }

                int num = 0;
                int.TryParse(tbTrackingNumber.Text, out num);
                if (num < 0)
                {
                    MessageBox.Show("Invalid tracking number");
                }
                FetchHistory(num);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package History: No record", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }
    }
}
