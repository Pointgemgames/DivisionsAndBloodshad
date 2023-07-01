using UnityEngine.EventSystems;
using UnityEngine;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Inventory inventory;
    public ItemData item;

    public void PlayHoverEffect()
    {
        inventory.PlayHoverEffect();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventory.UseItem(item);
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            inventory.DropItem(item);
        }
    }
}
