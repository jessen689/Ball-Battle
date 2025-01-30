using System;
using UnityEngine;

namespace BallBattle
{
	public class TimerHandler : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI timerText;

		private float timeCounter;
		private bool isCounting = false;

		public event Action OnTimeOut;

		private void Update()
		{
			if (!isCounting) return;

			timeCounter -= Time.deltaTime;
			if (timeCounter < 0)
			{
				//TIMEOUT
				timeCounter = 0f;
				OnTimeOut?.Invoke();
			}
			UpdateUITimer();
		}

		private void UpdateUITimer()
		{
			timerText.text = $"TIME LEFT: {Mathf.CeilToInt(timeCounter)}s";
		}

		public void StartCounting(float _time)
		{
			timeCounter = _time;
			SetCounting(true);
		}

		public void SetCounting(bool _value)
		{
			isCounting = _value;
			UpdateUITimer();
		}
	}
}