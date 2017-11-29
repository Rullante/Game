using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Gamelogic.Core;
using Improbable.Player;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine.UI;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class TextPoints : MonoBehaviour
    {
        [Require] private Score.Reader ScoreReader;
        [Require] private ClientAuthorityCheck.Writer ClientAuthorityCheckWriter;
        
        private Text textPoints;

        private void Awake()
        {
            textPoints = GameObject.Find("Canvas/TextPoints").GetComponent<Text>();
            if (SimulationSettings.Flag == false)
            {
                SimulationSettings.Flag = true;
                updateGUI(0);
              
            }
        }


        private void OnEnable()
        {
            // Register callback for when components change
            ScoreReader.NumberOfPointsUpdated.Add(OnNumberOfPointsUpdated);
        }

        private void OnDisable()
        {
            // Deregister callback for when components change
            ScoreReader.NumberOfPointsUpdated.Remove(OnNumberOfPointsUpdated);
        }

        private void OnNumberOfPointsUpdated(int numberOfPoint)
        {
            
            updateGUI(numberOfPoint);

        }

        void updateGUI(int score)
        {
            textPoints.text = "Points: " + score + "/3" ;
           
        }
    }
}
