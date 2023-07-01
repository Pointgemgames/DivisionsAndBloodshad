using UnityEngine;

public class InventorySelector : MonoBehaviour
{
    public string typeSelect;
    Inventory inventory;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void OnClick()
    {
        inventory.ResetUIList(typeSelect);
    }

    public void OnMouseEnter()
    {
        inventory.PlayHoverEffect();
    }
}
