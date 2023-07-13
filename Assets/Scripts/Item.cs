using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData info;
    public int rarity;
    public GameObject playerItem;
    public GameObject baseItem;
    public GameObject prefab;

    void Start()
    {
        baseItem = gameObject;
        StartCoroutine(setPlayerItem());
    }

    IEnumerator setPlayerItem()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
        this.playerItem = GameObject.FindGameObjectWithTag("Player").transform.Find(info.name).gameObject;
    }
}
