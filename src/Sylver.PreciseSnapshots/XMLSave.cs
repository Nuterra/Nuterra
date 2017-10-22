using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Sylver.PreciseSnapshots
{
    class XMLSave : MonoBehaviour
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftAlt) && Singleton.playerTank)
            {
                Console.WriteLine("Saving...");
                SaveTechAsXML(Singleton.playerTank);
            }
        }

        public void SaveTechAsXML(Tank tech)
        {
            XmlWriter saver = XmlWriter.Create(Path.Combine(PreciseSnapshotsMod.PreciseSnapshotsFolder, tech.name+".xml"),new XmlWriterSettings { Indent = true });

            saver.WriteStartDocument();
            saver.WriteStartElement("Tech");
            saver.WriteAttributeString("Name", tech.name);

                saver.WriteStartElement("Blocks");
                foreach (var block in tech.blockman.IterateBlocks())
                {
                    saver.WriteStartElement("Block");
                    saver.WriteAttributeString("Type", block.BlockType.ToString());
                    if(tech.blockman.IsRootBlock(block)) saver.WriteAttributeString("IsRootBlock","true");

                        saver.WriteStartElement("BlockSpec");

                            saver.WriteStartElement("OrthoRotation");
                                saver.WriteString(block.cachedLocalRotation.rot.ToString());
                            saver.WriteEndElement();

                            var localPos = new IntVector3(block.cachedLocalPosition);
                            saver.WriteStartElement("IntVector3");
                            saver.WriteAttributeString("x", localPos.x.ToString());
                            saver.WriteAttributeString("y", localPos.y.ToString());
                            saver.WriteAttributeString("z", localPos.z.ToString());
                            saver.WriteEndElement();

                        saver.WriteEndElement();

                        saver.WriteStartElement("Transform");
                            
                            var pos = block.trans.localPosition;
                            saver.WriteStartElement("Position");  
                            saver.WriteAttributeString("x", pos.x.ToString());
                            saver.WriteAttributeString("y", pos.y.ToString());
                            saver.WriteAttributeString("z", pos.z.ToString());
                            saver.WriteEndElement();

                            var rotation = block.trans.localRotation.eulerAngles;
                            saver.WriteStartElement("Rotation"); 
                            saver.WriteAttributeString("x", rotation.x.ToString());
                            saver.WriteAttributeString("y", rotation.y.ToString());
                            saver.WriteAttributeString("z", rotation.z.ToString());
                            saver.WriteEndElement();

                            var scale = block.trans.localScale;
                            saver.WriteStartElement("Scale"); 
                            saver.WriteAttributeString("x", scale.x.ToString());
                            saver.WriteAttributeString("y", scale.y.ToString());
                            saver.WriteAttributeString("z", scale.z.ToString());
                            saver.WriteEndElement();


                        saver.WriteEndElement();
                    saver.WriteEndElement();
                }
                saver.WriteEndElement();

            saver.WriteEndDocument();
            saver.Close();
        }
    }
}
