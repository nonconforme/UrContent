using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Urho;

namespace Designer.TextureMan {

    public abstract class TextureItem : BaseClass {
        public abstract string Name { get; }
        public abstract BitmapSource ImgSource { get; }
    }

    public class TextureFolder : TextureItem {
        ObservableCollection<TextureItem> contents_ = new ObservableCollection<TextureItem>();
        FileSystemWatcher watch_;

        public TextureFolder(string path) {
            name_ = path;
            watch_ = new FileSystemWatcher(path);
            watch_.Changed += watch__Changed;
            watch_.Created += watch__Created;
            watch_.Renamed += watch__Renamed;
            watch_.Deleted += watch__Deleted;
            watch_.EnableRaisingEvents = true;
            fill();
        }

        void fill() {
            foreach (string str in Directory.EnumerateFileSystemEntries(name_)) {
                FileAttributes fa = File.GetAttributes(str);
                if ((fa & FileAttributes.Directory) == FileAttributes.Directory) {
                    contents_.Add(new TextureFolder(str));
                } else {
                    if (Path.GetExtension(str).ToLower().Equals(".xml"))
                        continue;
                    contents_.Add(new TextureLeaf(new Urho.Texture(str)));
                }
            }
        }

        void watch__Changed(object sender, FileSystemEventArgs e) {
            MainWindow.inst().Dispatcher.Invoke(delegate() {
                TextureLeaf l = contents_.FirstOrDefault(f => f.Name.Equals(e.FullPath)) as TextureLeaf;
                if (l != null) {
                    int idx = contents_.IndexOf(l);
                    contents_[idx] = new TextureLeaf(new Urho.Texture(e.FullPath));
                    return;
                }
                contents_.Add(new TextureLeaf(new Urho.Texture(e.Name)));
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

        void GetFlat(ObservableCollection<Texture> holder) {
            foreach (TextureItem l in Contents) {
                if (l is TextureFolder)
                    ((TextureFolder)l).GetFlat(holder);
                else if (l is TextureLeaf) {
                    if (((TextureLeaf)l).Texture is Texture)
                        holder.Add(((TextureLeaf)l).Texture as Texture);
                }
            }
        }

        public ObservableCollection<Texture> GetFlat() {
            ObservableCollection<Texture> ret = new ObservableCollection<Texture>();
            GetFlat(ret);
            return ret;
        }

        private TextureFolder() { }

        public ObservableCollection<TextureItem> Contents { get { return contents_; } }
        public override string Name {
            get { return name_.Replace(UrhoPaths.inst().Root, ""); }
        }
        string name_;

        public static TextureFolder Root() {
            TextureFolder r = new TextureFolder();
            foreach (string s in UrhoPaths.inst().GetPaths(UrhoPaths.PATH_TEXTURES))
                r.Contents.Add(new TextureFolder(s));
            return r;
        }

        public override BitmapSource ImgSource { get { return null; } }
    }

    public class TextureLeaf : TextureItem {
        BaseTexture texture_;
        public BaseTexture Texture { get { return texture_; } }

        public TextureLeaf(BaseTexture aTex) {
            texture_ = aTex;
        }

        public bool IsCube {
            get { return texture_ is CubeMap; }
        }

        public override string Name {
            get { return texture_.Name.Substring(texture_.Name.LastIndexOf("\\") + 1); }
        }

        public override BitmapSource ImgSource
        {
            get
            {
                try
                {
                    FIBITMAP image = FreeImage.LoadEx(texture_.Name);
                    int width = (int)FreeImage.GetWidth(image);
                    int height = (int)FreeImage.GetHeight(image);
                    int fileBPP = (int)FreeImage.GetBPP(image);
                    return ToBitmapSource(image, null);
                } 
                catch (Exception ex)
                {
                    ErrorHandler.inst().Error(ex);
                    return null;
                }
            }
        }

        // Convert a Bitmap to a BitmapSource. 
        public static BitmapSource ToBitmapSource(FIBITMAP image, WriteableBitmap bitmap)
        {
            IntPtr ptr = FreeImage.GetHbitmap(image, new IntPtr(0), false); //obtain the Hbitmap

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            return bs;
        }
    }
}
