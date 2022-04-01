using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Stack__
{
    public partial class ScoreList : Form
    {
        public int Score;
        public int Level;

        public static string name_Score;
        public static string time_Score;
        public static string score_Score;
        public static string level_Score;

        bool snapShotSupported = false;

        YourStackTower yst = new YourStackTower();
        public ScoreList()
        {
            InitializeComponent();
        }

        private void ScoreList_Load(object sender, EventArgs e)
        {
            Score = Form1.ScoreForSaving;
            textBox1.Text = Score + "";

            Level = Form1.LevelForSaving;
            textBox2.Text = Level + "";

            if (Form1.ScoreForSaving == 0)
                checkBox1.Enabled = false;
            else checkBox1.Enabled = true;

            toolStripLabel1.Enabled = true;

            LoadScoreTable();
                       
        }

        private void LoadScoreTable()
        {
            LoadScoreData();
            listView1.ListViewItemSorter = new ScoreComparer(2);
            listView1.Sort();
        }

        private void LoadScoreData()
        {
            if(File.Exists("ScoreData.txt"))
            {
                foreach(string scoreData in File.ReadLines("ScoreData.txt"))
                {
                    string[] fieldOfScoreData = scoreData.Split('|');
                    ListViewItem ItemsForScoreDataRow = new ListViewItem(new[] { fieldOfScoreData[0], fieldOfScoreData[1], fieldOfScoreData[2], fieldOfScoreData[3] });
                    listView1.Items.Add(ItemsForScoreDataRow);
                }
            }
        }
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text!="")
            {
                string scoreData = comboBox1.Text + "|" + DateTime.UtcNow.ToString() + "|" + Score+"|"+Level;
                comboBox1.Items.Add(comboBox1.Text);

                StreamWriter file = new StreamWriter("ScoreData.txt", append: true);
                file.WriteLine(scoreData);
                file.Dispose();

                string getScoreDataFromFile = File.ReadLines("ScoreData.txt").Last();
                string[] fieldOfScoreData = getScoreDataFromFile.Split('|');

                ListViewItem ItemsForScoreDataRow = new ListViewItem(new[] { fieldOfScoreData[0], fieldOfScoreData[1], fieldOfScoreData[2], fieldOfScoreData[3]});
                listView1.Items.Add(ItemsForScoreDataRow);

                if(snapShotSupported) yst.ImageProcessor().Save(comboBox1.Text + "_" + Score + "_" + Level + ".jpg", ImageFormat.Jpeg);

                MessageBox.Show("Score saved successfully!");

                toolStripLabel1.Enabled = false;

            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
                if (File.Exists(listView1.Items[i].SubItems[0].Text + "_" + listView1.Items[i].SubItems[2].Text + "_" + listView1.Items[i].SubItems[3].Text + ".jpg"))
                    File.Delete(listView1.Items[i].SubItems[0].Text + "_" + listView1.Items[i].SubItems[2].Text + "_" + listView1.Items[i].SubItems[3].Text + ".jpg");

            if (File.Exists("ScoreData.txt"))
            {
                File.Delete("ScoreData.txt");
                listView1.Items.Clear();
                LoadScoreTable();
            }

           
        }

        public class ScoreComparer : IComparer
        {
            private int indexOfScoreTable;

            public ScoreComparer(int _indexOfScoreTable)
            {
                indexOfScoreTable = _indexOfScoreTable;
            }
            public int Compare(object x, object y)
            {
                int score1 = int.Parse(((ListViewItem)x).SubItems[indexOfScoreTable].Text);
                int score2 = int.Parse(((ListViewItem)y).SubItems[indexOfScoreTable].Text);
                return (-1)*score1.CompareTo(score2); //Descending Sort Order
            }
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                snapShotSupported = true;
            else snapShotSupported = false;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int selectedIndexOfListView = listView1.FocusedItem.Index;

            name_Score = listView1.SelectedItems[0].SubItems[0].Text;
            time_Score = listView1.SelectedItems[0].SubItems[1].Text;
            score_Score = listView1.SelectedItems[0].SubItems[2].Text;
            level_Score = listView1.SelectedItems[0].SubItems[3].Text;

            InfoAboutScore ias = new InfoAboutScore();
            ias.Show();

        }
    }
}
