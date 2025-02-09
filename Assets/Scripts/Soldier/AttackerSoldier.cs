using System;
using UnityEngine;

namespace BallBattle.Soldier
{
	public class AttackerSoldier : SoldierBase, ICaughtable
	{
		[SerializeField] private float holdingBallSpeed_;
		[SerializeField] private float holdingBallRange_;
		[SerializeField] private Transform holdingBallParent_;
		[SerializeField] private GameObject ballMarker;

		private Transform targetGate;
		private Transform targetBall;
		private Vector3 tempDirection;
		public event Action<Transform> OnPassingBall;

		public bool IsHoldingBall { get; private set; } = false;

		#region Interface Property
		public bool IsCaughtable { get; private set; }
		#endregion

		private const string TAG_TO_DESTROY = "Wall";

		#region Interface Function
		public void GettingCaught()
		{
			if (!IsActiveMode) return;
			//kick ball to nearest attacker
			OnPassingBall?.Invoke(transform);
			IsHoldingBall = false;
			ballMarker.SetActive(false);
			targetBall.parent = null;
			SetActiveMode(false);
			soldierAnimator_.PlayAnim(SoldierAnimHandler.AnimID.Fall);
		}

		public override void InitializeSoldier(bool _isPlayer, Color _colorFlag)
		{
			base.InitializeSoldier(_isPlayer, _colorFlag);
			ballMarker.SetActive(false);
			IsHoldingBall = false;
		}
		#endregion

		public void SetTarget(Transform _ball, Transform _gate)
		{
			targetBall = _ball;
			targetGate = _gate;
		}

		public void HoldingBall()
		{
			ballMarker.SetActive(true);
			IsHoldingBall = true;
			targetBall.parent = holdingBallParent_;
			targetBall.position = holdingBallParent_.position;
		}

		private void Update()
		{
			TriggerMove(IsActiveMode);
			IsCaughtable = IsActiveMode && IsHoldingBall;

			if (IsHoldingBall)
				Move(targetGate.position - transform.position, holdingBallSpeed_);
			else if (!IsHoldingBall && targetBall.parent == null && IsActiveMode) //not holding ball and no one holding ball
			{
				Move(targetBall.position - transform.position, MoveSpeed);
				if (Vector2.Distance(targetBall.position, transform.position) <= holdingBallRange_)
				{
					HoldingBall();
				}
			}
			else
			{
				tempDirection = targetGate.position - transform.position;
				tempDirection.x = 0;
				Move(tempDirection, MoveSpeed);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag(TAG_TO_DESTROY))
			{
				GameEvents.Instance.RemoveAtkSoldier(this);
				IsCaughtable = false;
			}
			else if(other.transform == targetGate && IsHoldingBall)
			{
				GameEvents.Instance.MatchFinished(IsPlayer ? Score.MatchResult.PlayerWin : Score.MatchResult.EnemyWin);
				IsHoldingBall = false;
			}
		}
	}
}