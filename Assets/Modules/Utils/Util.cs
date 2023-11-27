using System;
using Random = UnityEngine.Random;

namespace Cardinals.Utils
{
    public class Util
    {
        public static T GetRandomEnum<T>(int start = 0) where T : Enum
        {
            int idx = Random.Range(start, Enum.GetNames(typeof(T)).Length);
            return (T) System.Convert.ChangeType(idx, typeof(T));
        }
    }
}