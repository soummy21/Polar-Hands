using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;
public class objToBreak : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RayfireRigid r = GetComponent<RayfireRigid>();
        
        // "Rayfirerigid component already present" console line optimization
        if (r == null)
        {
            r = gameObject.AddComponent<RayfireRigid>();
        }
        

        InitializeRayfire(r);

       
    }

    void InitializeRayfire(RayfireRigid r)
    {
        //r.demolitionType = DemolitionType.Runtime;
        //r.simulationType = SimType.Sleeping;
        //Fading Settings
        r.fading.lifeTime = 0.6f;
        r.fading.fadeType = FadeType.ScaleDown;
        r.fading.fadeTime = 0.4f;
        //r.Initialize();
    }
}
