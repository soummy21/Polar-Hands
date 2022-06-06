using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class myMagnet : MonoBehaviour
{
    public enum swipe {
        left, right,
        up, down, none
    }

    [Header("Swipe Text")]
    [SerializeField] GameObject swipeText;
    [SerializeField] GameObject polarity;
    [SerializeField] GameObject []trails;

    public static myMagnet instance;
    private void Awake()
    {
        instance = this;
    }
    public ElectroMagnet leftElectroMagnet, rigthElectroMagnet, upElectroMagnet, downElectroMagnet;

    public Animator HandAnim;

    public List<GameObject> magneticObj = new List<GameObject>();
    public List<GameObject> magneticWhichRotateObj = new List<GameObject>();
    public List<GameObject> magneticSawObj = new List<GameObject>();
    public List<GameObject> currentBreakedAI = new List<GameObject>();

    bool canSwipe = true;

    public GameObject[] trailEffects;
    bool downSwipe, upSwipe,leftSwipe,rightSwipe;
    public swipe currentSwipe;

    private void Start()
    {
        currentSwipe = swipe.none;
    }
    int count = 0;
    public void MagnetWithSwipe(swipe s) {

        if(gameManager.instance.currentGameState != GameStates.travelling)
        {

            if (canSwipe)
            {
                canSwipe = false;
                HapticController.instance.HapticLow();
                //gameManager.instance.makeKilledEnemySpot();
                if (gameManager.instance.currentGameState == GameStates.inPuzzle)
                {
                    gameManager.instance.makeBotEnemyDead();
                    if (count < 1)
                    {
                        Invoke(nameof(InvokeUp), 0.3f);
                        count++;
                    }


                }

                switch (s)
                {
                    case swipe.left:
                        HandAnim.SetTrigger("left");
                        leftSwipe = true;
                        rightSwipe = false;
                        break;
                    case swipe.right:
                        rightSwipe = true;
                        leftSwipe = false;
                        HandAnim.SetTrigger("right");

                        break;
                    case swipe.up:
                        HandAnim.SetTrigger("up");
                        upSwipe = true;
                        downSwipe = false;
                        break;
                    case swipe.down:
                        HandAnim.SetTrigger("down");
                        downSwipe = true;
                        upSwipe = false;
                        break;

                }

            }
        } 

    }

    void InvokeUp()
    {
        if (currentBreakedAI.Count >0)
        {
            foreach (var item in currentBreakedAI)
            {
                item.transform.DOMoveY(item.transform.position.y + 0.7f, 0.1f);
            }
        }

    }

    public void SwipeAgain()
    {
        canSwipe = true;
    }

    public void TweenLeftAndRight(float moveVal,float moveTime)
    {
        trails[0].transform.localPosition+= Vector3.right * 4.2f * -Mathf.Sign(moveVal);
        trails[1].transform.localPosition += Vector3.right * 2.1f * -Mathf.Sign(moveVal);
        foreach (var trail in trails)
        {
            trail.gameObject.SetActive(true);
        }
        swipeText.transform.DOMove(swipeText.transform.position + Vector3.right * moveVal, moveTime).SetEase(Ease.OutCirc);
        polarity.transform.DOMove(polarity.transform.position + Vector3.right * moveVal, moveTime).SetEase(Ease.OutCirc);
    }

    public void TweenUpAndDown(float moveVal,float moveTime)
    {
        foreach (var trail in trails)
        {
            trail.gameObject.SetActive(true);
        }
        swipeText.transform.DOMove(swipeText.transform.position + Vector3.up * moveVal, moveTime).SetEase(Ease.OutCirc);
        polarity.transform.DOMove(polarity.transform.position + Vector3.up * moveVal, moveTime).SetEase(Ease.OutCirc);
    }
    public void FreezeConstraits(bool x, bool y) {
        if (x) {
            foreach (var item in magneticObj)
            {
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
        if (y)
        {
            foreach (var item in magneticObj)
            {
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
        if (x)
        {
            if (downSwipe)
            {
                downSwipe = false;
            
                foreach (var item in currentBreakedAI)
                {
                    item.transform.DOMoveY(item.transform.position.y + .7f, 0.1f);
                }
            }
            if (upSwipe)
            {
                upSwipe = false;
                foreach (var item in currentBreakedAI)
                {
                    item.transform.DOMoveY(item.transform.position.y - .7f, 0.1f);
                }
            }
            foreach (var item in magneticWhichRotateObj)
            {
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ ;
            }
        }
        if (y)
        {
            if (rightSwipe)
            {
                rightSwipe = false;

                foreach (var item in currentBreakedAI)
                {
                    item.transform.DOMoveX(item.transform.position.x - .7f, .1f);

                    

                }
            }
            if (leftSwipe)
            {
                leftSwipe = false;

                foreach (var item in currentBreakedAI)
                {
                    item.transform.DOMoveX(item.transform.position.x + .7f, .1f);
                    
                }
            }
            foreach (var item in magneticWhichRotateObj)
            {
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
        }
        if (x)
        {
           
           

            foreach (var item in magneticSawObj)
            {
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
        if (y)
        {
           
            foreach (var item in magneticSawObj)
            {
                item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll ;
            }
        }


    }

    public void setLeftElectroMagnetVal(float l,float r,float u,float d) {
        LeanTween.value(leftElectroMagnet.MagnetForce, l, .5f).setOnUpdate((a) => {
            leftElectroMagnet.MagnetForce = a;
            leftElectroMagnet.setChildMagnetVal();
            });
        LeanTween.value(rigthElectroMagnet.MagnetForce, r, .05f).setOnUpdate((b) => {
            rigthElectroMagnet.MagnetForce = b;
            rigthElectroMagnet.setChildMagnetVal();
        }
        );
        LeanTween.value(upElectroMagnet.MagnetForce, u, .05f).setOnUpdate((b) => {
            upElectroMagnet.MagnetForce = b;
            upElectroMagnet.setChildMagnetVal();
        }
        );
        LeanTween.value(downElectroMagnet.MagnetForce, d, .05f).setOnUpdate((b) => {
            downElectroMagnet.MagnetForce = b;
            downElectroMagnet.setChildMagnetVal();
        }
        );
    }
    public void setRightElectroMagnetVal(int l, int r,float u,float d)
    {
        LeanTween.value(rigthElectroMagnet.MagnetForce, r, .5f).setOnUpdate((a) => {
            rigthElectroMagnet.MagnetForce = a;
            rigthElectroMagnet.setChildMagnetVal(); } );

        LeanTween.value(leftElectroMagnet.MagnetForce, l, .05f).setOnUpdate((b) => {
            leftElectroMagnet.MagnetForce = b;
            leftElectroMagnet.setChildMagnetVal(); });
        LeanTween.value(upElectroMagnet.MagnetForce, u, .05f).setOnUpdate((b) => {
            upElectroMagnet.MagnetForce = b;
            upElectroMagnet.setChildMagnetVal(); });
        LeanTween.value(downElectroMagnet.MagnetForce, d, .05f).setOnUpdate((b) => {
            downElectroMagnet.MagnetForce = b;
            downElectroMagnet.setChildMagnetVal();
        }
        );
    }
    public void setUpElectroMagnetVal(int l, int r,float u,float d)
    {
        LeanTween.value(upElectroMagnet.MagnetForce, u, .5f).setOnUpdate((a) => {
            upElectroMagnet.MagnetForce = a;
            upElectroMagnet.setChildMagnetVal();
        });

        LeanTween.value(leftElectroMagnet.MagnetForce, l, .05f).setOnUpdate((b) => {
            leftElectroMagnet.MagnetForce = b;
            leftElectroMagnet.setChildMagnetVal();
        });
         LeanTween.value(rigthElectroMagnet.MagnetForce, r, .05f).setOnUpdate((b) => {
            rigthElectroMagnet.MagnetForce = b;
            rigthElectroMagnet.setChildMagnetVal();
        });
        LeanTween.value(downElectroMagnet.MagnetForce, d, .05f).setOnUpdate((b) => {
            downElectroMagnet.MagnetForce = b;
            downElectroMagnet.setChildMagnetVal();
        }
        );

    }
    public void setDownElectroMagnetVal(int l, int r, float u, float d)
    {
        LeanTween.value(downElectroMagnet.MagnetForce, d, .5f).setOnUpdate((a) => {
            downElectroMagnet.MagnetForce = a;
            downElectroMagnet.setChildMagnetVal();
        });

        LeanTween.value(leftElectroMagnet.MagnetForce, l, .05f).setOnUpdate((b) => {
            leftElectroMagnet.MagnetForce = b;
            leftElectroMagnet.setChildMagnetVal();
        });
        LeanTween.value(rigthElectroMagnet.MagnetForce, r, .05f).setOnUpdate((b) => {
            rigthElectroMagnet.MagnetForce = b;
            rigthElectroMagnet.setChildMagnetVal();
        });
        LeanTween.value(upElectroMagnet.MagnetForce, u, .05f).setOnUpdate((b) => {
            upElectroMagnet.MagnetForce = b;
            upElectroMagnet.setChildMagnetVal();
        }
        );

    }

    [ContextMenu("trail On")]
    public void ActivateTrailEffect() {
        foreach (var item in trailEffects)
        {
            item.SetActive(true);
        }
    }

    [ContextMenu("trail off")]
    public void DectivateTrailEffect() {
        foreach (var item in trailEffects)
        {
            item.SetActive(false);
        }
    }
    [ContextMenu("MagnetReset")]
    public void MagnetReset()
    {
        leftElectroMagnet.MagnetForce = 0;
        leftElectroMagnet.setChildMagnetVal();
        rigthElectroMagnet.MagnetForce = 0;
        rigthElectroMagnet.setChildMagnetVal();
        upElectroMagnet.MagnetForce = 0;
        upElectroMagnet.setChildMagnetVal();
        downElectroMagnet.MagnetForce = 0;
        downElectroMagnet.setChildMagnetVal();
    }

    public void ActivateFear()
    {
        HandAnim.SetTrigger("fear");
    }

}
