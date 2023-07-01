using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public LayerMask obstacles;

    GameObject player;
    PlayerController playerController;
    GameController gameController;
    float speed = 4;
    Vector3 movement;
    Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 2)
        {

        }

        else
        {
            Vector2 direction = (transform.position - player.transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, obstacles);
            if (hit.collider != null)
            {
                Vector2 perpendicular = Vector2.Perpendicular(hit.normal).normalized;

                transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
                rig.velocity = perpendicular * speed;
            }

            else
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                rig.velocity = direction * speed;
            }
        }
    }
}
