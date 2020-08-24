using System.IO;
using UnityEngine;

// Theoretically, we could have multiple such storage objects, used to store different things,
// or to provide access to different storage types. But in this tutorial we use just this
// single file storage object.

public class PersistentStorage : MonoBehaviour
{
    private string _savePath;
    private const string SaveFileName = "saveFile";

    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, $"{SaveFileName}.txt");
    }

    public void Save(PersistableObject o, int version)
    {
        using (var writer = new BinaryWriter(File.Open(_savePath, FileMode.Create)))
        {
            writer.Write(version);
            o.Save(new GameDataWriter(writer));
        }
    }

    public void Load(PersistableObject o)
    {
        using (var reader = new BinaryReader(File.Open(_savePath, FileMode.Open)))
        {
            o.Load(new GameDataReader(reader, reader.ReadInt32()));
        }
    }
    
    // Try-catch implementations
    
    // private void Save()
    // {
    //     try
    //     {
    //         FileStream stream = File.Open(_savePath, FileMode.Create);
    //         var writer = new BinaryWriter(stream, Encoding.UTF8);
    //         writer.Write(_objects.Count);
    //
    //         foreach (var o in _objects)
    //         {
    //             writer.Write(o.localPosition.x);
    //             writer.Write(o.localPosition.y);
    //             writer.Write(o.localPosition.z);
    //         }
    //
    //         Debug.Log($"Saved: {_savePath}");
    //         
    //         //TODO: is this the right way to close
    //         stream.Close();
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogWarning(e);
    //         //throw;
    //     }
    //     
    //     //TODO: Overwrite warning
    //     //TODO: save project  version
    //     //TODO: save in XML
    //     
    // }
    //
    // private void Load()
    // {
    //     //TODO: ask to save before Load (if it's not saved
    //     BeginNewGame();
    //     try
    //     {
    //         FileStream stream = File.OpenRead(_savePath);
    //         var reader = new BinaryReader(stream, Encoding.UTF8);
    //         int count = reader.ReadInt32();
    //         
    //         for (int i = 0; i < count; i++)
    //         {
    //             Vector3 p;
    //             p.x = reader.ReadSingle();
    //             p.y = reader.ReadSingle();
    //             p.z = reader.ReadSingle();
    //             
    //             Transform t = Instantiate(prefab);
    //             t.localPosition = p;
    //             _objects.Add(t);
    //         }
    //         
    //         //TODO: is this the right way to close
    //         //stream.Dispose();
    //         stream.Close();
    //         Debug.Log($"Load complete: {_savePath}");
    //
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogWarning(e);
    //         //throw;
    //     }
    // }
    
}
