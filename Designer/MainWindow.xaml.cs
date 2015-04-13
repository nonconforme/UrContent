using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
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

namespace Designer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow {
        static MainWindow inst_;

        public MainWindow() {
            inst_ = this;
            InitializeComponent();
            ContentSource = new Uri("LaunchScreen.xaml", UriKind.Relative);
            LinkNavigator.Commands.Add(new Uri("cmd://showSettings", UriKind.Absolute), new RelayCommand(o => showSettings()));
        }

        public static MainWindow inst() { return inst_; }

        void showSettings() {
            SettingsDlg dlg = new SettingsDlg();
            dlg.ShowDialog();
        }

        public static void FillLinks() {
            inst_.TitleLinks.Clear();
            inst_.TitleLinks.Add(new Link {
                DisplayName = "Materials",
                Source = new Uri("MaterialEdit/MaterialEditor.xaml", UriKind.Relative)
            });
            inst_.TitleLinks.Add(new Link {
                DisplayName = "Textures",
                Source = new Uri("TextureMan/TextureManager.xaml", UriKind.Relative)
            });
            inst_.TitleLinks.Add(new Link {
                DisplayName = "Effects",
                Source = new Uri("Effects/EffectList.xaml", UriKind.Relative)
            });
            inst_.TitleLinks.Add(new Link {
                DisplayName = "Sounds",
                Source = new Uri("Sounds/SoundList.xaml", UriKind.Relative)
            });
            //inst_.TitleLinks.Add(new Link {
            //    DisplayName = "Shaders",
            //    Source = new Uri("Graph/GraphCanvas.xaml", UriKind.Relative)
            //});
            //inst_.TitleLinks.Add(new Link {
            //    DisplayName = "Settings",
            //    Source = new Uri("Settings/SettingsPage.xaml", UriKind.Relative)
            //});
            inst_.ContentSource = new Uri("MaterialEdit/MaterialEditor.xaml", UriKind.Relative);
        }

        public static void ClearLinks() {
            inst_.TitleLinks.Clear();
        }
    }
}
