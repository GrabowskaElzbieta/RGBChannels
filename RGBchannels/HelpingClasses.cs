using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RGBchannels
{
    public class doublePoint
    {
        public double X { get; }
        public double Y { get; }
        public doublePoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    public class ColorSpace
    {
        public doublePoint Red { get; }
        public doublePoint Green { get; }
        public doublePoint Blue { get; }
        public double Gamma { get; }
        public string illuminnant { get; }

        public ColorSpace(doublePoint red, doublePoint green, doublePoint blue, double gamma, string illuminnant)
        {
            Red = red;
            Green = green;
            Blue = blue;
            this.Gamma = gamma;
            this.illuminnant = illuminnant;

        }
    }
    public class GlobalObjects
    {
        public DirectBitmap MainPicture;
        public DirectBitmap MainPictureOnView;
        public DirectBitmap Chanel1;
        public DirectBitmap Chanel2;
        public DirectBitmap Chanel3;
        

        public doublePoint[] IlluminantsValues = { new doublePoint(0.44757, 0.40745), new doublePoint(0.34842, 0.35161), new doublePoint(0.31006, 0.31616), new doublePoint(0.34567, 0.3585), new doublePoint(0.33242, 0.34743), new doublePoint(0.31271, 0.32902), new doublePoint(0.29902, 0.31485), new doublePoint(0.33333, 0.33333), new doublePoint(0.3131, 0.33727), new doublePoint(0.37208, 0.37529), new doublePoint(0.4091, 0.3943), new doublePoint(0.44018, 0.40329), new doublePoint(0.31379, 0.34531), new doublePoint(0.3779, 0.38835), new doublePoint(0.31292, 0.32933), new doublePoint(0.34588, 0.35875), new doublePoint(0.37417, 0.37281), new doublePoint(0.34609, 0.35986), new doublePoint(0.38052, 0.37713), new doublePoint(0.43695, 0.40441) };
        public string[] IlluminnatsNames = new string[] { "A", "B", "C", "D50", "D55", "D65", "D75", "E", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" };
        public string[] ColorSpaceNames = new string[] { "sRGB", "Adobe RGB", "Apple RGB", "CIE RGB", "Wide Gamut", "PAL/SECAM" };
        public ColorSpace[] ColorSpaceValues = { new ColorSpace(new doublePoint(0.64, 0.33), new doublePoint(0.3, 0.6), new doublePoint(0.15, 0.06), 2.2, "D65"), new ColorSpace(new doublePoint(0.64, 0.33), new doublePoint(0.21, 0.71), new doublePoint(0.15, 0.06), 2.2, "D65"), new ColorSpace(new doublePoint(0.625, 0.34), new doublePoint(0.28, 0.595), new doublePoint(0.155, 0.07), 1.8, "D65"), new ColorSpace(new doublePoint(0.735, 0.265), new doublePoint(0.274, 0.717), new doublePoint(0.167, 0.009), 2.2, "E"), new ColorSpace(new doublePoint(0.7347, 0.2653), new doublePoint(0.1152, 0.8264), new doublePoint(0.1566, 0.0177), 1.2, "D50"), new ColorSpace(new doublePoint(0.64, 0.33), new doublePoint(0.29, 0.6), new doublePoint(0.15, 0.06), 1.95, "D65") };

        public double[][] XYZ2sRGB;
        public double[][] sRGB2XYZs;

        public bool finished = false;
        public double Gamma;
    }

}
