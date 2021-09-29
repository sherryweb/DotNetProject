using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem
{
    class DataCreator
    {
        // the data class to prepare the piechart data used in manager dashboard
        public static List<ProcCenterItem> StatisticVolumePerProcCenter()
        {
            var ct = Globals.CityList.Count;
            var result = new List<ProcCenterItem>();
            for (var i = 0; i < ct; i++)
            {
                string city = Globals.CityList[i];

                short pcid = (short)(from c in Globals.ctx.ProcCenters where c.City == city select c.ProcCenterId).First();

                int bv = (from ph in Globals.ctx.PackageHistories where ph.ProcCenterId == pcid select ph.PackageId).Count();

                result.Add(new ProcCenterItem()
                {
                    City = city,
                    BusinessVolume = bv,

                });

            }
            return result;
        }
    }

    public class ProcCenterItem
    {
        public string City { get; set; }
        public int BusinessVolume { get; set; }
    }
}
