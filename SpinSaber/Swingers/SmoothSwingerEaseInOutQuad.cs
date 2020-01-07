using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinSaber.Swingers {

    class SmoothSwingerEaseInOutQuad : Swinger {

        public override Quaternion GetRotation(float t) {
            //return Quaternion.Euler(new Vector3(Mathf.SmoothStep(startRot.x, endRot.x, t), Mathf.SmoothStep(startRot.y, endRot.y, t), Mathf.SmoothStep(startRot.z, endRot.z, t)));
            if (t <= 0.5f) {
                t = 2 * t * t;
            } else {
                t = -1 + (4 - 2 * t) * t;
            }
            return Quaternion.Euler(Vector3.Lerp(startRot, endRot, t));
        }

    }

}
