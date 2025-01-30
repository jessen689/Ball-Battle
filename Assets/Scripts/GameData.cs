using UnityEngine;

namespace BallBattle
{
	public class GameData : MonoBehaviour
	{
		public enum GameState
		{
			PlayerAttack,
			PlayerDefend,
			Penalty
		}

		[SerializeField] private float matchTime_;
		[SerializeField] private int maxRounds_;
		
		[HideInInspector] public int currRound;

		public float MatchTime { get { return matchTime_; } }
		public int MaxRounds { get { return maxRounds_; } }
		public GameState CurrState { get; private set; }

		public void SetState(GameState _state)
		{
			CurrState = _state;
		}
	}
}
