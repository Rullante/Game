using UnityEngine;
using Improbable.Player;
using Improbable.Unity.Visualizer;

namespace Assets.Gamelogic.Shoots
{
	// This MonoBehaviour will be enabled on both client and server-side workers
	public class ShootFirer : MonoBehaviour
	{

		[Require] private PlayerInput.Reader PlayerInputReader;
				private Shoot shoot;

				private void OnEnable()
				{
					PlayerInputReader.FireTriggered.Add (OnFire);
	
				}

				private void OnDisable()
				{
					PlayerInputReader.FireTriggered.Remove(OnFire);
		
				}

				private void OnFire(Fire fire)
				{
                // Respond to FireLeft event
                var dir = new Vector3(PlayerInputReader.Data.joystick.xAxis, 0, PlayerInputReader.Data.joystick.yAxis);
                AttemptToFireCannons(dir);
                }

			

				private void Start()
				{
					// Cache entity's cannon gameobject
					shoot = gameObject.GetComponent<Shoot>();
				}

				public void AttemptToFireCannons(Vector3 direction)
				{
					if (shoot != null)
					{
						shoot.Fire(direction);
					}
				}
			}
		}

		
		
