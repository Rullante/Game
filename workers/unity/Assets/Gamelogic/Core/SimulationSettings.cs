﻿using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public static class SimulationSettings
    {
        public static readonly string PlayerPrefabName = "Player";
        public static readonly string PlayerCreatorPrefabName = "PlayerCreator";
		public static readonly string PlayerPrefabHealthplus = "health+";
		public static readonly string PlayerPrefabHealthless = "health-";
		public static readonly string PlayerPrefabPoints = "points";


        public static readonly Quaternion InitialThirdPersonCameraRotation = Quaternion.Euler(40, 0, 0);
		public static readonly float InitialThirdPersonCameraDistance = 15;


        public static readonly float HeartbeatCheckIntervalSecs = 3;
        public static readonly uint TotalHeartbeatsBeforeTimeout = 3;
        public static readonly float HeartbeatSendingIntervalSecs = 3;

        public static readonly int TargetClientFramerate = 50;
        public static readonly int TargetServerFramerate = 50;
        public static readonly int FixedFramerate = 60;
		public static readonly float ClientConnectionTimeoutSecs = 7;

		public static readonly int PointsToWin = 5;

        public static bool Flag
        {
            get;
            set;
        }

        public static readonly float PlayerCreatorQueryRetrySecs = 4;
        public static readonly float PlayerEntityCreationRetrySecs = 4;

        public static readonly string DefaultSnapshotPath = Application.dataPath + "/../../../snapshots/default.snapshot";

		public static readonly float PlayerSpawnHeight = 10;
		public static readonly float PlayerAcceleration = 12;

        public static readonly float bonusSpawnDiameter = 300;
        public static readonly float Total = 5;

    }
}
