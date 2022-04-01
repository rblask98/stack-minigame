using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Stack__
{
    public partial class Form1 : Form
    {
        ArrayList StackList = new ArrayList();
        public static ArrayList imagesOfStackTower = new ArrayList();


        int IndexOfStack;

        int StackWidth = Constants.StackWidth;

        int Score = 0;
        public static int ScoreForSaving;
        int Level = 1;
        public static int LevelForSaving;

        Point StackLocation;
        Color StackColor;
        Size StackSize;

        bool isGameOver = true;
        bool autoLevel = false;
        bool isPaused = false;

        public static int startPointOfGameBoardX;
        public static int startPointOfGameBoardY;
        public static int endingPointOfGameBoardX;
        public static int endingPointOfGameBoardY;
        public static int GameBoardWidth;
        public static int GameBoardHeigth;
        public static int GameBoardVerticalUnit;

        public static int NumberOfFrames = 1;

        public static int highScore;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
            HighScoreChecker();

            progressBar1.Maximum = highScore;           
        }

        private void ScreenShot()
        {
            float gameBoardVerticalUnit = (float)panel1.Size.Height / (float)Constants.Rows;

            GameBoardVerticalUnit = (int)gameBoardVerticalUnit;
            startPointOfGameBoardX = DesktopLocation.X + panel1.Location.X;
            startPointOfGameBoardY = DesktopLocation.Y + panel1.Location.Y+17*(int)gameBoardVerticalUnit;
            endingPointOfGameBoardX = DesktopLocation.X + panel1.Location.X + panel1.Width;
            endingPointOfGameBoardY =DesktopLocation.Y + panel1.Location.Y + panel1.Height;
            GameBoardWidth = panel1.Width;
            GameBoardHeigth = panel1.Height;

            Bitmap getAFrameSnapShotFromStackTower = new Bitmap(Form1.GameBoardWidth, Form1.GameBoardHeigth - 16 * Form1.GameBoardVerticalUnit-8);
            Graphics contextOfTheFrame = Graphics.FromImage(getAFrameSnapShotFromStackTower);
            contextOfTheFrame.CopyFromScreen(Form1.startPointOfGameBoardX, Form1.startPointOfGameBoardY, 0, 0, getAFrameSnapShotFromStackTower.Size);

            imagesOfStackTower.Add(getAFrameSnapShotFromStackTower);

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void HighScoreChecker()
        {
            List<int> scores = new List<int>();

            if (File.Exists("ScoreData.txt"))
            {
                string[] lines = File.ReadAllLines("ScoreData.txt");

                foreach (string line in lines)
                {
                    string[] splittedLine = line.Split('|');
                    scores.Add(int.Parse(splittedLine[2]));

                }
                scores.Sort();

                highScore = scores[scores.Count - 1];
            }
            else highScore = 1;

        }

        private struct Constants
        {

            public const int Rows = 32;

            public const int StackHeigth = 640 / Rows;
            public const int StackWidth = (int)(430 * 0.6);

            public const int StackStep = 400 / 20; //20 steps

            public const int TimerInterval = 500;
            public const int LevelMinimum = 1;
            public const int LevelMaximum = 40;

        }

        public Color[] StackColors = { Color.Red, Color.Blue, Color.Yellow, Color.Green, Color.Gray, Color.Purple, Color.Orange };

        private void MakeAStack(ArrayList stackList)
        {
            Panel stack = new Panel();
            Random colorSelector = new Random();

            stack.Size = StackSize;
            stack.Location = StackLocation;
            stack.BackColor = StackColor;

            StackColor = StackColors[colorSelector.Next(0, StackColors.Length)];

            StackList.Add(stack);

            IndexOfStack = StackList.Count;
            Controls.Add(stack);

            stack.BringToFront();

        }

        private void InitializeGame()
        {
            Random colorSelector = new Random();

            Score = 0;           

            StackLocation = new Point(panel1.Location.X + panel1.Size.Width / 4, panel1.Location.Y);
            StackColor = StackColors[colorSelector.Next(0, StackColors.Length)];
            StackSize = new Size(StackWidth, Constants.StackHeigth);

        }

        private void MoveInGravity()
        {
            Panel stack = (Panel)StackList[IndexOfStack-1];
            Random positionRandomer = new Random();

            float currentStackLocation = (float)(stack.Location.Y + panel1.Size.Height / Constants.Rows);
            float gameBoardVerticalUnit = (float)panel1.Size.Height / (float)Constants.Rows;
            float bottomOfGameBoardLocation = (float)(panel1.Location.Y + gameBoardVerticalUnit*(float)(Constants.Rows-IndexOfStack));

            if (currentStackLocation == bottomOfGameBoardLocation)
            {

                StackLocation = new Point(panel1.Location.X + positionRandomer.Next(0, (int)(panel1.Size.Width-StackWidth-1)), panel1.Location.Y);
                MakeAStack(StackList);
                Score++;

                if (Score >= highScore)
                    progressBar1.Value = highScore;
                else progressBar1.Value = Score;
            }
            else 
            {
                if (currentStackLocation == bottomOfGameBoardLocation - gameBoardVerticalUnit && IndexOfStack > 1)
                {
                    Panel previousStack = (Panel)StackList[IndexOfStack - 2];
                    Panel currentStack = (Panel)StackList[IndexOfStack - 1];

                    BrickTheStack(previousStack, currentStack);

                    StackLocation = new Point(panel1.Location.X + positionRandomer.Next(0, (int)(panel1.Size.Width - StackWidth - 1)), panel1.Location.Y);

                    if (!isGameOver)
                    {
                        //return;
                        Score++;

                        if (Score >= highScore)
                            progressBar1.Value = highScore;
                        else progressBar1.Value = Score;

                        MakeAStack(StackList);
                    }

                }
                else if (IndexOfStack >=Constants.Rows-15)
                {                  
                    NewFrameOfGameBoard(StackList);
                    MakeAStack(StackList);
                    return;             
                }
                else
                {
                    stack.Top += panel1.Size.Height / Constants.Rows;
                }

            }

        }

        private void NewFrameOfGameBoard(ArrayList stackList)
        {
            float gameBoardVerticalUnit = (float)panel1.Size.Height / (float)Constants.Rows;

            Random colorSelector = new Random();
            Random positionRandomer = new Random();

            ScreenShot();
            NumberOfFrames++;

            StackLocation = new Point(((Panel)stackList[IndexOfStack - 2]).Location.X, (int)(panel1.Location.Y + gameBoardVerticalUnit * (float)(Constants.Rows - 2)));
            StackSize = ((Panel)stackList[IndexOfStack - 2]).Size;
            StackColor = ((Panel)stackList[IndexOfStack - 2]).BackColor;

            ClearGameBoard();

            Panel stackFromPreviousFrame = new Panel();

            stackFromPreviousFrame.Size = StackSize;
            stackFromPreviousFrame.Location = StackLocation;
            stackFromPreviousFrame.BackColor = StackColor;

            stackList.Add(stackFromPreviousFrame);
            IndexOfStack = stackList.Count; 
            Controls.Add(stackFromPreviousFrame);
            stackFromPreviousFrame.BringToFront();

            StackLocation = new Point(panel1.Location.X +positionRandomer.Next(0,(int)panel1.Size.Width-StackWidth-1), panel1.Location.Y);
            StackSize = ((Panel)StackList[IndexOfStack - 1]).Size;
            StackColor = StackColors[colorSelector.Next(0,StackColors.Length-1)];

            if (autoLevel)
            {
                Level *= 2;
                if (Level >= Constants.LevelMaximum)
                    Level = Constants.LevelMaximum;

                timer1.Interval = Constants.TimerInterval / Level;
            }

            }

        private void BrickTheStack(Panel previousStack, Panel currentStack)
        {
            Random positionRandomer = new Random();
            
            float[] measurementsOfPreviousStack = MeasurementOfStack(previousStack);
            float[] measurementsOfCurrentStack = MeasurementOfStack(currentStack);

            float gameBoardVerticalUnit = (float)panel1.Size.Height / (float)Constants.Rows;

            if (measurementsOfCurrentStack[0] < measurementsOfPreviousStack[0] && measurementsOfCurrentStack[2] > measurementsOfPreviousStack[0])
            {
                float differenceOfStacksParam = measurementsOfPreviousStack[0] - measurementsOfCurrentStack[0];
                currentStack.Width -= (int)differenceOfStacksParam;
                currentStack.Location = new Point(previousStack.Location.X, currentStack.Location.Y + (int)gameBoardVerticalUnit);
            }
            else if (measurementsOfCurrentStack[2] > measurementsOfPreviousStack[2] && measurementsOfCurrentStack[0] < measurementsOfPreviousStack[2])
            {
                float differenceOfStacksParam2 = measurementsOfCurrentStack[2] - measurementsOfPreviousStack[2];
                float differenceOfStacksParam0 = measurementsOfPreviousStack[0] - measurementsOfCurrentStack[0];
                currentStack.Width -= (int)differenceOfStacksParam2;
                currentStack.Location = new Point(previousStack.Location.X - (int)differenceOfStacksParam0, currentStack.Location.Y + (int)gameBoardVerticalUnit);

            }
            else if (measurementsOfCurrentStack[2] < measurementsOfPreviousStack[0] || measurementsOfCurrentStack[0] > measurementsOfPreviousStack[2])
            {

                timer1.Stop();
                isGameOver = true;

                LevelForSaving = Level;
                ScoreForSaving = Score;

                currentStack.Location = new Point(currentStack.Location.X, currentStack.Location.Y + (int)gameBoardVerticalUnit);

                ScreenShot();

                DialogResult gameOverDialog = MessageBox.Show("Game Over!", "Stack++",MessageBoxButtons.OK, MessageBoxIcon.Information);


                button4.Enabled = true;

                StackWidth = Constants.StackWidth;
                InitializeGame();

                return;
            }
            else
            {
                currentStack.Location = new Point(currentStack.Location.X, currentStack.Location.Y + (int)gameBoardVerticalUnit);
            }

            StackSize = new Size(currentStack.Width, Constants.StackHeigth);



        }
        private float[] MeasurementOfStack(Panel stack)
        {
            float[] measurements = new float[3]; // 0 - upper-left location of measured stack, 1 - width of measured stack, 2 - upper-right location of measured stack

            measurements[0] = stack.Location.X;
            measurements[1] = stack.Size.Width;
            measurements[2] = stack.Location.X + stack.Size.Width;

            return measurements;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HighScoreChecker();
            progressBar1.Maximum = highScore;

            ClearGameBoard();

            StackIt();
           
        }

        private void StackIt()
        {
            isGameOver = false;
            button4.Enabled = false;

            Score = 0;
            
            imagesOfStackTower.Clear();

            NumberOfFrames = 1;

            MakeAStack(StackList);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveInGravity();
            label2.Text = "Score: " + Score;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MoveStackLeft((Panel)StackList[IndexOfStack - 1]);
        }

        private void MoveStackLeft(Panel currentStack)
        {
          
            float leftBound = panel1.Location.X;

            if (currentStack.Location.X <= leftBound)
            {
                return;
            }
            else
            {
                currentStack.Left -= Constants.StackStep;
            }
        }

        private void MoveStackRight(Panel currentStack)
        {
            float rightBound = panel1.Location.X + (panel1.Size.Width-currentStack.Width);

            if (currentStack.Location.X >= rightBound)
            {
                return;
            }
            else
            {
                currentStack.Left += Constants.StackStep;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MoveStackRight((Panel)StackList[IndexOfStack - 1]);
        }

        private void ClearGameBoard()
        {
            for (int index = 0; index <= IndexOfStack - 1; index++)
            {
                Control deletingStack = (Control)StackList[index];
                Controls.Remove(deletingStack);


            }

            StackList.Clear();
            IndexOfStack = 0;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch(e.KeyCode)
            {
                case Keys.A: MoveStackLeft((Panel)StackList[IndexOfStack-1]); break;
                case Keys.D: MoveStackRight((Panel)StackList[IndexOfStack - 1]); break;
                case Keys.P: PauseTheGame(); break;
                case Keys.Enter: StackIt(); break;
                case Keys.S: SnapIt(); break;
                case Keys.W:Level *= 2;
                    if (Level >= Constants.LevelMaximum)
                        Level = Constants.LevelMaximum;
                    timer1.Interval = Constants.TimerInterval / Level;
                    break;
                case Keys.X:Level /= 2;
                    if (Level <= Constants.LevelMinimum+1)
                        Level = Constants.LevelMinimum;
                    timer1.Interval = Constants.TimerInterval / Level;
                    break;

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex!=-1)
            {
                string getLevelValue = comboBox1.Text;
                Level = int.Parse(getLevelValue);
                timer1.Interval = Constants.TimerInterval / Level;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ScoreList sl = new ScoreList();
            if(isGameOver) sl.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                autoLevel = true;
                Level = 5;
                timer1.Interval = Constants.TimerInterval / Level;
            }
            else autoLevel = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SnapIt();
        }

        private void SnapIt()
        {
            ScreenShot();
            YourStackTower yst = new YourStackTower();
            yst.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PauseTheGame();
        }

        private void PauseTheGame()
        {
            if (!isPaused)
            {
                isPaused = true;
                timer1.Stop();
            }
            else
            {
                isPaused = false;
                timer1.Start();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
    }
}
