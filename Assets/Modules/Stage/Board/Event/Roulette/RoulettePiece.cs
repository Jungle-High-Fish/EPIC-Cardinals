using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Cardinals.BoardEvent.Roulette
{
	public class RoulettePiece : MonoBehaviour
	{
		[SerializeField] private Image imageIcon;
		[SerializeField] private TextMeshProUGUI textDescription;

		public void Setup(RoulettePieceDataSO pieceDataSo)
		{
			imageIcon.sprite = pieceDataSo.icon;
			textDescription.text = pieceDataSo.description;
		}
	}
}

