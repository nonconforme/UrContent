using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer {
    public class Project : NamedBaseClass {
        static Project inst_;
        public static Project inst() { return inst_; }
        public static bool Valid() { return inst_ != null; }

        UrhoPaths paths_;

        public MaterialEdit.MaterialFolder Materials { get; set; }
        public TextureMan.TextureFolder Textures { get; set; }
        public Sounds.SoundFolder SoundFiles { get; set; }
        public Effects.EffectFolder ParticleEffects { get; set; }

        public Project(string path) {
            inst_ = this;
            paths_ = new UrhoPaths(path);
            ParticleEffects = Effects.EffectFolder.Root();
            Materials = MaterialEdit.MaterialFolder.Root();
            SoundFiles = Sounds.SoundFolder.Root();
            Textures = TextureMan.TextureFolder.Root();
        }
    }
}
