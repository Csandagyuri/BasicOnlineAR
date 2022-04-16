using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    
    public static void SaveObject(GameObject gameObject)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/objects.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        ObjectData data = new ObjectData(gameObject);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static ObjectData LoadObjectPosition()
    {
        string path = Application.persistentDataPath + "/objects.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            Debug.Log(path);
            ObjectData data = formatter.Deserialize(stream) as ObjectData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }

    public static void SaveObjectInJson(GameObject gameObject)
    {
        //BinaryFormatter formatter = new BinaryFormatter();
        var objectAsJson = JsonUtility.ToJson(gameObject);

        string path = Application.persistentDataPath + "/objects.json";
        //FileStream stream = new FileStream(path, FileMode.Create);

        ObjectData data = new ObjectData(gameObject);

        //formatter.Serialize(stream, data);
        File.WriteAllText(path, objectAsJson);

        //stream.Close();
    }

    public static ObjectData LoadObjectPositionInJson()
    {
        string path = Application.persistentDataPath + "/objects.json";
        if (File.Exists(path))
        {
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            Debug.Log(path);
            ObjectData data = formatter.Deserialize(stream) as ObjectData;
            stream.Close();
            

            var inputString = File.ReadAllText(path);

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
}
