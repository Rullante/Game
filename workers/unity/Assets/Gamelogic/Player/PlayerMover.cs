﻿
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable;
using Improbable.Core;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;


[WorkerType(WorkerPlatform.UnityWorker)]
public class PlayerMover : MonoBehaviour {

	[Require] private Position.Writer PositionWriter;
	[Require] private Rotation.Writer RotationWriter;
	[Require] private Health.Writer HealthWriter;
	[Require] private PlayerInput.Reader PlayerInputReader;
    [Require] private Score.Writer ScoreWriter;
    private Rigidbody rigidbody;



	void OnEnable ()
	{
		rigidbody = GetComponent<Rigidbody>();
    }

	void FixedUpdate ()
	{
        
		var joystick = PlayerInputReader.Data.joystick;
		var direction = new Vector3(joystick.xAxis, 0, joystick.yAxis);
        rigidbody.AddForce(direction * SimulationSettings.PlayerAcceleration);
        rigidbody.AddForce(new Vector3(0,joystick.zAxis,0) * SimulationSettings.PlayerAcceleration);

        var pos = rigidbody.position;
		var positionUpdate = new Position.Update()
			.SetCoords(new Coordinates(pos.x, pos.y, pos.z));
		PositionWriter.Send(positionUpdate);

        
		var rotationUpdate = new Rotation.Update()
			.SetRotation(rigidbody.rotation.ToNativeQuaternion());
		RotationWriter.Send(rotationUpdate);
        
		if (HealthWriter.Data.currentHealth <= 0){
            if (ScoreWriter.Data.numberOfPoints > 0)
            {
                int newScore = ScoreWriter.Data.numberOfPoints - 1;
                ScoreWriter.Send(new Score.Update().SetNumberOfPoints(newScore));
            }
			HealthWriter.Send (new Health.Update ().SetCurrentHealth (1000));
			transform.position = new Vector3 (0, 10, 0);
			if (rigidbody != null)
				rigidbody.velocity = new Vector3 (0, 0, 0);

            
    }
}
}
 