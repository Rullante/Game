using UnityEngine;
using Improbable.Player;
using Improbable.Unity.Visualizer;

namespace Assets.Gamelogic.Shoot
{
	// This MonoBehaviour will be enabled on both client and server-side workers
	public class ShootFirer : MonoBehaviour
	{

		[Require] private PlayerInput.Reader PlayerControlsReader;
				private Shoot shoot;

				private void OnEnable()
				{
					PlayerControlsReader.FireTriggered.Add (OnFire);
	
				}

				private void OnDisable()
				{
					PlayerControlsReader.FireTriggered.Remove(OnFire);
		
				}

				private void OnFire(Fire fire)
				{
					// Respond to FireLeft event
					AttemptToFireCannons(transform.forward);
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

		
		
