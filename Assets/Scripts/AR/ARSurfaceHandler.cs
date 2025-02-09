using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BallBattle.AR
{
	public class ARSurfaceHandler : MonoBehaviour
	{
		[SerializeField] private ARPlaneManager planeManager_;
		[SerializeField] private ARRaycastManager arRaycastManager_;
		[SerializeField] private GameObject gameplayParent_;

		private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
		private bool isPlayed = false;

		private void OnEnable()
		{
			GameEvents.Instance.OnOpenMenu += ResetOnMainMenu;
		}

		private void OnDisable()
		{
			GameEvents.Instance.OnOpenMenu -= ResetOnMainMenu;
		}

		private void Update()
		{
			if(Input.touchCount > 0 && !isPlayed)
			{
				if(Input.touches[0].phase == TouchPhase.Began)
				{
					if(arRaycastManager_.Raycast(Input.GetTouch(0).position, raycastHits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
					{
						var hit = raycastHits[0].pose;

						gameplayParent_.transform.position = hit.position;
						gameplayParent_.transform.rotation = hit.rotation;
						gameplayParent_.SetActive(true);
						GameEvents.Instance.StartGame();

						isPlayed = true;
					}
				}
			}
		}

		private void ResetOnMainMenu(UI.UIManager.UIID obj)
		{
			if (obj == UI.UIManager.UIID.MainMenu)
				ResetPlay();
		}

		private void ResetPlay()
		{
			isPlayed = false;
		}
	}
}
