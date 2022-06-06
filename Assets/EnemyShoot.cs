using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject muzzelFlash;
    breaked breakedEnemy;
    killed killedEnemy;
    public void SHooot() 
    {
        if(gameManager.instance.currentGameState != GameStates.ended)
            Invoke(nameof(sho), .5f);
 
    }

    private void Start()
    {
        breakedEnemy = GetComponent<breaked>();
        killedEnemy = GetComponent<killed>();
    }
    void sho() {
        if(breakedEnemy != null)
        {
            if(breakedEnemy.puppet.state == RootMotion.Dynamics.PuppetMaster.State.Alive)
            {
                GameObject a = Effects.instance.spawnFromPool("muzzel", firePoint.position, firePoint.rotation);
                a.transform.localScale = Vector3.one * 1.65f;
                a.transform.parent = firePoint.transform;
                a.transform.localPosition = Vector3.zero;
                gameManager.instance.DecreasePlayerHealth();
            }
        }
        else if(killedEnemy != null)
        {
            if (killedEnemy.puppet.state == RootMotion.Dynamics.PuppetMaster.State.Alive)
            {
                GameObject a = Effects.instance.spawnFromPool("muzzel", firePoint.position, firePoint.rotation);
                a.transform.localScale = Vector3.one * 1.65f;
                a.transform.parent = firePoint.transform;
                a.transform.localPosition = Vector3.zero;
                gameManager.instance.DecreasePlayerHealth();

            }
        }
       
        
        
    }

}
