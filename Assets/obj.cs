using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class obj : MonoBehaviour
{
    public enum objN { 
        spikeButton,
        doorButton,
        sawButton,
        fenceButton,
        craneButton,
        rollingCylinderButton

    }

    public objN objectType;

    [Header("button")]
    public float buttonTriggerForce;

    public float buttonGoDownYFloat, buttonGoDownInTime;

    [Header("Spike Obj")]
    public GameObject buttonSpikeObj;
    public float spikeGoUpYFloat;
    [Header("Door Obj")]
    public GameObject[] doors;
    public int tutorialButtonNo;
    

    [Header("Fence Obj")]
    public GameObject[] fence;
    
    [Header("Crane Obj")]
    public GameObject[] cranes;

    [Header("rolling Cylinder Obj")] 
    public GameObject Holder;

    public float HolderYVal;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("demonish")) {
            switch (objectType) {
                case objN.spikeButton:
                    if (collision.relativeVelocity.magnitude > buttonTriggerForce)
                    {
                        //buttonSpikeObj.Activate();
                        ButtonGoDown();
                        SpikeGoUp();
                    }
                    break;
                case objN.doorButton:
                    if (collision.relativeVelocity.magnitude > buttonTriggerForce)
                    {
                        ButtonGoDown();
                        openDoor();
                    }
                    break;
                case objN.sawButton:
                    if (collision.relativeVelocity.magnitude > buttonTriggerForce)
                    {
                        ButtonGoDown();
                        openSaw();
                    }
                    break;

                case objN.fenceButton:
                    if(collision.relativeVelocity.magnitude > buttonTriggerForce)
                    {
                        ButtonGoDown();
                        MakeFence();
                      
                    }
                    break;
                case objN.craneButton:
                    if (collision.relativeVelocity.magnitude > buttonTriggerForce)
                    {
                        ButtonGoDown();
                        crane();

                    }
                    break;
                case objN.rollingCylinderButton:
                    if (collision.relativeVelocity.magnitude > buttonTriggerForce)
                    {
                        ButtonGoDown();
                        removeRollingCylinderHolder();

                    }
                    break;
            }
            
        }
    }

    private void removeRollingCylinderHolder()
    {
        Holder.transform.DOMoveY(HolderYVal, .3f);
    }

    private void crane()
    {
        foreach (var item in cranes)
        {
            item.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    [Header("Saw properties")]
    public GameObject sawHandle;
    public float defaultPosX, sawGoRightXFloat;

    private void openSaw()
    {
        sawHandle.transform.LeanMoveLocalX(sawGoRightXFloat, .6f).setOnComplete(closeSaw);

    }
    void closeSaw() {
        sawHandle.transform.LeanMoveLocalX(defaultPosX, 0.8f);
        ButtonGoUP();

    }
    private void openDoor()
    {
        foreach (var item in doors)
        {
             item.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void ButtonGoDown() {

        CustomTweener.Instance.StopButtonPointer(tutorialButtonNo);
        transform.LeanMoveLocalY(buttonGoDownYFloat, buttonGoDownInTime);
    }
    void ButtonGoUP()
    {
        transform.LeanMoveLocalY(1.22f, .5f);
    }
    void SpikeGoUp() {
        buttonSpikeObj.LeanMoveLocalY(spikeGoUpYFloat, .35f);

    }

    void setFalseAllCol() {

        BoxCollider[] bx = buttonSpikeObj.GetComponentsInChildren<BoxCollider>();
        foreach (var item in bx)
        {
            item.enabled = false;
        }
    }

    void MakeFence()
    {
        fence[0].GetComponent<Rigidbody>().isKinematic = false;
        fence[0].GetComponent<Rigidbody>().AddForce(Vector3.right * 9f, ForceMode.Impulse);
    }
}
