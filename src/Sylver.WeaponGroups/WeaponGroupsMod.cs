using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;

namespace Sylver.WeaponGroups
{
    public class WeaponGroupsMod : TerraTechMod
    {
        private GameObject _holder;

        public override string Name => "Weapon Groups";

        public override string Description => "A mod that add Weapon Groups";

        public List<HashSet<ModuleWeapon>> m_Groups;

        public override void Load()
        {
            base.Load();
            this.m_Groups = new List<HashSet<ModuleWeapon>>();
            for (int i = 0; i < 10; i++)
            {
                this.m_Groups.Add(new HashSet<ModuleWeapon>());
            }
            Hooks.Modules.TankControl.CanControlTank += TankControl_CanControlTank;
            Hooks.Modules.TechWeapon.WeaponAdded += TechWeapon_WeaponAdded;
            Hooks.Modules.TechWeapon.WeaponRemoved += TechWeapon_WeaponRemoved;
        }

        private void TankControl_CanControlTank(CanControlTankEvent obj)
        {
            if (Input.GetKey(KeyCode.CapsLock))
            {
                return;
            }
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Alpha0))
            {
                UpdateGroups();
            }
        }

        private void TechWeapon_WeaponRemoved(WeaponRemovedEvent obj)
        {
            foreach (HashSet<ModuleWeapon> current in this.m_Groups)
            {
                if (current.Contains(obj.WeaponComponent))
                {
                    current.Remove(obj.WeaponComponent);
                }
            }
        }

        private void TechWeapon_WeaponAdded(WeaponAddedEvent obj)
        {
            if (Input.GetKey(KeyCode.CapsLock))
            {
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    this.m_Groups[1].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    this.m_Groups[2].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha3))
                {
                    this.m_Groups[3].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha4))
                {
                    this.m_Groups[4].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha5))
                {
                    this.m_Groups[5].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha6))
                {
                    this.m_Groups[6].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha7))
                {
                    this.m_Groups[7].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha8))
                {
                    this.m_Groups[8].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha9))
                {
                    this.m_Groups[9].Add(obj.WeaponComponent);
                }
                if (Input.GetKey(KeyCode.Alpha0))
                {
                    this.m_Groups[0].Add(obj.WeaponComponent);
                }
            }
        }

        public void UpdateGroups()
        {
            if (Input.GetKey(KeyCode.Alpha0))
            {
                foreach (ModuleWeapon expr_41 in this.m_Groups[0])
                {
                    expr_41.ControlInputManual(1, 1);
                    expr_41.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                foreach (ModuleWeapon expr_8B in this.m_Groups[1])
                {
                    expr_8B.ControlInputManual(1, 1);
                    expr_8B.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                foreach (ModuleWeapon expr_D5 in this.m_Groups[2])
                {
                    expr_D5.ControlInputManual(1, 1);
                    expr_D5.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                foreach (ModuleWeapon expr_11F in this.m_Groups[3])
                {
                    expr_11F.ControlInputManual(1, 1);
                    expr_11F.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                foreach (ModuleWeapon expr_169 in this.m_Groups[4])
                {
                    expr_169.ControlInputManual(1, 1);
                    expr_169.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha5))
            {
                foreach (ModuleWeapon expr_1B3 in this.m_Groups[5])
                {
                    expr_1B3.ControlInputManual(1, 1);
                    expr_1B3.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha6))
            {
                foreach (ModuleWeapon expr_1FD in this.m_Groups[6])
                {
                    expr_1FD.ControlInputManual(1, 1);
                    expr_1FD.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha7))
            {
                foreach (ModuleWeapon expr_247 in this.m_Groups[7])
                {
                    expr_247.ControlInputManual(1, 1);
                    expr_247.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha8))
            {
                foreach (ModuleWeapon expr_291 in this.m_Groups[8])
                {
                    expr_291.ControlInputManual(1, 1);
                    expr_291.Process();
                }
            }
            if (Input.GetKey(KeyCode.Alpha9))
            {
                foreach (ModuleWeapon expr_2DC in this.m_Groups[9])
                {
                    expr_2DC.ControlInputManual(1, 1);
                    expr_2DC.Process();
                }
            }
        }
    }
}
