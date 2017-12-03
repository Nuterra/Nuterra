using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using UnityEngine;

namespace Sylver.RecipedBlock
{
    public class RecipedBlockMod : TerraTechMod
    {
        private GameObject _holder;

        public override string Name => "RecipedBlockMod";

        public override string Description => "";

        public override void Load()
        {
            new BlockPrefabBuilder()
                .SetBlockID(10000)
                .SetName("GSO Bacon strip")
                .SetDescription("A long strip of bacon with bullet absoring grease")
                .SetPrice(500)
                .SetFaction(FactionSubTypes.GSO)
                .SetCategory(BlockCategories.Standard)
                .SetSize(new Vector3I(4, 1, 2))
                .SetModel(@"Assets/Blocks/Bacon/BaconBlock.prefab")
                .SetIcon(@"Assets/Blocks/Bacon/bacon_icon.png")
                /*.SetRecipe(new List<ChunkTypes>()
                {
                    ChunkTypes.OleiteJelly,
                    ChunkTypes.Wood
                },
                new List<int>()
                {
                    4,
                    4
                },1)*/
                .Register();
            RegisterRecipe();

            new BlockPrefabBuilder()
                .SetBlockID(10001)
                .SetName("Venture Tail expander wing")
                .SetDescription("")
                .SetFaction(FactionSubTypes.VEN)
                .SetCategory(BlockCategories.Flight)
                .SetSize(new Vector3I(3, 3, 1), BlockPrefabBuilder.AttachmentPoints.All)
                .SetModel(@"Assets/Blocks/WingExpander/WingExpander.prefab")
                //.AddComponent<ModuleWing>()
                .Register();


        }

        public void RegisterRecipe()
        {
            List<RecipeTable.Recipe> GSORecipes = new List<RecipeTable.Recipe>();
            foreach (var list in RecipeManager.inst.recipeTable.m_RecipeLists)
            {
                if (list.m_Name == "gsofab") GSORecipes = list.m_Recipes;
            }
            

            var Bacon = new RecipeTable.Recipe()
            {
                m_BuildTimeSeconds = 1,
                m_InputItems = new RecipeTable.Recipe.ItemSpec[]
                {
                    new RecipeTable.Recipe.ItemSpec(new ItemTypeInfo(ObjectTypes.Chunk,(int)ChunkTypes.OleiteJelly),4),
                    new RecipeTable.Recipe.ItemSpec(new ItemTypeInfo(ObjectTypes.Chunk,(int)ChunkTypes.Wood),4)
                },
                m_OutputType = RecipeTable.Recipe.OutputType.Items,
                m_OutputItems = new RecipeTable.Recipe.ItemSpec[]
                {
                    new RecipeTable.Recipe.ItemSpec(new ItemTypeInfo(ObjectTypes.Block,10000),1)
                }
            };
            GSORecipes.Add(Bacon);
        }
    }
}
