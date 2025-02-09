using UnityEngine;


namespace BallBattle
{
	public class SoldierAnimHandler : MonoBehaviour
	{
		public enum AnimID
		{
			Run,
			Fall,
			Spawn,
			Idle
		}

		[SerializeField] private Animator animator_;

		private const string RUN_STRING = "RunAnim";
		private const string FALL_STRING = "FallAnim";
		private const string SPAWN_ANIM = "SpawnAnim";
		private const string IDLE_ANIM = "Idle";

		public void PlayAnim(AnimID _anim)
		{
			animator_.Play(_anim == AnimID.Run ? RUN_STRING :
				_anim == AnimID.Fall ? FALL_STRING :
				_anim == AnimID.Spawn ? SPAWN_ANIM : IDLE_ANIM);
		}
	}
}
