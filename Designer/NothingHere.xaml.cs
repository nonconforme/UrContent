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

namespace Designer
{
    /// <summary>
    /// Interaction logic for NothingHere.xaml
    /// </summary>
    public partial class NothingHere : UserControl
    {
        public NothingHere(string toSelect)
        {
            InitializeComponent();
            txtText.Text = String.Format("Select a {0}", toSelect);
        }
    }
}
