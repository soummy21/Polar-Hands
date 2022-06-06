using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addTrigger : MonoBehaviour
{
    Rigidbody[] rbs;
    public bool breakableEnemy;
    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var item in rbs)
        {
            getTrigger g = item.gameObject.AddComponent<getTrigger>();
            g.Main = gameObject;
            g.breakable = breakableEnemy;
            item.collisionDetectionMode = CollisionDetectionMode.Continuous;
            

        }
    }
}
