using System.Collections;

namespace Cardinals.BoardObject.Event
{
    public class AlchemyEvent : BaseBoardObject
    {
        protected override IEnumerator Execute()
        {
            yield return GameManager.I.UI.UIAlchemyEventPanel.Init();
        }
    }

}