using UnityEngine;

namespace Assets.Gamelogic.Shoots
{
	public class Shoot : MonoBehaviour
	{
		[SerializeField]
		private GameObject ShootPrefab;

		private float InitialVelocity = 100f;
		private float MaxPitch = 25f;
		private float MaxAimDeviationAngle = 1f;
		private float ShootDistance = 40f;

		private float timeShootsWereLastFired;
		private float shootRechargeTime;
		private Collider[] firerColliders;
		private float maxRange = 1f;



		void Start()
		{
			maxRange = CalculateMaxRange();
			firerColliders = gameObject.GetComponentsInChildren<Collider>();
		}

		public void Fire(Vector3 dir)
		{
			var firingPitch = Mathf.Clamp01(ShootDistance / maxRange) * MaxPitch;

			if (ShootPrefab != null)
			{
				var shoot = Instantiate(ShootPrefab, transform.position+dir*0.6f, transform.rotation) as GameObject;
				var entityId = gameObject.EntityId();
				shoot.GetComponent<DestroyShoot>().firerEntityId = entityId;
				EnsureShootWillNotCollideWithFirer(shoot);
				FireShoot(shoot, dir, firingPitch);
				//cannonFireAudioSource.PlayOneShot(cannonFireAudioClips[Random.Range(0, cannonFireAudioClips.Length)]);
			}
		}

		private void FireShoot(GameObject shoot, Vector3 firingDirection, float firingPitch)
		{
			var shootRigidbody = shoot.GetComponent<Rigidbody>();

			if (shootRigidbody != null)
			{
				var deviation = Vector2.zero;

				// Deviate the aim a little
				if (MaxAimDeviationAngle > 0f)
				{
					deviation = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
					deviation.Normalize();
					deviation *= MaxAimDeviationAngle;
				}

				// Calculate the initial velocity
				var firingDir = Quaternion.LookRotation(firingDirection);
				shootRigidbody.velocity = (firingDir * Quaternion.Euler(-firingPitch + deviation.x, deviation.y, 0f)) * Vector3.forward * InitialVelocity;
			}
			else
			{
				Debug.LogWarning("The shoot is missing its rigidbody");
			}
		}

		private float CalculateMaxRange()
		{
			// Vertical velocity can be calculated using the pitch and initial full velocity:
			var velocity = Mathf.Sin(Mathf.Deg2Rad * MaxPitch) * InitialVelocity;

			// This is how long it will take the fired cannon ball to reach the sea level
			var time = -velocity / (0.5f * Physics.gravity.y);

			// Now let's calculate the distance traveled horizontally in the same amount of time
			return Mathf.Cos(Mathf.Deg2Rad * MaxPitch) * InitialVelocity * time;
		}

		private void EnsureShootWillNotCollideWithFirer(GameObject shoot)
		{
			if (firerColliders == null) return;
			var col = shoot.GetComponent<Collider>();
			if (col == null) return;
			foreach (var c in firerColliders)
			{
				Physics.IgnoreCollision(c, col);
			}
		}
	}
}
