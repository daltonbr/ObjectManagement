using UnityEngine;

public class Shape : PersistableObject
{
    private int _shapeID = int.MinValue;

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
    
}
