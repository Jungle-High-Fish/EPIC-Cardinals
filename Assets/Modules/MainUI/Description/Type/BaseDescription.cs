using UnityEngine;

namespace Cardinals.UI.Description
{
    public abstract class BaseDescription : MonoBehaviour
    {
        public void Delete()
        {
            Destroy(this);
        }
    }
}