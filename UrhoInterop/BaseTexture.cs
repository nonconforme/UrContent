using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urho {
    public abstract class BaseTexture : NamedBaseClass {

        public abstract bool IsCube { get; }
        public bool IsHardFile { get; set; }
    }
}
