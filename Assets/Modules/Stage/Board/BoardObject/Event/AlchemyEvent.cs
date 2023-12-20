using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.BoardEvent.Event
{
    public class AlchemyEvent : BaseBoardEventObject
    {

        protected override IEnumerator Execute()
        {
            yield return GameManager.I.UI.UIAlchemyEventPanel.Init();
        }
    }

}