using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Urho;

namespace Designer.Controls {

    public class ColorToColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value is Urho.UColor) {
                Urho.UColor c = (Urho.UColor)value;
                return Color.FromArgb((byte)(c.A * 255), (byte)(c.R * 255), (byte)(c.G * 255), (byte)(c.B * 255));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value is Color) {
                Color c = (Color)value;
                return new Urho.UColor(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
            }
            return new Urho.UColor();
        }
    }

    public class ColorToBrushConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (!(value is Urho.UColor)) {
                return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            } else {
                Urho.UColor c = value as Urho.UColor;
                return new SolidColorBrush(Color.FromArgb((byte)(c.A * 255), (byte)(c.R * 255), (byte)(c.G * 255), (byte)(c.B * 255)));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }

    public class ColorToInvertedBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (!(value is Urho.UColor))
                return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Urho.UColor c = (Urho.UColor)value;
            int sum = (int)(c.R * 255 + c.G * 255 + c.B * 255);
            Color col = Color.FromRgb((byte)(255 - c.R*255), (byte)(255 - c.G*255), (byte)(255 - c.B*255));
            int nsum = col.R + col.G + col.B;
            if (Math.Max(nsum, sum) - Math.Min(nsum, sum) < 128) {
                if (nsum > 128 * 3) //new color is brighter
                    return new SolidColorBrush(Colors.Black);
                return new SolidColorBrush(Colors.White);
            }
            return new SolidColorBrush((Color)col);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }

    public class MinMaxConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value is Urho.MinMax)
                return ((Urho.MinMax)value).ToString();
            return value != null ? value.ToString() : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            string[] parts = value.ToString().Replace("  ", " ").Replace("  ", " ").Split(' ');
            if (parts.Length == 2) {
                MinMax v = new MinMax();
                v.Min = float.Parse(parts[0]);
                v.Max = float.Parse(parts[1]);
                return v;
            } else if (parts.Length == 1) {
                MinMax v = new MinMax();
                v.Min = v.Max = float.Parse(parts[0]);
                return v;
            } else {
                return null;
            }
        }
    }

    public class Vec2Converter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value != null && value is Vector2) {
                return ((Vector2)value).ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            string str = value.ToString();
            return Vector2.FromString(str);
        }
    }

    public class Vec3Converter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value != null && value is Vector3) {
                return ((Vector3)value).ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            string str = value.ToString();
            return Vector3.FromString(str);
        }
    }

    public class Vec4Converter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value != null && value is Vector4) {
                return ((Vector4)value).ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            string str = value.ToString();
            return Vector4.FromString(str);
        }
    }

    public class MinMaxValidator : ValidationRule {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            try {
                string[] parts = value.ToString().Split(' ');
                foreach (string s in parts)
                    float.Parse(s);
            }
            catch (Exception ex) {
                return new ValidationResult(false, "Incorrect format");
            }
            return new ValidationResult(true, null);
        }
    }

    public class Vec2Validator : ValidationRule {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            try {
                Vector2.FromString(value.ToString());
            } catch (Exception ex) {
                return new ValidationResult(false, "Incorrect format");
            }
            return new ValidationResult(true, null);
        }
    }

    public class Vec3Validator : ValidationRule {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            try {
                Vector3.FromString(value.ToString());
            } catch (Exception ex) {
                return new ValidationResult(false, "Incorrect format");
            }
            return new ValidationResult(true, null);
        }
    }

    public class Vec4Validator : ValidationRule {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            try {
                Vector4.FromString(value.ToString());
            } catch (Exception ex) {
                return new ValidationResult(false, "Incorrect format");
            }
            return new ValidationResult(true, null);
        }
    }
}
