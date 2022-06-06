using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;
using _Scripts.Analytics;
using RootMotion.Dynamics;
//STATES FOR GAME FLOW
public enum GameStates {inStart, travelling, inPuzzle , ended , video}
public enum GameResult {success , fail};
public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    private void Awake()
    {
        instance = this;
    }

    [HideInInspector]public int sceneIndex;

    [Header("GAME_FLOW -> DEBUG ONLY")]
    public GameStates currentGameState;
    public GameResult gameResult;

    [System.Serializable]
    
    public class waveList
    {
        public GameObject[] breakedEnemy;
        public GameObject[] killedEnemy;
    }
    [Header("Wave Information")]
    public waveList[] waveLists;
    public int atWave;
    public Transform[] position;

    private float timeTillBotShock = 0f;

    [Header("Blast Stuff")]
    public Material[] explosionMats;
    public GameObject DarkSmoke;

    bool hasReachedStation = false;
    
    [Header("Enemy Material")]
    public Material enemyDeadMat;
    [Header("Test")]
    public RootMotion.Dynamics.PuppetMaster[] puppet;
    bool stable;
    public int[] perPuzzleEnemies;
    int enemyDead = 0;

    [Header("Player Parameters")]
    public GameObject player;
    public NavMeshAgent agent;
    public float baseSpeed;
    bool MoveAcrossNavMeshesStarted;

    [Header("ScreenConfetti")]
    [SerializeField] ParticleSystem confetti;

    [SerializeField] Slider levelProgressionMeter;
    float maxProgressionLimit;

    public GameObject cutModel;

    public int score = 0;
    [SerializeField] ParticleSystem moneyBlastEffect;

    [SerializeField] int healthFactor = 12;

    AudioSource audioSource;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        audioSource = GetComponent<AudioSource>();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level" + (sceneIndex).ToString());
        currentGameState = GameStates.inStart;
        MakePositionsRelativeToPlayer();
        InitializeLevelProgression();
        //currentGameState = GameStates.video;
        //Invoke(nameof(InvokeLevel), 0.6f);
        //makeBreakedEnemySpot();
    }

    public void PlayBlast()
    {
        audioSource.Play();
    }
    public void StartGameplay()
    {
        if (currentGameState == GameStates.inStart)
        {
            currentGameState = GameStates.travelling;
            GameUIHandler.instance.DeactivateStartScreen();
            LionsAnalyticsController.Instance.OnLevelStart();
            moveWithAgent(position[atWave]);
            CustomTweener.Instance.FadeInFloatingArrow();
            CustomTweener.Instance.FadeFog();
        }

    }
    private void Update()
    {
        //Swipe the text off screen
        if (currentGameState == GameStates.travelling)
        {
            if (!hasReachedStation)
            {
                if (reached())
                {
                    // move(position[atWave]);
                    currentGameState = GameStates.inPuzzle;
                    Invoke(nameof(makeKilledEnemySpot), timeTillBotShock);
                    Invoke(nameof(makeBreakedEnemySpot), timeTillBotShock);
                    //myMagnet.instance.ActivateTrailEffect();
                    hasReachedStation = true;
                    if(atWave == perPuzzleEnemies.Length)
                    {
                        TweenBar();
                        HitFinishLine();
                    }

                }
                else
                {
                    MovePlayerToPosition();
                    UpdateLevelProgression();
                    //myMagnet.instance.DectivateTrailEffect();
                }
            }

        }
        //if (agent.isOnOffMeshLink)
        //{
        //    agent.speed = baseSpeed / 2;
        //    Debug.Log(agent.velocity);
        //}
        //else
        //{
        //    agent.speed = baseSpeed;
        //}

    }

    bool first;
    [HideInInspector]public int TryingInOfThisLevel;
    void getAllSaveData()
    {
        if (PlayerPrefs.HasKey("triedIntofLevel" + sceneIndex))
        {
            TryingInOfThisLevel = PlayerPrefs.GetInt("triedIntofLevel" + sceneIndex);
        }
        else
        {
            PlayerPrefs.SetInt("triedIntofLevel" + sceneIndex, 1);
            TryingInOfThisLevel = 1;
        }
    }

    public void setTryingLevelInt()
    {
        TryingInOfThisLevel += 1;
        PlayerPrefs.SetInt("triedIntofLevel" + sceneIndex, TryingInOfThisLevel);
    }

    #region EnemyInteractions
    public void makeBotEnemyDead() 
    {
        if (atWave < perPuzzleEnemies.Length)
        {
            foreach (var item in waveLists[atWave].breakedEnemy)
            {
                item.GetComponent<breaked>().puppet.state = PuppetMaster.State.Dead;
                //StartCoroutine(makeBotEnemyAlive(item));
            }
        }
    }
    public void makeKilledEnemySpot() 
    {
        if(atWave < perPuzzleEnemies.Length)
        {
            foreach (var item in waveLists[atWave].killedEnemy)
            {
                //tween the exclamaition mark

                item.GetComponent<enemyMove>().enemyState = enemyMove.eState.shock;
                item.GetComponent<enemyMove>().checkState();

                //  StartCoroutine(makeBotEnemyAlive(item));
            }
        }
       
    }
    public void makeBreakedEnemySpot() 
    {
        if(atWave < perPuzzleEnemies.Length)
        {
            foreach (var item in waveLists[atWave].breakedEnemy)
            {
                //tween the exclamaition mark
                item.GetComponent<enemyMove>().enemyState = enemyMove.eState.shock;
                item.GetComponent<enemyMove>().checkState();

                //  StartCoroutine(makeBotEnemyAlive(item));
            }
        }
      
    }

    #endregion

    //IEnumerator makeBotEnemyAlive(GameObject g) {
    //    yield return new WaitForSeconds(2f);
    //    g.GetComponent<breaked>().puppet.state = RootMotion.Dynamics.PuppetMaster.State.Alive;
    //}
   

 #region PlayerMovement
    void walkToNextWave() {
        hasReachedStation = false;
        //moveWithAgent(position[atWave]);
        player.GetComponent<health>().HealthReset();
        CustomTweener.Instance.Invoke("FadeFog", 1.2f);
        CustomTweener.Instance.Invoke("FadeInFloatingArrow", 1.5f);
        CustomTweener.Instance.Invoke("ResetText", 2f);
    }

    void MovePlayerToPosition()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, position[atWave].position, Time.deltaTime * baseSpeed);

    }

    bool reached()
    {    //change this function to move to next point     
        //float dist = agent.remainingDistance;
        //if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        //{      
        //    return true;
        //}
        //else {
        //    return false;
        //}

        float distanceBetween = position[atWave].position.z - player.transform.position.z;
        if (distanceBetween <= 1f)
            return true;
        else
            return false;
    }
    void MakePositionsRelativeToPlayer()
    {
        for (int i = 0; i < position.Length; i++)
        {
            var newPos = Vector3.up * player.transform.position.y + position[i].position;
            position[i].position = newPos;
        }
    }

    public void moveWithAgent(Transform t)
    {
        //goto this position 
        hasReachedStation = false;
        //agent.SetDestination(t.position);
    }

    #endregion

    #region EnemyShooting
    public void callShoot(Animator anim) {
        float t = Random.Range(1f, 2f);
        StartCoroutine(shootBullet(anim,t));
    }
    IEnumerator shootBullet(Animator anim,float t) {
        yield return new WaitForSeconds(t);
        anim.SetTrigger("hit");
        anim.transform.parent.GetComponent<EnemyShoot>().SHooot();

    }
    #endregion

    #region CheckOnEnemyDeath

    public void addInEnemyDiedAndCheck()
    {
        enemyDead++;
        setScore(50);
        HapticController.instance.HapticMedium();
        if (enemyDead == perPuzzleEnemies[atWave])
        {
            enemyDead = 0;
            atWave++;
            currentGameState = GameStates.travelling;
            Invoke(nameof(walkToNextWave), 4f);
            CustomTweener.Instance.Invoke("TweenTheIncomingText", 1f);
            Invoke(nameof(PlayConfetti), 1f);
            PolarizedObjects.instance.StopOutliningObjects();
            Invoke(nameof(InvokeVictoryAnimation), 0.45f);
            
        }
    }

    void InvokeVictoryAnimation()
    {
        myMagnet.instance.HandAnim.SetTrigger("vic");
    }

    #endregion



    public void DecreasePlayerHealth()
    {
        player.GetComponent<health>().DecreaseHealth(healthFactor);
    }
    public void PlayConfetti()
    {
        if(currentGameState == GameStates.ended && gameResult == GameResult.success)
            confetti.transform.GetChild(0).gameObject.SetActive(true);

            confetti.Play();

    }

    void HitFinishLine()
    {
        HapticController.instance.HapticSuccess();
        currentGameState = GameStates.ended;
        gameResult = GameResult.success;
        PlayConfetti();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level" + (sceneIndex).ToString());
        LionsAnalyticsController.Instance.OnLevelComplete(TryingInOfThisLevel);
        GameUIHandler.instance.Invoke("EnableEndScreen", 0.8f);
    }
    
    public void fallDown()
    {
        if (!stable)
        {
            stable = true;
            foreach (var item in puppet)
            {
                item.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
            }
        }
        else
        {
            stable = false;
            foreach (var item in puppet)
            {
                item.state = RootMotion.Dynamics.PuppetMaster.State.Alive;
            }
        }
    }

    public void InitializeLevelProgression()
    {
        maxProgressionLimit = position[perPuzzleEnemies.Length].position.z - player.transform.position.z;
        levelProgressionMeter.maxValue = maxProgressionLimit;
        levelProgressionMeter.minValue = 0f;
    }
    public void UpdateLevelProgression()
    {
        var currentProgression = (position[perPuzzleEnemies.Length].position.z - player.transform.position.z);
        levelProgressionMeter.value = Mathf.Abs(maxProgressionLimit - currentProgression);
    }

    void TweenBar()
    {
        levelProgressionMeter.GetComponent<RectTransform>().DOLocalMoveY(220f, 0.6f);
        player.GetComponent<health>().TweenHealthBar();
    }

    public void ChangeTimeScaleOfGameSlow(float newTimeScale, float resetTime)
    {
        //     LeanTween.value(1f, newTimeScale, inTime).setOnUpdate((a)=>
        //     Time.timeScale = a).setOnComplete(ChangeTimeScaleOfGameFast);
        //
        Time.timeScale = newTimeScale;
        Invoke(nameof(ResetTimeScale), resetTime);
    }

    void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }


    void setScore(int i)
    {
        int b = score + i;
        LeanTween.value(score, b, 1.3f).setOnUpdate((a) => {
            score = (int)a;
            PlayerPrefs.SetInt("score", score);
            GameUIHandler.instance.ChangeCoinText(score);
        }).setOnComplete(() => LeanTween.cancel(gameObject));
        Invoke(nameof(InvokeMoneyEffect), 0.2f);
    }



    void InvokeMoneyEffect()
    {
        moneyBlastEffect.Play();
    }

    void ActivateNextWave()
    {
        
        if (atWave < perPuzzleEnemies.Length)
        {
            Debug.Log("called");
            foreach (var enemy in waveLists[atWave].breakedEnemy)
            {
                enemy.GetComponent<RunTowards>().activated = true;

            }

            foreach (var enemy in waveLists[atWave].killedEnemy)
            {
                enemy.GetComponent<RunTowards>().activated = true;

            }
        }
    }



}


