using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallBattle.UI
{
	public class UIManager : MonoBehaviour
	{
		public enum UIID
		{
			HUD,
			GameOver,
			MainMenu
		}

		[Serializable] private struct UIReference
		{
			public UIID id;
			public GameObject canvas;
		}

		[SerializeField] private List<UIReference> uiReferences = new List<UIReference>();

		private void OnEnable()
		{
			GameEvents.Instance.OnOpenMenu += OpenMenu;
			GameEvents.Instance.OnCloseMenu += CloseMenu;
		}

		private void OnDisable()
		{
			GameEvents.Instance.OnOpenMenu -= OpenMenu;
			GameEvents.Instance.OnCloseMenu -= CloseMenu;
		}

		private void OpenMenu(UIID _id)
		{
			foreach(var ui in uiReferences)
			{
				if (ui.id == _id)
				{
					ui.canvas.SetActive(true);
					return;
				}
			}
		}

		private void CloseMenu(UIID _id)
		{
			foreach (var ui in uiReferences)
			{
				if (ui.id == _id)
				{
					ui.canvas.SetActive(false);
					return;
				}
			}
		}
	}
}
