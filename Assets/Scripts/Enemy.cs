using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = maxHealth;
    public float minDistanceFromPlayerToAttack;
    public float maxDistanceFromPlayerToWalk;
    public bool isAttacking = false;
    public Item equippedWeapon;
    public GameObject enemyArm;

    float speed = 4;
    static float maxHealth = 100;
    float rotationSpeed = 360;
    Transform player;
    GameController gameController;
    PlayerController playerController;
    Animator animator;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);

        bool mustAttack = distance <= minDistanceFromPlayerToAttack;
        bool mustWalk =
            distance > minDistanceFromPlayerToAttack &&
            distance < maxDistanceFromPlayerToWalk;

        agent.SetDestination(mustWalk ? player.position : transform.position);
        animator.SetBool("Moving", mustWalk);

        agent.isStopped = !mustWalk;

        if (mustWalk && !isAttacking && !gameController.isPaused)
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, new Vector2(agent.velocity.x, agent.velocity.y).normalized);
            transform.rotation = rotation;
        }

        if (mustAttack && !gameController.isPaused)
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, (player.position - transform.position).normalized);
            transform.rotation = rotation;

            if (!isAttacking)
            {
                Attack();
            }
        }
    }

    public void Attack()
    {
        if (!equippedWeapon)
        {
            Weapon punch = enemyArm.transform.Find("maosocohomem").GetComponent<Weapon>();
            StartCoroutine(punch.EnemyAttack(this));
        }

        else
        {
            if (equippedWeapon.info.type == "Weapon")
            {
                Weapon weapon = equippedWeapon.playerItem.transform.Find("espada").Find("adaga (1)").GetComponent<Weapon>();
                StartCoroutine(weapon.EnemyAttack(this));
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
