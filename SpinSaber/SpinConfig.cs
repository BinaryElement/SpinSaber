using SpinSaber.Swingers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace SpinSaber {

    [XmlRoot("SpinConfig")]
    public class SpinConfig {

        public class SpinConfigPeriod {

            [XmlElement("Duration")]
            public float duration;
            [XmlElement("Type")]
            public Swinger.Type type;
            [XmlElement("SmoothFactor")]
            public float smoothFactor;
            [XmlElement("StartRot")]
            public Vector3 startRot;
            [XmlElement("EndRot")]
            public Vector3 endRot;

            public SpinConfigPeriod() { }

            public SpinConfigPeriod(float duration, Swinger.Type type, float smoothFactor, Vector3 startRot, Vector3 endRot) {
                this.duration = duration;
                this.type = type;
                this.smoothFactor = smoothFactor;
                this.startRot = startRot;
                this.endRot = endRot;
            }

        }

        [XmlElement("Name")]
        public string name;

        [XmlElement("BPMFactor")]
        public float bpmFactor = 0;

        [XmlElement("SpinPeriod")]
        public SpinConfigPeriod[] periods;

        public void Validate() {
        }

        public List<Swinger> SetupSwingers() {
            List<Swinger> swingers = new List<Swinger>();
            float nextStartTime = 0;

            for (int i = 0; i < periods.Length; i++) {
                Swinger swinger;
                switch (periods[i].type) {
                    case Swinger.Type.NONE:
                        swinger = new StraightSwinger();
                        break;
                    case Swinger.Type.EASE_IN_OUT_QUADRATIC:
                        swinger = new SmoothSwingerEaseInOutQuad();
                        break;
                    case Swinger.Type.EASE_IN_QUADRATIC:
                        swinger = new SmoothSwingerEaseInQuad();
                        break;
                    case Swinger.Type.EASE_OUT_QUADRATIC:
                        swinger = new SmoothSwingerEaseOutQuad();
                        break;
                    default:
                        continue;
                }
                swinger.startTime = nextStartTime;
                swinger.endTime = nextStartTime + periods[i].duration;
                swinger.startRot = periods[i].startRot;
                swinger.endRot = periods[i].endRot;
                swinger.smoothFactor = periods[i].smoothFactor;
                nextStartTime = swinger.endTime;
                swingers.Add(swinger);
            }

            return swingers;
        }

    }

}
