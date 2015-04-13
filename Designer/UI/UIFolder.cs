using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer.UI {
    public class ElementItem {
        protected string path_;
    }

    public class ElementFolder : ElementItem {
        ObservableCollection<ElementItem> items_ = new ObservableCollection<ElementItem>();

        public ObservableCollection<ElementItem> Children { get { return items_; } }

        public ElementFolder(string path) {
            path_ = path;

            foreach (string str in Directory.EnumerateFiles(path)) {
                FileAttributes fa = File.GetAttributes(str);
                if ((fa & FileAttributes.Directory) == FileAttributes.Directory) {
                    items_.Add(new ElementFolder(str));
                } else {
                    items_.Add(new ElementLeaf(new Urho.Element(str)));
                }
            }
        }

        private ElementFolder() {
        }

        public static ElementFolder Root() {
            ElementFolder r = new ElementFolder();
            foreach (string s in UrhoPaths.inst().GetPaths(UrhoPaths.PATH_MATERIALS))
                r.Children.Add(new ElementFolder(s));
            return r;
        }

        public string Name {
            get { return path_.Replace(UrhoPaths.inst().Root, ""); }
        }
    }

    public class ElementLeaf : ElementItem {
        Urho.Element material_;

        public ElementLeaf(Urho.Element mat) {
            material_ = mat;
            path_ = mat.Name;
        }

        public Urho.Element Element { get { return material_; } }
        public string Name {
            get { return path_.Substring(path_.LastIndexOf("\\") + 1); }
        }
    }
}
