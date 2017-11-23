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
            PopulateSnapshotWithEnemyEntities(ref snapshotEntities, ref currentEntityId);

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


        public static void PopulateSnapshotWithEnemyEntities(ref Dictionary<EntityId, Entity> snapshotEntities, ref int nextAvailableId)
        {
            for (var i = 0; i < SimulationSettings.TotalEnemies; i++)
            {
                // Choose a starting position for this pirate entity
                var enemyCoordinates = new Vector3((Random.value - 0.5f) * SimulationSettings.EnemiesSpawnDiameter, 0,
                    (Random.value - 0.5f) * SimulationSettings.EnemiesSpawnDiameter);
                var enemyRotation = System.Convert.ToUInt32(Random.value * 360);

                snapshotEntities.Add(new EntityId(nextAvailableId++),
                    EntityTemplateFactory.CreateEnemyEntityTemplate(enemyCoordinates, enemyRotation));
            }
        }
    }
}
