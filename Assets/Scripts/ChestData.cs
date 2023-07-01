using System.Collections.Generic;

[System.Serializable]
public class ChestData 
{
    public float chestX;
    public float chestY;
    public List<ItemData> storedItems = new List<ItemData>();

    public ChestData(float chestX, float chestY, List<ItemData> items)
    {
        this.chestX = chestX;
        this.chestY = chestY;
        this.storedItems = items;
    }
}