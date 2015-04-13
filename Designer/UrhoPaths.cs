using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Designer {
    public class UrhoPaths {

        public static readonly String PATH_RENDERPATHS = "renderpaths";
        public static readonly String PATH_SHADERS = "shaders";
        public static readonly String PATH_TECHNIQUES = "techniques";
        public static readonly String PATH_FONTS = "fonts";
        public static readonly String PATH_LUA = "luascripts";
        public static readonly String PATH_MATERIALS = "materials";
        public static readonly String PATH_MODELS = "models";
        public static readonly String PATH_MUSIC = "music";
        public static readonly String PATH_OBJECTS = "objects";
        public static readonly String PATH_PARTICLES = "particle";
        public static readonly String PATH_POSTPROCESS = "postprocess";
        public static readonly String PATH_SCENES = "scenes";
        public static readonly String PATH_SCRIPTS = "scripts";
        public static readonly String PATH_SOUNDS = "sounds";
        public static readonly String PATH_TEXTURES = "textures";
        public static readonly String PATH_UI = "ui";
        public static readonly String PATH_2D = "urho2d";

        static UrhoPaths inst_;

        public static UrhoPaths inst() {
            return inst_;
        }

        string root_;
        Dictionary<string, List<string>> Paths = new Dictionary<string, List<string>>();

        public UrhoPaths(string root) {
            UserData.inst().AddRecentFile(root);
            inst_ = this;
            root_ = root;
            string coreData = Path.Combine(root, "CoreData");
            if (Directory.Exists(coreData)) {
                pushPath(coreData, PATH_RENDERPATHS, "RenderPaths");
                pushPath(coreData, PATH_SHADERS, "Shaders");
                pushPath(coreData, PATH_TECHNIQUES, "Techniques");
                pushPath(coreData, PATH_FONTS, "Fonts");
                pushPath(coreData, PATH_LUA, "LuaScripts");
                pushPath(coreData, PATH_MATERIALS, "Materials");
                pushPath(coreData, PATH_MODELS, "Models");
                pushPath(coreData, PATH_MUSIC, "Music");
                pushPath(coreData, PATH_OBJECTS, "Objects");
                pushPath(coreData, PATH_PARTICLES, "Particle");
                pushPath(coreData, PATH_POSTPROCESS, "PostProcess");
                pushPath(coreData, PATH_SCENES, "Scenes");
                pushPath(coreData, PATH_SCRIPTS, "Scripts");
                pushPath(coreData, PATH_SOUNDS, "Sounds");
                pushPath(coreData, PATH_TEXTURES, "Textures");
                pushPath(coreData, PATH_UI, "UI");
                pushPath(coreData, PATH_2D, "Urho2D");
            }
            string data = Path.Combine(root, "Data");
            if (Directory.Exists(data)) {
                pushPath(data, PATH_RENDERPATHS, "RenderPaths");
                pushPath(data, PATH_SHADERS, "Shaders");
                pushPath(data, PATH_TECHNIQUES, "Techniques");
                pushPath(data, PATH_FONTS, "Fonts");
                pushPath(data, PATH_LUA, "LuaScripts");
                pushPath(data, PATH_MATERIALS, "Materials");
                pushPath(data, PATH_MODELS, "Models");
                pushPath(data, PATH_MUSIC, "Music");
                pushPath(data, PATH_OBJECTS, "Objects");
                pushPath(data, PATH_PARTICLES, "Particle");
                pushPath(data, PATH_POSTPROCESS, "PostProcess");
                pushPath(data, PATH_SCENES, "Scenes");
                pushPath(data, PATH_SCRIPTS, "Scripts");
                pushPath(data, PATH_SOUNDS, "Sounds");
                pushPath(data, PATH_TEXTURES, "Textures");
                pushPath(data, PATH_UI, "UI");
                pushPath(data, PATH_2D, "Urho2D");
            }
        }

        public string Root { get { return root_; } }

        public List<string> GetPaths(string type) {
            if (Paths.ContainsKey(type))
                return Paths[type];
            return new List<string>();
        }

        public List<string> GetList(string type, bool noXML) {
            List<string> ret = new List<string>();
            if (Paths.ContainsKey(type)) {
                foreach (string path in Paths[type]) {
                    foreach (string file in System.IO.Directory.EnumerateFiles(path)) {
                        if (noXML && file.Contains(".xml"))
                            continue;
                        string p = file.Replace(Root + "\\Data\\", "").Replace(Root + "\\CoreData\\", "").Replace("\\","/");
                        ret.Add(p);
                    }
                }
            }
            return ret;
        }

        void pushPath(string basePath, string type, string name) {
            string p = Path.Combine(basePath, name);
            if (Directory.Exists(p)) {
                if (Paths.ContainsKey(type))
                    Paths[type].Add(p);
                else {
                    Paths[type] = new List<string>();
                    Paths[type].Add(p);
                }
            }
        }

    }
}