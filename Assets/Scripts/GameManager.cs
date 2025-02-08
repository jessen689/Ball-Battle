using BallBattle.Maze;
using BallBattle.Score;
using UnityEngine;

namespace BallBattle
{
	public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameData gameData_;
		[SerializeField] private TimerHandler timerHandler_;
		[SerializeField] private ScoreHandler scoreHandler_;
		[SerializeField] private Energy.EnergyHandler playerEnergy_;
		[SerializeField] private Energy.EnergyHandler enemyEnergy_;
		[SerializeField] private BallHandler ballHandler_;
		[SerializeField] private MazeGenerator mazeGenerator_;
		[SerializeField] private InputHandler inputHandler_;
		[Header("Field")]
		[SerializeField] private FieldHandler playerField_;
		[SerializeField] private FieldHandler enemyField_;
		[Header("Penalty")]
		[SerializeField] private Soldier.PenaltySoldier penaltySoldier_;

		private void OnEnable()
		{
			timerHandler_.OnTimeOut += TimeOut;
			GameEvents.Instance.OnStartingGame += StartGame;
			GameEvents.Instance.OnRetryGame += StartGame;
			GameEvents.Instance.OnMatchFinished += FinishMatch;
			GameEvents.Instance.OnPenaltyFinished += ShowGameOver;
		}

		private void OnDisable()
		{
			timerHandler_.OnTimeOut -= TimeOut;
			GameEvents.Instance.OnStartingGame -= StartGame;
			GameEvents.Instance.OnRetryGame -= StartGame;
			GameEvents.Instance.OnMatchFinished -= FinishMatch;
			GameEvents.Instance.OnPenaltyFinished -= ShowGameOver;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Z))
				FinishMatch(MatchResult.Draw);
		}

		private void StartGame()
		{
            gameData_.currRound = 1;
			scoreHandler_.ResetAllMatches();
            StartMatch();
		}

        private void StartMatch()
		{
			mazeGenerator_.HideMaze();
			inputHandler_.enabled = true;
			penaltySoldier_.gameObject.SetActive(false);
			if (gameData_.currRound == 1) //check first round?
			{
				gameData_.SetState(GameData.GameState.PlayerAttack);
			}
			else
			{
				gameData_.SetState(gameData_.CurrState == GameData.GameState.PlayerAttack ? GameData.GameState.PlayerDefend : GameData.GameState.PlayerAttack);
			}

			playerField_.SetType(gameData_.CurrState == GameData.GameState.PlayerAttack ? Soldier.SoldierID.Attacker : Soldier.SoldierID.Defender);
			enemyField_.SetType(gameData_.CurrState == GameData.GameState.PlayerAttack ? Soldier.SoldierID.Defender : Soldier.SoldierID.Attacker);
			timerHandler_.StartCounting(gameData_.MatchTime);
			playerEnergy_.InitializeEnergy();
			enemyEnergy_.InitializeEnergy();
			ballHandler_.GenerateBall();
		}

		private void TimeOut()
		{
			timerHandler_.SetCounting(false);
			if (gameData_.CurrState == GameData.GameState.Penalty)
				ShowGameOver(false);
			else
				FinishMatch(MatchResult.Draw);
		}

		private void FinishMatch(MatchResult _result)
		{
			scoreHandler_.SetMatchResult(gameData_.currRound, _result);
			GameEvents.Instance.RemoveAllSoldier();

			if (gameData_.currRound < gameData_.MaxRounds)
			{
                gameData_.currRound++;
                StartMatch();
			}
			else
			{
				CalculateMatchResult();
			}
		}

        private void CalculateMatchResult()
		{
			var (_playerPoint, _enemyPoint) = scoreHandler_.GetAllResult();

			if (_playerPoint == _enemyPoint)
				StartPenalty();
			else if(_playerPoint > _enemyPoint)
			{
				//player win the game
				Debug.Log("PLAYER WIN!!!");
				timerHandler_.SetCounting(false);
				ShowGameOver(true);
			}
			else
			{
				//enemy win the game
				Debug.Log("ENEMY WIN!!!");
				ShowGameOver(false);
				timerHandler_.SetCounting(false);
			}
			playerEnergy_.ResetEnergy();
			enemyEnergy_.ResetEnergy();
		}

        private void StartPenalty()
		{
            gameData_.SetState(GameData.GameState.Penalty);
			timerHandler_.StartCounting(gameData_.MatchTime);
			mazeGenerator_.InitializeMaze();
			ballHandler_.GenerateBallMaze(mazeGenerator_.GetRandomBlock().transform.position);
			inputHandler_.enabled = false;
			penaltySoldier_.Spawn(mazeGenerator_.GetFirstGeneratedBlock().transform.position);
			Debug.Log("PENALTY!!!");
		}

		private void ShowGameOver(bool _isWin)
		{
			penaltySoldier_.gameObject.SetActive(false);
			gameData_.SetWin(_isWin);
			GameEvents.Instance.OpenMenu(UI.UIManager.UIID.GameOver);
		}
	}
}
