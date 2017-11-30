using Improbable;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Worker;
namespace Assets.Gamelogic.Shoots
{
	// Add this MonoBehaviour on UnityWorker (server-side) workers only
	[WorkerType(WorkerPlatform.UnityWorker)]
	public class TakeDamage : MonoBehaviour
	{
		// Enable this MonoBehaviour only on the worker with write access for the entity's Health component
		[Require] private Health.Writer HealthWriter;
		[Require]
		private ClientConnection.Writer ClientConnectionWriter;
		[Require] private Score.Writer ScoreWriter;
		private int newHealth;
		private int newScore;
		private void OnTriggerEnter(Collider other)
		{
			/*
             * Unity's OnTriggerEnter runs even if the MonoBehaviour is disabled, so non-authoritative UnityWorkers
             * must be protected against null writers
             */
			if (HealthWriter == null)
				return;

			// Ignore collision if this ship is already dead
			if (HealthWriter.Data.currentHealth <= 0)
				return;

			if (other != null && other.gameObject.tag == "Cannonball")
			{
				// Reduce health of this entity when hit
				newHealth = HealthWriter.Data.currentHealth - 250;
				HealthWriter.Send(new Health.Update().SetCurrentHealth(newHealth));
				if (newHealth <= 0)
				{
					AwardPointsForKill(new EntityId(other.GetComponent<Shoots.DestroyShoot>().firerEntityId.Value.Id));
				}
			}
            if (other != null && other.gameObject.tag == "Lava")
            {
                // Player dies when collides with lava
				newHealth = 0;
				HealthWriter.Send(new Health.Update().SetCurrentHealth(newHealth));
				if (newHealth <= 0)
				{
					AwardPointsForKill(new EntityId(other.GetComponent<Shoots.DestroyShoot>().firerEntityId.Value.Id));
				}
            }

			if (other.gameObject.IsSpatialOsEntity()) {
				if (other != null && other.gameObject.GetSpatialOsEntity().PrefabName == "health+") {
					//Debug.LogWarning("0: Collision accepted from " + gameObject.GetSpatialOsEntity().PrefabName+ "with " + other.gameObject.GetSpatialOsEntity().PrefabName);

					SpatialOS.Commands.DeleteEntity(ClientConnectionWriter, other.gameObject.EntityId(), result => {
						if (result.StatusCode != StatusCode.Success) {
							Debug.Log("Failed to delete entity with error: " + result.ErrorMessage);
							return;
						}
						Debug.Log("Deleted entity: " + result.Response.Value);
					});
					if (newHealth < 1000){
					newHealth = HealthWriter.Data.currentHealth + 250;
					HealthWriter.Send(new Health.Update().SetCurrentHealth(newHealth));
					}
		}
			}

			if (other.gameObject.IsSpatialOsEntity()) {
				if (other != null && other.gameObject.GetSpatialOsEntity().PrefabName == "health-") {
					//Debug.LogWarning("0: Collision accepted from " + gameObject.GetSpatialOsEntity().PrefabName+ "with " + other.gameObject.GetSpatialOsEntity().PrefabName);

					SpatialOS.Commands.DeleteEntity(ClientConnectionWriter, other.gameObject.EntityId(), result => {
						if (result.StatusCode != StatusCode.Success) {
							Debug.Log("Failed to delete entity with error: " + result.ErrorMessage);
							return;
						}
						Debug.Log("Deleted entity: " + result.Response.Value);
					});
					if (newHealth >= 0){
						newHealth = HealthWriter.Data.currentHealth - 250;
						HealthWriter.Send(new Health.Update().SetCurrentHealth(newHealth));
					}
					if(newHealth <=0){
						newScore = ScoreWriter.Data.numberOfPoints -1;
						ScoreWriter.Send (new Score.Update ().SetNumberOfPoints (newScore));
					}
					}
				}

			if (other.gameObject.IsSpatialOsEntity()) {
				if (other != null && other.gameObject.GetSpatialOsEntity().PrefabName == "points") {
					//Debug.LogWarning("0: Collision accepted from " + gameObject.GetSpatialOsEntity().PrefabName+ "with " + other.gameObject.GetSpatialOsEntity().PrefabName);

					SpatialOS.Commands.DeleteEntity(ClientConnectionWriter, other.gameObject.EntityId(), result => {
						if (result.StatusCode != StatusCode.Success) {
							Debug.Log("Failed to delete entity with error: " + result.ErrorMessage);
							return;
						}
						Debug.Log("Deleted entity: " + result.Response.Value);
					});
					newScore = ScoreWriter.Data.numberOfPoints +1;
					ScoreWriter.Send (new Score.Update ().SetNumberOfPoints (newScore));
					}
				}
			}

		private void AwardPointsForKill(EntityId firerEntityId)
		{
		uint pointsToAward = 1;
		// Use Commands API to issue an AwardPoints request to the entity who fired the cannonball
		SpatialOS.Commands.SendCommand(HealthWriter, Score.Commands.AwardPoints.Descriptor, new AwardPoints(pointsToAward), firerEntityId)
			.OnSuccess(OnAwardPointsSuccess)
			.OnFailure(OnAwardPointsFailure);
		}

		private void OnAwardPointsSuccess(AwardResponse response)
		{
		Debug.Log("AwardPoints command succeeded. Points awarded: " + response.amount);
		}

		private void OnAwardPointsFailure(ICommandErrorDetails response)
		{
			Debug.LogError("Failed to send AwardPoints command with error: " + response.ErrorMessage);
		}
    	    
		}
}
