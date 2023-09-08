using System.Collections.Generic;

namespace GameCore
{
    public class Block
    {
        public PrefabType color;
        public int width;
        public int height;
    }
    public class BlockList
    {
        public List<Block> blocks = new List<Block>();
    }
    public enum PrefabType
    {
        Blue,
        Green,
        Orange,
        Purple,
        Red,
        Yellow,
        Random
    }

}
