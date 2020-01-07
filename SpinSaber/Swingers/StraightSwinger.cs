using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinSaber.Swingers {

    class StraightSwinger : Swinger {

        public override Quaternion GetRotation(float t) {
            return Quaternion.Euler(Vector3.Lerp(startRot, endRot, t));
        }

    }

}
