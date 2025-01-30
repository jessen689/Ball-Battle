using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallBattle.Score
{
	public class ScoreHandler : MonoBehaviour
	{
		[SerializeField] private List<Image> matchesResult_ = new List<Image>();

		[Header("Color Marker")]
		[SerializeField] private Color playerColor_;
		[SerializeField] private Color enemyColor_;
		[SerializeField] private Color drawColor_;
		[SerializeField] private Color emptyColor_;

		public void ResetAllMatches()
		{
			foreach(var img in matchesResult_)
			{
				img.color = emptyColor_;
			}
		}

		public void SetMatchResult(int _match, MatchResult _result)
		{
			matchesResult_[_match - 1].color = _result == MatchResult.PlayerWin ? playerColor_ : _result == MatchResult.EnemyWin ? enemyColor_ : drawColor_;
		}

		public (int _playerPoint, int _enemyPoint) GetAllResult()
		{
			int player = 0;
			int enemy = 0;

			foreach(var result in matchesResult_)
			{
				if (result.color == playerColor_)
					player++;
				else if (result.color == enemyColor_)
					enemy++;
			}

			return (player, enemy);
		}
	}
}
