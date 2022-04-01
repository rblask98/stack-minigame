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

namespace Stack__
{
    public partial class InfoAboutScore : Form
    {
        public InfoAboutScore()
        {
            InitializeComponent();
        }

        private void InfoAboutScore_Load(object sender, EventArgs e)
        {
            textBox1.Text = ScoreList.name_Score;
            textBox2.Text = ScoreList.time_Score;
            textBox3.Text = ScoreList.score_Score;
            textBox4.Text = ScoreList.level_Score;

            if (!File.Exists(ScoreList.name_Score + "_" + ScoreList.score_Score + "_" + ScoreList.level_Score + ".jpg"))
                return;
            else
            {
                pictureBox1.Image = Image.FromFile(ScoreList.name_Score + "_" + ScoreList.score_Score + "_" + ScoreList.level_Score + ".jpg");
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
    }
}
