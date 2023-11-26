using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class LookAtCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.LookAt(Camera.main.transform);
        }
    }

}