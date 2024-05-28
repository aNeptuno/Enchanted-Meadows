using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
public class JsonDataService : IDataService
{
    public bool SaveData<T>(string RelativePath, T Data, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data exist. Deleting old file and writing a new one");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Creating file for the first time");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }
    public T LoadData<T>(string RelativePath, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;
        if (!File.Exists(path))
        {
            Debug.Log($"Cannot load file at {path}. File doesnt exist");
            throw new FileNotFoundException($"{path} doesnt exist!");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch(Exception e)
        {
            Debug.Log($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

}
