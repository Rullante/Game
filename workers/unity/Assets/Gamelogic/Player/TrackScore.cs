using Improbable.Entity.Component;
using Assets.Gamelogic.Core;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Unity.Core.EntityQueries;
using Improbable.Unity.Core;
using Improbable.Collections;
using Improbable;
using Improbable.Worker;

using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Gamelogic.Player
{
	// Add this MonoBehaviour on UnityWorker (server-side) workers only
	[WorkerType(WorkerPlatform.UnityWorker)]
	public class TrackScore : MonoBehaviour
	{
		/*
         * An entity with this MonoBehaviour will only be enabled for the single UnityWorker
         * which has write access for its Score component.
         */
		[Require] private Score.Writer ScoreWriter;
        [Require] private Status.Writer StatusWriter;

        void OnEnable()
		{
			// Register command callback
			ScoreWriter.CommandReceiver.OnAwardPoints.RegisterResponse(OnAwardPoints);

        }

		private void OnDisable()
		{
			// Deregister command callbacks
			ScoreWriter.CommandReceiver.OnAwardPoints.DeregisterResponse();
		}

		// Command callback for handling points awarded by other entities when they sink
		private AwardResponse OnAwardPoints(AwardPoints request, ICommandCallerInfo callerInfo)
		{
			int newScore = ScoreWriter.Data.numberOfPoints + (int)request.amount;
			ScoreWriter.Send(new Score.Update().SetNumberOfPoints(newScore));
			if (newScore == SimulationSettings.PointsToWin) {
				ResetQuery();
                return new AwardResponse(request.amount);
            }
            else { 
            // Acknowledge command receipt
            return new AwardResponse(request.amount);
            }
        }

		void ResetQuery()
        {

            Debug.LogWarning("called reset");
            var query = Query.HasComponent<ClientConnection>().ReturnOnlyEntityIds();

            Debug.LogWarning("queryied");
            SpatialOS.Commands.SendQuery(StatusWriter, query)
              .OnSuccess(result => {
                  Debug.LogWarning("Found " + result.EntityCount + " nearby entities with a health component");
                  if (result.EntityCount < 1)
                  {
                      return;
                  }
                  Map<EntityId, Entity> resultMap = result.Entities;
                  foreach (EntityId id in resultMap.Keys)
                  {
                      Entity entity = SpatialOS.GetLocalEntity(id);
						if (entity != null){
                      	{
                          Debug.LogWarning("entity found");

							SpatialOS.Commands.SendCommand(StatusWriter, Status.Commands.GameWon.Descriptor, new Winner("Game over"), id)
                            .OnSuccess(OnGameWinSuccess)
                        	.OnFailure(OnGameWinFailure);

                      //StatusWriter.Send(new Status.Update().AddGameWon(new Win(gameObject.EntityId().ToString())));
                  		}
					}
                  }

              })
                  .OnFailure(errorDetails => Debug.LogWarning("Query failed with error: " + errorDetails));
        }
        private void OnGameWinSuccess(Win response)
        {
            Debug.LogWarning("Gamewin command succeeded.");
        }

        private void OnGameWinFailure(ICommandErrorDetails response)
        {
            Debug.LogError("Failed to send GameWin command with error: " + response.ErrorMessage);
        }
    }

}