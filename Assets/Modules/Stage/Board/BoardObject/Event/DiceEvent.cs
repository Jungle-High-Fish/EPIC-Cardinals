using System.Collections;
using System.Collections.Generic;
using Cardinals.BoardEvent;
using UnityEngine;

namespace Cardinals.BoardEvent.Event
{
    public class DiceEvent : BaseBoardEventObject
    {
        protected override IEnumerator Execute()
        {
            GameManager.I.UI.UIDiceEvent.Init();
            yield return null;
        }
    }
}