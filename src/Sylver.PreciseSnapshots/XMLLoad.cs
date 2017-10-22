using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Nuterra;

namespace Sylver.PreciseSnapshots
{
    public static class XMLLoad
    {
        /// <summary>
        /// Load XML Files as Techs
        /// </summary>
        /// <param name="path">Path of the Tech file</param>
        /// <param name="position">Tech position</param>
        /// <param name="rotation">Tech rotation</param>
        /// <returns></returns>
        public static Tank LoadXMLAsTech(string path, Vector3 position, Quaternion rotation)
        {
            XmlDocument TechXML = new XmlDocument();
            try
            {
                TechXML.Load(path);
            }
            catch
            {
                return null;
            }

            Tank tech = Singleton.Manager<ManSpawn>.inst.SpawnEmptyTech(0, position, rotation, true, false);
            tech.name = TechXML.GetElementsByTagName("Tech")[0].Attributes["Name"].Value;
            for(int i = 0; i < TechXML.GetElementsByTagName("Block").Count; i++)
            {
                var BlockXML = TechXML.GetElementsByTagName("Block")[i];
                try
                {
                    if (BlockXML.Attributes["IsRootBlock"] != null)
                    {
                        BlockTypes blockType = (BlockTypes)Enum.Parse(typeof(BlockTypes), BlockXML.Attributes["Type"].Value);
                        TankBlock block = Singleton.Manager<ManSpawn>.inst.SpawnBlock(blockType, position - Vector3.one, rotation);

                        string OrthoRotationString = TechXML.SelectNodes("//BlockSpec/OrthoRotation")[i].InnerText;
                        OrthoRotation.r OrthoRot = (OrthoRotation.r)Enum.Parse(typeof(OrthoRotation.r), OrthoRotationString);

                        var cahedLocalPositionXML = TechXML.SelectNodes("//BlockSpec/IntVector3")[i].Attributes;
                        IntVector3 localPositionIntVector = new IntVector3(int.Parse(cahedLocalPositionXML["x"].Value), int.Parse(cahedLocalPositionXML["y"].Value), int.Parse(cahedLocalPositionXML["z"].Value));

                        tech.blockman.AddBlock(block, localPositionIntVector, new OrthoRotation(OrthoRot));

                        var localPositionXML = TechXML.SelectNodes("//Transform/Position")[i].Attributes;
                        Vector3 localPosition = new Vector3(float.Parse(localPositionXML["x"].Value), float.Parse(localPositionXML["y"].Value), float.Parse(localPositionXML["z"].Value));

                        var localRotationXML = TechXML.SelectNodes("//Transform/Rotation")[i].Attributes;
                        Vector3 localRotation = new Vector3(float.Parse(localRotationXML["x"].Value), float.Parse(localRotationXML["y"].Value), float.Parse(localRotationXML["z"].Value));

                        var localScaleXML = TechXML.SelectNodes("//Transform/Scale")[i].Attributes;
                        Vector3 localScale = new Vector3(float.Parse(localScaleXML["x"].Value), float.Parse(localScaleXML["y"].Value), float.Parse(localScaleXML["z"].Value));

                        block.trans.localPosition = localPosition;
                        block.trans.localRotation = Quaternion.Euler(localRotation);
                        block.trans.localScale = localScale;
                    }
                }
                catch { break; }
                
            }
            for (int i=0;i<TechXML.GetElementsByTagName("Block").Count;i++)
            {
                try
                {

                    var BlockXML = TechXML.GetElementsByTagName("Block")[i];
                    if (BlockXML.Attributes["IsRootBlock"] != null) continue;

                    BlockTypes blockType = (BlockTypes)Enum.Parse(typeof(BlockTypes),BlockXML.Attributes["Type"].Value);
                    TankBlock block = Singleton.Manager<ManSpawn>.inst.SpawnBlock(blockType, position-Vector3.one, rotation);

                    string OrthoRotationString = TechXML.SelectNodes("//BlockSpec/OrthoRotation")[i].InnerText;
                    OrthoRotation.r OrthoRot = (OrthoRotation.r)Enum.Parse(typeof(OrthoRotation.r),OrthoRotationString);

                    var cahedLocalPositionXML = TechXML.SelectNodes("//BlockSpec/IntVector3")[i].Attributes;
                    IntVector3 localPositionIntVector = new IntVector3(int.Parse(cahedLocalPositionXML["x"].Value), int.Parse(cahedLocalPositionXML["y"].Value), int.Parse(cahedLocalPositionXML["z"].Value));

                    tech.blockman.AddBlock(block, localPositionIntVector, new OrthoRotation(OrthoRot));

                    //if (BlockXML.Attributes["IsRootBlock"] != null) tech.blockman.SetRootBlock(block);

                    var localPositionXML = TechXML.SelectNodes("//Transform/Position")[i].Attributes;
                    Vector3 localPosition = new Vector3(float.Parse(localPositionXML["x"].Value), float.Parse(localPositionXML["y"].Value), float.Parse(localPositionXML["z"].Value));

                    var localRotationXML = TechXML.SelectNodes("//Transform/Rotation")[i].Attributes;
                    Vector3 localRotation = new Vector3(float.Parse(localRotationXML["x"].Value), float.Parse(localRotationXML["y"].Value), float.Parse(localRotationXML["z"].Value));

                    var localScaleXML = TechXML.SelectNodes("//Transform/Scale")[i].Attributes;
                    Vector3 localScale = new Vector3(float.Parse(localScaleXML["x"].Value), float.Parse(localScaleXML["y"].Value), float.Parse(localScaleXML["z"].Value));

                    block.trans.localPosition = localPosition;
                    block.trans.localRotation = Quaternion.Euler(localRotation);
                    block.trans.localScale = localScale;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
            }

            if(tech.blockman.blockCount == 0) tech.blockman.AddBlock(Singleton.Manager<ManSpawn>.inst.SpawnBlock(BlockTypes.GSOCockpit_111, Vector3.zero, Quaternion.identity), IntVector3.zero);

            return tech;
        }
    }
}
