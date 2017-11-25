using Assets.Gamelogic.Shoots;
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
    private bool jump;


	void OnEnable()
	{
		cannonFirer = GetComponent<ShootFirer>();
        rb = GetComponent<Rigidbody>();
        jump = true;
	}

    private void OnCollisionStay(Collision collision)
    {
        jump = true;
    }

    void Update ()
	{
		var xAxis = Input.GetAxis("Horizontal");
		var yAxis = Input.GetAxis("Vertical");

		var update = new PlayerInput.Update();
		update.SetJoystick(new Joystick(xAxis, yAxis,0));
		PlayerInputWriter.Send(update);

        if (Input.GetKeyDown(KeyCode.Space) && jump)
        {
            PlayerInputWriter.Send(new PlayerInput.Update().SetJoystick(new Joystick(0, 0, 10)));
            jump = false;

        }

        if (Input.GetKeyDown(KeyCode.Q))
		{
			PlayerInputWriter.Send(new PlayerInput.Update().AddFire(new Fire()));
		}
		}
	}

