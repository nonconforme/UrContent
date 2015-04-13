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

namespace Designer.Graph {
    /// <summary>
    /// Interaction logic for GraphCanvas.xaml
    /// </summary>
    public partial class GraphCanvas : UserControl {

        List<GraphNode> nodes_ = new List<GraphNode>();
        List<CurvedArrow> arrows_ = new List<CurvedArrow>();

        public GraphCanvas() {
            InitializeComponent();
            GraphNode sp = new GraphNode();
            
            canvas.Children.Add(sp);
            sp.MouseDown += sp_MouseDown;
            sp.MouseUp += sp_MouseUp;
            sp.MouseMove += sp_MouseMove;
            dragTarget_ = sp;
            canvas.Background = new SolidColorBrush(Colors.Pink);
        }

        GraphNode dragTarget_;
        bool dragging_;
        void sp_MouseMove(object sender, MouseEventArgs e) {
            if (dragging_ && !firstDown_) {
                Point currentPosition = e.GetPosition(dragTarget_);

                var transform = dragTarget_.RenderTransform as TranslateTransform;
                if (transform == null) {
                    transform = new TranslateTransform();
                    dragTarget_.RenderTransform = transform;
                }

                transform.X += currentPosition.X - clickPosition.X;
                transform.Y += currentPosition.Y - clickPosition.Y;
                clickPosition = e.GetPosition(dragTarget_);
                //transform.X = currentPosition.X - clickPosition.X;
                //transform.Y = currentPosition.Y - clickPosition.Y;
                if (transform.X + dragTarget_.ActualWidth > canvas.MinWidth) {
                    canvas.MinWidth = canvas.Width = transform.X + dragTarget_.ActualWidth * 2;
                    canvas.InvalidateMeasure();
                    scroller.InvalidateMeasure();
                    scroller.InvalidateScrollInfo();
                }
            } else if (firstDown_)
                firstDown_ = false;
        }

        void sp_MouseUp(object sender, MouseButtonEventArgs e) {
            dragging_ = false;
            dragTarget_.ReleaseMouseCapture();
        }

        Point clickPosition;
        bool firstDown_;
        void sp_MouseDown(object sender, MouseButtonEventArgs e) {
            if (!dragging_) {
                dragging_ = true;
                clickPosition = e.GetPosition(dragTarget_);
                //var transform = dragTarget_.RenderTransform as TranslateTransform;
                //if (transform == null) {
                //    transform = new TranslateTransform();
                //    dragTarget_.RenderTransform = transform;
                //    transform.X = 0;
                //    transform.Y = 0;
                //}
                firstDown_ = true;
                dragTarget_.CaptureMouse();
            }
        }
    }
}
