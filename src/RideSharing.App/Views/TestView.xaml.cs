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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RideSharing.App.Views
{
    /// <summary>
    /// Interakční logika pro TestView.xaml
    /// </summary>
    public partial class TestView : Page
    {
        public TestView(Guid userId)
        {
            InitializeComponent();
        }
    }
}
