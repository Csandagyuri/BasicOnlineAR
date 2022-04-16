using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectData
{
    public float[] position;

    public ObjectData(GameObject gameObject)
    {
        position = new float[3];
        position[0] = gameObject.transform.position.x;
        position[1] = gameObject.transform.position.y;
        position[2] = gameObject.transform.position.z;
    }

}