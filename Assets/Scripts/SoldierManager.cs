using BallBattle.Soldier;
using BallBattle.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallBattle
{
	public class SoldierManager : MonoBehaviour
	{
		[Serializable]
		private struct SoldierData
		{
			public SoldierID id;
			public GameObject prefabs;
		}

		[SerializeField] private List<SoldierData> soldiers_ = new List<SoldierData>();
		[SerializeField] private GameData gameData_;
		[SerializeField] private PassingHandler passingHandler_;

		private ObjectPooler<DefenderSoldier> soldierDefPooler = new ObjectPooler<DefenderSoldier>();
		private ObjectPooler<AttackerSoldier> soldierAtkPooler = new ObjectPooler<AttackerSoldier>();

		private void OnEnable()
		{
			GameEvents.Instance.OnSpawnSoldier += SummonSoldier;
			GameEvents.Instance.OnRemoveAllSoldier += RemoveAllSoldiers;
			GameEvents.Instance.OnRemoveAttacker += RemoveAttackSoldier;
			GameEvents.Instance.OnRemoveDefender += RemoveDefendSoldier;
		}

		private void OnDisable()
		{
			GameEvents.Instance.OnSpawnSoldier -= SummonSoldier;
			GameEvents.Instance.OnRemoveAllSoldier -= RemoveAllSoldiers;
			GameEvents.Instance.OnRemoveAttacker -= RemoveAttackSoldier;
			GameEvents.Instance.OnRemoveDefender -= RemoveDefendSoldier;
		}

		private void SummonSoldier(SoldierID _soldierType, bool _isPlayerSide, Vector3 _position)
		{
			foreach(var data in soldiers_)
			{
				if(data.id == _soldierType)
				{
					//var summoned = _soldierType == SoldierID.Attacker ? soldierAtkPooler.SummonObject(data.prefabs) : soldierDefPooler.SummonObject(data.prefabs);
					if(_soldierType == SoldierID.Attacker)
					{
						if (soldierAtkPooler.SummonObject(data.prefabs).TryGetComponent(out AttackerSoldier tempAtkSummoned))
						{
							tempAtkSummoned.InitializeSoldier(_isPlayerSide, _isPlayerSide ? gameData_.PlayerColorFlag : gameData_.EnemyColorFlag);
							tempAtkSummoned.SetTarget(gameData_.BallTransform, _isPlayerSide ? gameData_.EnemyGateTransform : gameData_.PlayerGateTransform);
							tempAtkSummoned.OnPassingBall += TriggerPass;
							_position.z = data.prefabs.transform.position.z;
							tempAtkSummoned.transform.position = _position;
						}
						else
							Debug.Log("Attacker not found...");
					}
					else
					{
						if (soldierDefPooler.SummonObject(data.prefabs).TryGetComponent(out DefenderSoldier tempDefSummoned))
						{
							tempDefSummoned.InitializeSoldier(_isPlayerSide, _isPlayerSide ? gameData_.PlayerColorFlag : gameData_.EnemyColorFlag);
							_position.z = data.prefabs.transform.position.z;
							tempDefSummoned.SetInitialLocation(_position, Quaternion.LookRotation(((_isPlayerSide ? gameData_.EnemyGateTransform : gameData_.PlayerGateTransform).position - _position).normalized, Vector3.back));
							tempDefSummoned.transform.position = _position;
						}
						else
							Debug.Log("Defender not found...");
					}
					return;
				}
			}
		}

		private void RemoveAttackSoldier(AttackerSoldier _target)
		{
			_target.OnPassingBall -= TriggerPass;
			soldierAtkPooler.RemoveObject(_target.gameObject);
		}

		private void RemoveDefendSoldier(DefenderSoldier _target)
		{
			soldierDefPooler.RemoveObject(_target.gameObject);
		}

		private void RemoveAllSoldiers()
		{
			foreach(var atker in soldierAtkPooler.GetActivePool()._class)
			{
				atker.OnPassingBall -= TriggerPass;
			}
			soldierAtkPooler.RemoveAllObject();
			soldierDefPooler.RemoveAllObject();
		}

		private void TriggerPass(Transform _soldierPass)
		{
			passingHandler_.PassBallToNearby(_soldierPass.transform, gameData_.BallTransform, soldierAtkPooler.GetActivePool()._class);
		}
	}
}
