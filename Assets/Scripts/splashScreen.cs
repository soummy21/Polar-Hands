using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using _Scripts.Analytics;

public class splashScreen : MonoBehaviour
{
    private void Awake()
    {
        
    }
    public Slider slider;
    public LeanTweenType lean;
    public float loadTime;
    private void Start()
    {
        // LionsAnalyticsController.Instance.OnGameStart();
        //startLoading();
        Invoke(nameof(LoadSaveScene), 3f);
    }
    [ContextMenu("Load")]
    void startLoading() {
        LeanTween.value(0, 1, loadTime).setOnUpdate((a) => slider.value = a).setEase(lean).setOnComplete(LoadSaveScene);
    }

    void LoadSaveScene() {
        if (PlayerPrefs.HasKey("onLevel"))
        {
            int a = PlayerPrefs.GetInt("onLevel");
            SceneManager.LoadScene(a);
        }
        else {
            PlayerPrefs.SetInt("onLevel", 1) ;
            SceneManager.LoadScene(1);
        }
    } 
}
