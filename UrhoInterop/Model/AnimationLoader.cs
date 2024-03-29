﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace UrhoInterop.Model
{
    public static class AnimationLoader
    {
        public static void ReadAnimation(BinaryReader rdr)
        {
            byte[] magic = rdr.ReadBytes(4);

            Animation ret = new Animation();

            ret.Name = ModelLoader.ReadCString(rdr);
            ret.Length = rdr.ReadSingle();
            uint tracks = rdr.ReadUInt32();

            for (int t = 0; t < tracks; ++t)
            {
                Track track = new Track();
                track.Name = ModelLoader.ReadCString(rdr);
                track.Mask = rdr.ReadByte();
                uint keyframes = rdr.ReadUInt32();
                for (int k = 0; k < keyframes; ++k)
                {
                    Keyframe key = new Keyframe();
                    key.Time = rdr.ReadSingle();
                    if ((track.Mask & 1) != 0)
                    {
                        key.Pos = ModelLoader.ReadVector3(rdr);
                    }
                    if ((track.Mask & 2) != 0)
                    {
                        key.Rot = ModelLoader.ReadVector4(rdr);
                    }
                    if ((track.Mask & 4) != 0)
                    {
                        key.Scl = ModelLoader.ReadVector3(rdr);
                    }
                    track.Keys.Add(key);
                }
                ret.Tracks.Add(track);
            }
        }
    }
}
