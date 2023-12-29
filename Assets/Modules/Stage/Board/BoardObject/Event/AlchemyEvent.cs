using System.Collections;

namespace Cardinals.BoardObject.Event
{
    public class AlchemyEvent : BaseBoardObject
    {
        protected override IEnumerator Execute()
        {
            bool backup = GameManager.I.UI.UIAntiTouchPanel.activeSelf; 
            GameManager.I.UI.UIAntiTouchPanel.SetActive(false);
            
            yield return GameManager.I.UI.UIAlchemyEventPanel.Init();
            
            GameManager.I.UI.UIAntiTouchPanel.SetActive(backup);
        }
    }

}