using UnityEngine;
using UnityEngine.UI;

namespace BallBattle.UI
{
	public class MainMenuCanvas : MonoBehaviour
	{
		[SerializeField] private Button playBtn_;
		[SerializeField] private Button exitBtn_;

		private void Awake()
		{
			playBtn_.onClick.AddListener(StartGame);
			exitBtn_.onClick.AddListener(CloseGame);
		}

		private void CloseGame()
		{
			Application.Quit();
		}

		private void StartGame()
		{
			GameEvents.Instance.StartGame();
			gameObject.SetActive(false);
		}
	}
}
