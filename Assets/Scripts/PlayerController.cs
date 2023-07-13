using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Item equippedTool = null;
    public static float maxHealth = 100;
    public float health = maxHealth;
    public float speed;
    public float rotationSpeed;
    public Animator anim;
    public GameObject playerArm;
    public Animator armAnim;
    public bool isAttacking = false;
    public bool isRespawning = false;

    GameController gameController;
    Inventory inventory;
    Vector2 movement;
    Rigidbody2D rig;
    GameObject dieScreen;
    Text dieCountdown;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();

        dieScreen = gameController.FindCanvas("DieUI");
        dieCountdown = dieScreen?.transform.Find("Background").Find("Countdown").GetComponent<Text>();
    }

    IEnumerator OnDie()
    {
        dieScreen.SetActive(true);
        isRespawning = true;

        int i = 3;
        while (i > 0)
        {
            dieCountdown.text = $"Respawning in {i} seconds...";
            yield return new WaitForSeconds(1);
            i--;
        }

        dieScreen.SetActive(false);
        gameController.LoadCheckpoint();

        health = maxHealth;
        isRespawning = false;
    }

    public void TakeDamage(float damage)
    {
        if (!gameController.isPaused && !isRespawning)
        {
            health -= damage;
            if (health <= 0)
            {
                StartCoroutine(OnDie());
            }
        }
    }

    void UseTool()
    {
        if (!equippedTool)
        {
            Weapon punch = playerArm.transform.Find("maosocohomem").GetComponent<Weapon>();
            StartCoroutine(punch.PlayerAttack());
        }

        else
        {
            if (equippedTool.info.type == "Weapon")
            {
                Weapon weapon = equippedTool.playerItem.transform.Find("espada").Find("adaga (1)").GetComponent<Weapon>();
                StartCoroutine(weapon.PlayerAttack());
            }
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical).normalized;
        anim.SetBool("Moving", movement != Vector2.zero && !isAttacking && !gameController.isPaused);

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            inventory.OpenOrClose();
        }

        if (movement != Vector2.zero && !gameController.isPaused)
        {
            Vector2 rotation = new Vector2(horizontal, vertical) * rotationSpeed * Time.timeScale;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, rotation);
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking && !gameController.isPaused)
        {
            UseTool();
        }
    }

    void FixedUpdate()
    {
        rig.velocity = (!isAttacking && !gameController.isPaused) ?
            movement * speed :
            Vector2.zero;
    }

    public void EquipWeapon(Item tool)
    {
        if (equippedTool != null)
        {
            equippedTool.playerItem.SetActive(false);
            isAttacking = false;
        }

        equippedTool = (equippedTool == tool) ? null : tool;
    }
}