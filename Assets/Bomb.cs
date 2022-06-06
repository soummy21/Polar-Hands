using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;
public class Bomb : MonoBehaviour
{
    public GameObject blastImage;
    public float explosionRadius,explosionForce,explosionSize, explosionForceForBreakable;
    bool check,exploded;
    public float delayTime;
    [SerializeField] float invokeTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 7) {
            if (!check)
            {
                check = true;
                GetComponent<BoxCollider>().enabled = false;
                Invoke(nameof(Blast), delayTime);
                Invoke(nameof(InvokingSlowMo), delayTime + invokeTime);
                CustomTweener.Instance.StopButtonPointer(1);

            }
        }
    }


    ParticleSystem nukeBlastParticles;

    private void Start()
    {
        nukeBlastParticles = Instantiate(Effects.instance.NukeVerticalExplosionFire, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        nukeBlastParticles.transform.localScale = Vector3.one * explosionSize;
    }
    void Blast()
    {
        if (!exploded)
        {
            nukeBlastParticles.Play();
            gameManager.instance.PlayBlast();
            HapticController.instance.HapticHigh();
            exploded = true;
            //myMagnet.instance.HandAnim.SetTrigger("fear");
            Collider[] colToDead = Physics.OverlapSphere(transform.position, explosionRadius);
            
            //Breaks any breakable enemies in the radius of the bomb
            foreach (var item in colToDead)
            {
                //Yellow Enemy
                if (item.GetComponent<getTrigger>() != null)
                {
                    item.GetComponent<getTrigger>().callBreak();
                }

                if (item.GetComponent<objToBreak>() != null)
                {
                    if (item.GetComponent<RayfireRigid>() != null)
                    {
                        item.GetComponent<RayfireRigid>().Demolish();

                    }
                }

                //if (item.GetComponent<Bomb>() != null)
                //{
                //    item.GetComponent<Bomb>().Blast();
                //}
            }


            //Adding Effect and Force to objects in the radius of the blast
            Collider[] colToMove = Physics.OverlapSphere(transform.position, explosionRadius *2);

            foreach (var item in colToMove)
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    //var meshRenderer = rb.GetComponent<MeshRenderer>();
                    //var skinnedmeshRenderer = rb.GetComponent<SkinnedMeshRenderer>();
                    if (!rb.CompareTag("breakable"))
                    {
                            
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                    else
                    {
                        rb.AddExplosionForce(explosionForceForBreakable,transform.position, explosionRadius);
                    }
                    //GameObject gameObjectMR = Instantiate(gameManager.instance.DarkSmoke, Vector3.zero, Quaternion.identity);
                    //gameObjectMR.transform.parent = rb.GetComponent<MeshRenderer>().gameObject.transform;
                    //gameObjectMR.transform.localScale = Vector3.one;
                    //gameObjectMR.transform.localPosition = Vector3.zero;
                        //GameObject gameObjectSMR = Instantiate(gameManager.instance.DarkSmoke, Vector3.zero, Quaternion.identity);
                        //gameObjectSMR.transform.parent = rb.GetComponent<SkinnedMeshRenderer>().gameObject.transform;
                        //gameObjectSMR.transform.localScale = Vector3.one;
                        //gameObjectSMR.transform.localPosition = Vector3.zero;
                    }

                    
                }
            

            // Effects on Blast
            Invoke(nameof(setMark), .2f);
            CameraFunctions.instance.ShakeCameraOnCallGeneral();
            //myMagnet.instance.ActivateFear(); - needs to improve
            EnableBlastShader();
            //Disable the bomb quickly
            for (int i = 0; i < transform.childCount; i++)
            {
                if(i < 3)
                    transform.GetChild(i).gameObject.SetActive(false);
            }
            
            Destroy(gameObject, .5f);
        }
    }
    void setMark() {
        blastImage.SetActive(true);
    }
    void EnableBlastShader()
    {
        CustomTweener.Instance.ScaleBlast(transform.GetChild(2).transform);
    }


    void InvokingSlowMo()
    {
        gameManager.instance.ChangeTimeScaleOfGameSlow(0.1f, 0.25f);
    }
}
