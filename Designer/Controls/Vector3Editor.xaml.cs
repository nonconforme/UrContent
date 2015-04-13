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

namespace Designer.Controls {
    /// <summary>
    /// Interaction logic for Vector3Editor.xaml
    /// </summary>
    public partial class Vector3Editor : UserControl {
        public Vector3Editor(string name) {
            InitializeComponent();
            DataContextChanged += Vector3Editor_DataContextChanged;
            xVal.SetBinding(TextBox.TextProperty, new Binding(name + ".X"));
            yVal.SetBinding(TextBox.TextProperty, new Binding(name + ".Y"));
            zVal.SetBinding(TextBox.TextProperty, new Binding(name + ".Z"));
        }

        void Vector3Editor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            xVal.DataContext = DataContext;
            yVal.DataContext = DataContext;
            zVal.DataContext = DataContext;
        }
    }
}
