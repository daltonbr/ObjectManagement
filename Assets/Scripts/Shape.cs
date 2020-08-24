using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Shape : PersistableObject
{
    private int _shapeID = int.MinValue;
    private Color _color;
    private MeshRenderer _meshRenderer;
    private static readonly int ColorPropertyID = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock _sharedPropertyBlock;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public int ShapeID
    {
        //  _shapeID has to be set exactly once per instance,
        // when it is instantiated by the factory.
        // Setting it again after that would be a mistake.
        get => _shapeID;
        set
        {
            if (_shapeID == int.MinValue && value != int.MinValue)
            {
                _shapeID = value;
            }
            else
            {
                Debug.LogError("[Shape] Not allowed to change _shapeId.");
            }
        }
    }
    
    public int MaterialID { get; private set; }
    
    public void SetMaterial(Material material, int materialID)
    {
        _meshRenderer.material = material;
        MaterialID = materialID;
    }

    public void SetColor(Color color)
    {
        _color = color;
        //var propertyBlock = new MaterialPropertyBlock();
        if (_sharedPropertyBlock == null)
        {
            _sharedPropertyBlock = new MaterialPropertyBlock();
        }
        _sharedPropertyBlock.SetColor(ColorPropertyID, color);
        _meshRenderer.SetPropertyBlock(_sharedPropertyBlock);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(_color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.ReadColor());
    }
}
