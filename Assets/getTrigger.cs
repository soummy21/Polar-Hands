using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getTrigger : MonoBehaviour
{
    public GameObject Main;
    public bool breakable;
    List<GameObject> bloodSpot = new List<GameObject>();
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("demonish")) {

            if (breakable)
            {

                if (Main.GetComponent<breaked>() != null)
                {
                   // Main.GetComponent<breaked>().checkBreak();
                }
            }
            else
            {
                if (Main.GetComponent<killed>() != null)
                {
                    
                    if (bloodSpot.Count < 1)
                    {
                        if (collision.collider.GetComponent<objThatKill>() != null)
                        {

                            if (collision.collider.GetComponent<objThatKill>().thisObj == objThatKill.obj.spike)
                            {

                                CustomTweener.Instance.StopButtonPointer(1);

                                if (!Main.GetComponent<killed>().bloodSplat)
                                {
                                    ContactPoint contact = collision.contacts[0];
                                    Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                                    Vector3 pos = contact.point;

                                    GameObject a =  Effects.instance.spawnFromPool("bloodSplat", pos, rot);
                                    Debug.Log(a);
                                    a.transform.localScale *= 3f;
                                    bloodSpot.Add(a);
                                    Main.GetComponent<killed>().checkKill();
                                    Main.GetComponent<killed>().bloodSplat = true;

                                }
                                

                            }
                            if (collision.collider.GetComponent<objThatKill>().thisObj == objThatKill.obj.saw)
                            {
                                if (!Main.GetComponent<killed>().headCut)
                                {
                                    Main.GetComponent<killed>().headCut = true;
                                    ContactPoint contact = collision.contacts[0];
                                    Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                                    Vector3 pos = contact.point;

                                    GameObject a = Instantiate(Effects.instance.BloodSplatDirectionalAnimated, pos, rot);
                                    bloodSpot.Add(a);
                                    Main.SetActive(false);
                                    Instantiate(gameManager.instance.cutModel, Main.transform.position, Main.transform.rotation);
                                    
                                    gameManager.instance.addInEnemyDiedAndCheck();
                                }
                            }

                            if (collision.collider.GetComponent<objThatKill>().thisObj == objThatKill.obj.cylinder)
                            {
                                if (Main.GetComponent<killed>())
                                {
                                    Main.GetComponent<killed>().CollideAndKillEnemy(collision.collider.gameObject);

                                }
                            }
                            if (collision.collider.GetComponent<objThatKill>().thisObj == objThatKill.obj.crane)
                            {
                                if (Main.GetComponent<killed>())
                                {
                                    Main.GetComponent<killed>().checkKill();

                                }
                            }


                        }
                    }
                    
                }
            }
            
           
        }
    }

    public void callBreak() {
        if (Main.GetComponent<breaked>() != null)
        {
            Main.GetComponent<breaked>().checkBreak();
        }
    }
}
