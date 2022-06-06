using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RootMotion.Dynamics;
public class enemyMove : MonoBehaviour
{
    public enum eState { 
        idle,
        patrol,
        shock,
        Aim,
        shoot
    }
    Animator anim;
    public eState enemyState;
    public GameObject enemyWeapon;
    public SkinnedMeshRenderer body,eye,face;
    CharacterController cc;
    public float gravity = -9.81f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDist;
    public LayerMask groundMask;

    bool isGrounded;
    public bool isKilledAI;

    NavMeshAgent agent;

    private PuppetMaster puppet;
    private void Start()
    {
        puppet = GetComponentInChildren<PuppetMaster>();
        cc = GetComponent<CharacterController>();
        anim = transform.GetChild(2).GetComponent<Animator>();

        if (isKilledAI)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            GotoNextPoint();
            checkState();

        }

    }
    bool spotted;
    private void Update()
    {
        if (spotted)
        {
            if (groundCheck != null)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }
                velocity.y += gravity * Time.deltaTime;
                cc.Move(velocity * Time.deltaTime);
            }
        }
        else
        {
            if (isKilledAI)
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f && enemyState == eState.patrol)
                    GotoNextPoint();
            }
        }
    }
    bool lookAtPlayer;


    private void LateUpdate()
    {
        if (lookAtPlayer)
        {
            if(puppet.state != RootMotion.Dynamics.PuppetMaster.State.Dead)
                transform.LookAt(gameManager.instance.player.transform);
                   
        }
    }
    bool detach;
    public void detachWeapon() {
        if (!detach)
        {
            detach = true;
            enemyWeapon.gameObject.SetActive(false);
            //var rb = enemyWeapon.AddComponent<Rigidbody>();
            //rb.AddForce(Vector3.up * Random.Range(0f, 1f), ForceMode.Impulse);
        }
    }

    public void changeToDeadMat() {
        
        Material[] mats = new Material[] { gameManager.instance.enemyDeadMat, gameManager.instance.enemyDeadMat };
        body.materials = mats;
        eye.materials = mats;
        face.material = gameManager.instance.enemyDeadMat;
    }

    public void checkState() {
        switch (enemyState)
        {
            case eState.idle:
                anim.SetBool("idle", true);
                lookAtPlayer = true;
                break;
            case eState.patrol:
                anim.SetBool("patrol", true);
                cc.enabled = false;
                lookAtPlayer = false;
                break;
            case eState.shock:
                if (cc != null)
                {
                    cc.enabled = true;
                    Invoke(nameof(loook), 1f);

                }
                else {
                    Invoke(nameof(loook), 1f);
                }
                spotted = true;
                anim.SetBool("spot", true);
                anim.SetBool("patrol", false);
                if (agent != null)
                {
                    agent.enabled = false;

                }
                break;
            case eState.Aim:
                anim.SetBool("idle", true);
                break;

        }
    }

    void loook() {
        lookAtPlayer = true;

    }

    public Transform[] points;
    private int destPoint = 0;
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


   



}
