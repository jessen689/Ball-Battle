namespace BallBattle.Soldier
{
	public interface IActivatable
	{
		bool IsActiveMode { get; set; }

		void SetActiveMode(bool _activeValue, bool _isFirstTime);
	}
}
