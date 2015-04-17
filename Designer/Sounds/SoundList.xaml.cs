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
        NothingHere nothing_;

        public SoundList() {
            InitializeComponent();
            formStack.Children.Add(form_ = new ReflectiveForm(typeof(Urho.Sound)));
            form_.Visibility = System.Windows.Visibility.Collapsed;
            texTree.DataContext = Project.inst().SoundFiles;
            texTree.SelectedItemChanged += texTree_SelectedItemChanged;

            DataGridTemplateColumn col = new DataGridTemplateColumn();
            Binding imgBinding = new Binding("Name");
            FrameworkElementFactory vbtn = new FrameworkElementFactory(typeof(Button));
            vbtn.SetValue(Button.ContentProperty, "Play");
            vbtn.SetValue(Button.MarginProperty, new Thickness(2));
            vbtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(onPlayGridSound));
            ((DataGridTemplateColumn)col).CellTemplate = new DataTemplate();
            ((DataGridTemplateColumn)col).CellTemplate.VisualTree = vbtn;
            col.Header = "Play";
            gridView.Columns.Add(col);

            gridView.DataContext = Project.inst().SoundFiles.GetFlat();

            nothing_ = new NothingHere("Sound Effect");
            formStack.Children.Add(nothing_);
            nothing_.Visibility = System.Windows.Visibility.Visible;
        }

        void texTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            SoundLeaf l = e.NewValue as SoundLeaf;
            if (l != null)
            {
                nothing_.Visibility = System.Windows.Visibility.Collapsed;
                form_.Visibility = System.Windows.Visibility.Visible;
                form_.SetObject(l.Name, l.Sound);
            }
            else
            {
                nothing_.Visibility = System.Windows.Visibility.Visible;
                form_.Visibility = System.Windows.Visibility.Collapsed;
            }
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

        void onPlayGridSound(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                // This is a little hacky
                DependencyObject obj = VisualTreeHelper.GetParent(btn);
                if (obj is ContentPresenter)
                {
                    Urho.Sound snd = ((ContentPresenter)obj).DataContext as Urho.Sound;
                    if (player_ == null)
                        player_ = new System.Media.SoundPlayer();
                    player_.SoundLocation = snd.Name;
                    player_.Play();
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
