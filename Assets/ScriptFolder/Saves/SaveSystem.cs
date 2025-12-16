using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    public static void saveFileData (GameManagerScript gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path,FileMode.Create);

        SaveFile data = new SaveFile(gameManager);

        formatter.Serialize(stream,data);
        stream.Close();
    }

    public static SaveFile loadSaveFile()
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);

            SaveFile saveFile = formatter.Deserialize(stream) as SaveFile;
            stream.Close();

            return saveFile;
        }
        else
        {
            Debug.LogError("Save File Not Found in "+path);
            return null;
        }
    }

    public static void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/player.fun";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted successfully: " + path);
        }
        else
        {
            Debug.LogWarning("No save file to delete at: " + path);
        }
    }

    public static void NewSaveFile()
    {
        GameManagerScript managerScript = new GameManagerScript();
        managerScript.unlockedLevel = 1;
        managerScript.currentLevel = 1;
        managerScript.levelScore = new int[] {0,0,0,0,0,0,0};
        saveFileData(managerScript);
    }
}
