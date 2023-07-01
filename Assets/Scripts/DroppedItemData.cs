[System.Serializable]
public class DroppedItemData : ItemData {
    public float itemX;
    public float itemY;

    public DroppedItemData(string name, string type, float x, float y) : base(name, 1, type)
    {
        this.name = name;
        this.quantity = 1;
        this.type = type;
        this.itemX = x;
        this.itemY = y;
    }
}