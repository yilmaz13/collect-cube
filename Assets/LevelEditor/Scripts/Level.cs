using System.Collections.Generic;

namespace GameCore
{
    public enum LevelType
    {
        One,
        Time,
        Multiplayer,
    }

    public class Level
    {
        public int id;
        public LevelType levelType;
        public int width;
        public int height;
        public int currentIndex= 0;
        public int index = 0;
        public List<Block> tiles = new List<Block>();
        public List<BlockList> blockList = new List<BlockList>();
        public float time;
        public int oneCubeScore;
        public int score;
    }
}
