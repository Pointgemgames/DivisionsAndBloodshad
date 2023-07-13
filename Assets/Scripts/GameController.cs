using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool isPaused = false;
    public int currentSlot;
    public GameObject playerModel;
    public GameObject baseItems;
    public GameObject emptyChestModel;
    public GameData currentGame;
    public GameData checkpoint;
    public GameObject playMenu;
    public GameObject slotsMenu;
    public GameObject loadingScreen;
    public List<GameObject> playerHUD;
    public Vector2 defaultSpawnPosition;

    float timeToAutoSave = 1000 * 60 * 5;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        timeToAutoSave -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Menu")
        {
            playMenu.SetActive(true);
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentGame.player.UpdatePlayer(SceneManager.GetActiveScene().name);
        }

        if (timeToAutoSave <= 0)
        {
            timeToAutoSave = 1000 * 60 * 5;
            UpdateCurrentGame(SceneManager.GetActiveScene().name);
        }
    }

    void UpdateCurrentGame(string currentSceneName)
    {
        SceneData currentScene = currentGame.scenes.Find((s) => s.name == currentSceneName);

        currentGame.player.UpdatePlayer(currentSceneName);
        currentScene.UpdateSceneChests();
        currentScene.UpdateSceneItems();

        checkpoint = currentGame;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu") return;

        SceneData targetSceneData = currentGame.scenes.Find((s) => s.name == scene.name);

        if (targetSceneData == null)
        {
            targetSceneData = new SceneData(scene.name, new List<ChestData>());
            currentGame.scenes.Add(targetSceneData);
        }

        else
        {
            List<DroppedItemData> droppedItems = targetSceneData.GetSceneItems();
            List<GameObject> prefabDroppedItems = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));

            foreach (GameObject prefab in prefabDroppedItems)
            {
                Destroy(prefab);
            }

            foreach (DroppedItemData droppedItem in droppedItems)
            {
                GameObject baseItem = baseItems.transform.Find(droppedItem.name).gameObject;
                Item item = baseItem.GetComponent<Item>();

                Instantiate(item.prefab, new Vector3(droppedItem.itemX, droppedItem.itemY, 0), Quaternion.identity);
            }
        }

        List<ChestData> chests = targetSceneData.GetSceneChests();
        List<GameObject> prefabChests = new List<GameObject>(GameObject.FindGameObjectsWithTag("Chest"));

        foreach (ChestData chestData in chests)
        {
            GameObject chestObj = prefabChests.Find(
                (c) => c.transform.position.x == chestData.chestX && c.transform.position.y == chestData.chestY
            );

            if (chestObj == null)
            {
                chestObj = Instantiate(emptyChestModel, new Vector3(chestData.chestX, chestData.chestY, 0), Quaternion.identity);
            }

            Chest chest = chestObj.GetComponent<Chest>();
            chest.storedItems = chestData.storedItems;
        }

        UpdateCurrentGame(scene.name);
        timeToAutoSave = 1000 * 60 * 5;

        playerHUD.ForEach((c) => c.SetActive(true));
        loadingScreen.SetActive(false);
    }

    public GameObject FindCanvas(string tag)
    {
        Transform canvas = transform.parent.Find("Canvas");
        List<Transform> children = new List<Transform>(canvas.GetComponentsInChildren<Transform>(true));

        Transform child = children.Find((child) => child.CompareTag(tag));

        return child != null ? child.gameObject : null;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        playMenu.SetActive(false);

        Time.timeScale = 1;
        isPaused = false;
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        if (!File.Exists(Application.persistentDataPath + $"/Slot-{currentSlot}.dat"))
        {
            file = File.Create(Application.persistentDataPath + $"/Slot-{currentSlot}.dat");
        }

        else
        {
            file = File.Open(Application.persistentDataPath + $"/Slot-{currentSlot}.dat", FileMode.Open);
        }

        string currentSceneName = SceneManager.GetActiveScene().name;

        UpdateCurrentGame(currentSceneName);
        bf.Serialize(file, currentGame);
    }

    public IEnumerator LoadGame(int slot)
    {
        currentSlot = slot;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;

        GameObject player = Instantiate(playerModel, defaultSpawnPosition, Quaternion.identity);
        Inventory inventory = player.GetComponent<Inventory>();

        if (!File.Exists(Application.persistentDataPath + $"/Slot-{slot}.dat"))
        {
            slotsMenu.SetActive(false);
            PlayerData newPlayer = new PlayerData("Map", player);

            currentGame = new GameData(newPlayer, new List<SceneData>());
            checkpoint = currentGame;

            ChangeScene("Map", player.transform.position);
        }

        else
        {
            yield return new WaitUntil(() => inventory.isInitialized);
            slotsMenu.SetActive(false);

            try
            {
                file = File.Open(Application.persistentDataPath + $"/Slot-{slot}.dat", FileMode.Open);
                file.Position = 0;
   
                currentGame = (GameData)bf.Deserialize(file);
                checkpoint = currentGame;
                file.Close();

                currentGame.player.items.ForEach((item) => {
                    inventory.PutItem(item);
                });

                ChangeScene(
                    currentGame.player.scene,
                    new Vector2(currentGame.player.playerX, currentGame.player.playerY)
                );
            }

            catch
            {
                ExitCurrentGame(false);
            }
        }
    }

    public void LoadCheckpoint()
    {
        currentGame = checkpoint;
        currentGame.player.health = 100;

        GameObject player = Instantiate(playerModel, defaultSpawnPosition, Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>();
        Inventory inventory = player.GetComponent<Inventory>();

        inventory.storedItems = currentGame.player.items;
        ChangeScene(currentGame.player.scene, new Vector2(currentGame.player.playerX, currentGame.player.playerY));
    }

    public void ChangeScene(string targetScene, Vector2 playerSpawnPosition)
    {
        GameObject essentials = GameObject.FindGameObjectWithTag("Essentials");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerHUD.ForEach((c) => c.SetActive(false));
        loadingScreen.SetActive(true);

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "Menu")
        {
            SceneData currentSceneData = currentGame.scenes.Find((s) => s.name == currentScene.name);
            currentSceneData.UpdateSceneChests();
            currentSceneData.UpdateSceneItems();
        }

        SceneManager.LoadScene(targetScene);

        if (targetScene != "Menu")
        {
            player.transform.position = playerSpawnPosition;
            DontDestroyOnLoad(essentials);
            DontDestroyOnLoad(player);
        }

        else
        {
            currentGame = null;
            currentSlot = 0;

            SceneManager.sceneLoaded -= OnSceneLoaded;

            Destroy(essentials);
            Destroy(player);
        }
    }

    public void ExitCurrentGame(bool save)
    {
        if (save) SaveGame();
        ChangeScene("Menu", Vector2.zero);
    }
}
