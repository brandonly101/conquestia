using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager {

    // Public static variable that contains all the current game data.
    public static GameData GameDataSave;

    public static void GameNew () {
        GameDataSave = new GameData();
        GameSave();
    }

    public static void GameSave () {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameDataSave.gd");
        formatter.Serialize(file, SaveManager.GameDataSave);
        file.Close();
        MonoBehaviour.print("Game: Saved");
    }

    public static void GameLoad () {
        if (File.Exists(Application.persistentDataPath + "/GameDataSave.gd")) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameDataSave.gd", FileMode.Open);
            GameDataSave = (GameData) formatter.Deserialize(file);
            file.Close();
        }
        MonoBehaviour.print("Game: Loaded");
    }
}
