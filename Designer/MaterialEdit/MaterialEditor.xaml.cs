using Designer.Controls;
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

namespace Designer.MaterialEdit {
    /// <summary>
    /// Main ui for editing materials
    /// </summary>
    public partial class MaterialEditor : UserControl {
        ReflectiveForm form_;
        NothingHere nothing_;

        public MaterialEditor() {
            InitializeComponent();
            formStack.Children.Add(form_ = new ReflectiveForm(typeof(Urho.Material)));
            texTree.DataContext = Project.inst().Materials;
            texTree.SelectedItemChanged += texTree_SelectedItemChanged;
            formStack.Children.Add(nothing_ = new NothingHere("Material"));
            nothing_.Visibility = System.Windows.Visibility.Visible;
            form_.Visibility = System.Windows.Visibility.Collapsed;
        }

        void texTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            MaterialLeaf l = e.NewValue as MaterialLeaf;
            if (l != null) {
                form_.Visibility = System.Windows.Visibility.Visible;
                nothing_.Visibility = System.Windows.Visibility.Collapsed;
                form_.SetObject(l.Name, l.Material);
            } else {
                form_.Visibility = System.Windows.Visibility.Collapsed;
                nothing_.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            MaterialLeaf l = texTree.SelectedItem as MaterialLeaf;
            if (l != null)
                l.Material.Save();
        }
    }
}
