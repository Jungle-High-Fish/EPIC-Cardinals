using Cardinals.Game;
using UnityEngine;

namespace Cardinals
{
    public class UIManager
    {
        [SerializeField] private UIStage _uiStage;
        public UIStage UIStage => _uiStage;
    }
}