using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTriggers : MonoBehaviour
{
    [SerializeField] ParticleSystem[] shockWave;
    // right - 0 left- 1 up-2 down-3

    public void TriggerHandMovementFunctions(int vals)
    {
        switch (vals)
        {
            
            case 0: CameraFunctions.instance.QuickTilt(Vector3.forward, false);
                    CustomTweener.Instance.ArrowTween(-90f);
                
                if (gameManager.instance.currentGameState == GameStates.inPuzzle && myMagnet.instance.currentSwipe != myMagnet.swipe.right)
                {
                    
                    myMagnet.instance.FreezeConstraits(true, false);
                    myMagnet.instance.setRightElectroMagnetVal(0, 600, 0, 0);
                    myMagnet.instance.currentSwipe = myMagnet.swipe.right;
                }
                else if(gameManager.instance.currentGameState == GameStates.inStart)
                {
                    Invoke("ResetPolarity", 1f);
                    myMagnet.instance.TweenLeftAndRight(8f, 0.65f);
                    gameManager.instance.Invoke("StartGameplay", 1.35f);
                    
                }
                
                break;

            case 1:
                CameraFunctions.instance.QuickTilt(Vector3.back, false);
                CustomTweener.Instance.ArrowTween(90f);
                if (gameManager.instance.currentGameState == GameStates.inPuzzle && myMagnet.instance.currentSwipe != myMagnet.swipe.left)
                {
                    
                    myMagnet.instance.FreezeConstraits(true, false);
                    myMagnet.instance.setLeftElectroMagnetVal(600, 0, 0, 0);
                    myMagnet.instance.currentSwipe = myMagnet.swipe.left;
                }
                else if (gameManager.instance.currentGameState == GameStates.inStart)
                {
                    Invoke("ResetPolarity", 1f);
                    myMagnet.instance.TweenLeftAndRight(-8f, 0.65f);
                    gameManager.instance.Invoke("StartGameplay", 1.35f);
                }
                
                break;

            case 2:
                CameraFunctions.instance.QuickTilt(Vector3.right, true);
                CustomTweener.Instance.ArrowTween(0f);
                if (gameManager.instance.currentGameState == GameStates.inPuzzle && myMagnet.instance.currentSwipe != myMagnet.swipe.up)
                {
                    
                    myMagnet.instance.FreezeConstraits(false, true);
                    myMagnet.instance.setUpElectroMagnetVal(0, 0, 600, 0);
                    myMagnet.instance.currentSwipe = myMagnet.swipe.up;
                }
                else if (gameManager.instance.currentGameState == GameStates.inStart)
                {
                    Invoke("ResetPolarity", 1f);
                    myMagnet.instance.TweenUpAndDown(15f, 1f);
                    gameManager.instance.Invoke("StartGameplay", 1.35f);
                }
                
                break;

            case 3:
                CustomTweener.Instance.ArrowTween(180f);
                CameraFunctions.instance.QuickTilt(Vector3.left, true);
                if (gameManager.instance.currentGameState == GameStates.inPuzzle && myMagnet.instance.currentSwipe != myMagnet.swipe.down)
                {
                    
                    myMagnet.instance.FreezeConstraits(false, true);
                    myMagnet.instance.setDownElectroMagnetVal(0, 0, 0, 600);
                    myMagnet.instance.currentSwipe = myMagnet.swipe.down;
                }
                else if (gameManager.instance.currentGameState == GameStates.inStart)
                {
                    Invoke("ResetPolarity", 1f);
                    myMagnet.instance.TweenUpAndDown(-2f, 10f);
                    gameManager.instance.Invoke("StartGameplay", 1.35f);
                }
                
                break;
        }

       
        
    }

    void ResetPolarity()
    {
        CustomTweener.Instance.ArrowTween(180f);
    }

   void PlayEffect()
    {
        foreach(var shockwave in shockWave)
        {
            shockwave.Play();
        }
        
    }

    void StopEffect()
    {
        foreach (var shockwave in shockWave)
        {
           
            shockwave.Stop();
        }
        myMagnet.instance.SwipeAgain();
    }

}
