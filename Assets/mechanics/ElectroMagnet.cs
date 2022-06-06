using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroMagnet : MonoBehaviour
{
    [Range(0.0f, 2000.0f)]
    public float MagnetForce;

    Magnet[] m_magnets;

    [SerializeField] bool isRight = true;
    bool hitOnce = false;

    void OnValidate()
    {
        if (m_magnets == null)
            return;

        foreach (var m in m_magnets)
        {
            m.MagnetForce = MagnetForce;
        }
    }


    public void setChildMagnetVal()
    {
        if (m_magnets == null)
            return;

        foreach (var m in m_magnets)
        {
            m.MagnetForce = MagnetForce;
        }
    }
    void Start()
    {
        m_magnets = GetComponentsInChildren<Magnet>();
        setChildMagnetVal();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(isRight)
        {
            if (collision.gameObject.CompareTag("demonish"))
            {
                if (collision.gameObject.GetComponentInParent<breaked>())
                {
                    
                    if (!collision.gameObject.GetComponentInParent<breaked>().objBreak)
                    {
                        collision.gameObject.GetComponentInParent<breaked>().checkBreak();

                    }

                    
                }
            }
        }


    }

}
