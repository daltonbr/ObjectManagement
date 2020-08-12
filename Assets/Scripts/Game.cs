using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform prefab;
    [SerializeField] private KeyCode createKey = KeyCode.C;
    
    private void Update ()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
    }
    
    private void CreateObject ()
    {
        Transform t = Instantiate(prefab);
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
    }
}
