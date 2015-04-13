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

namespace Designer.Sounds {
    /// <summary>
    /// Interaction logic for SoundList.xaml
    /// </summary>
    public partial class SoundList : UserControl {
        ReflectiveForm form_;
        System.Media.SoundPlayer player_;

        public SoundList() {
            InitializeComponent();
            formStack.Children.Add(form_ = new ReflectiveForm(typeof(Urho.Sound)));
            texTree.DataContext = Project.inst().SoundFiles;
            texTree.SelectedItemChanged += texTree_SelectedItemChanged;
        }

        void texTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            SoundLeaf l = e.NewValue as SoundLeaf;
            if (l != null)
                form_.SetObject(l.Name, l.Sound);
        }

        private void btnPlaySnd_Click(object sender, RoutedEventArgs e) {
            if (player_ == null)
                player_ = new System.Media.SoundPlayer();
            Button btn = sender as Button;
            player_.SoundLocation = (btn.Tag as SoundLeaf).Sound.Name;
            player_.Play();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            SoundLeaf l = texTree.SelectedItem as SoundLeaf;
            if (l != null)
                l.Sound.Save();
        }
    }
}
