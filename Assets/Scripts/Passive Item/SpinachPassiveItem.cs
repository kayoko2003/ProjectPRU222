using UnityEngine;

public class SpinachPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentMight *= 1 + passiveItemData.Mutipler / 100f;
    }
}
