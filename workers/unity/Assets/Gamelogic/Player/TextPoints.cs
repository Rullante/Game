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
using Improbable.Entity.Component;
using Improbable.Unity.Core;
using Assets.Gamelogic.Utils;

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
			StatusReader.GameOverUpdated.Add(OnGameOver);
        }

        private void OnDisable()
        {
            // Deregister callback for when components change
            ScoreReader.NumberOfPointsUpdated.Remove(OnNumberOfPointsUpdated);
			StatusReader.GameOverUpdated.Remove(OnGameOver);
        }
        private void OnNumberOfPointsUpdated(int numberOfPoint)
        {
            
            updateGUI(numberOfPoint);

        }
		void updateGUI(int score)
		{
			textPoints.text = "Points: " + score + "/3" ;
			if (score == 1000) {
				textWin.text = "YOU WIN";
				textPoints.enabled = false;
			} else if (score == -1){
				textWin.text = "YOU LOSE";
				textPoints.enabled = false;
			}

		}

		private void OnGameOver(int gameover){
			if(gameover == 2){
			if (ScoreReader.Data.numberOfPoints == 3) {
				textWin.text = "YOU WIN";
				textPoints.enabled = false;
			} else if (ScoreReader.Data.numberOfPoints != 3){
				textWin.text = "YOU LOSE";
				textPoints.enabled = false;
				}

				FindObjectOfType<Bootstrap>().ConnectToClient();
				StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.ClientConnectionTimeoutSecs, ConnectionTimeout));
			}
		}
		private void ConnectionTimeout(){}
    }
}
