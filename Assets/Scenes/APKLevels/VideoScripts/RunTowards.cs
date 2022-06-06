using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTowards : MonoBehaviour
{

    public bool isPolarised = false;
    public bool activated;
    bool canRun;
    [SerializeField] Transform finalLocation;
    [SerializeField] float speed = 8f;


    private void Start()
    {
        canRun = true;
    }
    //Activate Wave To walk towards you
    private void Update()
    {
        if (activated)
        {
            if (canRun)
            {
                GetComponentInChildren<Animator>().SetBool("Run", true);
                canRun = false;
                
            }
            transform.position = Vector3.MoveTowards(transform.position, finalLocation.position, Time.deltaTime * speed);
        }

    }



}
