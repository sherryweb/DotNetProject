using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeliverySystem
{
    class ChartDataCreator
    {
        // the data class to prepare the flexchart data used in manager dashboard
        public static List<DataItem> CreateData()
        {
            var data = new List<DataItem>();

            List<Staff> stafflist = new List<Staff>();

            stafflist = (from s in Globals.ctx.Staffs where s.Position == "Courier" select s).ToList<Staff>();
            if (stafflist.Count() > 0)
            {
                foreach (Staff staff in stafflist)
                {

                    string name = staff.Name;

                    int pickingup = (from sd in Globals.ctx.ScheduledDeliveries where (sd.StaffId == staff.StaffId && sd.ScheduleType == 0 ) select sd.PackageId).Count();

                    int delivery  = (from sd in Globals.ctx.ScheduledDeliveries where (sd.StaffId == staff.StaffId && sd.ScheduleType == 1 ) select sd.PackageId).Count();

                    data.Add(new DataItem(name, pickingup, delivery));

                }
            }
            else
            {
                MessageBox.Show("No package picking-up request need to be handled!");
            }

            return data;
        }
    }

    public class DataItem
    {
        public DataItem(string name, int pickingup, int delivery)
        {
            StaffName = name;
            PickingUp = pickingup;
            Delivery = delivery;
        }

        public string StaffName { get; set; }
        public int PickingUp { get; set; }
        public int Delivery { get; set; }
    }

}
