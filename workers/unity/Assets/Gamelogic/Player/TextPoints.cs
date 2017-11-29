using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Gamelogic.Core;
using Improbable.Player;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine.UI;
using System;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class TextPoints : MonoBehaviour
    {
        [Require] private Score.Reader ScoreReader;
        [Require] private Status.Reader StatusReader;
        [Require] private ClientAuthorityCheck.Writer ClientAuthorityCheckWriter;
        
        private Text textPoints;
        private Text textWin;

        private void Awake()
        {

            textWin = GameObject.Find("Canvas/TextWin").GetComponent<Text>();
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
            StatusReader.ComponentUpdated.Add(VerifyWin);
        }

        private void OnDisable()
        {
            // Deregister callback for when components change
            ScoreReader.NumberOfPointsUpdated.Remove(OnNumberOfPointsUpdated);
            StatusReader.ComponentUpdated.Remove(VerifyWin);
        }

        private void VerifyWin(Status.Update obj)
        {
            Debug.LogWarning("WIN-LOSE");
            if (ScoreReader.Data.numberOfPoints == 3) {
                textWin.text = "YOU WIN";
            } else {
                textWin.text = "YOU LOSE";
            }
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
