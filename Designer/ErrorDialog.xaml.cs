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
using System.Windows.Shapes;

namespace Designer
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog : ModernDialog
    {
        public static void Show(string aText, string aURL = "") {
            ErrorDialog dlg = new ErrorDialog(aText, aURL);
            dlg.ShowDialog();
        }

        public ErrorDialog(string aErrorText, string aReportTo = "") {
            InitializeComponent();

            errorText.TextWrapping = TextWrapping.Wrap;
            errorText.Text = aErrorText;

            Button rptButton = new Button { Content = "Report" };
            rptButton.Click += onReportClick;
            Button closeButton = new Button { Content = "Close" };
            closeButton.Click += onClose;

            Buttons = new Button[] { 
                rptButton,
                closeButton
            };
            foreach (Button bt in Buttons)
                bt.Style = FindResource("StyledButton") as Style;
        }

        void onReportClick(object sender, EventArgs e) {
            //Debugger.Net.Emailer.MailTo(errorText.Text);
            Close();
        }

        void onClose(object sender, EventArgs e) {
            Close();
        }
    }
}
