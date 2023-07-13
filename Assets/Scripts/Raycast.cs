using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    GameController gameController;
    Transform playerTransform;
    Text raycastText;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        Transform hud = gameController.FindCanvas("HUD").transform;
        raycastText = hud.Find("RaycastText").GetComponent<Text>();
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);

        if (hit.collider == null)
        {
            raycastText.enabled = false;
            return;
        }

        if (hit.collider.tag == "Item" && Vector2.Distance(playerTransform.position, hit.transform.position) < 3f)
        {
            Item item = hit.collider.gameObject.GetComponent<Item>();
            Inventory inventory = GetComponent<Inventory>();

            if (Input.GetMouseButtonDown(1))
            {
                inventory.PutItem(item.info);
                Destroy(hit.collider.gameObject);
            }

            Text text = raycastText.GetComponent<Text>();
            text.text = "[RMB] TO TAKE THE " + item.info.name;

            raycastText.transform.position = mousePosition + new Vector2(17.2f, -45);
            text.enabled = true;
        }

        else if (hit.collider.tag == "fechadura" && Vector2.Distance(playerTransform.position, hit.transform.position) < 4f)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Chest chest = hit.transform.parent.GetComponent<Chest>();
                chest.Open();
            }

            raycastText.text = "[RMB] TO OPEN THE CHEST";

            raycastText.transform.position = mousePosition + new Vector2(17.2f, -45);
            raycastText.enabled = true;
        }

        else
        {
            raycastText.enabled = false;
        }
    }
}