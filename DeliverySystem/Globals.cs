using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DeliverySystem
{
    public class Globals
    {
        // Declare some global varaibles used in the projects
        // The remote database connection context
        public static DeliverySystemDBConnection ctx;
        // The Province list used in project
        public static List<string> ProvinceList = new List<string> {"QC", "ON", "MB", "SK", "AB", "BC", "YT", "NT", "NU", "NL", "PE", "NS", "NB"};
        // The City list used in project
        public static List<string> CityList = new List<string> { "Brossard", "Laval", "Montreal", "Kirkland", "Beaconsfield", "Longueuil" };
        // The Position list used in project
        public static List<string> PositionList = new List<string> { "Courier", "Manager", "General Manager", "Sysadmin"};

        // The variable to save current logined courier id
        public static short curCourierId;
        // The variable to save current logined manager id
        public static short ManagerId;

    }
}
