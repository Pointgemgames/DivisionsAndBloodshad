using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController player;
    Image healthBar;

    void Start()
    {
        healthBar = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = player.health / 100;
    }
}
