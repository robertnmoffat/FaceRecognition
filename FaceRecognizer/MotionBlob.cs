using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    class MotionBlob
    {
        public Point topLeft,topRight,bottomLeft,bottomRight;
        public Point vector;
        public int count=0, distanceThreshold=16;


        public MotionBlob() {
        }

        public bool withinThreshold(Point point) {
            if (point.X > topLeft.X - distanceThreshold
                && point.X < topRight.X + distanceThreshold
                && point.Y > topLeft.Y - distanceThreshold
                && point.Y < bottomLeft.Y + distanceThreshold) return true;

            return false;
        }
    }
}
