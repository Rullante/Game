using Assets.Gamelogic;
using UnityEngine;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using Improbable.Player;

[WorkerType(WorkerPlatform.UnityClient)]
public class PlayerInputSender : MonoBehaviour
{

	[Require] private PlayerInput.Writer PlayerInputWriter;
	private CannonFirer cannonFirer;


	void OnEnable()
	{
		cannonFirer = GetComponent<CannonFirer>();
	
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
			// Port broadside (Fire the left cannons)
			if (cannonFirer != null)
			{
				cannonFirer.AttemptToFireCannons(transform.forward);
			}
		}
	}
}
