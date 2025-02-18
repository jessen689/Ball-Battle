﻿using System.Collections;
using UnityEngine;

namespace BallBattle.Soldier
{
	public class SoldierBase : MonoBehaviour, ISoldier
	{
		#region Interface Properties
		public SoldierID soldierID { get { return GetComponent<AttackerSoldier>() ? SoldierID.Attacker : SoldierID.Defender; }}
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
		[SerializeField] private GameObject moveDirectionMarker_;
		[SerializeField] protected SoldierAnimHandler soldierAnimator_;

		#region Interface Function
		public virtual void InitializeSoldier(bool _isPlayer, Color _colorFlag)
		{
			IsPlayer = _isPlayer;
			SoldierColorFlag = _colorFlag;
			moveDirectionMarker_.SetActive(false);
			SetActiveMode(false, true);
			soldierAnimator_.PlayAnim(SoldierAnimHandler.AnimID.Spawn);
		}

		public void Move(Vector3 _direction, float _speed)
		{
			moveDirectionMarker_.SetActive(IsMoving);
			if (!IsMoving) return;
			//Debug.Log($"moving to {_direction} at {_speed} speed");
			_direction.z = 0;
			transform.position += _speed * Time.deltaTime * _direction.normalized;
			if(_direction.normalized != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(_direction.normalized, transform.up);

			soldierAnimator_.PlayAnim(SoldierAnimHandler.AnimID.Run);
		}

		public void TriggerMove(bool _value)
		{
			IsMoving = _value;
		}

		public void SetActiveMode(bool _activeValue, bool _isFirstTime = false)
		{
			if (!gameObject.activeSelf)
				return;

			IsActiveMode = _activeValue;

			if (_activeValue)
				SetSoldierColor(SoldierColorFlag);
			else
			{
				SetSoldierColor(inactiveColor_);

				if(_isFirstTime)
					StartCoroutine(ActivationDelay(spawnDelay_));
				else
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
