using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Item equipedTool = null;
    public float health = maxHealth;
    public Animator anim;
    public GameObject playerArm;
    public Animator armAnim;

    static float maxHealth = 100;
    float speed = 7f;
    bool isAttacking = false;
    GameController gameController;
    Vector2 movement;
    Rigidbody2D rig;
    GameObject dieScreen;
    Text dieCountdown;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();

        dieScreen = gameController.FindCanvas("DieUI");
        dieCountdown = dieScreen?.transform.Find("Background").Find("Countdown").GetComponent<Text>();
    }

    IEnumerator OnDie()
    {
        dieScreen.SetActive(true);
        health = maxHealth;

        int i = 5;
        while (i > 0)
        {
            dieCountdown.text = $"Respawning in {i} seconds...";
            yield return new WaitForSeconds(1);
            i--;
        }

        dieScreen.SetActive(false);
        gameController.LoadCheckpoint();
    }

    public void TakeDamage(float damage)
    {
        if (!gameController.isPaused)
        {
            health -= damage;
            if (health <= 0)
            {
                StartCoroutine(OnDie());
            }
        }
    }

    IEnumerator UseTool()
    {
        isAttacking = true;

        if (!equipedTool)
        {
            playerArm.SetActive(true);
            armAnim.Play("Attack", -1, 0f);

            yield return new WaitForSeconds(0.18f);
            playerArm.SetActive(false);

            yield return new WaitForSeconds(.05f);
            isAttacking = false;
        }

        else
        {
            equipedTool.item.SetActive(true);

            Animator toolAnim = equipedTool.item.GetComponent<Animator>();
            toolAnim.Play("Attack", -1, 0f);

            yield return new WaitForSeconds(equipedTool.animationTime);
            
            if (equipedTool != null)
            {
                equipedTool.item.SetActive(false);
            }

            yield return new WaitForSeconds(equipedTool.attackDelay);
            isAttacking = false;
        }

    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        anim.SetBool("Moving", (horizontalInput != 0 || verticalInput != 0) && !isAttacking && !gameController.isPaused);
        movement = new Vector2(horizontalInput, verticalInput).normalized;

        if (movement != Vector2.zero && !gameController.isPaused)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, movement);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Inventory inventory = GetComponent<Inventory>();
            inventory.OpenOrClose();
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking && !gameController.isPaused)
        {
            StartCoroutine(UseTool());
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
        if (equipedTool != null)
        {
            equipedTool.item.SetActive(false);
            isAttacking = false;
        }

        equipedTool = (equipedTool == tool) ? null : tool;
    }
}