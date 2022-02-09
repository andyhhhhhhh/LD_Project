using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Smartray.Sample
{
    class TranformPixelsToWorld
    {

    }

    public class PointD3D
    {
        private double _X;

        public double X
        {
            get { return _X; }
            set { _X = value; }
        }
        private double _Y;

        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        private double _Z;

        public double Z
        {
            get { return _Z; }
            set { _Z = value; }
        }
        public PointD3D()
        {
            this._X = 0d;
            this._Y = 0d;
            this._Z = 0d;
        }
    }
}
