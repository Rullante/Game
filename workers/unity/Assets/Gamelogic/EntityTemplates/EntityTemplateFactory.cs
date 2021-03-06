﻿using Assets.Gamelogic.Core;
using Improbable;
using Improbable.Core;
using Improbable.Player;
using Improbable.Unity.Core.Acls;
using Improbable.Worker;
using Quaternion = UnityEngine.Quaternion;
using UnityEngine;
using Improbable.Unity.Entity;

namespace Assets.Gamelogic.EntityTemplates
{
    public class EntityTemplateFactory : MonoBehaviour
    {
        public static Entity CreatePlayerCreatorTemplate()
        {
            var playerCreatorEntityTemplate = EntityBuilder.Begin()
                .AddPositionComponent(Improbable.Coordinates.ZERO.ToUnityVector(), CommonRequirementSets.PhysicsOnly)
                .AddMetadataComponent(entityType: SimulationSettings.PlayerCreatorPrefabName)
                .SetPersistence(true)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .AddComponent(new Rotation.Data(Quaternion.identity.ToNativeQuaternion()), CommonRequirementSets.PhysicsOnly)
                .AddComponent(new PlayerCreation.Data(), CommonRequirementSets.PhysicsOnly)
                .Build();

            return playerCreatorEntityTemplate;
        }

        public static Entity CreatePlayerTemplate(string clientId, string name)
        {
            var playerTemplate = EntityBuilder.Begin()
				.AddPositionComponent(new Improbable.Coordinates(0, SimulationSettings.PlayerSpawnHeight, 0).ToUnityVector(), CommonRequirementSets.PhysicsOnly)
                .AddMetadataComponent(entityType: SimulationSettings.PlayerPrefabName)
                .SetPersistence(false)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .AddComponent(new Rotation.Data(Quaternion.identity.ToNativeQuaternion()), CommonRequirementSets.PhysicsOnly)
                .AddComponent(new ClientAuthorityCheck.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
                .AddComponent(new ClientConnection.Data(SimulationSettings.TotalHeartbeatsBeforeTimeout), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new ClientAuthorityCheck.Data(), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new ClientConnection.Data(SimulationSettings.TotalHeartbeatsBeforeTimeout), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new PlayerInput.Data(new Joystick(xAxis: 0, yAxis: 0,zAxis: 0)), CommonRequirementSets.SpecificClientOnly(clientId))
				.AddComponent(new Health.Data(1000), CommonRequirementSets.PhysicsOnly)
				.AddComponent(new Score.Data(0), CommonRequirementSets.PhysicsOnly)
                .AddComponent(new Status.Data(0), CommonRequirementSets.PhysicsOnly)
                .AddComponent(new PlayerData.Data(name), CommonRequirementSets.PhysicsOnly)
                .Build();

            return playerTemplate;
        }



		public static Entity CreatehealthplusTemplate(Vector3 initialPosition, uint initialRotation)
		{
			var healthplus = EntityBuilder.Begin()
				.AddPositionComponent(initialPosition, CommonRequirementSets.PhysicsOnly)
				.AddMetadataComponent(SimulationSettings.PlayerPrefabHealthplus)
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.AddComponent(new Rotation.Data(Quaternion.identity.ToNativeQuaternion()), CommonRequirementSets.PhysicsOnly)
				.Build();

			return healthplus;
		}

		public static Entity CreatehealthlessTemplate(Vector3 initialPosition, uint initialRotation)
		{
			var healthless= EntityBuilder.Begin()
				.AddPositionComponent(initialPosition, CommonRequirementSets.PhysicsOnly)
				.AddMetadataComponent(SimulationSettings.PlayerPrefabHealthless)
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.AddComponent(new Rotation.Data(Quaternion.identity.ToNativeQuaternion()), CommonRequirementSets.PhysicsOnly)
				.Build();

			return healthless;
		}

		public static Entity CreatepointsTemplate(Vector3 initialPosition, uint initialRotation)
		{
			var points = EntityBuilder.Begin()
				.AddPositionComponent(initialPosition, CommonRequirementSets.PhysicsOnly)
				.AddMetadataComponent(SimulationSettings.PlayerPrefabPoints)
				.SetPersistence(true)
				.SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
				.AddComponent(new Rotation.Data(Quaternion.identity.ToNativeQuaternion()), CommonRequirementSets.PhysicsOnly)
				.Build();

			return points;
		}
     


    }
}
