using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for ReflectiveForm.xaml
    /// </summary>
    public partial class ReflectiveForm : UserControl {
        Type type_;
        List<Control> controls_ = new List<Control>();
        object target_;
        public ReflectiveForm(Type dataType) {
            InitializeComponent();
            type_ = dataType;
            fill();
        }

        void fill() {
            controlStack.Children.Clear();
            foreach (PropertyInfo pi in type_.GetProperties()) {
                Label lbl = new Label { Content = pi.Name };
                lbl.FontWeight = FontWeights.DemiBold;
                lbl.MinWidth = 120;
                StackPanel pnl = new StackPanel { Orientation = Orientation.Horizontal };
                pnl.Margin = new Thickness(5);
                if (pi.PropertyType == typeof(int) || pi.PropertyType == typeof(float)) {
                    TextBox tb = new TextBox();
                    tb.MinWidth = 240;
                    tb.SetBinding(TextBox.TextProperty, new Binding(pi.Name));
                    tb.Tag = pi;
                    pnl.Children.Add(lbl);
                    pnl.Children.Add(tb);
                    controls_.Add(tb);
                } else if (pi.PropertyType == typeof(bool)) {
                    CheckBox cb = new CheckBox();
                    cb.Content = pi.Name;
                    Binding binding = new Binding(pi.Name);
                    if (!pi.CanWrite)
                        binding.Mode = BindingMode.OneWay;
                    cb.SetBinding(CheckBox.IsCheckedProperty, binding);
                    cb.Tag = pi;
                    pnl.Children.Add(cb);
                    controls_.Add(cb);
                } else if (pi.PropertyType == typeof(string)) {
                    Urho.Resource res = pi.GetCustomAttribute<Urho.Resource>();
                    if (res != null) {
                        ComboBox cb = new ComboBox();
                        if (res.Type == typeof(Urho.Material)) {
                            cb.ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_MATERIALS, false);
                        } else if (res.Type == typeof(Urho.Shader)) {
                            cb.ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_SHADERS, false);
                        } else if (res.Type == typeof(Urho.Technique)) {
                            cb.ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_TECHNIQUES, false);
                        } else if (res.Type == typeof(Urho.BaseTexture) || res.Type == typeof(Urho.Texture)) {
                            cb.ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_TEXTURES, true);
                        }
                        cb.SetBinding(ComboBox.SelectedItemProperty, new Binding(pi.Name));
                        cb.MinWidth = 240;
                        cb.Tag = pi;
                        pnl.Children.Add(lbl);
                        pnl.Children.Add(cb);
                        controls_.Add(cb);
                    } else {
                        TextBox tb = new TextBox();
                        tb.MinWidth = 240;
                        tb.SetBinding(TextBox.TextProperty, new Binding(pi.Name));
                        tb.Tag = pi;
                        pnl.Children.Add(lbl);
                        pnl.Children.Add(tb);
                        controls_.Add(tb);
                    }
                } else if (pi.PropertyType == typeof(Urho.MinMax)) {
                    MinMaxEditor tb = new MinMaxEditor(pi.Name);
                    tb.Tag = pi;
                    tb.SetBinding(MinMaxEditor.DataContextProperty, new Binding(pi.Name));
                    pnl.Children.Add(lbl);
                    pnl.Children.Add(tb);
                    controls_.Add(tb);
                } else if (pi.PropertyType == typeof(Urho.Vector2)) {
                    Vector2Editor tb = new Vector2Editor(pi.Name);
                    tb.Tag = pi;
                    tb.SetBinding(Vector2Editor.DataContextProperty, new Binding(pi.Name));
                    pnl.Children.Add(lbl);
                    pnl.Children.Add(tb);
                    controls_.Add(tb);
                } else if (pi.PropertyType == typeof(Urho.Vector3)) {
                    Vector3Editor tb = new Vector3Editor(pi.Name);
                    tb.SetBinding(Vector3Editor.DataContextProperty, new Binding(pi.Name));
                    tb.Tag = pi;
                    pnl.Children.Add(lbl);
                    pnl.Children.Add(tb);
                    controls_.Add(tb);
                } else if (pi.PropertyType == typeof(Urho.Vector4)) {
                    Vector4Editor tb = new Vector4Editor(pi.Name);
                    tb.SetBinding(Vector4Editor.DataContextProperty, new Binding(pi.Name));
                    tb.Tag = pi;
                    pnl.Children.Add(lbl);
                    pnl.Children.Add(tb);
                    controls_.Add(tb);
                } else if (pi.PropertyType == typeof(Urho.UColor)) {
                    //TODO show a color picker
                    Xceed.Wpf.Toolkit.ColorPicker col = new Xceed.Wpf.Toolkit.ColorPicker();
                    Binding binding = new Binding(pi.Name);
                    binding.Converter = new ColorToColorConverter();
                    col.SetBinding(Xceed.Wpf.Toolkit.ColorPicker.SelectedColorProperty, binding);
                    col.Tag = pi;
                    pnl.Children.Add(lbl);
                    pnl.Children.Add(col);
                    controls_.Add(col);
                } else if (pi.PropertyType.IsEnum) {
                    ComboBox cb = new ComboBox();
                    cb.ItemsSource = Enum.GetValues(pi.PropertyType);
                    cb.SetBinding(ComboBox.SelectedItemProperty, new Binding(pi.Name));
                    cb.Tag = pi;
                    pnl.Children.Add(lbl);
                    pnl.Children.Add(cb);
                    controls_.Add(cb);
                } else {
                    try {
                        if (pi.PropertyType.GetGenericTypeDefinition() == typeof(ObservableCollection<>)) { //LIST
                            GridEditor dg = new GridEditor(pi.PropertyType.GetGenericArguments()[0]);
                            dg.Grid.SetBinding(DataGrid.ItemsSourceProperty, new Binding(pi.Name));
                            pnl.Children.Add(lbl);
                            pnl.Children.Add(dg);
                            controls_.Add(dg);
                        } else {
                            ReflectiveForm form = new ReflectiveForm(pi.PropertyType);
                            form.Tag = pi;
                            pnl.Children.Add(lbl);
                            pnl.Children.Add(form);
                            controls_.Add(form);
                        }
                    } catch (Exception ex) {
                        ReflectiveForm form = new ReflectiveForm(pi.PropertyType);
                        pnl.Children.Add(lbl);
                        pnl.Children.Add(form);
                        controls_.Add(form);
                    }
                }
                controlStack.Children.Add(pnl);
            }
        }

        public void SetObject(string title, object obj) {
            this.title.Text = title;
            foreach (Control ctrl in controls_) {
                if (ctrl is ReflectiveForm) {
                    ((ReflectiveForm)ctrl).SetObject("", ((PropertyInfo)((ReflectiveForm)ctrl).Tag).GetValue(obj));
                } else if (ctrl is GridEditor) {
                    ((GridEditor)ctrl).Grid.DataContext = obj;
                } else
                    ctrl.DataContext = obj;
            }
        }

    }
}
