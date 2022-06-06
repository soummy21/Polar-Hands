using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position + new Vector3(Random.Range(-1f, 1f),
            Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
    }
}
