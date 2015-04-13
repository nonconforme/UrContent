using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace Designer.Effects.Model {
    public class PEProject : BaseClass {
        ObservableCollection<ParticleEffect> effects_;

        public PEProject() {
            effects_ = new ObservableCollection<ParticleEffect>();
        }

        public ObservableCollection<ParticleEffect> Effects { get { return effects_; } }
    }
}
