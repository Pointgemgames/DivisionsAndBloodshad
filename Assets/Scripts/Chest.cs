using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<ItemData> storedItems = new List<ItemData>();
    public GameObject slotModel;
    public int maxSize;
    public int maxStackSize;
    public AudioClip openSound;
    public AudioClip closeSound;

    GameController gameController;   
    GameObject chestUI;
    GameObject hud;
    GameObject minimap;
    GameObject baseItems;
    Inventory playerInventory;
    AudioSource source;
    Image list;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerInventory = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Inventory>();

        hud = gameController.FindCanvas("HUD");
        minimap = gameController.FindCanvas("Minimap");

        chestUI = gameController.FindCanvas("ChestUI");
        list = gameController.FindCanvas("ChestList")?.GetComponent<Image>();
        source = gameObject.GetComponent<AudioSource>();

        baseItems = GameObject.FindGameObjectWithTag("BaseItems");

        foreach (Transform baseItemObj in baseItems.transform)
        {
            Item baseItem = baseItemObj.GetComponent<Item>();
            if (storedItems.Find((item) => item.name == baseItem.info.name && item.type == baseItem.info.type) == null)
            {
                storedItems.Add(new ItemData(baseItem.info.name, 0, baseItem.info.type));
            }
        }
    }

    public void PutItem(ItemData item)
    {
        if (storedItems.Count < maxSize)
        {
            ItemData storedItem = storedItems.Find((storedItem) => storedItem.name == item.name && storedItem.type == item.type);
            storedItem.quantity++;

            ResetUIList();
        }
    }

    public void ResetUIList()
    {
        foreach (Transform child in list.transform)
        {
            if (child.tag == "Slot")
            {
                Destroy(child.gameObject);
            }
        }

        int i = 0;
        foreach (ItemData item in storedItems)
        {
            if (item.quantity == 0) continue;

            GameObject slotObj = Instantiate(slotModel);
            GameObject textObj = slotObj.transform.GetChild(0).gameObject;
            Vector2 slotPos = new Vector2(0, (-30 * i) / 2);

            ChestSlot slot = slotObj.GetComponent<ChestSlot>();
            Text text = textObj.GetComponent<Text>();
            RectTransform rectTransform = slotObj.GetComponent<RectTransform>();

            slot.item = item;
            slot.chest = this;
            text.text = $"{item.name} x{item.quantity}";

            rectTransform.SetParent(list.transform);
            rectTransform.localScale = new Vector2(list.rectTransform.localScale.x, rectTransform.localScale.y);
            rectTransform.localPosition = slotPos;

            i++;
        }
    }

    public void Open()
    {
        ResetUIList();

        hud.SetActive(false);
        minimap.SetActive(false);
        chestUI.SetActive(true);
    
        source.PlayOneShot(openSound);
        playerInventory.OpenWithChest(this);
    }

    public void Close()
    {
        hud.SetActive(true);
        minimap.SetActive(true);
        chestUI.SetActive(false);

        source.PlayOneShot(closeSound, 1);
        playerInventory.openedChest = null;
    }

    public void MoveItem(ItemData item)
    {
        ItemData storedItem = storedItems.Find(i => i.name == item.name && i.type == item.type);
        
        if (storedItem.quantity > 0) {
            playerInventory.PutItem(new ItemData(item.name, 1, item.type));
            storedItem.quantity--;
            ResetUIList();
        }
    }
}
