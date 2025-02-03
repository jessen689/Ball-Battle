using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBattle.Soldier
{
	public class AttackerSoldier : SoldierBase, ICaughtable
	{
		[SerializeField] private float holdingBallSpeed_;
		[SerializeField] private float holdingBallRange_;
		[SerializeField] private Transform holdingBallParent_;
		[SerializeField] private Transform targetGate;
		[SerializeField] private Transform targetBall;

		public bool IsHoldingBall { get; private set; } = false;

		private void Start()
		{
			SetActiveMode(true);
		}

		#region Interface Function
		public void GettingCaught()
		{
			//kick balls to nearest attacker

			SetActiveMode(false);
		}
		#endregion

		private void Update()
		{
			TriggerMove(IsActiveMode);

			if (IsHoldingBall)
				Move(targetGate.position - transform.position, holdingBallSpeed_);
			else if (!IsHoldingBall && targetBall.parent == null) //not holding ball and no one holding ball
			{
				Move(targetBall.position - transform.position, MoveSpeed);
				if (Vector3.Distance(targetBall.position, transform.position) <= holdingBallRange_)
				{
					IsHoldingBall = true;
					targetBall.parent = holdingBallParent_;
					targetBall.position = holdingBallParent_.position;
				}
			}
			else
			{
				Vector3 tempDirection = targetGate.position - transform.position;
				tempDirection.x = 0;
				Move(tempDirection, MoveSpeed);
			}

		}
	}
}