using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
    [SerializeField] private ShapeFactory shapeFactory;
    [SerializeField] private KeyCode createKey = KeyCode.C;
    [SerializeField] private KeyCode newGameKey = KeyCode.N;
    [SerializeField] private KeyCode saveKey = KeyCode.S;
    [SerializeField] private KeyCode loadKey = KeyCode.L;
    
    private const int SaveVersion = 1;
    
    private List<Shape> _shapes;
    public PersistentStorage storage;
    
    private void Awake()
    {
        _shapes = new List<Shape>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateShape();
        }
        else if (Input.GetKey(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this, SaveVersion);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);            
        }
    }
    
    private void BeginNewGame()
    {
        foreach (Shape o in _shapes)
        {
            Destroy(o.gameObject);
        }
        _shapes.Clear();
    }

    private void CreateShape()
    {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        instance.SetColor(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f));
        _shapes.Add(instance);
    }
    
    public override void Save(GameDataWriter writer)
    {
        writer.Write(_shapes.Count);
        for (int i = 0; i < _shapes.Count; i++)
        {
            writer.Write(_shapes[i].ShapeID);
            writer.Write(_shapes[i].MaterialID);
            _shapes[i].Save(writer);
        }
    }
    
    public override void Load (GameDataReader reader)
    {
        int version = reader.Version;
        if (version > SaveVersion)
        {
            Debug.LogError($"[Game] Unsupported future save version {version}");
            return;
        }
        
        int count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            int shapeID = reader.ReadInt();
            int materialID = reader.ReadInt();
            Shape instance = shapeFactory.Get(shapeID, materialID);
            instance.Load(reader);
            _shapes.Add(instance);
        }
    }
    
}
