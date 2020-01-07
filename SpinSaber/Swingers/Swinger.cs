using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinSaber.Swingers {

    public abstract class Swinger {

        public enum Type {
            NONE,
            EASE_IN_OUT_QUADRATIC,
            EASE_IN_QUADRATIC,
            EASE_OUT_QUADRATIC,
        }

        public float startTime = 0;
        public float endTime = 0;
        public float smoothFactor = 0;
        public Vector3 startRot;
        public Vector3 endRot;

        public abstract Quaternion GetRotation(float t);

    }

}
