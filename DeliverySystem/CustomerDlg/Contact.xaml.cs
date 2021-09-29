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
    /// Interaction logic for Contact.xaml
    /// </summary>
    public partial class Contact : Page
    {
        private CustomerDialog _parentWin;
        public CustomerDialog ParentWindow
        {
            get { return _parentWin; }
            set { _parentWin = value; }
        }

        public Contact()
        {
            InitializeComponent();
        }

    }
}
