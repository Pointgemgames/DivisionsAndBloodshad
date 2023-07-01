using UnityEngine.EventSystems;
using UnityEngine;

public class ChestSlot : MonoBehaviour, IPointerClickHandler
{
    public Chest chest;
    public ItemData item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            chest.MoveItem(item);
        }
    }
}
