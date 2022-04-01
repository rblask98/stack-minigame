using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Stack__
{
    public partial class YourStackTower : Form
    {
        Form1 f1 = new Form1();

        Bitmap pictureOfYourStackTower;
        public YourStackTower()
        {
            InitializeComponent();
        }

        private void YourStackTower_Load(object sender, EventArgs e)
        {
            pictureOfYourStackTower = ImageProcessor();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "jpg";
            sfd.Filter = "JPG images (*.jpg)|*,jpg";
            if(sfd.ShowDialog()==DialogResult.OK)
            {
                string fileName = sfd.FileName;
                if (Path.HasExtension(fileName) || Path.GetExtension(fileName) != "jpg")
                    fileName = fileName + ".jpg";

                pictureOfYourStackTower.Save(fileName, ImageFormat.Jpeg);
            }
            sfd.Dispose();
        }

        public Bitmap ImageProcessor()
        {
            int panoramaOfStackTowerHeight = 0;
            Bitmap tempimage = (Bitmap)(Form1.imagesOfStackTower[0]);
            int panoramaOfStackTowerWidth = tempimage.Width; //312H 400W

            for (int i = 0; i < Form1.NumberOfFrames; i++)
                panoramaOfStackTowerHeight += ((Bitmap)Form1.imagesOfStackTower[i]).Height;

            Bitmap panoramaOfStackTower = new Bitmap(panoramaOfStackTowerWidth, panoramaOfStackTowerHeight);
            Graphics g = Graphics.FromImage(panoramaOfStackTower);

            for (int indexOfImage = 0; indexOfImage < Form1.NumberOfFrames; indexOfImage++)
            {
                if (indexOfImage == Form1.NumberOfFrames - 1)
                    g.DrawImage((Bitmap)(Form1.imagesOfStackTower[indexOfImage]), 0, panoramaOfStackTowerHeight - (indexOfImage + 1) * tempimage.Height);
                else g.DrawImage((Bitmap)(Form1.imagesOfStackTower[indexOfImage]), 0, panoramaOfStackTowerHeight - (indexOfImage + 1) * tempimage.Height - (Form1.GameBoardVerticalUnit - 4)); 
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = panoramaOfStackTower;

            return panoramaOfStackTower;
        }

    }
}
