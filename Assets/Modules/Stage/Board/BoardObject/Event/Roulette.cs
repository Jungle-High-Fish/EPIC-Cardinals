using System.Collections;

namespace Cardinals.BoardObject.Event
{
    public class Roulette : BaseBoardObject
    {
        protected override IEnumerator Execute()
        {
            yield return GameManager.I.UI.UIRoulette.Execute();
        }
    }
}