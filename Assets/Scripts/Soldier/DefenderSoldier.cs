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
				targetMove = _objDetected.transform;
				IsDetecting = true;
				currTarget = target;
				TriggerMove(true);
			}
		}
		#endregion

		private void Start()
		{
			SetActiveMode(true);
			initialPos = transform.position;
			initialRotation = transform.rotation;
		}

		private void Update()
		{
			if (IsDetecting)
			{
				Debug.Log(targetMove.position + " -- " + MoveSpeed);
				Move(targetMove.position - transform.position, MoveSpeed);
				
				if(Vector3.Distance(targetMove.position, transform.position) < caughtRange_)
				{
					Debug.Log("caught target!");
					IsDetecting = false;
					SetActiveMode(false);
					currTarget.GettingCaught();
					TriggerMove(false);
				}
			}
			else if(!IsDetecting && !IsActiveMode)
			{
				TriggerMove(true);
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
			if(!IsDetecting && IsActiveMode)
				DetectedInRange(other.gameObject);
		}
	}
}
