using UnityEngine;

namespace BallBattle
{
	public class BallHandler : MonoBehaviour
	{
		[SerializeField] private GameData gameData_;
		[SerializeField] private float xLimit_;
		[SerializeField] private float YLimit_;

		private Transform ball;
		private Vector3 randomPos;

		public void GenerateBall()
		{
			if (ball == null)
				ball = gameData_.BallTransform;

			ball.parent = null;

			randomPos.x = Random.Range(-xLimit_, xLimit_);
			randomPos.y = gameData_.CurrState == GameData.GameState.PlayerAttack ? Random.Range(-YLimit_, 0f) : Random.Range(0f, YLimit_);
			randomPos.z = ball.position.z;
			ball.position = randomPos;
		}

		public void GenerateBallMaze(Vector2 _blockPos)
		{
			ball.parent = null;

			randomPos = _blockPos;
			ball.position = randomPos;
		}
	}
}
