using System.IO;
using UnityEngine;
using UnityEngine.UI;

class SlotSelect : MonoBehaviour
{
    public int slotNumber;
    public Text text;
    public GameController gameController;

    void Start()
    { 
        bool existsSlot = File.Exists(Application.persistentDataPath + $"/Slot-{slotNumber}.dat");
        text.text = existsSlot ? "Load Game" : "Create Game";
    }

    public void OnClick()
    {
        StartCoroutine(gameController.LoadGame(slotNumber));
    }
}