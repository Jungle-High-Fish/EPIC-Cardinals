using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals
{
    public class StageEffect : MonoBehaviour
    {
        private ComponentGetter<ParticleSystem> _speedLineParticle = new(TypeOfGetter.ChildByName, "SpeedLine");

        public void SetSpeedLine(bool isActive)
        {
            _speedLineParticle.Get(gameObject).gameObject.SetActive(isActive);
        }
    }

}