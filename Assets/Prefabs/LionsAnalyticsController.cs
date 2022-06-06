
using LionStudios.Suite.Analytics;
using LionStudios.Suite.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

namespace _Scripts.Analytics
{
    public class LionsAnalyticsController : MonoBehaviour
    {

        public static LionsAnalyticsController Instance;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
            }
            DontDestroyOnLoad(gameObject);
            

        }
        private void Start()
        {
            LionDebugger.Hide();
            OnGameStart();
            

        }

        public void OnGameStart()
        {      
            LionAnalytics.GameStart();
            GameAnalytics.Initialize();
        }
        public void OnLevelStart()
        {
            
            int levelNo = SceneManager.GetActiveScene().buildIndex;
            //int attemptNum = (DLConstant.levelInfo.levelDataList.Count > 0 && DLConstant.LoadedLevel <= DLConstant.levelInfo.levelDataList.Count - 1) ? DLConstant.levelInfo.levelDataList[DLConstant.LoadedLevel - 1].levelNo : 1;

            //Debug.Log(attemptNum+"start level lion anlytics" + DLConstant.LoadedLevel);
            LionAnalytics.LevelStart(levelNo, 1);
        }
        public void OnLevelComplete(int i)
        {
            int levelNo = SceneManager.GetActiveScene().buildIndex ;
            //int attemptNum = (DLConstant.levelInfo.levelDataList.Count > 0 && DLConstant.LoadedLevel <= DLConstant.levelInfo.levelDataList.Count - 1) ? DLConstant.levelInfo.levelDataList[DLConstant.LoadedLevel - 1].levelNo : 1;
            LionAnalytics.LevelComplete(levelNo, i);
            Debug.Log("lion analytics level Complete " + levelNo + "Val " + i);
        }
        public void OnLevelFail(int i)
        {
            
            int levelNo = SceneManager.GetActiveScene().buildIndex;
            Debug.Log("lion analytics level fail "+ levelNo + "Val "+ i);
            //int attemptNum = (DLConstant.levelInfo.levelDataList.Count > 0 && DLConstant.LoadedLevel <= DLConstant.levelInfo.levelDataList.Count - 1) ? DLConstant.levelInfo.levelDataList[DLConstant.LoadedLevel - 1].levelNo : 1;
            LionAnalytics.LevelFail(levelNo, i);
        }

        public void OnLevelRestart(int i)
        {
            //int attemptNum = (DLConstant.levelInfo.levelDataList.Count > 0 && DLConstant.LoadedLevel<=DLConstant.levelInfo.levelDataList.Count-1) ? DLConstant.levelInfo.levelDataList[DLConstant.LoadedLevel - 1].levelNo : 1;
            int levelNo = SceneManager.GetActiveScene().buildIndex;
            Debug.Log("lion analytics level restart " + levelNo + "Val " + i);
            LionAnalytics.LevelRestart(SceneManager.GetActiveScene().buildIndex, i);
        }
    }
}