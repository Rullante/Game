using Assets.Gamelogic.Core;
using Assets.Gamelogic.EntityTemplates;
using Improbable;
using Improbable.Worker;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor 
{
	public class SnapshotMenu : MonoBehaviour
	{
		[MenuItem("Improbable/Snapshots/Generate Default Snapshot")]
		private static void GenerateDefaultSnapshot()
		{
			var snapshotEntities = new Dictionary<EntityId, Entity>();
			var currentEntityId = 1;

			snapshotEntities.Add(new EntityId(currentEntityId++), EntityTemplateFactory.CreatePlayerCreatorTemplate());
			PopulateSnapshotWithHealthlessEntities(ref snapshotEntities, ref currentEntityId);
			PopulateSnapshotWithHealthplusEntities(ref snapshotEntities, ref currentEntityId);
			PopulateSnapshotWithPointsEntities(ref snapshotEntities, ref currentEntityId);

            SaveSnapshot(snapshotEntities);
		}

		private static void SaveSnapshot(IDictionary<EntityId, Entity> snapshotEntities)
		{
			File.Delete(SimulationSettings.DefaultSnapshotPath);
			var maybeError = Snapshot.Save(SimulationSettings.DefaultSnapshotPath, snapshotEntities);

			if (maybeError.HasValue)
			{
				Debug.LogErrorFormat("Failed to generate initial world snapshot: {0}", maybeError.Value);
			}
			else
			{
				Debug.LogFormat("Successfully generated initial world snapshot at {0}", SimulationSettings.DefaultSnapshotPath);
			}
		}

		public static void PopulateSnapshotWithHealthplusEntities(ref Dictionary<EntityId, Entity> snapshotEntities, ref int nextAvailableId)
		{
			for (var i = 0; i < SimulationSettings.Total; i++)
			{
				// Choose a starting position for this pirate entity
				var bonusCoordinates = new Vector3((Random.value - 0.5f) * SimulationSettings.bonusSpawnDiameter, 0,
					(Random.value - 0.5f) * SimulationSettings.bonusSpawnDiameter);
				var bonusRotation = System.Convert.ToUInt32(Random.value * 360);

				snapshotEntities.Add(new EntityId(nextAvailableId++),
					EntityTemplateFactory.CreatehealthplusTemplate(bonusCoordinates, bonusRotation));
			}
		}

		public static void PopulateSnapshotWithHealthlessEntities(ref Dictionary<EntityId, Entity> snapshotEntities, ref int nextAvailableId)
		{
			for (var i = 0; i < SimulationSettings.Total; i++)
			{
				// Choose a starting position for this pirate entity
				var bonusCoordinates = new Vector3((Random.value - 0.5f) * SimulationSettings.bonusSpawnDiameter, 0,
					(Random.value - 0.5f) * SimulationSettings.bonusSpawnDiameter);
				var bonusRotation = System.Convert.ToUInt32(Random.value * 360);

				snapshotEntities.Add(new EntityId(nextAvailableId++),
					EntityTemplateFactory.CreatehealthlessTemplate(bonusCoordinates, bonusRotation));
			}
		}

		public static void PopulateSnapshotWithPointsEntities(ref Dictionary<EntityId, Entity> snapshotEntities, ref int nextAvailableId)
		{
			for (var i = 0; i < SimulationSettings.Total; i++)
			{
				// Choose a starting position for this pirate entity
				var bonusCoordinates = new Vector3((Random.value - 0.5f) * SimulationSettings.bonusSpawnDiameter, 0,
					(Random.value - 0.5f) * SimulationSettings.bonusSpawnDiameter);
				var bonusRotation = System.Convert.ToUInt32(Random.value * 360);

				snapshotEntities.Add(new EntityId(nextAvailableId++),
					EntityTemplateFactory.CreatepointsTemplate(bonusCoordinates, bonusRotation));
			}
		}
	}
}