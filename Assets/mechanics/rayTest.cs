using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;

public class rayTest : MonoBehaviour
{
    public float size;
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.relativeVelocity.magnitude > 3) {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            GameObject a = Effects.instance.spawnFromPool("CartoonyPunch", pos, rot);
            a.transform.localScale = new Vector3(size, size, size);
        }
    }

}
