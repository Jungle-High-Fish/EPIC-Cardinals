using System.Collections;

namespace Cardinals.BoardObject.Event
{
    public class Roulette : BaseBoardObject
    {
        protected override IEnumerator Execute()
        {
            GameManager.I.UI.UIRoulette.Init();
            yield return null;
        }
    }
}