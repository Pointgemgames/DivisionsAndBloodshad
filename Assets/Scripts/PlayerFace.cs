using UnityEngine;
using UnityEngine.UI;

public class PlayerFace : MonoBehaviour
{
    public PlayerController player;
    public Sprite Awesome;
    public Sprite Good;
    public Sprite Bad;
    Image playerFace;

    void Start()
    {
        playerFace = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.health / 100 > .50)
        {
            playerFace.sprite = Awesome;
        }

        else if (player.health / 100 > .10)
        {
            playerFace.sprite = Good;
        }

        else
        {
            playerFace.sprite = Bad;
        }
    }
}
