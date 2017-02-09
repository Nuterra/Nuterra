using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Maritaria
{
	public static class BlockLoader
	{
		private static readonly Dictionary<int, CustomBlock> CustomBlocks = new Dictionary<int, CustomBlock>();
		private static readonly List<CustomBlock> PreBootRegistrationQueue = new List<CustomBlock>();

		public static void Register(CustomBlock block)
		{
			if (PreBootRegistrationQueue != null)
			{
				PreBootRegistrationQueue.Add(block);
			}
			else
			{
				RegisterImmediatly(block);
			}
		}

		private static void RegisterImmediatly(CustomBlock block)
		{
			int blockID = block.BlockID;
			CustomBlocks.Add(blockID, block);
			int hashCode = ItemTypeInfo.GetHashCode(ObjectTypes.Block, blockID);
			ManSpawn spawnManager = Singleton.Manager<ManSpawn>.inst;
			spawnManager.VisibleTypeInfo.SetDescriptor<FactionSubTypes>(hashCode, block.Faction);
			spawnManager.VisibleTypeInfo.SetDescriptor<BlockCategories>(hashCode, block.Category);
			spawnManager.AddBlockToDictionary(block.Prefab);
		}

		public static class Hooks_ManSpawn
		{
			//Hook to be called at the end of ManSpawn.Start
			public static void Start()
			{
				foreach (CustomBlock queuedBlock in PreBootRegistrationQueue)
				{
					RegisterImmediatly(queuedBlock);
				}
				Register(new SmileyBlock());
				Register(new BaconBlock());
				Register(new Sylver.GyroRotor());
			}
		}

		public static class Hooks_BlockUnlockTable
		{
			//Hook to be called at the beginning of BlockUnlockTable.Init()
			public static void Init(BlockUnlockTable unlockTable)
			{
				//For now, all custom blocks are level 1
				BlockUnlockTable.CorpBlockData[] blockList = unlockTable.m_CorpBlockList;

				foreach (CustomBlock block in CustomBlocks.Values)
				{
					BlockUnlockTable.CorpBlockData corpData = blockList[(int)block.Faction];
					BlockUnlockTable.UnlockData[] unlocked = corpData.m_GradeList[0].m_BlockList;

					Array.Resize(ref unlocked, unlocked.Length + 1);
					unlocked[unlocked.Length - 1] = new BlockUnlockTable.UnlockData
					{
						m_BlockType = (BlockTypes)block.BlockID,
						m_BasicBlock = true,
						m_DontRewardOnLevelUp = true,
					};
					corpData.m_GradeList[0].m_BlockList = unlocked;
				}
			}
		}

		public static class Hooks_ManLicenses
		{
			//Hook to be called at beginning of SetupLicenses
			//Discovers all the custom blocks (should change later)
			public static void SetupLicenses(ManLicenses licenses)
			{
				foreach (CustomBlock block in CustomBlocks.Values)
				{
					licenses.DiscoverBlock((BlockTypes)block.BlockID);
				}
			}
		}

		public static class Hooks_IntStatList
		{
			//Hook by replacement ManStats.IntStatList.OnSerializing()
			//Fixes blocks not loading because the name of the block is serialized into a number and can't be resolved by Enum.GetName()
			public static void OnSerializing(ManStats.IntStatList list, StreamingContext context)
			{
				list.m_StatPerTypeSerialized = new Dictionary<string, int>(list.m_StatPerType.Count);
				foreach (KeyValuePair<int, int> current in list.m_StatPerType)
				{
					string value = Enum.GetName(list.m_EnumType, current.Key) ?? current.Key.ToString();
					list.m_StatPerTypeSerialized.Add(value, current.Value);
				}
			}
		}

		public static class Hooks_StringLookup
		{
			//Hook at start of method, override result if not null (http://prntscr.com/dqv0zy)
			public static string GetString(int itemType, LocalisationEnums.StringBanks itemEnum)
			{
				switch (itemEnum)
				{
					case LocalisationEnums.StringBanks.BlockNames:
						return GetString_BlockName(itemType);

					case LocalisationEnums.StringBanks.BlockDescription:
						return GetString_BlockDescription(itemType);

					default:
						return null;
				}
			}

			private static string GetString_BlockName(int blockID)
			{
				CustomBlock block;
				if (CustomBlocks.TryGetValue(blockID, out block))
				{
					return block.Name;
				}
				return null;
			}

			private static string GetString_BlockDescription(int blockID)
			{
				CustomBlock block;
				if (CustomBlocks.TryGetValue(blockID, out block))
				{
					return block.Description;
				}
				return null;
			}
		}

		public static class Hooks_SpriteFetcher
		{
			//Hook at start of method, override result if not null (http://prntscr.com/dqvhrh)
			public static Sprite GetSprite(ObjectTypes objectType, int itemType)
			{
				switch (objectType)
				{
					case ObjectTypes.Block:
						return GetSprite_Block(itemType);
				}
				return null;
			}

			private static Sprite GetSprite_Block(int itemType)
			{
				CustomBlock block;
				if (CustomBlocks.TryGetValue(itemType, out block))
				{
					return block.DisplaySprite;
				}
				return null;
			}
		}
	}
}
