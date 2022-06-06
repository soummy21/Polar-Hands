using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class CustomTweener : MonoBehaviour
{
    #region Variables
    public static CustomTweener Instance;

    [Header("Compass")]
    [SerializeField] private RectTransform []arrowRect;

    [SerializeField] float moveInTime;
    [SerializeField] float arrowFadeInTime;

    [Header("Floating Arrow")]
    [SerializeField] MeshRenderer []floatingGround;
    [SerializeField] MeshRenderer []floatingPointer;
    [SerializeField] Transform []trigPos;
    [SerializeField] Transform player;
    [SerializeField] float floatingIn;
    [SerializeField] float floatingOut;
    [SerializeField] float rotationFactor;
    [SerializeField] float verticleMovementFactor;
    [SerializeField] float threshold;
    float time = 0f;
    bool isTriggered = false;

    [Header("Fade Fog")]
    [SerializeField] private MeshRenderer [] fogRenderer;
    [SerializeField] float fadeDuration;

    [Header("Clouds")]
    [SerializeField] private Material[] cloudMats;


    [Header("Scale Blast")]
    [SerializeField] private float finalScale;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;


    [Header("Tutorial Pointer")]
    [SerializeField] private Transform[] buttonPointer;
    MeshRenderer[] buttonMeshRenderers;

    [SerializeField] float moveFactor;
    [SerializeField] float fadeFactor;
    [SerializeField] float blinkFactor;
    Vector3[] op;
    bool[] removeButtonPointer = { true, true };

    #endregion

    [Header("Word Generator")]
    [SerializeField] Image mainImage;
    [SerializeField] Sprite[] words;
    [SerializeField] float rectTweenTime;
    [SerializeField] float maxRotationFactor;
    [SerializeField] float scaleValue;
    RectTransform imageRect;
    Vector3 currentPosition;
    Quaternion currentRotation;

    public bool multipleTutorials = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        
        imageRect = mainImage.GetComponent<RectTransform>();
        //Current Position 
        currentPosition = imageRect.position;
        currentRotation = imageRect.rotation;


        Initializations();
        InitilizeMeshRenderers();
    }

    private void Update()
    {
        MoveUpAndDown();

        for (int i = 0; i< buttonPointer.Length; i++)
        {
            if(!removeButtonPointer[i])
            {
                if(i == 0)
                {
                    MoveButtonPointer(Vector3.up, 0);
                }
                else
                {
                    MoveButtonPointer(Vector3.right, 1);
                }
            }
        }


        
    }
    void Initializations()
    {
        FadeArrow(1f,0.8f);
        FadeInCloud();
        
    }

    #region Tweening
    public void ArrowTween(float rotFact)
    {
        arrowRect[0].DORotate(Vector3.forward * rotFact, moveInTime);
        arrowRect[1].DORotate(Vector3.forward * rotFact, moveInTime);
    }

    public void FadeArrow(float fade,float arrowFadeInTime)
    {
        for (int i = 0; i < arrowRect.Length; i++)
        {
            arrowRect[i].GetComponent<Image>().DOFade(fade, arrowFadeInTime);
        }
    }

    public void FadeFog()
    {
        if (gameManager.instance.atWave < gameManager.instance.perPuzzleEnemies.Length + 1) 
            fogRenderer[gameManager.instance.atWave].material.DOFade(0f, fadeDuration).OnComplete(()=> ActivateOnFogFade());
        FadeOutClouds();
    }

    void ActivateOnFogFade()
    {
        StartButtonPointer();
        PolarizedObjects.instance.ActivateOutlineByWave();
        

    }
    public void ScaleBlast(Transform blastEffectTransform)
    {
        blastEffectTransform.DOScale(finalScale, fadeInTime).OnComplete(() => FadeScaleBlast(blastEffectTransform));
    }
    
    void FadeScaleBlast(Transform blastEffectTransform)
    {
        blastEffectTransform.DOScale(0f, fadeOutTime).SetEase(Ease.OutQuint);
        blastEffectTransform.GetComponent<MeshRenderer>().material.DOFloat(0f,"Alpha",fadeOutTime).SetEase(Ease.OutQuint);
    }
    public void FadeInFloatingArrow()
    {
        if(gameManager.instance.atWave < 1)
        {
            floatingGround[gameManager.instance.atWave].material.DOFade(1f, floatingIn);
            floatingPointer[gameManager.instance.atWave].material.DOFade(1f, floatingIn);
        }

    }
    public void FadeOutFloatingArrow()
    {
        if(gameManager.instance.atWave < 1)
        {
            floatingGround[gameManager.instance.atWave].material.DOFade(0f, floatingOut).OnComplete(() => turnOnTrigger());
            floatingPointer[gameManager.instance.atWave].material.DOFade(0f, floatingOut);
        }

    }

    void turnOnTrigger()
    {
        isTriggered = false;
    }
    public void MoveUpAndDown()
    {
        time += Time.deltaTime;
        if (gameManager.instance.atWave < gameManager.instance.perPuzzleEnemies.Length)
        {
            Vector3 newPostion = transform.localPosition + Vector3.up * Mathf.Sin(time) * verticleMovementFactor;
            floatingPointer[gameManager.instance.atWave].transform.localPosition = newPostion;
            floatingPointer[gameManager.instance.atWave].transform.Rotate(Vector3.up * Time.deltaTime * rotationFactor);
            if (!isTriggered)
            {
                float dis = Vector3.Distance(trigPos[gameManager.instance.atWave].position, player.position);
                if (dis < threshold)
                {
                    FadeOutFloatingArrow();
                    isTriggered = true;
                }
            }
        }


       
    }

    void FadeInCloud()
    {
        for (int i = 0; i < cloudMats.Length; i++)
        {
            cloudMats[i].DOFade(1f, fadeDuration);
        }
    }

    void FadeOutClouds()
    {
        for (int i = 0; i < cloudMats.Length; i++)
        {
            cloudMats[i].DOFade(0f, fadeDuration + 0.5f);
        }
    }
    
    void InitilizeMeshRenderers()
    {
        
        buttonMeshRenderers = new MeshRenderer[buttonPointer.Length];
        op = new Vector3[buttonPointer.Length];
        for (int i = 0; i < buttonMeshRenderers.Length; i++)
        {
            op[i] = buttonPointer[i].localPosition;
            buttonMeshRenderers[i] = buttonPointer[i].GetComponent<MeshRenderer>();
        }
        
    }
    public void MoveButtonPointer(Vector3 direction, int i)
    {
        time += Time.deltaTime;

        Vector3 newPostion = op[i] + direction * Mathf.Sin(time) * moveFactor;
        buttonPointer[i].transform.localPosition = newPostion;
        Color newColor = buttonMeshRenderers[i].material.color;
        newColor.a = Mathf.Abs(Mathf.Sin(time / blinkFactor) * fadeFactor);
        buttonMeshRenderers[i].material.color = newColor;

    }

    public void StopButtonPointer(int i)
    {
        buttonPointer[i].GetComponent<MeshRenderer>().material.DOFade(0f, 0.4f);
        removeButtonPointer[i] = true;    
    }
    void StartButtonPointer()
    {    
        for (int i = 0; i < buttonPointer.Length; i++)
        {
            removeButtonPointer[i] = false;
     
        }
    }
    #endregion


    #region WordGenerator
    public void TweenTheIncomingText()
    {
        var currentWord = words[Random.Range(0, words.Length)];
        mainImage.sprite = currentWord;
        //Random Position and Rotation
        Vector3 rot = imageRect.eulerAngles + Vector3.forward * Random.Range(-maxRotationFactor, maxRotationFactor);
        imageRect.eulerAngles = rot;
        Vector3 pos = imageRect.position + Vector3.up * Random.Range(-40f, 40f) + Vector3.right * Random.Range(-40f, 40f);

        //Tween
        imageRect.DOScale(Vector3.one * scaleValue, rectTweenTime).SetEase(Ease.OutElastic).OnComplete(()=> Invoke(nameof(ScaleBack),1.5f));
        imageRect.DOMove(pos, rectTweenTime).SetEase(Ease.OutExpo);

    }

    void ScaleBack()
    {
        imageRect.DOScale(Vector3.zero, rectTweenTime - 0.3f).SetEase(Ease.OutBack);
    }
    public void ResetText()
    {
        
        imageRect.position = currentPosition;
        imageRect.rotation = currentRotation;
        
    }

    #endregion

}
