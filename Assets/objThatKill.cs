using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objThatKill : MonoBehaviour
{
    public enum obj { 
        spike,
        saw,
        cylinder,
        crane
    }
    public obj thisObj;


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(gameObject.name + collision.gameObject.name);

        if(collision.gameObject.CompareTag("demonish"))
        {

            if(collision.gameObject.GetComponentInParent<breaked>())
            {
                CustomTweener.Instance.StopButtonPointer(1);
                collision.gameObject.GetComponentInParent<breaked>().KillTheBreakedEnemy();
            }
            
        }
    }
}
