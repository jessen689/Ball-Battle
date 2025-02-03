using UnityEngine;

namespace BallBattle.Soldier
{
	public interface IDetecting
	{
		bool IsDetecting { get; set; }
		float DetectionRange { get; }

		void DetectedInRange(GameObject _objDetected);
	}
}
