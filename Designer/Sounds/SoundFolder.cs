using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer.Sounds {
    public class SoundItem {
        public string path_;
    }

    public class SoundFolder : SoundItem {
        ObservableCollection<SoundItem> items_ = new ObservableCollection<SoundItem>();

        public ObservableCollection<SoundItem> Children { get { return items_; } }
        FileSystemWatcher watch_;

        public SoundFolder(string path) {
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
                    items_.Add(new SoundFolder(str));
                } else {
                    items_.Add(new SoundLeaf(new Urho.Sound(str)));
                }
            }
        }

        void watch__Changed(object sender, FileSystemEventArgs e) {
            MainWindow.inst().Dispatcher.Invoke(delegate() {
                SoundLeaf l = items_.FirstOrDefault(f => f.path_.Equals(e.FullPath)) as SoundLeaf;
                if (l != null) {
                    int idx = items_.IndexOf(l);
                    items_[idx] = new SoundLeaf(new Urho.Sound(e.FullPath));
                    return;
                }
                items_.Add(new SoundLeaf(new Urho.Sound(e.Name)));
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

        private SoundFolder() {
        }

        public static SoundFolder Root() {
            SoundFolder r = new SoundFolder();
            foreach (string s in UrhoPaths.inst().GetPaths(UrhoPaths.PATH_SOUNDS))
                r.Children.Add(new SoundFolder(s));
            return r;
        }

        public string Name {
            get { return path_.Replace(UrhoPaths.inst().Root, ""); }
        }

        void GetFlat(ObservableCollection<Urho.Sound> holder)
        {
            foreach (SoundItem l in this.Children)
            {
                if (l is SoundFolder)
                    ((SoundFolder)l).GetFlat(holder);
                else if (l is SoundLeaf)
                {
                    
                    holder.Add(((SoundLeaf)l).Sound);
                }
            }
        }

        public ObservableCollection<Urho.Sound> GetFlat()
        {
            ObservableCollection<Urho.Sound> ret = new ObservableCollection<Urho.Sound>();
            GetFlat(ret);
            return ret;
        }
    }

    public class SoundLeaf : SoundItem {
        Urho.Sound sound_;

        public SoundLeaf(Urho.Sound mat) {
            sound_ = mat;
            path_ = mat.Name;
        }

        public Urho.Sound Sound { get { return sound_; } }
        public string Name {
            get { return path_.Substring(path_.LastIndexOf("\\") + 1); }
        }
    }
}
