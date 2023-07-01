using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemData> storedItems = new List<ItemData>();
    public string uiListType = "";
    public GameObject slotModel;
    public Chest openedChest = null;
    public int maxSize = 20;
    public int maxStackSize = 5;
    public bool isInitialized { get; private set; }
    
    GameObject baseItems;
    Image list;
    AudioSource soundEffect;
    GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        list = gameController.FindCanvas("InventoryList")?.GetComponent<Image>();
        soundEffect = list?.GetComponent<AudioSource>();
        
        baseItems = GameObject.FindGameObjectWithTag("BaseItems");

        foreach (Transform baseItemObj in baseItems.transform)
        {
            Item baseItem = baseItemObj.GetComponent<Item>();
            if (storedItems.Find((item) => item.name == baseItem.info.name && item.type == baseItem.info.type) == null)
            {
                storedItems.Add(new ItemData(baseItem.info.name, 0, baseItem.info.type));
            }
        }

        isInitialized = true;
    }

    public void PutItem(ItemData item)
    {
        if (storedItems.Count < maxSize)
        {
            ItemData storedItem = storedItems.Find((storedItem) => storedItem.name == item.name && storedItem.type == item.type);
            if (storedItem == null) storedItems.Add(item);
            else storedItem.quantity += item.quantity;

            ResetUIList(uiListType);
        }
    }

    public void ResetUIList(string itemType)
    {
        uiListType = itemType;
        foreach (Transform child in list.transform)
        {
            if (child.tag == "Slot")
            {
                Destroy(child.gameObject);
            }
        }

        List<ItemData> filteredItems = storedItems.FindAll(i => i.type == itemType);
        int i = 0;
        
        foreach (ItemData item in filteredItems)
        {
            if (item.quantity == 0) continue;

            GameObject slotObj = Instantiate(slotModel);
            GameObject textObj = slotObj.transform.GetChild(0).gameObject;
            Vector2 slotPos = new Vector2(0, (-30 * i) / 2);

            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            Text text = textObj.GetComponent<Text>();
            RectTransform rectTransform = slotObj.GetComponent<RectTransform>();

            slot.inventory = this;
            slot.item = item;
            text.text = $"{item.name} x{item.quantity}";

            rectTransform.SetParent(list.transform);
            rectTransform.localScale = new Vector2(list.rectTransform.localScale.x, rectTransform.localScale.y);
            rectTransform.localPosition = slotPos;

            i++;
        }
    }

    public void PlayHoverEffect()
    {
        if (soundEffect.isPlaying) soundEffect.Stop();
        soundEffect.Play();
    }

    public void DropItem(ItemData dropItem)
    {
        ItemData storedItem = storedItems.Find(item => item.name == dropItem.name && item.type == dropItem.type);
        if (storedItem.quantity <= 0) return;

        storedItem.quantity--;

        if (!openedChest)
        {
            GameObject baseItemObj = baseItems.transform.Find(dropItem.name).gameObject;
            Item baseItem = baseItemObj.GetComponent<Item>();

            GameObject obj = Instantiate(baseItem.prefab, transform.position, Quaternion.identity);
            obj.SetActive(true);
        }

        else
        {
            openedChest.PutItem(new ItemData(dropItem.name, 1, dropItem.type));
        }

        if (storedItem.quantity <= 0)
        {
            List<GameObject> slots = new List<GameObject>(GameObject.FindGameObjectsWithTag("Slot"));
            PlayerController player = GetComponent<PlayerController>();
        
            if (player.equipedTool != null && player.equipedTool.name == storedItem.name)
            {
                player.equipedTool = null;
            }
        }

        ResetUIList(uiListType);
    }

    public void UseItem(ItemData item)
    {
        ItemData storedItem = storedItems.Find(i => i.name == item.name && i.type == item.type);

        if (storedItem.type == "Weapon")
        {
            PlayerController player = GetComponent<PlayerController>();
            GameObject baseItemObj = baseItems.transform.Find(item.name).gameObject;
            Item baseItem = baseItemObj.GetComponent<Item>();

            player.EquipWeapon(baseItem);
        }

        else if (storedItem.type == "Consumable")
        {
            storedItem.quantity--;
            ResetUIList(uiListType);
        }
    }

    public void Open()
    {
        list.transform.parent.gameObject.SetActive(true);
        gameController.PauseGame();
    }

    public void Close()
    {
        list.transform.parent.gameObject.SetActive(false);
        gameController.ResumeGame();

        if (openedChest != null)
        {
            openedChest.Close();
            openedChest = null;
        }
    }

    public void OpenOrClose()
    {
        if (!list.transform.parent.gameObject.active) Open();
        else Close();
    }

    public void OpenWithChest(Chest chest)
    {
        Open();
        openedChest = chest;
    }

    public void UnequipItem()
    {
        PlayerController player = GetComponent<PlayerController>();
        player.equipedTool = null;
    }
}
