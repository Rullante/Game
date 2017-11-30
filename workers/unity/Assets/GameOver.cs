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

// Add this MonoBehaviour on UnityWorker (server-side) workers only
[WorkerType(WorkerPlatform.UnityWorker)]
public class GameOver : MonoBehaviour {

	[Require] private Status.Writer StatusWriter;

	void OnEnable()
	{
		// Register command callback
		StatusWriter.CommandReceiver.OnGameWon.RegisterResponse(onWin);

	}

	private void OnDisable()
	{
		// Deregister command callbacks
		StatusWriter.CommandReceiver.OnGameWon.DeregisterResponse();
	}

	// Command callback for handling points awarded by other entities when they sink

	private Win onWin(Winner request,ICommandCallerInfo callerInfo)

	{	

		if (StatusWriter == null) {

		}else
		StatusWriter.Send (new Status.Update().SetGameOver(2));

		return new Win();
	}
}