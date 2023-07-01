
[System.Serializable]
public class ItemData {
    public string name;
    public int quantity;
    public string type;

    public ItemData(string name, int quantity, string type)
    {
        this.name = name;
        this.quantity = quantity;
        this.type = type;
    }
}