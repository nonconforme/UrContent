using Designer.Controls;
using FirstFloor.ModernUI.Windows;
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

namespace Designer.Effects {
    /// <summary>
    /// Interaction logic for EffectList.xaml
    /// </summary>
    public partial class EffectList : UserControl, IContent {
        EffectFolder effects_;
        ReflectiveForm form_;

        public EffectList() {
            InitializeComponent();
            form.Children.Add(form_ = new ReflectiveForm(typeof(Urho.ParticleEffect)));
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e) {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e) {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e) {
            effectList.DataContext = Project.inst().ParticleEffects;
            effectList.SelectedItemChanged += effectList_SelectedItemChanged;
        }

        void effectList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            if (e.NewValue is EffectLeaf)
                form_.SetObject(((EffectLeaf)e.NewValue).DisplayName, ((EffectLeaf)e.NewValue).Effect);
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e) {
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            EffectLeaf l = effectList.SelectedItem as EffectLeaf;
            if (l != null) {
                l.Effect.Save();
            }
        }
    }
}
