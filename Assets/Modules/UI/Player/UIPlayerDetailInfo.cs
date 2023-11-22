using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals.UI {

    public class UIPlayerDetailInfo : MonoBehaviour {
        private ComponentGetter<Transform> _blessListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Bless List Panel/Scroll/Viwport/Content"
        );

        private ComponentGetter<Transform> _artifactListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Artifact List Panel/Scroll/Viwport/Content"
        );

        private ComponentGetter<Transform> _potionListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName, 
            "Potion List Panel/Potion Slot Area"
        );

        
    }

}