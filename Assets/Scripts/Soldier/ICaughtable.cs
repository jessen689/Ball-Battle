namespace BallBattle.Soldier
{
	public interface ICaughtable
	{
		bool IsCaughtable { get; }
		void GettingCaught();
	}
}
