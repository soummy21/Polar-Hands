using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HapticController : MonoBehaviour
{
    //public InputField inputField;
    [SerializeField] private HapticTypes typeSuccess;
    [SerializeField] private HapticTypes typeFailure;
    [SerializeField] private HapticTypes typeMedium;
    [SerializeField] private HapticTypes typeHigh;
    [SerializeField] private HapticTypes typeLow;


    public static HapticController instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerPrefs.SetInt("vib", PlayerPrefs.GetInt("vib",1));
    }
    public void HapticSuccess()
    {
        HapticForType(typeSuccess);
    }
    public void HapticFailure()
    {
        HapticForType(typeFailure);
    }
    public void HapticMedium()
    {
        HapticForType(typeMedium);
    }

    public void HapticHigh()
    {
        HapticForType(typeHigh);
    }

    public void HapticLow()
    {
        HapticForType(typeLow);
    }

    private void HapticForType(HapticTypes type)
    {
        //Debug.Log(DLConstant.hapticOn+"Haptic " + type);
        //if (!DLConstant.hapticOn) return;
        if(PlayerPrefs.GetInt("vib") == 1)
            MMVibrationManager.Haptic(type, false, true, this);
    }
}
