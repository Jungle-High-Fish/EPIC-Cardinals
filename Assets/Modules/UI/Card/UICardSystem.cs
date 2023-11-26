using System.Collections;
using System.Collections.Generic;
using Cardinals.UI;
using UnityEngine;
using Util;

namespace Cardinals.Game {

	public class UICardSystem : MonoBehaviour {
		ComponentGetter<UICardDeck> _cardDeck
			= new ComponentGetter<UICardDeck>(TypeOfGetter.ChildByName, "CardDeck");

		ComponentGetter<UICardUseSlot> _cardMoveSlot
			= new ComponentGetter<UICardUseSlot>(TypeOfGetter.ChildByName, "CardMoveSlot");
		ComponentGetter<UICardUseSlot> _cardActionSlot
			= new ComponentGetter<UICardUseSlot>(TypeOfGetter.ChildByName, "CardActionSlot");

		public void Init() {
			GameManager.I.Stage.CardManager.SetCardDeckUIParent(_cardDeck.Get(gameObject).transform);
			_cardDeck.Get(gameObject).Init(OnCardDeckMouseHover);
		}

		private void OnCardDeckMouseHover(bool isMouseHover) {
			GameManager.I.Stage.CardManager.SetCardMouseState(isMouseHover);
		}
	}

}
