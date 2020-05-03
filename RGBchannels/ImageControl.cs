using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace RGBchannels
{
    public static class ImageControl
    {
        public static void SaveImage(Image image)
        {
            if (image == null)
            {
                MessageBox.Show("Picture not found");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPeg Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";
            saveFileDialog.Title = "Save an Image File";
            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                        image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        image.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 3:
                        image.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                }
            }
        }
        public static bool AddImage(ref GlobalObjects GObjects, int width, int height, int BigWidth, int BigHeight)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP; *.JPG; *.PNG; *.GIF)| *.BMP; *.JPG; *.PNG; *.GIF";
            openFileDialog.Title = "Open an Image File";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                GObjects.MainPicture = new DirectBitmap(width, height);
                
                Bitmap temp = new Bitmap(Image.FromFile(openFileDialog.FileName), width, height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        GObjects.MainPicture.SetPixel(j, i, temp.GetPixel(j, i));
                       
                    }
                }
                temp = new Bitmap(Image.FromFile(openFileDialog.FileName), BigWidth, BigHeight);
                GObjects.MainPictureOnView = new DirectBitmap(BigWidth, BigHeight);
               
                for (int i = 0; i < BigHeight; i++)
                {
                    for (int j = 0; j < BigWidth; j++)
                    {
                        GObjects.MainPictureOnView.SetPixel(j, i, temp.GetPixel(j, i));
                    }
                }
                temp.Dispose();
                return true;
            }
            return false;
        }

        public static bool ToYCbCr( ref GlobalObjects GObjects)
        {
            if (GObjects.MainPicture == null) return false ;
            GObjects.Chanel1 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);
            GObjects.Chanel2 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);
            GObjects.Chanel3 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);
            for (int i = 0; i < GObjects.MainPicture.Height; i++)
            {
                for (int j = 0; j < GObjects.MainPicture.Width; j++)
                {
                    int R = GObjects.MainPicture.GetPixel(j, i).R;
                    int G = GObjects.MainPicture.GetPixel(j, i).G;
                    int B = GObjects.MainPicture.GetPixel(j, i).B;

                    int YColor = Math.Min(255, Math.Max(0,Convert.ToInt32( 0.299 * R + 0.587 * G + 0.114 * B)));
                    int CbColor = Math.Min(255,Math.Max(0, Convert.ToInt32( 128 - 0.169 * R - 0.331 * G + 0.5 * B)));
                    int CrColor = Math.Min(255,Math.Max(0, Convert.ToInt32(128 + 0.5 * R - 0.419 * G - 0.081 * B)));

                    GObjects.Chanel1.SetPixel(j, i, Color.FromArgb(YColor, YColor, YColor));
                    GObjects.Chanel2.SetPixel(j, i, Color.FromArgb(127, 255-CbColor, CbColor));
                    GObjects.Chanel3.SetPixel(j, i, Color.FromArgb(CrColor, 255-CrColor, 127));
                }
            }
            return true;
        }

        public static bool ToHSV(ref GlobalObjects GObjects)
        {
            if (GObjects.MainPicture == null) return false;
            GObjects.Chanel1 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);
            GObjects.Chanel2 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);
            GObjects.Chanel3 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);
            for (int i = 0; i < GObjects.MainPicture.Height; i++)
            {
                for (int j = 0; j < GObjects.MainPicture.Width; j++)
                {
                    double R = GObjects.MainPicture.GetPixel(j, i).R/255.0;
                    double G = GObjects.MainPicture.GetPixel(j, i).G / 255.0;
                    double B = GObjects.MainPicture.GetPixel(j, i).B / 255.0;

                    double MinRGB = Math.Min(R, Math.Min(G, B));
                    double MaxRGB = Math.Max(R, Math.Max(G, B));

                    double H = CalculateH(R, G, B, MaxRGB, MinRGB);
                    double S = 0;
                    if (MaxRGB != 0) S = (MaxRGB - MinRGB) / MaxRGB;
                    double V = MaxRGB;

                    int iH = Convert.ToInt32(255 * H / 360);
                    int iS = Convert.ToInt32(255 * S);
                    int iV = Convert.ToInt32(255 *V);
                    GObjects.Chanel1.SetPixel(j, i, Color.FromArgb(iH, iH, iH));
                    GObjects.Chanel2.SetPixel(j, i, Color.FromArgb(iS, iS, iS));
                    GObjects.Chanel3.SetPixel(j, i, Color.FromArgb(iV, iV, iV));
                }
            }

            return true;
        }

        private static double CalculateH(double R, double G, double B, double M, double m)
        {
            double value=0;
            if (M == m) return 0;
            if (M == R) value = (G - B) / (M - m);
            if (M == G) value = 2  + (B - R) / (M - m);
            if (M == B) value = 4 + (R - G) / (M - m);

            value *= 60;
            if (value < 0) value += 360;
            return value;
        }

        public static bool ToLAB(ref GlobalObjects GObjects, ColorSpace colorSpace, doublePoint whitePoint)
        {
            if (GObjects.MainPicture == null) return false;
            GObjects.Chanel1 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);
            GObjects.Chanel2 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);
            GObjects.Chanel3 = new DirectBitmap(GObjects.MainPicture.Width, GObjects.MainPicture.Height);

            double[][] matrix = TransformationMatrix(colorSpace, whitePoint);
            double[][] WPvector = new double[][] { new double[] { whitePoint.X/whitePoint.Y }, new double[] {1 }, new double[] {( 1-whitePoint.X-whitePoint.Y)/whitePoint.Y } };
            for (int i = 0; i < GObjects.MainPicture.Height; i++)
            {
                for (int j = 0; j < GObjects.MainPicture.Width; j++)
                {
                    double R = Math.Pow( GObjects.MainPicture.GetPixel(j, i).R/255.0, colorSpace.Gamma);
                    double G = Math.Pow( GObjects.MainPicture.GetPixel(j, i).G/255.0, colorSpace.Gamma);
                    double B = Math.Pow( GObjects.MainPicture.GetPixel(j, i).B/255.0, colorSpace.Gamma);

                    double[][] RGBvector = new double[][] { new double[] { R }, new double[] { G }, new double[] { B } };
                    double[][] XYZvector = MatrixCalculations.MatrixProduct(matrix, RGBvector);

                    var Values = VectorFunction(XYZvector[0][0] / WPvector[0][0], XYZvector[1][0] / WPvector[1][0], XYZvector[2][0] / WPvector[2][0]);

                    int Lvalue = Convert.ToInt32( 116 * Values.fy - 16);
                    int Avalue = Convert.ToInt32( 500 * (Values.fx-Values.fy));
                    int Bvalue = Convert.ToInt32( 200 * (Values.fy - Values.fz));

                    GObjects.Chanel1.SetPixel(j, i, Color.FromArgb(Math.Max(Math.Min(255, Lvalue), 0), Math.Max(Math.Min(255, Lvalue), 0), Math.Max(Math.Min(255, Lvalue), 0)));
                    GObjects.Chanel2.SetPixel(j, i, Color.FromArgb(Math.Max(0,Math.Min(255,127+Avalue)),Math.Max(0,Math.Min(255, 127-Avalue)), 127));
                    GObjects.Chanel3.SetPixel(j, i, Color.FromArgb(Math.Max(0,Math.Min(255, 127+Bvalue)), 127,Math.Max(0,Math.Min(255, 127-Bvalue))));
                    
                }
            }
            return true;
        }

        private static (double fx, double fy, double fz) VectorFunction(double xr, double yr, double zr)
        {
            double E = 0.008856;
            double K = 903.3;
            double fx = xr > E ? Math.Pow(xr, 1.0 / 3) : (K * xr + 16) / 116;
            double fy = yr > E ? Math.Pow(yr, 1.0 / 3) : (K * yr + 16) / 116;
            double fz = zr > E ? Math.Pow(zr, 1.0 / 3) : (K * zr + 16) / 116;
            return (fx, fy, fz);
        }
        public static double[][] TransformationMatrix(ColorSpace colorSpace, doublePoint whitePoint)
        {
            double[][] value = new double[3][];
            value[0] = new double[3] { colorSpace.Red.X, colorSpace.Green.X, colorSpace.Blue.X };
            value[1] = new double[3] { colorSpace.Red.Y, colorSpace.Green.Y, colorSpace.Blue.Y };
            value[2] = new double[3];
            for (int i = 0; i < 3; i++) value[2][i] = 1 - value[1][i] - value[0][i];

            double[][] WPvector = new double[3][];
            WPvector[0] = new double[1] { whitePoint.X / whitePoint.Y };
            WPvector[1] = new double[1] { 1 };
            WPvector[2] = new double[1] { (1 - whitePoint.X - whitePoint.Y) / whitePoint.Y };

            double[][] result = MatrixCalculations.MatrixProduct((MatrixCalculations.MatrixInverse(value)), WPvector);
            double[][] temp = MatrixCalculations.MatrixCreate(3, 3);
            for (int i = 0; i < 3; i++) temp[i][i] = result[i][0];
            result = MatrixCalculations.MatrixProduct(value, temp);
            return result;
        }

        public static void  CreateBase(ref GlobalObjects GObjects, int width, int height, int BigWidth, int BigHeight)
        {
            GObjects.MainPicture = Createtemp(width,height);
            GObjects.MainPictureOnView = Createtemp(BigWidth, BigHeight);
        }

        private static  DirectBitmap Createtemp(int Width, int Height)
        {
            DirectBitmap Main = new DirectBitmap(Width, Height);
            int x0 = Width / 2;
            int y0 = Height / 2;
            int r = Height / 3;

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (InsidetheCircle(j, i, x0, y0, r))
                    {

                        double H =Convert.ToInt32( Math.Atan2((i - y0), (j - x0)) / (2 * Math.PI) * 360.0);
                        if (H < 0) H += 360;
                        double S = (Math.Sqrt(((x0 - j) * (x0 - j) + (y0 - i) * (y0 - i) )/ (r * r*1.0))); //0-1
                        double V = 1;
                        Main.SetPixel(j, i, Color.FromArgb(Convert.ToInt32( Function2RGB(H, S, V, 5)*255),Convert.ToInt32( Function2RGB(H, S, V, 3)*255),Convert.ToInt32( Function2RGB(H, S, V, 1)*255)));
                    }
                    else Main.SetPixel(j, i, Color.White);
                }
            }
            for (int i = 0; i < Main.Width; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Main.SetPixel(i, j, Color.Black);
                    Main.SetPixel(i, Main.Height - 1 - j, Color.Black);
                }
            }
            for (int i = 0; i < Main.Height; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Main.SetPixel(j, i, Color.Black);
                    Main.SetPixel(Main.Width - 1 - j, i, Color.Black);
                }
            }//ramka

            
            return Main;
        }
        private static double Function2RGB(double H, double S, double V, int n)
        {
            double k = (n + H / 60.0) % 6;
            return V - V * S * Math.Max(0, Math.Min(Math.Min(k, 4 - k), 1));
        }
        private static bool InsidetheCircle(int x, int y, int x0, int y0, int r)
        {
            return (x - x0) * (x - x0) + (y - y0) * (y - y0) <= r*r;
        }
    }
}
