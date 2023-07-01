using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public string targetScene;
    public Vector3 spawnPosition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            gameController.ChangeScene(targetScene, spawnPosition);
            player.transform.position = spawnPosition;
        }
    }
}
