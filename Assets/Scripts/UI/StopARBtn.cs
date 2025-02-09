using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BallBattle.UI
{
	[RequireComponent(typeof(Button))]
	public class StopARBtn : MonoBehaviour
	{
		[SerializeField] private GameObject loadingObj_;
		private Button stopBtn;

		private const string MAIN_SCENE_STRING = "Main";

		private void Awake()
		{
			stopBtn = GetComponent<Button>();
			stopBtn.onClick.AddListener(BackToMainScene);
		}

		private void BackToMainScene()
		{
			StartCoroutine(LoadingScene());
		}

		private IEnumerator LoadingScene()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MAIN_SCENE_STRING);

			while (!asyncLoad.isDone)
			{
				loadingObj_.SetActive(true);
				yield return null;
			}
			loadingObj_.SetActive(false);
		}
	}
}
