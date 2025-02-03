using UnityEngine;

namespace BallBattle.Soldier
{
	public interface IMoveable
	{
		float MoveSpeed { get; }
		bool IsMoving { get; }

		void Move(Vector3 _direction, float _speed);
		void TriggerMove(bool _value);
	}
}
