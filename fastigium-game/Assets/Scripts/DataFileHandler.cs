using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataFileHandler {
    private string path = "";
    private string fileName = "";

    public DataFileHandler(string path, string fileName) {
        this.path = path;
        this.fileName = fileName;
    }

    public GameData Load() {
        string fullPath = Path.Combine(path, fileName);
        GameData loadedData = null;

        if (File.Exists(fullPath)) {
            try {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from json to GameData
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            } catch (Exception e) { 
                Debug.LogError("Couldn't save to file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data){
        string fullPath = Path.Combine(path, fileName);
        try {
            // create dir for save file if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        } catch (Exception e) {
            Debug.LogError("Couldn't save to file: " + fullPath + "\n" + e);
        }
    }
}