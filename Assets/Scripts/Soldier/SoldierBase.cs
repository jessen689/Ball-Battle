using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BallBattle.Soldier
{
	public class SoldierBase : MonoBehaviour, ISoldier
	{
		#region Interface Properties
		public bool IsPlayer { get; set; }
		public Color SoldierColorFlag { get; set; }

		public int EnergyCost { get; protected set; }

		public float MoveSpeed { get { return moveSpeed_; } }

		public bool IsMoving { get; private set; }

		public bool IsActiveMode { get; set; }
		#endregion

		[SerializeField] private float moveSpeed_;
		[SerializeField] private Color inactiveColor_;
		[SerializeField] private float spawnDelay_;
		[SerializeField] private float reactivateTime_;
		[SerializeField] private MeshRenderer soldierRenderer_;

		#region Interface Function
		public void InitializeSoldier(bool _isPlayer, Color _colorFlag)
		{
			IsPlayer = _isPlayer;
			SoldierColorFlag = _colorFlag;
			StartCoroutine(ActivationDelay(spawnDelay_));
		}

		public void Move(Vector3 _direction, float _speed)
		{
			if (!IsMoving) return;
			//Debug.Log($"moving to {_direction} at {_speed} speed");
			transform.position += _speed * Time.deltaTime * _direction.normalized;
			transform.rotation = Quaternion.LookRotation(_direction.normalized, transform.up);
		}

		public void TriggerMove(bool _value)
		{
			IsMoving = _value;
		}

		public void SetActiveMode(bool _activeValue)
		{
			IsActiveMode = _activeValue;

			if (_activeValue)
				SetSoldierColor(SoldierColorFlag);
			else
			{
				SetSoldierColor(inactiveColor_);
				StartCoroutine(ActivationDelay(reactivateTime_));
			}
		}
		#endregion

		private void SetSoldierColor(Color _color)
		{
			soldierRenderer_.material.color = _color;
		}

		private IEnumerator ActivationDelay(float _delay)
		{
			yield return new WaitForSeconds(_delay);
			SetActiveMode(true);
		}
	}
}
