using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData player;
    public List<SceneData> scenes = new List<SceneData>();

    public GameData(PlayerData player, List<SceneData> scenes)
    {
        this.player = player;
        this.scenes = scenes;
    }
}