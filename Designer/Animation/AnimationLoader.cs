using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace Designer.Animation
{
    public static class AnimationLoader
    {
        public static void ReadAnimation(BinaryReader rdr)
        {
            byte[] magic = rdr.ReadBytes(4);

            string animName = ModelLoader.ReadCString(rdr);
            float length = rdr.ReadSingle();
            uint tracks = rdr.ReadUInt32();

            for (int t = 0; t < tracks; ++t)
            {
                string trackName = ModelLoader.ReadCString(rdr);
                byte mask = rdr.ReadByte();
                uint keyframes = rdr.ReadUInt32();
                for (int k = 0; k < keyframes; ++k)
                {
                    float timePos = rdr.ReadSingle();
                    if ((mask & 1) != 0)
                    {
                        Vector3 pos = ModelLoader.ReadVector3(rdr);
                    }
                    if ((mask & 2) != 0)
                    {
                        Vector4 rot = ModelLoader.ReadVector4(rdr);
                    }
                    if ((mask & 4) != 0)
                    {
                        Vector3 scl = ModelLoader.ReadVector3(rdr);
                    }
                }
            }
        }
    }
}
