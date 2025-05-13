using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveFile saveFile = new SaveFile();
    
    public static void load(string filename)
    {
        filename = Application.persistentDataPath + "/" + filename;
        if (File.Exists(filename))
        {
            saveFile = new BinaryFormatter().Deserialize(File.OpenRead(filename)) as SaveFile;
        }
        else
        {
            Debug.LogError($"file {filename} not found");
        }
    }

    public static void save(string filename)
    {
        filename = Application.persistentDataPath + "/" + filename;
        if (File.Exists(filename))
        {
            new BinaryFormatter().Serialize(File.OpenWrite(filename), saveFile);
        }
        else
        {
            new BinaryFormatter().Serialize(File.Create(filename), saveFile);
        }
    }
}
