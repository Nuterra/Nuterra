using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public static class BlockLoader
	{
		private static readonly Dictionary<int, CustomBlock> CustomBlocks = new Dictionary<int, CustomBlock>();
		
		//Hook to be called at the end of ManSpawn.Start
		public static void Init()
		{
			RegisterCustomBlock(new SmileyBlock());
		}
		
		public static void RegisterCustomBlock(CustomBlock block)
		{
			int blockID = block.BlockID;
			CustomBlocks.Add(blockID, block);
			int hashCode = ItemTypeInfo.GetHashCode(ObjectTypes.Block, blockID);
			ManSpawn spawnManager = Singleton.Manager<ManSpawn>.inst;
			spawnManager.VisibleTypeInfo.SetDescriptor<FactionSubTypes>(hashCode, block.Faction);
			spawnManager.VisibleTypeInfo.SetDescriptor<BlockCategories>(hashCode, block.Category);
			spawnManager.AddBlockToDictionary(block.Prefab);
		}
		
		//Hook to be called at the beginning of BlockUnlockTable.Init()
		public static void BlockUnlockTable_Init(BlockUnlockTable unlockTable)
		{
			//For now, all custom blocks are level 1
			BlockUnlockTable.CorpBlockData[] blockList = unlockTable.m_CorpBlockList;
			
			foreach(CustomBlock block in CustomBlocks.Values)
			{
				BlockUnlockTable.CorpBlockData corpData = blockList[(int)block.Faction];
				BlockUnlockTable.UnlockData[] unlocked = corpData.m_GradeList[0].m_BlockList;
				
				Array.Resize(ref unlocked, unlocked.Length + 1);
				unlocked[unlocked.Length - 1] = new BlockUnlockTable.UnlockData {
					m_BlockType = (BlockTypes)block.BlockID,
					m_BasicBlock = true,
					m_DontRewardOnLevelUp = true,
				};
				corpData.m_GradeList[0].m_BlockList = unlocked;
			}
		}
		
		//Hook to be called at beginning of SetupLicenses
		public static void ManLicenses_SetupLicenses(ManLicenses licenses)
		{
			foreach(CustomBlock block in CustomBlocks.Values)
			{
				licenses.DiscoverBlock((BlockTypes)block.BlockID);
			}
		}
		
		//Hook by replacement ManStats.IntStatList.OnSerializing()
		public static void IntStatList_OnSerializing(ManStats.IntStatList list)
		{
			list.m_StatPerTypeSerialized = new Dictionary<string, int>(list.m_StatPerType.Count);
			foreach (KeyValuePair<int, int> current in list.m_StatPerType)
			{
				string value = Enum.GetName(list.m_EnumType, current.Key) ?? current.Key.ToString();
				list.m_StatPerTypeSerialized.Add(value, current.Value);
			}
		}
		//Hook at start of method, override result if not null (http://prntscr.com/dqv0zy)
		public static string StringLookup_GetString(int itemType, LocalisationEnums.StringBanks itemEnum)
		{
			switch(itemEnum)
			{
				case LocalisationEnums.StringBanks.BlockNames:
					return StringLookup_GetString_BlockName(itemType);
				break;
				case LocalisationEnums.StringBanks.BlockDescription:
					return StringLookup_GetString_BlockDescription(itemType);
				default:
					return null;
			}
		}
		
		private static string StringLookup_GetString_BlockName(int blockID)
		{
			CustomBlock block;
			if (CustomBlocks.TryGetValue(blockID, out block))
			{
				return block.Name;
			}
			return null;
		}
		
		private static string StringLookup_GetString_BlockDescription(int blockID)
		{
			CustomBlock block;
			if (CustomBlocks.TryGetValue(blockID, out block))
			{
				return block.Description;
			}
			return null;
		}
	}
}