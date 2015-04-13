using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Designer {
    public static class ReflectMatch {
        public static void Match(object left, object right) {
            foreach (PropertyInfo pi in right.GetType().GetProperties()) {
                if (pi.GetSetMethod() != null)
                    pi.SetValue(left, pi.GetValue(right));
            }
            if (left is Urho.BaseClass)
                ((Urho.BaseClass)left).OnPropertyChanged();
        }
    }
}
