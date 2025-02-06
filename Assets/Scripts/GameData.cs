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

		[Header("Color Flag")]
		[SerializeField]
		private Color playerColorFlag_;
		[SerializeField]
		private Color enemyColorFlag_;

		[Header("Other References")]
		[SerializeField]
		private Transform ballTransform_;
		[SerializeField]
		private Transform playerGateTransform_;
		[SerializeField]
		private Transform enemyGateTransform_;
		
		[HideInInspector] public int currRound;

		public float MatchTime { get { return matchTime_; } }
		public int MaxRounds { get { return maxRounds_; } }
		public Color PlayerColorFlag { get { return playerColorFlag_; } }
		public Color EnemyColorFlag { get { return enemyColorFlag_; } }
		public Transform BallTransform { get { return ballTransform_; } }
		public Transform PlayerGateTransform { get { return playerGateTransform_; } }
		public Transform EnemyGateTransform { get { return enemyGateTransform_; } }
		public GameState CurrState { get; private set; }

		public void SetState(GameState _state)
		{
			CurrState = _state;
		}
	}
}
