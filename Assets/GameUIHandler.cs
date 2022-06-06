using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using _Scripts.Analytics;
public class GameUIHandler : MonoBehaviour
{
    public static GameUIHandler instance;

    //Game Sucess Stuff
    [Header("Result UI")]
    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject levelSuccess;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject rewardAdButton;
    [SerializeField] GameObject levelFail;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject[] panelStuff;
    [SerializeField] GameObject coinImage;
    [SerializeField] GameObject flare;

    //Start Screen
    [Header("Start Screen")]
    [SerializeField] GameObject settings;
    [SerializeField] GameObject removeAds;
    [SerializeField] GameObject currency;
    [SerializeField] GameObject hapticButton;

    //Game Failed Stuff
    [Header("Start Screen")]

    //HUD Texts
    [SerializeField] Text coinText;
    [SerializeField] TextMeshProUGUI levelText;

    float fulldistance;
    Image endPanelImage;

   


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("score"))
        {
            int a = PlayerPrefs.GetInt("score");
            gameManager.instance.score = PlayerPrefs.GetInt("score");

            coinText.text = a.ToString();

        }
        else
        {
            PlayerPrefs.SetInt("score", 0);
            gameManager.instance.score = 0;

            coinText.text = PlayerPrefs.GetInt("score").ToString();

        }

        LevelText();
        endPanelImage = endPanel.GetComponent<Image>();
        
    }

    private void Update()
    {
        
    }

    void TweenCurrency()
    {
        currency.GetComponent<RectTransform>().DOLocalMoveX(450f, 0.85f);
        levelText.GetComponent<RectTransform>().DOLocalMoveY(1000f, 0.8f);
    }

    public void EnableEndScreen()
    {
        
        TweenCurrency();
        CustomTweener.Instance.FadeArrow(0f, 0.35f);
        if (gameManager.instance.gameResult == GameResult.success)
        {  
            nextButton.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBounce);
            coinImage.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutExpo);
            rewardAdButton.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBounce);
            flare.transform.DOScale(Vector3.one * 2f, 0.8f).SetEase(Ease.OutBounce);
            panelStuff[0].GetComponent<RectTransform>().DOLocalMoveY(1046f, 0.8f).SetEase(Ease.OutCirc);
            panelStuff[1].GetComponent<RectTransform>().DOLocalMoveY(-1037f, 0.8f).SetEase(Ease.OutCirc);
        }
        else if(gameManager.instance.gameResult == GameResult.fail)
        {
            endPanelImage.color = Color.red;
            restartButton.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBounce);
            levelFail.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBounce);
            panelStuff[2].GetComponent<RectTransform>().DOLocalMoveY(1046f, 0.8f).SetEase(Ease.OutCirc);
            panelStuff[3].GetComponent<RectTransform>().DOLocalMoveY(-1037f, 0.8f).SetEase(Ease.OutCirc);
        }
        endPanelImage.DOFade(0.6f, 0.6f);
    }

    public void ActivateStartScreen()
    {
        //currency.SetActive(true);
        settings.SetActive(true);
    }

    public void DeactivateStartScreen()
    {
        settings.GetComponent<RectTransform>().DOMoveX(-350f, 0.7f);
        removeAds.GetComponent<RectTransform>().DOMoveX(-350f, 0.7f);
        hapticButton.GetComponent<RectTransform>().DOMoveX(-350f, 0.7f);

    }

    public void NextButton(int i)
    {
        PlayerPrefs.SetInt("onLevel", i);
        int newLevelNo = GetLevelNo() + 1;
        SetLevelNo(newLevelNo);
        SceneManager.LoadScene(i);

    }
    public void NextRandom()
    {
        int random = Random.Range(6, 10);
        PlayerPrefs.SetInt("onLevel", random);
        int newLevelNo = GetLevelNo() + 1;
        SetLevelNo(newLevelNo);
        SceneManager.LoadScene(random);
    }
    public void RedoButton()
    {
        gameManager.instance.setTryingLevelInt();
        LionsAnalyticsController.Instance.OnLevelRestart(gameManager.instance.TryingInOfThisLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 

    public void HapticButton()
    {
        if (PlayerPrefs.GetInt("vib") == 0)
            PlayerPrefs.SetInt("vib", 1);
        else
            PlayerPrefs.SetInt("vib", 0);

        Debug.Log(PlayerPrefs.GetInt("vib"));

    }
    public void ChangeCoinText(int coinVal)
    {
        coinText.text = coinVal.ToString();
    }

    void LevelText()
    {
        levelText.text = "Level " + GetLevelNo().ToString();
    }


    #region PlayerPrefsForLevel

    int GetLevelNo()
    {
        return PlayerPrefs.GetInt("levelNo", 1);
    }

    void SetLevelNo(int newLevel)
    {
        PlayerPrefs.SetInt("levelNo", newLevel);
    }

    #endregion

}
