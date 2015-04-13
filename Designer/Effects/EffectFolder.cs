using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer.Effects {

    public abstract class EffectItem : NamedBaseClass {
        public abstract string DisplayName { get; }
    }

    public class EffectFolder : EffectItem {
        FileSystemWatcher watch_;
        ObservableCollection<EffectItem> contents_ = new ObservableCollection<EffectItem>();

        public ObservableCollection<EffectItem> Children { get { return contents_; } }

        public EffectFolder(string path) {
            Name = path;
            watch_ = new FileSystemWatcher(path);
            watch_.Changed += watch__Changed;
            watch_.Created += watch__Created;
            watch_.Renamed += watch__Renamed;
            watch_.Deleted += watch__Deleted;
            watch_.EnableRaisingEvents = true;
            fill();
        }

        void fill() {
            contents_.Clear();
            foreach (string str in Directory.EnumerateFiles(Name)) {
                FileAttributes fa = File.GetAttributes(str);
                if ((fa & FileAttributes.Directory) == FileAttributes.Directory) {
                    contents_.Add(new EffectFolder(str));
                } else {
                    //if (Path.GetExtension(str).ToLower().Equals(".xml"))
                    //    continue;
                    //try {
                        contents_.Add(new EffectLeaf(new Urho.ParticleEffect(str)));
                    //}
                    //catch (Exception ex) {
                    //    ErrorHandler.inst().Error(ex);
                   // }
                }
            }
        }

        void watch__Changed(object sender, FileSystemEventArgs e) {
            MainWindow.inst().Dispatcher.Invoke(delegate() {
                EffectLeaf l = contents_.FirstOrDefault(f => f.Name.Equals(e.FullPath)) as EffectLeaf;
                if (l != null) {
                    int idx = contents_.IndexOf(l);
                    contents_[idx] = new EffectLeaf(new Urho.ParticleEffect(e.FullPath));
                    return;
                }
                contents_.Add(new EffectLeaf(new Urho.ParticleEffect(e.Name)));
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

        public override string DisplayName {
            get {
                if (Name.Contains(UrhoPaths.inst().Root))
                    return Name.Replace(UrhoPaths.inst().Root, "");
                return Name;
            }
        }

        private EffectFolder() { }

        public static EffectFolder Root() {
            EffectFolder ret = new EffectFolder();
            foreach (string s in UrhoPaths.inst().GetPaths(UrhoPaths.PATH_PARTICLES))
                ret.Children.Add(new EffectFolder(s));
            return ret;
        }
    }

    public class EffectLeaf : EffectItem {
        Urho.ParticleEffect effect_;

        public EffectLeaf(Urho.ParticleEffect effect) {
            Name = effect.Name;
            effect_ = effect;
        }
        public Urho.ParticleEffect Effect { get { return effect_; } }

        public override string DisplayName {
            get { return effect_.Name.Substring(effect_.Name.LastIndexOf("\\") + 1); }
        }
    }
}
