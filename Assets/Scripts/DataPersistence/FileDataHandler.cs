using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirectoryPath = "";

    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "churbulla";

    public FileDataHandler(string dataDirectoryPath, string dataFileName, bool useEncryption)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string saveSlot)
    {
        // Use Path.Combine to get a full path which isn't operating system dependent.
        string fullPath = Path.Combine(dataDirectoryPath, saveSlot, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // Deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data, string saveSlot)
    {
        // Use Path.Combine to get a full path which isn't operating system dependent.
        string fullPath = Path.Combine(dataDirectoryPath, saveSlot, dataFileName);
        try
        {
            // Create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // Optionally encrypt data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // Write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> saveSlotDictionary = new Dictionary<string, GameData>();

        // Loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirectoryPath).EnumerateDirectories();
        foreach (var dirInfo in dirInfos)
        {
            string saveSlot = dirInfo.Name;

            // Check for savegame data in directory before adding it
            string fullPath = Path.Combine(dataDirectoryPath, saveSlot, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading save slots because it doesn't contain savegame data: " + saveSlot);
                continue;
            }

            // Load the game data for this save slot and put it in the dictionary
            GameData saveSlotData = Load(saveSlot);

            if (saveSlotData != null)
            {
                saveSlotDictionary.Add(saveSlot, saveSlotData);
            }
            else
            {
                Debug.LogError("Tried to load profile but an error occured. Save slot: " + saveSlot);
            }
        }

        return saveSlotDictionary;
    }

    // Simple XOR encryption for Json playerData

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

}
