using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breath : MonoBehaviour
{
    Vector3 startPos;
    public float amplitude = 10f;
    public float period = 5f;

    protected void Start()
    {
        startPos = transform.position;
    }

    protected void Update()
    {
        float theta = Time.timeSinceLevelLoad / period;
        float distance = amplitude * Mathf.Sin(theta);
        transform.position = new Vector3(transform.position.x, startPos.y + 1 * distance, transform.position.z);
    }
}
