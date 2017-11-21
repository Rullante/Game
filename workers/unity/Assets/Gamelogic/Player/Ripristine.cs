using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class Ripristine : MonoBehaviour {
	// Add this MonoBehaviour on client workers only
	[WorkerType(WorkerPlatform.UnityClient)]
	public class death : MonoBehaviour
	{
		// Inject access to the entity's Health component
		[Require] private Health.Reader HealthReader;
		[Require] private Health.Writer HealthWriter;



		private void OnEnable()
		{
			// Register callback for when components change
			HealthReader.CurrentHealthUpdated.Add(OnCurrentHealthUpdated);
		}

		private void OnDisable()
		{
			// Deregister callback for when components change
			HealthReader.CurrentHealthUpdated.Remove(OnCurrentHealthUpdated);
		}

		private void OnCurrentHealthUpdated(int currentHealth)
		{	
			if (HealthReader.Data.currentHealth <= 0)
				HealthWriter.Send (new Health.Update ().SetCurrentHealth (1000));
			
		}
}
}
