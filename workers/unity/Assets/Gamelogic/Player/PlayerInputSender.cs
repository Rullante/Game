using Assets.Gamelogic.Shoot;
using UnityEngine;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using Improbable.Player;

[WorkerType(WorkerPlatform.UnityClient)]
public class PlayerInputSender : MonoBehaviour
{

	[Require] private PlayerInput.Writer PlayerInputWriter;
	private ShootFirer cannonFirer;


	void OnEnable()
	{
		cannonFirer = GetComponent<ShootFirer>();
	
	}

	void Update ()
	{
		var xAxis = Input.GetAxis("Horizontal");
		var yAxis = Input.GetAxis("Vertical");

		var update = new PlayerInput.Update();
		update.SetJoystick(new Joystick(xAxis, yAxis));
		PlayerInputWriter.Send(update);

		if (Input.GetKeyDown(KeyCode.Q))
		{
			PlayerInputWriter.Send(new PlayerInput.Update().AddFire(new Fire()));
		}
		}
	}

