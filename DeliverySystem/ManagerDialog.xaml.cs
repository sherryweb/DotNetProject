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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Windows.Documents.Serialization;

namespace DeliverySystem
{
    /// <summary>
    /// Interaction logic for ManagerDialog.xaml
    /// </summary>
    /// The manager dashboard, In this window, Manager: 
    ///     can manage the staffs(Manager and courier in different processing center), manage the different processing center information
    ///     can schedule the package picking-up tasks and package delivery tasks( to different couriers) automatically 
    ///     can schedule package shipment tasks (from sendercity to recipientcity) manually
    ///     can look the business statistic charts


    public partial class ManagerDialog : Window
    {
        List<ProcCenterItem> _data;
        List<DataItem> _chartData;

        public ManagerDialog()
        {
            InitializeComponent();

            // Initialize the piechart and Flexchart information
            lblPendingPickUp.Content = GetPendingPickup();
            lblPendingShipment.Content = (from p in Globals.ctx.Packages where (p.Status == "PackageReceived" || p.Status == "Routed") select p).ToList<Package>().Count;
            lblPendingDelivery.Content = GetPendingDelivery();
            
            flexPie.Header = "BusinessVolumes / Processing Center";
            flexPie.Footer = "Total BusinessVolumes";
            flexPie.AnimationSettings = C1.Chart.AnimationSettings.Load;
            flexPie.AnimationUpdate.Easing = C1.Chart.Easing.Linear;
            flexPie.AnimationUpdate.Duration = 500;
            flexPie.AnimationLoad.Type = C1.Chart.AnimationType.Series;

            flexChart.Header = "Tasks Statistics / Staff";
            flexChart.Footer = "Total tasks";

        }
        // Initlaize the piechart data
        public List<ProcCenterItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.StatisticVolumePerProcCenter();
                }
                return _data;
            }
        }
        // Initlaize the flexchart data
        public List<DataItem> ChartData
        {
            get
            {
                if (_chartData == null)
                {
                    _chartData = ChartDataCreator.CreateData();
                }

                return _chartData;
            }
        }

        // calculate the pending picking-up packages number information
        private int GetPendingPickup()
        {
            List<Package> PackageList = new List<Package>();
            int number = 0;

            try
            {
                PackageList = (from p in Globals.ctx.Packages where p.Status == "InfoReceived" select p).ToList<Package>();
                number = PackageList.Count();

                if (PackageList.Count() > 0)
                {
                    foreach (Package package in PackageList)
                    {
                        int count = (from s in Globals.ctx.ScheduledDeliveries where s.PackageId == package.PackageId select s).ToList<ScheduledDelivery>().Count;
                        if (count > 0)
                        {
                            number--;
                            continue;
                        }
                    }
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package picking-up schedule : Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return number;

        }

        // calculate the pending delivery packages number information
        private int GetPendingDelivery()
        {
            List<Package> PackageList = new List<Package>();
            int number = 0;

            try
            {
                PackageList = (from p in Globals.ctx.Packages where p.Status == "OutForDeliver" select p).ToList<Package>();
                number = PackageList.Count();

                if (PackageList.Count() > 0)
                {
                    foreach (Package package in PackageList)
                    {
                        int count = (from s in Globals.ctx.ScheduledDeliveries where s.PackageId == package.PackageId select s).ToList<ScheduledDelivery>().Count;
                        if (count > 1)
                        {
                            number--;
                            continue;
                        }
                    }
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package delivery schedule : Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return number;

        }

        // Get the processing center id information according the city(sender or recipient) information
        // if there are more than one processing centers in one city,then fetch the processing center id randomly

        private short GetProcCenterId(string city)
        {
            List<ProcCenter> pclist = new List<ProcCenter>();
            Random random = new Random();

            try
            {
                pclist = (from c in Globals.ctx.ProcCenters where c.City == city select c).ToList<ProcCenter>();    //ToList<ProcCenter>();

                if (pclist.Count > 1)
                {
                    return pclist[random.Next(pclist.Count)].ProcCenterId;
                }
                else if (pclist.Count == 1)
                {
                    return pclist[0].ProcCenterId;
                }
                else
                {
                    return -1;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("No ProcCenter id is found!" + ex.Message);
                return -1;
            }
        }

        
        private short GetProcCenterIdByZipcode(Package p)
        {
            try
            {
                ProcCenter pc = (from c in Globals.ctx.ProcCenters where c.Zipcode.Substring(0, 3) == p.SenderZipcode.Substring(0, 3) select c).First();    //ToList<ProcCenter>();

                if (pc != null)
                {
                    return pc.ProcCenterId;
                }
                else
                {
                    MessageBox.Show("No ProcCenter id is found!");
                    return -1;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("No ProcCenter id is found!" + ex.Message);
                return -1;
            }
        }

        // Get the avalible staffid information according to the processing center id
        // if there are more than one couriers in a processing center,then fetch the staffid randomly
        private short GetStaffId(short proccenterid)
        {
            List<Staff> stafflist = new List<Staff>();
            Random random = new Random();

            try
            {
                stafflist = (from s in Globals.ctx.Staffs where (s.ProcCenterId == proccenterid && s.Position == "Courier") select s).ToList<Staff>();

                if (stafflist.Count > 1)
                {
                    return stafflist[random.Next(stafflist.Count)].StaffId;
                }
                else if (stafflist.Count == 1)
                {
                    return stafflist[0].StaffId;
                }
                else
                {
                    return -1;
                }

            }
            catch (SystemException ex)
            {
                MessageBox.Show("No Staff id is found!" + ex.Message);
                return -1;
            }
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Maintain the staffs(Couriers and Managers) information: adding , updating and deleting
        // By invoke the Staff_AddUpdate_Dialog
        private void BtStaffs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Staff_AddUpdate_Dialog saDialog = new Staff_AddUpdate_Dialog();
                saDialog.Owner = this;

                bool? result = saDialog.ShowDialog();
                if (result == true)
                {
                    MessageBox.Show("A new staff was added successfully");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database Operation error!" + ex.Message);
                return;
            }

        }

        // Maintain the processing center information: adding , updating and deleting
        // By invoke the ProceCenter_AddUpdate_Dialog
        private void BtProcCenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcCenter_AddUpdate_Dialog paDialog = new ProcCenter_AddUpdate_Dialog();
                paDialog.Owner = this;

                bool? result = paDialog.ShowDialog();
                if (result == true)
                {
                    MessageBox.Show("A new Proc Center was added successfully");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Database Operation error!" + ex.Message);
                return;
            }
        }

        ///     can schedule the package picking-up tasks and package delivery tasks( to different couriers) automatically 
        ///     can schedule package shipment tasks (from sendercity to recipientcity) manually
        // schedule the package picking-up tasks ( to different couriers ) automatically
        private void BtPickUpSchedule_Click(object sender, RoutedEventArgs e)
        {
            List<Package> PackageList = new List<Package>();

            try
            {
                // Get all packages information need to be scheduled to be picked up
                PackageList = (from p in Globals.ctx.Packages where p.Status == "InfoReceived" select p).ToList<Package>();
                if (PackageList.Count() > 0)
                {
                    foreach (Package package in PackageList)
                    {
                        // To check whether the task has been scheduled, if so, skip the package
                        int count = (from s in Globals.ctx.ScheduledDeliveries where s.PackageId == package.PackageId select s).ToList<ScheduledDelivery>().Count;
                        if ( count > 0)
                        {
                            continue;
                        }

                        // get the processing center id according to the sender city information
                        short pcid = GetProcCenterId(package.SenderCity);
                        if (pcid == -1)
                        {
                            MessageBox.Show("There is no Proc Center in that city,Please check the ProcCenter information");
                            continue;
                        }

                        // get the availible courier id information according to the processing center id information
                        short staffid = GetStaffId(pcid);
                        if (staffid == -1)
                        {
                            MessageBox.Show("There is no couriers in that Proc Center,Please check the staffs information");
                            continue;
                        }

                        // assign this package ( a package picking-up task) to the courier 
                        ScheduledDelivery sd = new ScheduledDelivery
                        {
                            PackageId = package.PackageId,
                            StaffId = staffid,
                            ScheduledDateTime = System.DateTime.Today,
                            ScheduleType = 0,          // schedule Id = 0 means it's a package picking-up request
                            Status = 0,                // Status = 0 means it's scheduled but not processed,after the courier's process, Status will be assigned to 1 
                        };
                        Globals.ctx.ScheduledDeliveries.Add(sd);
                        Globals.ctx.SaveChanges();
                    }
                    MessageBox.Show("All package picking-up request processed!");
                    lblPendingPickUp.Content = 0;
                }
                else
                {
                    MessageBox.Show("No package picking-up request need to be handled!");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package picking-up schedule : Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        // schedule package shipment tasks (from sendercity to recipientcity) manually
        // invoke the Package_Shipment_Schedule_Dialog to process the shipment operations
        private void BtShipmentSchedule_Click(object sender, RoutedEventArgs e)
        {
            List<Package> PackageList = new List<Package>();

            try
            {
                // Get all packages information need to be scheduled to be shipped
                PackageList = (from p in Globals.ctx.Packages where (p.Status == "PackageReceived" || p.Status == "Routed") select p).ToList<Package>();
                if (PackageList.Count() > 0)
                {
                    foreach (Package package in PackageList)
                    {

                        if (package.RecipientCity.Equals(package.SenderCity))
                        {
                            // Condition 1: If the recipientcity == sendercity
                            short pcid = GetProcCenterId(package.SenderCity);
                            if (pcid == -1)
                            {
                                MessageBox.Show("There is no Proc Center in that city,Please check the ProcCenter information");
                                continue;
                            }

                            PackageHistory ph = new PackageHistory
                            {
                                PackageId = package.PackageId,
                                ProcCenterId = pcid,
                                ProcDate = System.DateTime.Today,
                            };

                            Globals.ctx.PackageHistories.Add(ph);
                            Globals.ctx.SaveChanges();

                            package.Status = "OutForDeliver";
                            Globals.ctx.SaveChanges();
                        }
                        else
                        {
                            // Condition 2: If the RecipientCity != Sendercity and Status = "PackageReceived"
                            if (package.Status.Equals("PackageReceived"))
                            {
                                short pid = GetProcCenterId(package.SenderCity);
                                if (pid == -1)
                                {
                                    MessageBox.Show("There is no Proc Center in that city,Please check the ProcCenter information");
                                    continue;
                                }

                                PackageHistory ph = new PackageHistory
                                {
                                    PackageId = package.PackageId,
                                    ProcCenterId = pid,
                                    ProcDate = System.DateTime.Today,
                                };
                                Globals.ctx.PackageHistories.Add(ph);
                                Globals.ctx.SaveChanges();

                                Package_Shipment_Schedule_Dialog pssDialog = new Package_Shipment_Schedule_Dialog(package, package.SenderCity);
                                pssDialog.Owner = this;

                                bool? result = pssDialog.ShowDialog();
                                if (result == true)
                                {
                                    MessageBox.Show("Package Shipment Dialog is finished");
                                }

                            }
                            // Condition 3: If the RecipientCity != Sendercity and Status = "Routed"
                            else
                            {
                                short proccenterid = (from ph in Globals.ctx.PackageHistories where ph.PackageId == package.PackageId orderby ph.ProcDate descending select ph.ProcCenterId).First();

                                string procentercity = (from pc in Globals.ctx.ProcCenters where pc.ProcCenterId == proccenterid select pc.City).First();

                                Package_Shipment_Schedule_Dialog pssDialog = new Package_Shipment_Schedule_Dialog(package, procentercity);
                                pssDialog.Owner = this;

                                bool? result = pssDialog.ShowDialog();
                                if (result == true)
                                {
                                    MessageBox.Show("Package Shipment Dialog is finished");
                                }
                            }
                        }
                    }
                    MessageBox.Show("All package shipment requests processed!");
                    lblPendingShipment.Content = 0;
                }
                else
                {
                    MessageBox.Show("No package shipment request need to be handled!");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "ProcCenter Management: Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        // schedule the package delivery tasks ( to different couriers ) automatically
        private void BtDiliverySchedule_Click(object sender, RoutedEventArgs e)
        {
            List<Package> PackageList = new List<Package>();

            try
            {
                // Get all packages information need to be scheduled to be delivered
                PackageList = (from p in Globals.ctx.Packages where p.Status == "OutForDeliver" select p).ToList<Package>();
                if (PackageList.Count() > 0)
                {
                    foreach (Package package in PackageList)
                    {
                        // To check whether the task has been scheduled, if so, skip the package
                        int count = (from s in Globals.ctx.ScheduledDeliveries where s.PackageId == package.PackageId select s).ToList<ScheduledDelivery>().Count;
                        if (count > 1)
                        {
                            continue;
                        }

                        // get the processing center id according to the recipient city information
                        short pcid = GetProcCenterId(package.RecipientCity);
                        if (pcid == -1)
                        {
                            MessageBox.Show("There is no Proc Center in that city,Please check the ProcCenter information");
                            continue;
                        }

                        // get the availible courier id information according to the processing center id information
                        short staffid = GetStaffId(pcid);
                        if (staffid == -1)
                        {
                            MessageBox.Show("There is no couriers in that Proc Center,Please check the staffs information");
                            continue;
                        }

                        // assign this package ( a package delivery task)to the courier 
                        ScheduledDelivery sd = new ScheduledDelivery
                        {
                            PackageId = package.PackageId,
                            StaffId = staffid,
                            ScheduledDateTime = System.DateTime.Today,
                            ScheduleType = 1,          // schedule Id = 1 means it's a package delivery request
                            Status = 0,                // Status = 0 means it's scheduled but not processed,after the courier's process, Status will be assigned to 1
                        };
                        Globals.ctx.ScheduledDeliveries.Add(sd);
                        Globals.ctx.SaveChanges();
                    }
                    MessageBox.Show("All package delivery request processed!");
                    lblPendingDelivery.Content = 0;
                }
                else
                {
                    MessageBox.Show("No package picking-up request need to be handled!");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Package delivery schedule : Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        // print the content in manager dashboard with a printdialog
        private void BtPrinter_Click(object sender, RoutedEventArgs e)
        {
            // Create the print dialog object and set options
            PrintDialog pDialog = new PrintDialog();
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = true;

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = pDialog.ShowDialog();
            if (print == true)
            {
                string xpsfilename = "test.xps";
                if (File.Exists(xpsfilename) == true)
                    File.Delete(xpsfilename);

                XpsDocument xpsDocument = new XpsDocument(xpsfilename, FileAccess.ReadWrite);

                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);

                SerializerWriterCollator output_document = writer.CreateVisualsCollator();

                output_document.BeginBatchWrite();
                output_document.Write(PrintArea);
                output_document.EndBatchWrite();

                FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
                pDialog.PrintDocument(fixedDocSeq.DocumentPaginator, "Print Dashboard");
            }
        }


        // manager can change his password with the Manager_Change_Password_Dialog 
        private void BtAccount_Click(object sender, RoutedEventArgs e)
        {
            Manager_Change_Password_Dialog mdialog = new Manager_Change_Password_Dialog();
            mdialog.Owner = this;
            mdialog.ShowDialog();
        }

    }
}
