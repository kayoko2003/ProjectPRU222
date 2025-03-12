using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats playerStats;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    {

    }

    void Start()
    {
        playerStats = Object.FindAnyObjectByType<PlayerStats>();   
        ApplyModifier();
    }
}
