using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BallBattle.Energy
{
	public class EnergyPoint : MonoBehaviour
	{
		[SerializeField] private Image energyFillImg_;
		[SerializeField] private float regenOpacity_;
		[SerializeField] [Tooltip("in seconds")] private float regenTime_;

		private Vector3 fillScale = Vector3.one;
		private Color energyColor;
		private float regenCounter;

		public event Action OnEnergyFilled;

		public void SetEnergyColor(Color _color)
		{
			energyColor = _color;
			energyFillImg_.color = energyColor;
		}

		public void GenerateEnergy()
		{
			regenCounter = 0;
			energyFillImg_.color = new Color(energyFillImg_.color.r, energyFillImg_.color.g, energyFillImg_.color.b, regenOpacity_ / 255f); //divided by 255 to get value between 0 & 1
			StartCoroutine(GeneratingEnergy());
		}

		public void RemoveEnergy()
		{
			fillScale.x = 0f;
			energyFillImg_.transform.localScale = fillScale;
			StopAllCoroutines();
		}

		private IEnumerator GeneratingEnergy()
		{
			while(regenCounter < regenTime_)
			{
				regenCounter += Time.deltaTime;
				fillScale.x = Mathf.Clamp01(regenCounter / regenTime_);
				energyFillImg_.transform.localScale = fillScale;
				yield return null;
			}
			OnEnergyFilled?.Invoke();
			energyFillImg_.color = energyColor;
		}
	}
}
