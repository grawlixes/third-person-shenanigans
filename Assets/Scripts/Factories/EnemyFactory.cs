using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : Factory
{
    public override GameObject GetObject(string name, Vector3 initialPosition)
    {
        GameObject newEnemy = base.GetObject(name, initialPosition);

        return new GameObject();
    }
}