using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Scripts.ControllerRelated
{
    public class FacebookController : MonoBehaviour
    {
        public static FacebookController Instance;

        //#region SINGLETON_PATTERN
        //private static FacebookController _instance;
        //public static FacebookController Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = FindObjectOfType<FacebookController>();
        //            if (_instance == null)
        //            {
        //                GameObject FacebookController = new GameObject("FacebookController");
        //                _instance = FacebookController.AddComponent<FacebookController>();
        //            }
        //            DontDestroyOnLoad(_instance.gameObject);
        //        }
        //        return _instance;
        //    }
        //}
        //#endregion
        void Awake ()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }
      

        void OnApplicationPause (bool pauseStatus)
        {
            // Check the pauseStatus to see if we are in the foreground
            // or background
            if (!pauseStatus) {
                //app resume
                if (FB.IsInitialized) {
                    FB.ActivateApp();
                } else {
                    //Handle FB.Init
                    FB.Init( () => {
                        FB.ActivateApp();
                    });
                }
            }
        }
     
       // public void FbLevelComplete()
       // {
       //     Debug.Log("FbLevelComplete");
       //     string currentLevel = "Level Complete "+ (DLConstant.LoadedLevel);

       //     FB.LogAppEvent(currentLevel);
       // }

       //public void FbLevelFail()
       // {
       //     Debug.Log("FbLevelFail");
       //     string currentLevel = "Level Fail";
       //     var levelParams = new Dictionary<string, object>()
       //     {
       //         {
       //             "Level", (DLConstant.LoadedLevel).ToString()
       //         }
         
       //     };


       //     FB.LogAppEvent(currentLevel, null, levelParams);
       // }
       // public void FbStartLevel()
       // {
       //     Debug.Log("FbStartLevel");
       //     string currentLevel = "Level Start";
       //     var levelParams = new Dictionary<string, object>()
       //     {
       //         {
       //             "Level", (DLConstant.LoadedLevel).ToString()
       //         }
          
       //     };


       //     FB.LogAppEvent(currentLevel, null, levelParams);
       // }
    }
}
