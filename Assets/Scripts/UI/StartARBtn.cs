using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BallBattle.UI
{
	[RequireComponent(typeof(Button))]
	public class StartARBtn : MonoBehaviour
	{
		[SerializeField] private GameObject loadingCanvas_;
		private Button arBtn;
		private bool tapped = false;

		private const string AR_SCENE_STRING = "AR";

		private void Awake()
		{
			arBtn = GetComponent<Button>();
			arBtn.onClick.AddListener(TryLoadScene);
		}

		private void TryLoadScene()
		{
			if (Permission.HasUserAuthorizedPermission(Permission.Camera))
			{
				StartCoroutine(LoadScene());
			}
			else
			{
				//cannot use PermissionCallbacks, not supported in unity ver 2019
				Permission.RequestUserPermission(Permission.Camera);
				tapped = true;
			}
		}

		private IEnumerator LoadScene()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(AR_SCENE_STRING);

			while (!asyncLoad.isDone)
			{
				loadingCanvas_.SetActive(true);
				yield return null;
			}
			loadingCanvas_.SetActive(false);
		}

		private void OnApplicationFocus(bool focus)
		{
			if (focus && tapped)
			{
				TryLoadScene();
				tapped = false;
			}
		}
	}
}
