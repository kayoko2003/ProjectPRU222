using UnityEngine;

public class Passive : Item
{
    public PassiveData data;
    [SerializeField] CharacterData.Stats currentBoots;

    [System.Serializable]
    public struct Modifier
    {
        public string name, description;
        public CharacterData.Stats boosts;
    }

    public virtual void Initialise(PassiveData data)
    {
        base.Intialise(data);
        this.data = data;
        currentBoots = data.baseStats.boosts;
    }

    public virtual CharacterData.Stats GetBoosts()
    {
        return currentBoots;
    }

    public override bool DoLevelUp()
    {
        return base.DoLevelUp();

        if (!CanLevelUp())
        {
            return false;
        }

        currentBoots += data.GetLevelData(++currentLevel).boosts;
        return true;
    }
}
