using UnityEngine;

[System.Serializable]
public class ItemData {
    public string name;
    public int quantity;
    public string type;
    public Sprite sprite;

    public ItemData(string name, int quantity, string type, Sprite sprite = null)
    {
        this.name = name;
        this.quantity = quantity;
        this.type = type;
        this.sprite = sprite;
    }
}