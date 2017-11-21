
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

		var pos = rigidbody.position;
		var positionUpdate = new Position.Update()
			.SetCoords(new Coordinates(pos.x, pos.y, pos.z));
		PositionWriter.Send(positionUpdate);

		var rotationUpdate = new Rotation.Update()
			.SetRotation(rigidbody.rotation.ToNativeQuaternion());
		RotationWriter.Send(rotationUpdate);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.velocity = new Vector3(0.0f, 5, 0.0f);
        }
		if (HealthWriter.Data.currentHealth <= 0){
			HealthWriter.Send (new Health.Update ().SetCurrentHealth (1000));
			transform.position = new Vector3 (0, 10, 0);
    }
}
}