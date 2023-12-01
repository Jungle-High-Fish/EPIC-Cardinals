using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public class LookAtCameraUpdate : MonoBehaviour
    {
        // Start is called before the first frame update
        void Update()
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }

}