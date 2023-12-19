using Febucci.UI;
using TMPro;
using UnityEngine;
using Util;

namespace Cardinals.UI {
    public class UITurnRoundStatus : MonoBehaviour
    {
        public TextMeshProUGUI TurnText => _turnText.Get(gameObject);
        public TextMeshProUGUI RoundText => _roundText.Get(gameObject);

        private ComponentGetter<TextMeshProUGUI> _turnText 
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Turn/Text");
        private ComponentGetter<TypewriterByCharacter> _turnTypewriter 
            = new ComponentGetter<TypewriterByCharacter>(TypeOfGetter.ChildByName, "Turn/Text");
        private ComponentGetter<TextMeshProUGUI> _roundText 
            = new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Round/Text");
        private ComponentGetter<TypewriterByCharacter> _roundTypewriter
            = new ComponentGetter<TypewriterByCharacter>(TypeOfGetter.ChildByName, "Round/Text");

        public void Set(int turn, int round)
        {
            SetTurn(turn);
            SetRound(round);
        }

        public void SetTurn(int turn)
        {
            _turnTypewriter.Get(gameObject).ShowText(turn.ToString("D2"));
        }

        public void SetRound(int round)
        {
            _roundTypewriter.Get(gameObject).ShowText(round.ToString("D2"));
        }
    }
}