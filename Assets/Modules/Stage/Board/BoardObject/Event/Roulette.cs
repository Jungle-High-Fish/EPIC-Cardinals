using System.Collections;

namespace Cardinals.BoardEvent.Event
{
    public class Roulette : BaseBoardEventObject
    {
        protected override IEnumerator Execute()
        {
            GameManager.I.UI.UIRoulette.Init();
            yield return null;
        }
    }
}