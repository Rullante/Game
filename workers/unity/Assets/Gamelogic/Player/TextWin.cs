using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Gamelogic.Core;
using Improbable.Player;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine.UI;
using Improbable.Unity.Core.EntityQueries;
using Improbable.Unity.Core;
using UnityEngine.SceneManagement;
using System;

namespace Assets.Gamelogic.Player
{
    
    public class TextWin : MonoBehaviour
    {
        [Require] private ClientAuthorityCheck.Writer ClientAuthorityCheckWriter;
        

        private void OnEnable()
        {
            // Register callback for when components change
        }

        
        private void OnDisable()
        {
            // Deregister callback for when components change
        }

        private void OnWin(Win obj)
        {
            Debug.LogWarning("OOOOK");
            SceneManager.LoadScene(BuildSettings.SplashScreenScene, LoadSceneMode.Additive);
        }


    }
}
