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
    private Rigidbody rb;


	void OnEnable()
	{
		cannonFirer = GetComponent<ShootFirer>();
        rb = GetComponent<Rigidbody>();
	}



    void Update ()
	{
		var xAxis = Input.GetAxis("Horizontal");
		var yAxis = Input.GetAxis("Vertical");

		var update = new PlayerInput.Update();
		update.SetJoystick(new Joystick(xAxis, yAxis,0));
		PlayerInputWriter.Send(update);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerInputWriter.Send(new PlayerInput.Update().SetJoystick(new Joystick(0, 0, 10)));


        }

        if (Input.GetKeyDown(KeyCode.Q))
		{
			PlayerInputWriter.Send(new PlayerInput.Update().AddFire(new Fire()));
		}
		}
	}

