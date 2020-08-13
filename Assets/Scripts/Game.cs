using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
    [SerializeField] private PersistableObject prefab;
    [SerializeField] private KeyCode createKey = KeyCode.C;
    [SerializeField] private KeyCode newGameKey = KeyCode.N;
    [SerializeField] private KeyCode saveKey = KeyCode.S;
    [SerializeField] private KeyCode loadKey = KeyCode.L;
    
    private List<PersistableObject> _objects;
    public PersistentStorage storage;
    
    private void Awake()
    {
        _objects = new List<PersistableObject>();
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
            storage.Save(this);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);            
        }
    }
    
    private void BeginNewGame()
    {
        foreach (PersistableObject o in _objects)
        {
            Destroy(o.gameObject);
        }
        _objects.Clear();
    }

    private void CreateObject()
    {
        PersistableObject o = Instantiate(prefab);
        Transform t = o.transform;
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        _objects.Add(o);
    }
    
    public override void Save(GameDataWriter writer)
    {
        writer.Write(_objects.Count);
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].Save(writer);
        }
    }
    
    public override void Load (GameDataReader reader)
    {
        int count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            PersistableObject o = Instantiate(prefab);
            o.Load(reader);
            _objects.Add(o);
        }
    }
    
}
