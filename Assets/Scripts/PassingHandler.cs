using BallBattle.Soldier;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBattle
{
	public class PassingHandler : MonoBehaviour
	{
		[SerializeField] private float passingSpeed_;
		[SerializeField] private GameData gameData_;

		private float nearestTarget;
		private float tempDistance;
		private AttackerSoldier passTarget;

		private const float RECEIVE_BALL_DISTANCE = .2f;

		public void PassBallToNearby(Transform _passer, Transform _ball, List<AttackerSoldier> _activeAttackers)
		{
			nearestTarget = -1;
			foreach(var soldier in _activeAttackers)
			{
				if (soldier.IsActiveMode && soldier.transform != _passer)
				{
					tempDistance = Vector3.Distance(_passer.position, soldier.transform.position);
					if (tempDistance < nearestTarget || nearestTarget == -1)
					{
						passTarget = soldier;
						nearestTarget = tempDistance;
					}
				}
			}

			if(nearestTarget == -1)
			{
				//no nearby active target
				Debug.Log("NO OTHER ACTIVE ATTACKER FOUND!");
				GameEvents.Instance.MatchFinished(gameData_.CurrState == GameData.GameState.PlayerAttack ? Score.MatchResult.EnemyWin : Score.MatchResult.PlayerWin);
			}
			else
			{
				StartCoroutine(Passing(_ball));
			}
		}

		private IEnumerator Passing(Transform _ball)
		{
			while(Vector3.Distance(_ball.position, passTarget.transform.position) > RECEIVE_BALL_DISTANCE)
			{
				if (!passTarget.IsActiveMode)
				{
					_ball.parent = null;
					StopAllCoroutines();
				}
				
				_ball.position += passingSpeed_ * Time.deltaTime * (passTarget.transform.position - _ball.position).normalized;
				yield return new WaitForEndOfFrame();
			}
			passTarget.HoldingBall();
			passTarget.TriggerMove(true);
		}
	}
}
