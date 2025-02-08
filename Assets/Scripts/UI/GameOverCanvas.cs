using UnityEngine;
using UnityEngine.UI;

namespace BallBattle.UI
{
	public class GameOverCanvas : MonoBehaviour
	{
		[SerializeField] private GameData gameData_;
		[SerializeField] private Button retryBtn_;
		[SerializeField] private Button exitBtn_;
		[SerializeField] private TMPro.TextMeshProUGUI winText_;

		private const string WIN_STRING = "YOU WIN!";
		private const string LOSE_STRING = "YOU LOSE!";

		private void Awake()
		{
			retryBtn_.onClick.AddListener(RetryGame);
			exitBtn_.onClick.AddListener(OpenMainMenu);
		}

		private void OnEnable()
		{
			winText_.text = gameData_.IsWinGame ? WIN_STRING : LOSE_STRING;
		}

		private void OpenMainMenu()
		{
			GameEvents.Instance.OpenMenu(UIManager.UIID.MainMenu);
			CloseMenu();
		}

		private void RetryGame()
		{
			GameEvents.Instance.RetryGame();
			CloseMenu();
		}

		private void CloseMenu()
		{
			gameObject.SetActive(false);
		}
	}
}
