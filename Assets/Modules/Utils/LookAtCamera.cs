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
            //transform.LookAt(Camera.main.transform);
            //transform.Rotate(0, Camera.main.transform.rotation.y, 0);
            transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, 0);
        }
    }

}