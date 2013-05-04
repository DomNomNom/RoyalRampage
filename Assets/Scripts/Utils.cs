using UnityEngine;
using System.Collections;

namespace Utils {
    public class VectorUtils : MonoBehaviour {
        public static Vector3 clone(Vector3 v) {
            return new Vector3(v.x, v.y, v.z);
        }

    }
}