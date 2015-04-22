using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace Designer.Animation
{
    public static class ModelLoader
    {
        public static void ReadModel(BinaryReader rdr)
        {
            byte[] magic = rdr.ReadBytes(4);
            if (!BitConverter.ToString(magic, 0).Equals("UMDL"))
                return;

            uint vertexBufferCt = rdr.ReadUInt32();
            //Read the vertex buffers
            for (uint i = 0; i < vertexBufferCt; ++i)
            {
                uint vertCt = rdr.ReadUInt32();
                uint vertMask = rdr.ReadUInt32();
                uint vertexSize = GetVertexSize(vertMask);
                uint morphStart = rdr.ReadUInt32();
                uint morphCt = rdr.ReadUInt32();
                byte[] vertexData = rdr.ReadBytes((int)(vertCt * vertexSize));
            }

            uint indexBufferCt = rdr.ReadUInt32();
            for (uint i = 0; i < indexBufferCt; ++i)
            {
                uint indexCt = rdr.ReadUInt32();
                uint indexSize = rdr.ReadUInt32();
                byte[] indexData = rdr.ReadBytes((int)(indexCt * indexSize));
            }

            uint geomCt = rdr.ReadUInt32();
            for (uint i = 0; i < geomCt; ++i)
            {
                uint boneMapCt = rdr.ReadUInt32();
                byte[] boneMappingBytes = rdr.ReadBytes((int)(sizeof(uint) * boneMapCt));
                uint[] boneMapping = new uint[boneMapCt];
                for (int idx = 0, boneIdx = 0; idx < boneMappingBytes.Length; idx += 4, boneIdx += 1)
                {
                    boneMapping[boneIdx] = BitConverter.ToUInt32(boneMappingBytes, idx);
                }
                uint lodCt = rdr.ReadUInt32();

                for (int lodIdx = 0; lodIdx < lodCt; ++lodIdx)
                {
                    float dist = rdr.ReadSingle();
                    uint prim = rdr.ReadUInt32();
                    uint vertBufferIndex = rdr.ReadUInt32();
                    uint indexBufferIndex = rdr.ReadUInt32();
                    uint drawRangeStart = rdr.ReadUInt32();
                    uint drawRangeEnd = rdr.ReadUInt32();

                }
            }

            uint vertexMorphCt = rdr.ReadUInt32();

            for (int i = 0; i < vertexBufferCt; ++i)
            {
                string name = ReadCString(rdr);
                uint affectedBuffers = rdr.ReadUInt32();

                for (int bufIdx = 0; bufIdx < affectedBuffers; ++i)
                {
                    uint affectBuffIdx = rdr.ReadUInt32();
                    uint affectBuffMask = rdr.ReadUInt32();
                    uint affectedVertCt = rdr.ReadUInt32();

                    for (int vertIdx = 0; vertIdx < affectedVertCt; ++vertIdx)
                    {
                        uint morphVertexIndex = rdr.ReadUInt32();
                        //\todo check mask
                        Vector3 pos = ReadVector3(rdr);
                        Vector3 norm = ReadVector3(rdr);
                        Vector3 tan = ReadVector3(rdr);
                    }
                }
            }

            //Skeleton data

            uint boneCount = rdr.ReadUInt32();
            for (int i = 0; i < boneCount; ++i)
            {
                string boneName = ReadCString(rdr);
                uint parentIdx = rdr.ReadUInt32();
                Vector3 initPos = ReadVector3(rdr);
                Vector4 initRot = ReadVector4(rdr);
                Vector3 initScale = ReadVector3(rdr);

                //??How is data on bounding spheres and boxes identified?
                //float boneRadius = rdr.ReadSingle();
                //
                //Vector3 boneBndsMin = ReadVector3(rdr);
                //Vector3 boneBndsMax = ReadVector3(rdr);
            }

            Vector3 boundsMin = ReadVector3(rdr);
            Vector3 boundsMax = ReadVector3(rdr);

            for (int i = 0; i < geomCt; ++i)
            {
                Vector3 geoCenter = ReadVector3(rdr);
            }
        }

        static uint[] elementSize =
            {
                3 * sizeof(float), // Position
                3 * sizeof(float), // Normal
                4 * sizeof(char), // Color
                2 * sizeof(float), // Texcoord1
                2 * sizeof(float), // Texcoord2
                3 * sizeof(float), // Cubetexcoord1
                3 * sizeof(float), // Cubetexcoord2
                4 * sizeof(float), // Tangent
                4 * sizeof(float), // Blendweights
                4 * sizeof(char), // Blendindices
                4 * sizeof(float), // Instancematrix1
                4 * sizeof(float), // Instancematrix2
                4 * sizeof(float) // Instancematrix3
            };

        static uint GetVertexSize(uint elementMask)
        {
            uint vertexSize = 0;
    
            for (int i = 0; i < 13; ++i) //13, magic max vertex elements
            {
                if ((elementMask & (1 << i)) != 0)
                    vertexSize += elementSize[i];
            }
    
            return vertexSize;
        }

        public static string ReadCString(BinaryReader rdr)
        {
            string ret = "";
            char c = rdr.ReadChar();
            while (c != '\0')
            {
                ret += c;
                c = rdr.ReadChar();
            }
            return "";
        }

        public static Vector3 ReadVector3(BinaryReader rdr)
        {
            Vector3 ret = new Vector3();
            ret.X = rdr.ReadSingle();
            ret.Y = rdr.ReadSingle();
            ret.Z = rdr.ReadSingle();
            return ret;
        }

        public static Vector4 ReadVector4(BinaryReader rdr)
        {
            Vector4 ret = new Vector4();
            ret.X = rdr.ReadSingle();
            ret.Y = rdr.ReadSingle();
            ret.Z = rdr.ReadSingle();
            ret.W = rdr.ReadSingle();
            return ret;
        }
    }
}
