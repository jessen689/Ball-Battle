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
		private bool isPassTarget = false;
		public event Action<Transform> OnPassingBall;

		public bool IsHoldingBall { get; private set; } = false;

		#region Interface Property
		public bool IsCaughtable { get; private set; }
		#endregion

		private const float DISTANCE_TO_GOAL = .2f;

		#region Interface Function
		public void GettingCaught()
		{
			//kick ball to nearest attacker
			OnPassingBall?.Invoke(transform);
			IsHoldingBall = false;
			ballMarker.SetActive(false);
			SetActiveMode(false);
		}

		public override void InitializeSoldier(bool _isPlayer, Color _colorFlag)
		{
			base.InitializeSoldier(_isPlayer, _colorFlag);
			ballMarker.SetActive(false);
		}
		#endregion

		public void SetTarget(Transform _ball, Transform _gate)
		{
			targetBall = _ball;
			targetGate = _gate;
		}

		public void SetAsPassTarget()
		{
			isPassTarget = true;
		}

		public void HoldingBall()
		{
			ballMarker.SetActive(true);
			isPassTarget = false;
			IsHoldingBall = true;
			targetBall.parent = holdingBallParent_;
			targetBall.position = holdingBallParent_.position;
		}

		private void Update()
		{
			TriggerMove(IsActiveMode && !isPassTarget);
			IsCaughtable = IsActiveMode && IsHoldingBall;

			if (IsHoldingBall)
			{
				Move(targetGate.position - transform.position, holdingBallSpeed_);
				if(Vector3.Distance(targetGate.position, transform.position) <= DISTANCE_TO_GOAL)
				{
					GameEvents.Instance.MatchFinished(IsPlayer ? Score.MatchResult.PlayerWin : Score.MatchResult.EnemyWin);
					IsHoldingBall = false;
				}
			}
			else if (!IsHoldingBall && targetBall.parent == null) //not holding ball and no one holding ball
			{
				Move(targetBall.position - transform.position, MoveSpeed);
				if (Vector3.Distance(targetBall.position, transform.position) <= holdingBallRange_)
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
	}
}