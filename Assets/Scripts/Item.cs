using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData info;
    public float animationTime;
    public float attackDelay;
    public GameObject item;
    public GameObject prefab;

    void Start()
    {
        StartCoroutine(setPlayerItem());
    }

    IEnumerator setPlayerItem()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
        this.item = GameObject.FindGameObjectWithTag("Player").transform.Find(info.name).gameObject;
    }
}
