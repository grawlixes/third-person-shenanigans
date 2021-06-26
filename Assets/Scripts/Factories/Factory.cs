using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory
{
    public virtual GameObject GetObject(string name, Vector3 initialWorldPosition) {
        GameObject newObject = new GameObject(name);
        newObject.SetActive(false);

        Transform transform = newObject.transform;
        transform.position = initialWorldPosition;

        return newObject;
    }
}