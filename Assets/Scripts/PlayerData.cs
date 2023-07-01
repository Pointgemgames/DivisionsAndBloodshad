using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string scene;
    public float playerX;
    public float playerY;
    public float health;
    public List<ItemData> items = new List<ItemData>();
    public List<string> completedMissions = new List<string>();

    public PlayerData(string scene, GameObject player, List<string> completedMissions = null)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        Inventory inventory = player.GetComponent<Inventory>();

        this.scene = scene;
        this.playerX = player.transform.position.x;
        this.playerY = player.transform.position.y;
        this.health = playerController.health;

        foreach (ItemData item in inventory.storedItems)
        {
            if (item.quantity > 0)
            {
                items.Add(item);
            }
        }

        if (completedMissions != null)
        {
            this.completedMissions = completedMissions;
        }
    }

    public void UpdatePlayer(string currentScene)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        Inventory inventory = player.GetComponent<Inventory>();

        this.health = playerController.health;
        this.items = inventory.storedItems.FindAll(item => item.quantity > 0);
        this.scene = currentScene;
        this.playerX = player.transform.position.x;
        this.playerY = player.transform.position.y;
    }
}