using UnityEngine;

namespace BallBattle.Soldier
{
	public class DefenderSoldier : SoldierBase, IDetecting
	{
		#region Interface Property
		public bool IsDetecting { get; set; } = false;

		public float DetectionRange { get; }
		#endregion

		[SerializeField] private float caughtRange_;
		[SerializeField] private float returnSpeed_;

		private Vector3 initialPos;
		private Quaternion initialRotation;
		private Transform targetMove;
		private ICaughtable currTarget;

		private const float RANGE_TO_SNAP = .1f;

		#region Interface Function
		public void DetectedInRange(GameObject _objDetected)
		{
			if (_objDetected.TryGetComponent(out ICaughtable target))
			{
				Debug.Log($"Detected :{target}");
				currTarget = target;
				targetMove = _objDetected.transform;

				if (!target.IsCaughtable)
					return;

				StartChase();
			}
		}

		public override void InitializeSoldier(bool _isPlayer, Color _colorFlag)
		{
			base.InitializeSoldier(_isPlayer, _colorFlag);
			TriggerMove(false);
			IsDetecting = false;
			currTarget = null;
		}
		#endregion

		public void SetInitialLocation(Vector3 _position, Quaternion _rotation)
		{
			initialPos = _position;
			initialRotation = _rotation;
		}

		private void StartChase()
		{
			IsDetecting = true;
			TriggerMove(true);
		}

		private void Update()
		{
			if (currTarget != null)
			{
				if (!targetMove.gameObject.activeSelf)
				{
					currTarget = null;
					return;
				}

				if (currTarget.IsCaughtable && !IsDetecting && IsActiveMode)
					StartChase();
			}

			if (IsDetecting && IsActiveMode)
			{
				//Debug.Log(targetMove.position + " -- " + MoveSpeed);
				Move(targetMove.position - transform.position, MoveSpeed);
				
				if(Vector3.Distance(targetMove.position, transform.position) < caughtRange_ || !currTarget.IsCaughtable)
				{
					Debug.Log("caught target!");
					IsDetecting = false;
					SetActiveMode(false);
					currTarget.GettingCaught();
					currTarget = null;
				}
			}
			else if(!IsDetecting && !IsActiveMode)
			{
				Move(initialPos - transform.position, returnSpeed_);

				if(Vector3.Distance(initialPos, transform.position) < RANGE_TO_SNAP)
				{
					transform.position = initialPos;
					transform.rotation = initialRotation;
					TriggerMove(false);
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			Debug.Log(other.name);
			if(!IsDetecting)
				DetectedInRange(other.gameObject);
		}

		private void OnTriggerExit(Collider other)
		{
			if(other.TryGetComponent(out ICaughtable target))
			{
				if (target == currTarget)
					currTarget = null;
			}
		}
	}
}
