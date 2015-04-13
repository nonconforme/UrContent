using Designer.Controls;
using FirstFloor.ModernUI.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Designer.TextureMan {
    /// <summary>
    /// Interaction logic for TextureManager.xaml
    /// </summary>
    public partial class TextureManager : UserControl, IContent {
        ObservableCollection<Urho.BaseTexture> textures_;
        ReflectiveForm textureForm_;
        ReflectiveForm cubeForm_;
        ObservableCollection<Urho.Texture> flat_ = new ObservableCollection<Urho.Texture>();

        public TextureManager() {
            InitializeComponent();

            DataGridTemplateColumn col = new DataGridTemplateColumn();
            Binding imgBinding = new Binding("Name");
            FrameworkElementFactory imgFactory = new FrameworkElementFactory(typeof(Image));
            imgFactory.SetBinding(Image.SourceProperty, imgBinding);
            imgFactory.SetValue(Image.MaxHeightProperty, 64.0);
            imgFactory.SetValue(Image.MaxWidthProperty, 64.0);
            ((DataGridTemplateColumn)col).CellTemplate = new DataTemplate();
            ((DataGridTemplateColumn)col).CellTemplate.VisualTree = imgFactory;
            col.Header = "Preview";
            gridView.Columns.Add(col);

            formStack.Children.Add(textureForm_ = new ReflectiveForm(typeof(Urho.Texture)));
            texTree.DataContext = Project.inst().Textures;
            texTree.SelectedItemChanged += texTree_SelectedItemChanged;
            gridView.DataContext = flat_ = Project.inst().Textures.GetFlat();
        }

        void texTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            TextureLeaf l = e.NewValue as TextureLeaf;
            if (l != null) {
                textureForm_.SetObject(l.Name, l.Texture as Urho.Texture);
                preview.Source = new BitmapImage(new Uri(l.Texture.Name));
            } else
                preview.Source = null;
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e) {
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e) {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e) {
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e) {
        }

        private void btnSaveTexture_Click(object sender, RoutedEventArgs e) {
            TextureLeaf l = texTree.SelectedItem as TextureLeaf;
            if (l != null) {
                l.Texture.Save();
            }
        }

        private void txtQuery_PreviewKeyUp(object sender, KeyEventArgs e) {

        }
    }
}
