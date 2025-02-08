using BallBattle.Soldier;
using UnityEngine;

namespace BallBattle
{
	public class FieldHandler : MonoBehaviour
	{
		[SerializeField] private bool isPlayerSide_;
		[SerializeField] private Energy.EnergyHandler energyHandler_;

		public SoldierID SummonedType { get; private set; }

		private const int ATTACKER_COST = 2;
		private const int DEFENDER_COST = 3;

		public void SetType(SoldierID _type)
		{
			SummonedType = _type;
		}

		public void TriggerSummonSoldier(Vector3 _pos)
		{
			if(energyHandler_.UseEnergy(SummonedType == SoldierID.Attacker ? ATTACKER_COST : DEFENDER_COST))
				GameEvents.Instance.SpawnSoldier(SummonedType, isPlayerSide_, _pos);
		}
	}
}
