using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer.MaterialEdit {
    public class MaterialItem {
        public string path_;
    }

    public class MaterialFolder : MaterialItem {
        ObservableCollection<MaterialItem> items_ = new ObservableCollection<MaterialItem>();
        FileSystemWatcher watch_;
        public ObservableCollection<MaterialItem> Children { get { return items_; } }

        public MaterialFolder(string path) {
            path_ = path;
            watch_ = new FileSystemWatcher(path);
            watch_.Changed += watch__Changed;
            watch_.Created += watch__Created;
            watch_.Renamed += watch__Renamed;
            watch_.Deleted += watch__Deleted;
            watch_.EnableRaisingEvents = true;
            fill();
        }

        void fill() {
            foreach (string str in Directory.EnumerateFiles(path_)) {
                FileAttributes fa = File.GetAttributes(str);
                if ((fa & FileAttributes.Directory) == FileAttributes.Directory) {
                    items_.Add(new MaterialFolder(str));
                } else {
                    items_.Add(new MaterialLeaf(new Urho.Material(str)));
                }
            }
        }

        void watch__Changed(object sender, FileSystemEventArgs e) {
            MainWindow.inst().Dispatcher.Invoke(delegate() {
                MaterialLeaf l = items_.FirstOrDefault(f => f.path_.Equals(e.FullPath)) as MaterialLeaf;
                if (l != null) {
                    int idx = items_.IndexOf(l);
                    ReflectMatch.Match(((MaterialLeaf)items_[idx]).Material, new Urho.Material(e.FullPath));
                    return;
                }
                items_.Add(new MaterialLeaf(new Urho.Material(e.Name)));
            });
        }

        void watch__Created(object sender, FileSystemEventArgs e) {
            MainWindow.inst().Dispatcher.Invoke(delegate() {
                fill();
            });
        }

        void watch__Renamed(object sender, RenamedEventArgs e) {
            MainWindow.inst().Dispatcher.Invoke(delegate() {
                fill();
            });
        }

        void watch__Deleted(object sender, FileSystemEventArgs e) {
            MainWindow.inst().Dispatcher.Invoke(delegate() {
                fill();
            });
        }

        private MaterialFolder() {
        }

        public static MaterialFolder Root() {
            MaterialFolder r = new MaterialFolder();
            foreach (string s in UrhoPaths.inst().GetPaths(UrhoPaths.PATH_MATERIALS))
                r.Children.Add(new MaterialFolder(s));
            return r;
        }

        public string Name {
            get { return path_.Replace(UrhoPaths.inst().Root, ""); }
        }
    }

    public class MaterialLeaf : MaterialItem {
        Urho.Material material_;

        public MaterialLeaf(Urho.Material mat) {
            material_ = mat;
            path_ = mat.Name;
        }

        public Urho.Material Material { get { return material_; } }
        public string Name {
            get { return path_.Substring(path_.LastIndexOf("\\") + 1); }
        }
    }
}
