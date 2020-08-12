using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform prefab;
    [SerializeField] private KeyCode createKey = KeyCode.C;
    [SerializeField] private KeyCode newGameKey = KeyCode.N;
    [SerializeField] private KeyCode saveKey = KeyCode.S;
    [SerializeField] private KeyCode loadKey = KeyCode.L;

    private const string SaveFileName = "saveFile";
    
    private List<Transform> _objects;
    private string _savePath;
    
    private void Awake()
    {
        _objects = new List<Transform>();
        _savePath = Application.persistentDataPath;
        _savePath = Path.Combine(Application.persistentDataPath, $"{SaveFileName}.txt");
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKey(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
        else if (Input.GetKeyDown(loadKey))
        {
            Load();
        }
    }
    
    private void BeginNewGame()
    {
        foreach (Transform o in _objects)
        {
            Destroy(o.gameObject);
        }
        _objects.Clear();
    }

    private void CreateObject()
    {
        Transform t = Instantiate(prefab);
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        _objects.Add(t);
    }
    
    private void Save()
    {
        try
        {
            FileStream stream = File.Open(_savePath, FileMode.Create);
            var writer = new BinaryWriter(stream, Encoding.UTF8);
            writer.Write(_objects.Count);

            foreach (var o in _objects)
            {
                writer.Write(o.localPosition.x);
                writer.Write(o.localPosition.y);
                writer.Write(o.localPosition.z);
            }

            Debug.Log($"Saved: {_savePath}");
            
            //TODO: is this the right way to close
            stream.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            //throw;
        }
        
        //TODO: Overwrite warning
        //TODO: save project  version
        //TODO: save in XML
        
    }

    private void Load()
    {
        //TODO: ask to save before Load (if it's not saved
        BeginNewGame();
        try
        {
            FileStream stream = File.OpenRead(_savePath);
            var reader = new BinaryReader(stream, Encoding.UTF8);
            int count = reader.ReadInt32();
            
            for (int i = 0; i < count; i++)
            {
                Vector3 p;
                p.x = reader.ReadSingle();
                p.y = reader.ReadSingle();
                p.z = reader.ReadSingle();
                
                Transform t = Instantiate(prefab);
                t.localPosition = p;
                _objects.Add(t);
            }
            
            //TODO: is this the right way to close
            //stream.Dispose();
            stream.Close();
            Debug.Log($"Load complete: {_savePath}");

        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            //throw;
        }
    }
}
