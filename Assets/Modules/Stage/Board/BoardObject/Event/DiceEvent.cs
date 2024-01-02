using System.Collections;
using System.Collections.Generic;
using Cardinals.BoardEvent;
using UnityEngine;

namespace Cardinals.BoardObject.Event
{
    public class DiceEvent : BaseBoardObject
    {
        protected override IEnumerator Execute()
        {
            GameManager.I.UI.UIDiceEvent.Init();
            yield return null;
        }
    }
}