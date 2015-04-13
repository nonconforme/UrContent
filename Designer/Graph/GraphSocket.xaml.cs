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

namespace Designer.Graph {
    /// <summary>
    /// Interaction logic for GraphSocket.xaml
    /// </summary>
    public partial class GraphSocket : UserControl {
        public GraphNode OwnerNode { get; set; }

        public GraphSocket(Color ringColor, Color centerColor) {
            InitializeComponent();
            ring.Fill = new SolidColorBrush(ringColor);
            area.Fill = new SolidColorBrush(centerColor);
        }
    }
}
