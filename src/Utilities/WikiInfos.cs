using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Nuterra.Internal;

namespace Utilities
{
    class WikiInfos
    {
        public static void GetBlocksInfos()
        {
            string infos = "";//"ID;Object name;Corp;License Level;Category;Rarity;Buy Price;Mass;Health;Gun Damage;Drill Damage;Hammer Damage;Energy Storage Capacity;Fuel Storage Capacity;Fuel Storage Refill Rate;Energy Consumption Per Sec;Energy Consumed Per Point Healed;Energy Consumed Per Damage Point;Fuel Consumed Per Sec\n";
            foreach (BlockTypes type in Enum.GetValues(typeof(BlockTypes)))
            {
                TankBlock prefab = Singleton.Manager<ManSpawn>.inst.SpawnBlock(type, Vector3.zero, Quaternion.identity);
                if (prefab)
                {
                    int hashCode = ItemTypeInfo.GetHashCode(ObjectTypes.Block, (int)type);

                    infos +=
                        prefab.name + ";" +
                        type.ToString() + ";" +
                        Singleton.Manager<ManSpawn>.inst.VisibleTypeInfo.GetDescriptor<FactionSubTypes>(hashCode).ToString() + ";" +
                        (ManLicenses.inst.GetBlockUnlockTable().GetBlockTier(type,true)+1) + ";" +
                        prefab.BlockCategory.ToString() + ";" +
                        prefab.BlockRarity.ToString() + ";" +
                        Singleton.Manager<RecipeManager>.inst.GetBlockBuyPrice(type).ToString() + ";" +
                        prefab.CurrentMass.ToString() + ";" +
                        prefab.visible.damageable.Health.ToString();

                    try
                    {
                        infos += ";"+ prefab.GetComponentInParent<ModuleWeaponGun>().FiringData.m_BulletPrefab.m_Damage;
                    }
                    catch { infos += ";"; }
                    try
                    {
                        infos += ";" + prefab.GetComponentInParent<ModuleDrill>().damagePerSecond;// +";"+ prefab.GetComponentInParent<ModuleDrill>().impactDamageMultiplier;
                    }
                    catch { infos += ";"; }
                    try
                    {
                        infos += ";" + prefab.GetComponentInParent<ModuleHammer>().impactDamage;
                    }
                    catch { infos += ";"; }
                    try
                    {
                        infos += ";" + prefab.GetComponentInParent<ModuleEnergy>().Store.m_Capacity;
                    }
                    catch { infos += ";"; }
                    try
                    {
                        infos += ";" + prefab.GetComponentInParent<ModuleFuelTank>().Capacity +";"+ prefab.GetComponentInParent<ModuleFuelTank>().RefillRate;
                    }
                    catch { infos += ";"; }
                    try
                    {
                        var shield = prefab.GetComponentInParent<ModuleShieldGenerator>();
                        infos += ";" + shield.m_EnergyConsumptionPerSec+";"+shield.m_EnergyConsumedPerPointHealed+";"+shield.m_EnergyConsumedPerDamagePoint;
                    }
                    catch { infos += ";"; }
                    try
                    {
                        infos += ";" + prefab.GetComponentInParent<ModuleBooster>().FuelBurnPerSecond();
                    }
                    catch { infos += ";"; }

                    infos += "\n";
                }
            }
            //infos += "ID;Object name;Corp;License Level;Category;Rarity;Buy Price;Mass;Health;Gun Damage;Drill Damage;Hammer Damage;Energy Storage Capacity;Fuel Storage Capacity;Fuel Storage Refill Rate;Energy Consumption Per Sec;Energy Consumed Per Point Healed;Energy Consumed Per Damage Point;Fuel Consumed Per Sec";
            File.WriteAllText(Path.Combine(FolderStructure.DataFolder, "BlocksInfos.csv"), infos);
        }

        public static void GetResourcesInfos()
        {
            string infos = "";
            foreach (ChunkTypes type in Enum.GetValues(typeof(ChunkTypes)))
            {
                ResourcePickup prefab = Singleton.Manager<ResourceManager>.inst.SpawnResource(type, Vector3.zero, Quaternion.identity);
                if (prefab)
                {
                    infos +=
                        prefab.name + ";" +
                        type.ToString() + ";" +
                        prefab.ChunkRarity.ToString() + ";" +
                        RecipeManager.inst.GetChunkPrice(type) + ";" +
                        prefab.rbody.mass+"\n";

                }
            }
            infos += "ID;Object Name;Rarity;Value;Mass";
            File.WriteAllText(Path.Combine(FolderStructure.DataFolder, "ResourcesInfos.csv"), infos);

        }
    }
}
