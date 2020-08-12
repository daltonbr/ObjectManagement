using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform prefab;
    [SerializeField] private KeyCode createKey = KeyCode.C;
    [SerializeField] private KeyCode newGameKey = KeyCode.N;
    
    private List<Transform> _objects;
    
    private void Awake ()
    {
        _objects = new List<Transform>();
    }
    
    private void Update ()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKey(newGameKey))
        {
            BeginNewGame();
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

    private void CreateObject ()
    {
        Transform t = Instantiate(prefab);
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        _objects.Add(t);
    }
}
