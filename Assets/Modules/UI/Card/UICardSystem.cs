using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals.Game {

	public class UICardSystem : MonoBehaviour {
		ComponentGetter<RectTransform> _cardDeckTransform 
			= new ComponentGetter<RectTransform>(TypeOfGetter.ChildByName, "CardDeck");
		ComponentGetter<UICardUseSlot> _cardMoveSlot
			= new ComponentGetter<UICardUseSlot>(TypeOfGetter.ChildByName, "CardMoveSlot");
		ComponentGetter<UICardUseSlot> _cardActionSlot
			= new ComponentGetter<UICardUseSlot>(TypeOfGetter.ChildByName, "CardActionSlot");

		public void Init() {
			GameManager.I.Stage.CardManager.SetCardDeckUIParent(_cardDeckTransform.Get(gameObject));
		}
	}

}
