using UnityEngine;

namespace BallBattle.Soldier
{
	public class PenaltySoldier : MonoBehaviour, IMoveable
	{
		[SerializeField] private GameObject moveDirectionMarker_;
		[SerializeField] private GameObject holdingBallMarker_;
		[SerializeField] private float moveSpeed_;
		[SerializeField] private float moveWBallSpeed_;
		[SerializeField] private Transform ballParent_;
		[SerializeField] private Transform targetGate_;
		[SerializeField] private Transform targetBall_;

		#region Interface Property
		public float MoveSpeed { get { return moveSpeed_; } }

		public bool IsMoving { get; private set; }
		#endregion

		private Vector3 tempPos;
		private bool isHoldingBall;

		private const float REACH_DESTINATION_DISTANCE = .25f;
		private const float GET_BALL_DISTANCE = .35f;
		private const string GATE_TAG = "Gate";

		#region Interface Function
		public void Move(Vector3 _direction, float _speed)
		{
			moveDirectionMarker_.SetActive(IsMoving);
			if (!IsMoving) return;

			_direction.z = 0;
			transform.position += _speed * Time.deltaTime * _direction.normalized;
			if (_direction.normalized != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(_direction.normalized, transform.up);
		}

		public void TriggerMove(bool _value)
		{
			IsMoving = _value;
		}
		#endregion

		public void Spawn(Vector3 _point)
		{
			_point.z = transform.position.z;
			transform.position = _point;
			gameObject.SetActive(true);
		}

		private void Update()
		{
			holdingBallMarker_.SetActive(isHoldingBall);

			if (Input.GetMouseButton(0))
			{
				tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				TriggerMove(Vector2.Distance(tempPos, transform.position) > REACH_DESTINATION_DISTANCE);
				Move(tempPos - transform.position, MoveSpeed);
			}
			else
			{
				TriggerMove(false);
			}

			if(Vector2.Distance(targetBall_.position, transform.position) <= GET_BALL_DISTANCE && !isHoldingBall)
			{
				targetBall_.parent = ballParent_;
				targetBall_.position = ballParent_.position;
				isHoldingBall = true;
			}

			//if(isHoldingBall && Vector2.Distance(targetGate_.position, transform.position) <= REACH_DESTINATION_DISTANCE)
			//{
			//	targetBall_.parent = null;
			//	GameEvents.Instance.PenaltyFinished(true);
			//}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(GATE_TAG))
			{
				if(other.transform == targetGate_)
				{
					targetBall_.parent = null;
					GameEvents.Instance.PenaltyFinished(true);
				}
			}
		}
	}
}
