using BallBattle.Score;
using BallBattle.Soldier;
using System;
using UnityEngine;

namespace BallBattle
{
	public class GameEvents
	{
		//Singleton
		private static readonly GameEvents instance = new GameEvents();
		public static GameEvents Instance { get { return instance; } }

		//Gameflow Events
		public event Action OnStartingGame;
		public event Action<MatchResult> OnMatchFinished;
		public event Action<bool> OnPenaltyFinished;
		public event Action OnRetryGame;

		//Soldier Events
		public event Action<SoldierID, bool, Vector3> OnSpawnSoldier;
		public event Action OnRemoveAllSoldier;
		public event Action<AttackerSoldier> OnRemoveAttacker;
		public event Action<DefenderSoldier> OnRemoveDefender;

		//UI Events
		public event Action<UI.UIManager.UIID> OnOpenMenu;
		public event Action<UI.UIManager.UIID> OnCloseMenu;

		#region Gameflow Action Invoke
		public void StartGame()
		{
			OnStartingGame?.Invoke();
		}

		public void MatchFinished(MatchResult _result)
		{
			OnMatchFinished?.Invoke(_result);
		}

		public void PenaltyFinished(bool _isWin)
		{
			OnPenaltyFinished?.Invoke(_isWin);
		}

		public void RetryGame()
		{
			OnRetryGame?.Invoke();
		}
		#endregion

		#region Soldier Action Invoke
		public void SpawnSoldier(SoldierID _type, bool _isPlayer, Vector3 _pos)
		{
			OnSpawnSoldier?.Invoke(_type, _isPlayer, _pos);
		}

		public void RemoveAllSoldier()
		{
			OnRemoveAllSoldier?.Invoke();
		}

		public void RemoveAtkSoldier(AttackerSoldier _target)
		{
			OnRemoveAttacker?.Invoke(_target);
		}

		public void RemoveDefSoldier(DefenderSoldier _target)
		{
			OnRemoveDefender?.Invoke(_target);
		}
		#endregion

		#region UI Action Invoke
		public void OpenMenu(UI.UIManager.UIID _id)
		{
			OnOpenMenu?.Invoke(_id);
		}

		public void CloseMenu(UI.UIManager.UIID _id)
		{
			OnCloseMenu?.Invoke(_id);
		}
		#endregion
	}
}
