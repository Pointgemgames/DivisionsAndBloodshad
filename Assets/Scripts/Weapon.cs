using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public bool isLongRange;
    public float animationTime;
    public float delay;
    public GameObject playerItem;

    public IEnumerator PlayerAttack()
    {
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        playerController.isAttacking = true;
        playerItem.SetActive(true);

        Animator toolAnim = playerItem.GetComponent<Animator>();
        toolAnim.Play("Attack", -1, 0f);

        yield return new WaitForSeconds(animationTime);
        playerItem.SetActive(false);

        yield return new WaitForSeconds(delay);
        playerController.isAttacking = false;
    }

    public IEnumerator EnemyAttack(Enemy enemy)
    {
        enemy.isAttacking = true;
        playerItem.SetActive(true);

        Animator toolAnim = playerItem.GetComponent<Animator>();
        toolAnim.Play("Attack", -1, 0f);

        yield return new WaitForSeconds(animationTime);
        playerItem.SetActive(false);

        yield return new WaitForSeconds(delay);
        enemy.isAttacking = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.TakeDamage(damage);
        }

        else if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
