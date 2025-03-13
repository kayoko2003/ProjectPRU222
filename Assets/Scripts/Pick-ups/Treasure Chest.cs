using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    InventoryManager inventoryManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = Object.FindAnyObjectByType<InventoryManager>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenTreasureChest();
            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest()
    {
        if(inventoryManager.GetPossibleEvolutions().Count <= 0)
        {
            return;
        }

        WeaponEvolutionBlueprint toEvolve = inventoryManager.GetPossibleEvolutions()[Random.Range(0, inventoryManager.GetPossibleEvolutions().Count)];
        inventoryManager.EvolWeapon(toEvolve);
    }
}
