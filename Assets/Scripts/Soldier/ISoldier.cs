using UnityEngine;

namespace BallBattle.Soldier
{
	public interface ISoldier : IMoveable, IActivatable
	{
		SoldierID soldierID { get; }
		bool IsPlayer { get; set; }
		Color SoldierColorFlag { get; set; }
		int EnergyCost { get; }

		void InitializeSoldier(bool _isPlayer, Color _colorFlag);
	}
}
