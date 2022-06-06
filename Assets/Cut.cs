using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour
{
    [SerializeField] float upForce;
    [SerializeField] float rotForce;
    private void OnEnable()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * upForce, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(Vector3.forward * Random.Range(-rotForce, rotForce), ForceMode.Impulse);
    }
}
