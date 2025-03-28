using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public Sprite icon;
    public int maxLevel;

    [System.Serializable]
    public struct Evolution
    {
        public string name;
        public enum Condition { auto, treasureChest}

        public Condition condition;

        [System.Flags] public enum Consumption { passives = 1, weapon = 2 }
        public Consumption consumes;

        public int evolutionLevel;
        public Config[] catalyst;
        public Config outcome;

        [System.Serializable]
        public struct Config
        {
            public ItemData itemType;
            public int level;
        }
    }

    public Evolution[] evolutionData;
}
