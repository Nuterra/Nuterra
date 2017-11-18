using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sylver.Piston
{
    class ModulePiston : Module
    {
        public List<TankBlock> Childs = new List<TankBlock>();

        private Transform trans { get; set; }

        private void OnPool()
        {
            base.block.AttachEvent += this.OnAttach;
            base.block.DetachEvent += this.OnDetach; 
        }

        private void OnSpawn()
        {
            trans = base.block.trans;
        }

        private void OnDetach()
        {
            Childs.Clear();
            base.block.tank.AttachEvent.Unsubscribe(this.BlockAttached);
            base.block.tank.DetachEvent.Unsubscribe(this.BlockDetached);

        }

        private void OnAttach()
        {
            Childs.Clear();
            base.block.tank.AttachEvent.Subscribe(this.BlockAttached);
            base.block.tank.DetachEvent.Subscribe(this.BlockDetached);
        }

        private void BlockDetached(TankBlock block, Tank tank)
        {
            Childs.Remove(block);
            block.trans.parent = null;
        }

        private void BlockAttached(TankBlock block, Tank tank)
        {
            if(block.ConnectedBlocksByAP.ToList().Contains(this.block))
            {
                Childs.Add(block);
                block.trans.parent = trans;
            }
            else
            {
                foreach(TankBlock child in Childs)
                {
                    if(block.ConnectedBlocksByAP.ToList().Contains(child))
                    {
                        if(!Childs.Contains(block))
                        {
                            Childs.Add(block);
                            block.trans.parent = trans;
                        }
                        break;
                    }
                }
            }

        }

        private void Update()
        {
            if (!block||!trans) return;
            trans.position = block.trans.position;
            trans.rotation = block.trans.rotation;
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {

                var pos = trans.localPosition;
                pos += trans.forward * Time.deltaTime * 1;
                trans.localPosition = pos;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {

                var pos = trans.localPosition;
                pos -= trans.forward * Time.deltaTime * 1;
                trans.localPosition = pos;
            }
        }
    }
}
