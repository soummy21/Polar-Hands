using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameAnalyticsSDK;
using _Scripts.Analytics;

public class health : MonoBehaviour
{
    [SerializeField] Image hurtRedScreen;
    [SerializeField] Slider healthSlider;
    [SerializeField] float Health = 100f;

    private void Start()
    {
        healthSlider.maxValue = Health;
    }

    public void DecreaseHealth(float h)
    {
        //LeanTween.value(Health,Health + h, .5f).setOnUpdate((a) => {

        //    HealthImageBorder.color = new Color32((byte)255, (byte)0, (byte)0, (byte)a);


        //});
        Health -= h;
        MakeScreenRed();
        if (Health <= 0f) {
            Health = 0f;
            
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level" + (gameManager.instance.sceneIndex).ToString());
            gameManager.instance.setTryingLevelInt();
            LionsAnalyticsController.Instance.OnLevelFail(gameManager.instance.TryingInOfThisLevel);
            gameManager.instance.currentGameState = GameStates.ended;
            gameManager.instance.gameResult = GameResult.fail;
            GameUIHandler.instance.Invoke("EnableEndScreen", 1f);
            HapticController.instance.HapticFailure();
        }

        healthSlider.DOValue(Health, 0.3f);
    }
    public void HealthReset() {
        //LeanTween.value(Health, 0, .5f).setOnUpdate((a) => {

        //    HealthImageBorder.color = new Color32((byte)255, (byte)0, (byte)0, (byte)a);

        //});
        Health = 100f;
        healthSlider.DOValue(Health, 0.7f);
    }
    void MakeScreenRed()
    {
        hurtRedScreen.gameObject.SetActive(true);
        hurtRedScreen.DOFade(0.5f, 0.1f).OnComplete(() => 
        hurtRedScreen.DOFade(0f, 0.2f).OnComplete(()=> hurtRedScreen.gameObject.SetActive(false)));
    }

    public void TweenHealthBar()
    {
        healthSlider.GetComponent<RectTransform>().DOMoveX(-450f, 0.6f);
    }

    

}
