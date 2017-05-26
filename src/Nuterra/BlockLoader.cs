using System;
using System.Collections.Generic;
using Nuterra.Internal;

namespace Nuterra
{
	public static class BlockLoader
	{
		private static readonly Dictionary<int, CustomBlock> CustomBlocks = new Dictionary<int, CustomBlock>();

		public static void Register(CustomBlock block)
		{
			Console.WriteLine($"Registering block: {block.GetType()} #{block.BlockID} '{block.Name}'");
			int blockID = block.BlockID;
			CustomBlocks.Add(blockID, block);
			int hashCode = ItemTypeInfo.GetHashCode(ObjectTypes.Block, blockID);
			ManSpawn spawnManager = ManSpawn.inst;
			spawnManager.VisibleTypeInfo.SetDescriptor(hashCode, block.Faction);
			spawnManager.VisibleTypeInfo.SetDescriptor(hashCode, block.Category);
			spawnManager.AddBlockToDictionary(block.Prefab);
			RecipeManager.inst.m_BlockPriceLookup.Add(blockID, block.Price);
		}

		internal static void PostModsLoaded()
		{
			Singleton.DoOnceAfterStart(FixBlockUnlockTable);
			Hooks.Managers.Licenses.OnInitializing += SetupLicenses;
			Hooks.ResourceLookup.OnStringLookup += ResourceLookup_OnStringLookup;
			Hooks.ResourceLookup.OnSpriteLookup += ResourceLookup_OnSpriteLookup;
		}

		private static void FixBlockUnlockTable()
		{
			//For now, all custom blocks are level 1
			BlockUnlockTable.CorpBlockData[] blockList = ManLicenses.inst.GetBlockUnlockTable().m_CorpBlockList;
			foreach (CustomBlock block in CustomBlocks.Values)
			{
				BlockUnlockTable.CorpBlockData corpData = blockList[(int)block.Faction];
				BlockUnlockTable.UnlockData[] unlocked = corpData.m_GradeList[0].m_BlockList;
				//TODO: Only resize once
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

		public static void SetupLicenses()
		{
			var licenses = ManLicenses.inst;
			foreach (CustomBlock block in CustomBlocks.Values)
			{
				licenses.DiscoverBlock((BlockTypes)block.BlockID);
			}
		}

		private static void ResourceLookup_OnSpriteLookup(SpriteLookupEvent eventInfo)
		{
			if (eventInfo.ObjectType == ObjectTypes.Block)
			{
				CustomBlock block;
				if (CustomBlocks.TryGetValue(eventInfo.ItemType, out block))
				{
					eventInfo.Result = block.DisplaySprite;
				}
			}
		}

		private static void ResourceLookup_OnStringLookup(StringLookupEvent eventInfo)
		{
			CustomBlock block;
			switch (eventInfo.StringBank)
			{
				case LocalisationEnums.StringBanks.BlockNames:
					if (CustomBlocks.TryGetValue(eventInfo.EnumValue, out block))
					{
						eventInfo.Result = block.Name;
					}
					break;

				case LocalisationEnums.StringBanks.BlockDescription:
					if (CustomBlocks.TryGetValue(eventInfo.EnumValue, out block))
					{
						eventInfo.Result = block.Description;
					}
					break;
			}
		}
	}
}