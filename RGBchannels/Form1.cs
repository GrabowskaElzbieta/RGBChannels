using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBchannels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            GObjects = new GlobalObjects();
            InitializeComponent();
            GObjects.finished = true;
            SeparationOption_Combo.SelectedIndex = 0;
            Illuminant_Combo.Items.AddRange(GObjects.IlluminnatsNames);
            ColorSpace_Combo.Items.AddRange(GObjects.ColorSpaceNames);
            ColorSpace_Combo.SelectedIndex = 0;
            GObjects.sRGB2XYZs = ImageControl.TransformationMatrix(GObjects.ColorSpaceValues[0], GObjects.IlluminantsValues[5]);
            GObjects.XYZ2sRGB = MatrixCalculations.MatrixInverse(GObjects.sRGB2XYZs);
         //spytac o LAB   
        }
        public GlobalObjects GObjects;
        private void Download_Pic_button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "Download_ch1Pic_button":
                    ImageControl.SaveImage(Ch1_PictureBox.Image);
                    break;
                case "Download_ch2Pic_button":
                    ImageControl.SaveImage(Ch2_PictureBox.Image);
                    break;
                case "Download_ch3Pic_button":
                    ImageControl.SaveImage(Ch2_PictureBox.Image);
                    break;
                default:
                    ImageControl.SaveImage(Main_PictureBox.Image);
                    break;
            }
        }

        private void Add_Pic_button_Click(object sender, EventArgs e)
        {
            if (ImageControl.AddImage(ref GObjects, Ch1_PictureBox.Width, Ch1_PictureBox.Height, Main_PictureBox.Width, Main_PictureBox.Height))
            {
                Ch1_PictureBox.Image = Ch2_PictureBox.Image = Ch3_PictureBox.Image = null;
                Main_PictureBox.Image = GObjects.MainPictureOnView.Bitmap;
            }
        }

        private void Illuminant_Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Illuminant_Combo.SelectedIndex == -1) return;
            WhiteX_numeric.Value = (decimal)GObjects.IlluminantsValues[Illuminant_Combo.SelectedIndex].X;
            WhiteY_numeric.Value = (decimal)GObjects.IlluminantsValues[Illuminant_Combo.SelectedIndex].Y;
        }

        private void ColorProfile_Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColorSpace_Combo.SelectedIndex == -1) return;
            ColorSpace colorSpace = GObjects.ColorSpaceValues[ColorSpace_Combo.SelectedIndex];
            RedX_numeric.Value = (decimal)colorSpace.Red.X;
            RedY_numeric.Value = (decimal)colorSpace.Red.Y;
            GreenX_numeric.Value = (decimal)colorSpace.Green.X;
            GreenY_numeric.Value = (decimal)colorSpace.Green.Y;
            BlueX_numeric.Value = (decimal)colorSpace.Blue.X;
            BlueY_numeric.Value = (decimal)colorSpace.Blue.Y;
            Gamma_numeric.Value = (decimal)colorSpace.Gamma;
            Illuminant_Combo.SelectedIndex = Array.FindIndex(GObjects.IlluminnatsNames,(x)=>x==colorSpace.illuminnant);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool success=false;
            switch (SeparationOption_Combo.SelectedIndex)
            {
                case 0:
                    success = ImageControl.ToYCbCr(ref GObjects);
                    break;
                case 1:
                    success = ImageControl.ToHSV(ref GObjects);
                    break;
                case 2:
                    ColorSpace cs = new ColorSpace(new doublePoint((double)RedX_numeric.Value, (double)RedY_numeric.Value), new doublePoint((double)GreenX_numeric.Value, (double)GreenY_numeric.Value), new doublePoint((double)BlueX_numeric.Value, (double)BlueY_numeric.Value), (double)Gamma_numeric.Value, "");
                    success = ImageControl.ToLAB(ref GObjects, cs, new doublePoint((double)WhiteX_numeric.Value, (double)WhiteY_numeric.Value));
                    break;
            }
            if (success)
            {
                Ch1_PictureBox.Image = GObjects.Chanel1.Bitmap;
                Ch2_PictureBox.Image = GObjects.Chanel2.Bitmap;
                Ch3_PictureBox.Image = GObjects.Chanel3.Bitmap;
                Ch1_PictureBox.Refresh();
                Ch2_PictureBox.Refresh();
                Ch3_PictureBox.Refresh();
            }
        }

        private void SeparationOption_Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SeparationOption_Combo.SelectedIndex)
            {
                case 0:
                    Ch1_label.Text = "Y";
                    Ch2_label.Text = "Cb";
                    Ch3_label.Text = "Cr";
                    LabSettings_groupbox.Enabled = false;
                    break;
                case 1:
                    Ch1_label.Text = "H";
                    Ch2_label.Text = "S";
                    Ch3_label.Text = "V";
                    LabSettings_groupbox.Enabled = false;
                    break;
                case 2:
                    Ch1_label.Text = "L";
                    Ch2_label.Text = "A";
                    Ch3_label.Text = "B";
                    LabSettings_groupbox.Enabled = true;
                    break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ImageControl.CreateBase(ref GObjects, Ch1_PictureBox.Width, Ch1_PictureBox.Height, Main_PictureBox.Width, Main_PictureBox.Height);
            Main_PictureBox.Image = GObjects.MainPictureOnView.Bitmap;
        }
    }
}
