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
    /// Interaction logic for MinMaxEditor.xaml
    /// </summary>
    public partial class MinMaxEditor : UserControl {
        public MinMaxEditor(string name) {
            InitializeComponent();
            this.DataContextChanged += MinMaxEditor_DataContextChanged;
            minVal.SetBinding(TextBox.TextProperty, new Binding(name + ".Min"));
            maxVal.SetBinding(TextBox.TextProperty, new Binding(name + ".Min"));
        }

        void MinMaxEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            minVal.DataContext = DataContext;
            maxVal.DataContext = DataContext;
        }
    }
}
