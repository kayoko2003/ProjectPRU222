using UnityEngine;

public class WingsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentMoveSpeed *= 1 + passiveItemData.Mutipler / 100f; 
    }
}
