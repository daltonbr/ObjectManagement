using UnityEngine;

[CreateAssetMenu(fileName = "ShapeFactory", menuName = "ShapeFactory", order = 0)]
public class ShapeFactory : ScriptableObject
{
    [SerializeField] private Shape[] prefabs;
    [SerializeField] private Material[] materials;
    
    public Shape Get(int shapeId = 0, int materialID = 0)
    {
        Shape instance = Instantiate(prefabs[shapeId]);
        instance.ShapeID = shapeId;
        instance.SetMaterial(materials[materialID], materialID);
        return instance;
    }
    
    public Shape GetRandom()
    {
        return Get(
            Random.Range(0, prefabs.Length),
            Random.Range(0, materials.Length)
            );
    }
}
