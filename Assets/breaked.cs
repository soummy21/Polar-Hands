using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;
using RootMotion.Dynamics;
public class breaked : MonoBehaviour
{
    public bool objBreak;
    public RayfireRigid rayfireRigid;
    public PuppetMaster puppet;
    [SerializeField] GameObject lightningTrail;
    [SerializeField] Material deadMat;
    int i = 0;
    public void checkBreak() {
        if (!objBreak) {
            objBreak = true;
            //rayfireRigid.Initialize();
            rayfireRigid.Demolish();
            if (GetComponent<enemyMove>() != null)
            {
                lightningTrail.SetActive(false);
                GetComponent<enemyMove>().detachWeapon();
                puppet.state = PuppetMaster.State.Dead;
                gameManager.instance.addInEnemyDiedAndCheck();   
                
            }
        }
    }

    public void KillTheBreakedEnemy()
    {
        if (!objBreak)
        {
            objBreak = true;
            //rayfireRigid.Initialize();
            //rayfireRigid.Demolish();
            if (GetComponent<enemyMove>() != null)
            {
                lightningTrail.SetActive(false);
                GetComponent<enemyMove>().detachWeapon();
                //puppet.state = PuppetMaster.State.Frozen;
                gameManager.instance.addInEnemyDiedAndCheck();
                //ChangeMaterialOnBreak();
                Invoke(nameof(FreezeConstraints),0.5f);

            }
        }
    }

    void SlowPuppetStop()
    {
       
    }
    void FreezeConstraints()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach(var rb in rbs)
        {
            float random = Random.Range(0f, 1f);
            if(random > 0.15f)
                rb.isKinematic = true;
        }
    }

}



