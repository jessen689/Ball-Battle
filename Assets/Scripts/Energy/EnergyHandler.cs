using System.Collections.Generic;
using UnityEngine;

namespace BallBattle.Energy
{
	public class EnergyHandler : MonoBehaviour
	{
		[SerializeField] private Color energyColor_;
		[SerializeField] private Transform energyParent_;

		private List<EnergyPoint> energyPoints = new List<EnergyPoint>();
		private int currEnergy;
		private EnergyPoint tempEnergy;

		public void InitializeEnergy()
		{
			if(energyPoints.Count <= 0)
			{
				for(int i = energyParent_.childCount - 1; i >= 0; i--)
				{
					energyPoints.Add(energyParent_.GetChild(i).GetComponent<EnergyPoint>());
					energyPoints[energyPoints.Count - 1].RemoveEnergy();
					energyPoints[energyPoints.Count - 1].SetEnergyColor(energyColor_);
					energyPoints[energyPoints.Count - 1].OnEnergyFilled += AddEnergyPoint;
				}
			}
			else
			{
				ResetEnergy();
			}

			currEnergy = 0;
			energyPoints[0].GenerateEnergy();
		}

		private void AddEnergyPoint()
		{
			currEnergy++;
			if(currEnergy < energyPoints.Count)
			{
				energyPoints[currEnergy].GenerateEnergy();
			}
		}

		public void UseEnergy(int cost)
		{
			if (cost > currEnergy)
				return;

			RemoveEnergyPoint(cost);
			//trigger use for what (?)

		}

		private void RemoveEnergyPoint(int _value)
		{
			for(int loop = 0; loop < _value; loop++)
			{
				tempEnergy = energyPoints[0];
				energyPoints.Remove(tempEnergy);
				tempEnergy.transform.SetAsFirstSibling();
				tempEnergy.RemoveEnergy();
				energyPoints.Add(tempEnergy);

				if (currEnergy == energyPoints.Count)
					tempEnergy.GenerateEnergy();
				currEnergy--;
			}
		}

		public void ResetEnergy()
		{
			RemoveEnergyPoint(energyPoints.Count);
		}
	}
}
