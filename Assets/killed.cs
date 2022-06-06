using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
public class killed : MonoBehaviour
{
    bool objKilled;
    public PuppetMaster puppet;
    public bool headCut, bloodSplat;
    public void checkKill()
    {
        if (!objKilled)
        {
            objKilled = true;
            //rayfireRigid.Initialize();


            puppet.state = PuppetMaster.State.Dead;
            GetComponent<enemyMove>().detachWeapon();
            GetComponent<enemyMove>().changeToDeadMat();
            gameManager.instance.addInEnemyDiedAndCheck();
            Instantiate(Effects.instance.BloodPoolGrowing, new Vector3(transform.GetChild(0).transform.position.x, -1.66f, transform.GetChild(0).transform.position.z) , Quaternion.identity);
            
        }
    }

    public void CollideAndKillEnemy(GameObject g)
    {
        if (!objKilled)
        {
            objKilled = true;
            puppet.state = PuppetMaster.State.Dead;
            GetComponent<enemyMove>().detachWeapon();
            GetComponent<enemyMove>().changeToDeadMat();
            var rbs = GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
            {
                rb.AddForce((transform.position - g.transform.position) * 52f, ForceMode.Force);
            }
            gameManager.instance.addInEnemyDiedAndCheck();
        }
    }
  
}
