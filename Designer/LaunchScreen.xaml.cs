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
    /// Interaction logic for LaunchScreen.xaml
    /// </summary>
    public partial class LaunchScreen : UserControl {
        public LaunchScreen() {
            InitializeComponent();
            foreach (string str in UserData.inst().RecentFiles) {
                if (System.IO.Directory.Exists(str)) {
                    Button tb = new Button { Content = str, Tag = str };
                    stackRecent.Children.Add(tb);
                    tb.Click += tb_Click;
                }
            }
        }

        void tb_Click(object sender, RoutedEventArgs e) {
            Button btn = sender as Button;
            new Project(btn.Tag.ToString());
            MainWindow.FillLinks();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                new Project(dlg.SelectedPath);
                MainWindow.FillLinks();
            }
        }
    }
}
