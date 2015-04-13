using System;
using System.Collections.Generic;
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
using Xceed.Wpf.Toolkit;

namespace Designer.Controls {
    /// <summary>
    /// Interaction logic for GridEditor.xaml
    /// </summary>
    public partial class GridEditor : UserControl {
        public GridEditor(Type aFillType) {
            InitializeComponent();
            Bind(aFillType);
        }

        public DataGrid Grid { get { return grid; } }

        void Bind(Type aType) {
            foreach (PropertyInfo pi in aType.GetProperties()) {
                DataGridColumn col = null;
                if (pi.PropertyType == typeof(int)) {
                    col = new DataGridTextColumn();
                    ((DataGridTextColumn)col).Binding = new Binding(pi.Name);
                } else if (pi.PropertyType == typeof(float)) {
                    col = new DataGridTextColumn();
                    ((DataGridTextColumn)col).Binding = new Binding(pi.Name);
                } else if (pi.PropertyType == typeof(bool)) {
                    col = new DataGridCheckBoxColumn();
                    ((DataGridCheckBoxColumn)col).Binding = new Binding(pi.Name);
                } else if (pi.PropertyType.IsEnum) {
                    col = new DataGridComboBoxColumn();
                    ((DataGridComboBoxColumn)col).SelectedItemBinding = new Binding(pi.Name);
                    ((DataGridComboBoxColumn)col).ItemsSource = Enum.GetValues(pi.PropertyType);
                } else if (pi.PropertyType == typeof(Urho.MinMax)) {
                    col = new DataGridTextColumn();
                    ((DataGridTextColumn)col).Binding = new Binding(pi.Name) { Converter = new MinMaxConverter() };
                    ((Binding)((DataGridTextColumn)col).Binding).ValidationRules.Add(new MinMaxValidator());
                } else if (pi.PropertyType == typeof(Urho.Vector2)) {
                    col = new DataGridTextColumn();
                    ((DataGridTextColumn)col).Binding = new Binding(pi.Name) { Converter = new Vec2Converter() };
                    ((Binding)((DataGridTextColumn)col).Binding).ValidationRules.Add(new Vec2Validator());
                } else if (pi.PropertyType == typeof(Urho.Vector3)) {
                    col = new DataGridTextColumn();
                    ((DataGridTextColumn)col).Binding = new Binding(pi.Name) { Converter = new Vec3Converter() };
                    ((Binding)((DataGridTextColumn)col).Binding).ValidationRules.Add(new Vec3Validator());
                } else if (pi.PropertyType == typeof(Urho.Vector4)) {
                    col = new DataGridTextColumn();
                    ((DataGridTextColumn)col).Binding = new Binding(pi.Name) { Converter = new Vec4Converter() };
                    ((Binding)((DataGridTextColumn)col).Binding).ValidationRules.Add(new Vec4Validator());
                } else if (pi.PropertyType == typeof(Urho.UColor)) {
                    DataGridTemplateColumn c = null;
                    col = c = new DataGridTemplateColumn();
                    DataGridTemplateColumn guidColumn = c as DataGridTemplateColumn;

                    Binding bind = new Binding(pi.Name);
                    bind.Converter = new ColorToColorConverter();
                    bind.Mode = BindingMode.TwoWay;

                    Binding colorBind = new Binding(pi.Name);
                    colorBind.Converter = new ColorToBrushConverter();
                    colorBind.Mode = BindingMode.OneWay;

                    Binding fgBind = new Binding(pi.Name);
                    fgBind.Converter = new ColorToInvertedBrushConverter();
                    fgBind.Mode = BindingMode.OneWay;

                    // Create the TextBlock
                    FrameworkElementFactory textFactory = new FrameworkElementFactory(typeof(TextBlock));
                    textFactory.SetBinding(TextBlock.TextProperty, bind);
                    textFactory.SetBinding(TextBlock.BackgroundProperty, colorBind);
                    textFactory.SetBinding(TextBlock.ForegroundProperty, fgBind);
                    DataTemplate textTemplate = new DataTemplate();
                    textTemplate.VisualTree = textFactory;

                    // Create the ComboBox
                    Binding comboBind = new Binding(pi.Name);

                    FrameworkElementFactory comboFactory = new FrameworkElementFactory(typeof(Xceed.Wpf.Toolkit.ColorPicker));
                    comboFactory.SetBinding(ColorPicker.SelectedColorProperty, bind);
                    comboFactory.SetValue(ColorPicker.ShowAdvancedButtonProperty, true);

                    DataTemplate comboTemplate = new DataTemplate();
                    comboTemplate.VisualTree = comboFactory;

                    // Set the Templates to the Column
                    guidColumn.CellTemplate = textTemplate;
                    guidColumn.CellEditingTemplate = comboTemplate;

                } else if (pi.PropertyType == typeof(string)) {
                    Urho.Resource res = pi.GetCustomAttribute<Urho.Resource>();
                    if (res == null)
                        res = pi.PropertyType.GetCustomAttribute<Urho.Resource>();
                    if (res == null && pi.Name.ToLower().Equals("name"))
                        res = aType.GetCustomAttribute<Urho.Resource>();
                    if (res == null) {
                        col = new DataGridTextColumn();
                        ((DataGridTextColumn)col).Binding = new Binding(pi.Name);
                    } else { //combo box
                        col = new DataGridComboBoxColumn();
                        if (res.Type == typeof(Urho.Material)) {
                            ((DataGridComboBoxColumn)col).SelectedItemBinding = new Binding(pi.Name);
                            ((DataGridComboBoxColumn)col).ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_MATERIALS, false);
                        } else if (res.Type == typeof(Urho.Technique)) {
                            ((DataGridComboBoxColumn)col).SelectedItemBinding = new Binding(pi.Name);
                            ((DataGridComboBoxColumn)col).ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_TECHNIQUES, false);
                        } else if (res.Type == typeof(Urho.BaseTexture)) {
                            ((DataGridComboBoxColumn)col).SelectedItemBinding = new Binding(pi.Name);
                            ((DataGridComboBoxColumn)col).ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_TEXTURES, true);
                        } else if (res.Type == typeof(Urho.Texture)) {
                            ((DataGridComboBoxColumn)col).SelectedItemBinding = new Binding(pi.Name);
                            ((DataGridComboBoxColumn)col).ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_TEXTURES, true);
                        } else if (res.Type == typeof(Urho.Shader)) {
                            ((DataGridComboBoxColumn)col).SelectedItemBinding = new Binding(pi.Name);
                            ((DataGridComboBoxColumn)col).ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_SHADERS, false);
                        } else if (res.Type == typeof(Urho.Sound)) {
                            ((DataGridComboBoxColumn)col).SelectedItemBinding = new Binding(pi.Name);
                            ((DataGridComboBoxColumn)col).ItemsSource = UrhoPaths.inst().GetList(UrhoPaths.PATH_SOUNDS, false);
                        }
                    }
                }
                if (col != null) {
                    col.Header = pi.Name;
                    grid.Columns.Add(col);
                }
            }
        }
    }
}
