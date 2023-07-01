using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public List<ChestData> chests = new List<ChestData>();
    public List<DroppedItemData> droppedItems = new List<DroppedItemData>();
    public string name;

    public SceneData(string name, List<ChestData> chests = null)
    {
        this.name = name;
        if (chests != null)
        {
            this.chests = chests;
        }
    }

    public List<ChestData> GetSceneChests()
    {
        if (this.chests.Count == 0)
        {
            for (int i = 0; i < Random.Range(3, 7); i++)
            {
                ChestData chestData = new ChestData(Random.Range(0, 5), Random.Range(0, 5), new List<ItemData>());
                this.chests.Add(chestData);
            }
        }

        return this.chests;
    }

    public List<DroppedItemData> GetSceneItems() 
    {
        return this.droppedItems;
    }

    public void UpdateSceneItems()
    {
        List<DroppedItemData> newDroppedItems = new List<DroppedItemData>();
        foreach (GameObject itemObj in GameObject.FindGameObjectsWithTag("Item"))
        {
            Item item = itemObj.GetComponent<Item>();
            newDroppedItems.Add(new DroppedItemData(item.info.name, item.info.type, item.transform.position.x, item.transform.position.y));
        }

        this.droppedItems = newDroppedItems;
    } 

    public void UpdateSceneChests()
    {
        List<ChestData> newChests = new List<ChestData>();
        foreach (GameObject chestObj in GameObject.FindGameObjectsWithTag("Chest"))
        {
            Chest chest = chestObj.GetComponent<Chest>();
            List<ItemData> chestItems = new List<ItemData>();

            foreach (ItemData item in chest.storedItems)
            {
                chestItems.Add(item);
            }

            newChests.Add(new ChestData(chestObj.transform.position.x, chestObj.transform.position.y, chestItems));
        }

        this.chests = newChests;
    }
}