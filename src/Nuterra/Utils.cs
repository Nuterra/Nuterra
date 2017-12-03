using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuterra
{
    public static class Utils
    {
        private static int currentWindowID = int.MaxValue;

        public static int GetWindowID()
        {
            return currentWindowID--;
        }

        public static ItemTypeInfo ITIFromBlock(BlockTypes type)
        {
            return new ItemTypeInfo(ObjectTypes.Block, (int)type);
        }

        public static ItemTypeInfo ITIFromChunk(ChunkTypes type)
        {
            return new ItemTypeInfo(ObjectTypes.Chunk, (int)type);
        }
    }
}
