using UnityEngine;

namespace BallBattle.UI
{
	public class HUDCanvas : MonoBehaviour
	{
		[SerializeField] private GameData gameData_;
		[SerializeField] private TMPro.TextMeshProUGUI enemyBannerText_;
		[SerializeField] private TMPro.TextMeshProUGUI playerBannerText_;

		private const string ATTACKER_STRING = "(Attacker)";
		private const string DEFENDER_STRING = "(Defender)";

		private void OnEnable()
		{
			gameData_.OnStateChange += UpdateTextBanner;
		}

		private void OnDisable()
		{
			gameData_.OnStateChange -= UpdateTextBanner;
		}

		private void UpdateTextBanner()
		{
			enemyBannerText_.text = "Enemy - AI " + ((gameData_.CurrState == GameData.GameState.PlayerAttack) ? DEFENDER_STRING : ATTACKER_STRING);
		}
	}
}
