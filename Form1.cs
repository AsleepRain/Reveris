using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        private readonly string[] Info = new string[61];
        private readonly int[,] Map = new int[10, 10];
        //0 represent no
        //1 represent black
        //2 represent white

        private int MyColor;
        private int x1;
        private int y1;
        //position

        public Form1()
        {
            InitializeComponent();
        }

        private void Show_Can_Position()
        {
            //use image show can position
            int i;
            int j;
            Graphics g = pictureBox1.CreateGraphics();
            Bitmap bitmap = new Bitmap("Info2.png");

            //info image
            int n = 0;
            for (i = 1; i <= 8; i++)
            {
                for (j = 1; j <= 8; j++)
                {
                    if (Map[i, j] == 0 & Can_go(i, j))
                    {
                        Info[n] = i + "|" + j;
                        n = n + 1;
                        g.DrawImage(bitmap, (i - 1)*45 + 26, (j - 1)*45 + 26, 30, 30);
                    }
                }
            }
        }

        //calculate available position
        private int Show_Can_Num()
        {
            int i, j;
            int n = 0;
            for (i = 1; i <= 8; i++)
            {
                for (j = 1; j <= 8; j++)
                {
                    if (Can_go(i, j))
                    {
                        Info[n] = i + "|" + j;
                        n = n + 1;
                    }
                }
            }
            return n;
            //num of available position
        }

        private void Cls_Can_Position()
        {
            int n;
            string a;
            string b;
            int x;
            int y;
            string s;
            Graphics g = pictureBox1.CreateGraphics();
            Bitmap bitmap = new Bitmap("BackColor.png");
            //Background image 
            for (n = 0; n <= 60; n++)
            {
                s = Info[n];
                if (string.IsNullOrEmpty(s)) break;

                a = s.Substring(0, 1);
                b = s.Substring(s.IndexOf('|', 1) + 1);
                x = Convert.ToInt16(a);
                y = Convert.ToInt16(b);
                if (Map[x, y] == 0)
                {
                    g.DrawImage(bitmap, (x - 1)*45 + 26, (y - 1)*45 + 26, 30, 30);
                }
            }
        }

        private bool CheckDirect(int x1, int y1, int dx, int dy)
        {
            int x, y;
            bool flag;
            x = x1 + dx;
            y = y1 + dy;
            flag = false;
            while (InBoard(x, y) & !Ismychess(x, y) & Map[x, y] != 0)
            {
                x += dx;
                y += dy;
                flag = true; //Constitute the trend of attack 
            }
            if (InBoard(x, y) & Ismychess(x, y) & flag)
            {
                return true; //direction available
            }
            return false;
        }

        private void DirectReverse(int x1, int y1, int dx, int dy)
        {
            int x, y;
            bool flag;
            x = x1 + dx;
            y = y1 + dy;
            flag = false;
            while (InBoard(x, y) & !Ismychess(x, y) & Map[x, y] != 0)
            {
                x += dx;
                y += dy;
                flag = true; //Constitute the trend of attack
            }
            if (InBoard(x, y) & Ismychess(x, y) & flag)
            {
                do
                {
                    x -= dx;
                    y -= dy;
                    if ((x != x1 || y != y1)) FanQi(x, y);
                } while ((x != x1 || y != y1));
            }
        }

        private bool InBoard(int x, int y)
        {
            if (x >= 1 & x <= 8 & y >= 1 & y <= 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Can_go(int x1, int y1)
        {
            //decide from left, top left, top, top right, right, lower right, lower, lower left
            if (CheckDirect(x1, y1, -1, 0))//left
            {
                return true;
            }
            if (CheckDirect(x1, y1, -1, -1))//top left
            {
                return true;
            }
            if (CheckDirect(x1, y1, 0, -1))//top
            {
                return true;
            }
            if (CheckDirect(x1, y1, 1, -1))//top right
            {
                return true;
            }
            if (CheckDirect(x1, y1, 1, 0))//right
            {
                return true;
            }
            if (CheckDirect(x1, y1, 1, 1))//lower right
            {
                return true;
            }
            if (CheckDirect(x1, y1, 0, 1))//lower
            {
                return true;
            }
            if (CheckDirect(x1, y1, -1, 1))//lower left
            {
                return true;
            }

            return false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int x, y;
            int n = 0;
            x1 = (e.X - 22)/45 + 1;
            y1 = (e.Y - 22)/45 + 1;
            if (!Can_go(x1, y1))
            {
                toolStripStatusLabel1.Text = "You can not place piece on here!";
                return;
            }

            if (MyColor == 1)
            {
                listBox1.Items.Add("New Black on(" + Convert.ToChar(x1 + 64).ToString() + ", " + y1 + ")");
            }
            if (MyColor == 2)
            {
                listBox1.Items.Add("New White on(" + Convert.ToChar(x1 + 64).ToString() + ", " + y1 + ")");
            }

            //(x1,y1) back original color
            Graphics g = pictureBox1.CreateGraphics();
            Bitmap bitmap = new Bitmap("WhiteStone.png");
            if (MyColor == 2)
            {
                Map[x1, y1] = 2;
                g.DrawImage(bitmap, (x1 - 1) * 45 + 22, (y1 - 1) * 45 + 22, 45, 45);
            }
            if (MyColor == 1)
            {
                Map[x1, y1] = 1;
                bitmap = new Bitmap("BlackStone.png");
                g.DrawImage(bitmap, (x1 - 1) * 45 + 22, (y1 - 1) * 45 + 22, 45, 45);
            }

            //reverse from left, top left, top, top right, right, lower right, lower, lower left
            if (CheckDirect(x1, y1, -1, 0))//reverse from left
            {
                DirectReverse(x1, y1, -1, 0);
            }
            if (CheckDirect(x1, y1, -1, -1))//reverse from top left
            {
                DirectReverse(x1, y1, -1, -1);
            }
            if (CheckDirect(x1, y1, 0, -1))//reverse from top
            {
                DirectReverse(x1, y1, 0, -1);
            }
            if (CheckDirect(x1, y1, 1, -1))//reverse from top right
            {
                DirectReverse(x1, y1, 1, -1);
            }
            if (CheckDirect(x1, y1, 1, 0))//reverse from right
            {
                DirectReverse(x1, y1, 1, 0);
            }
            if (CheckDirect(x1, y1, 1, 1))//reverse from lower right
            {
                DirectReverse(x1, y1, 1, 1);
            }
            if (CheckDirect(x1, y1, 0, 1))//reverse from lower
            {
                DirectReverse(x1, y1, 0, 1);
            }
            if (CheckDirect(x1, y1, -1, 1))//reverse from lower left
            {
                DirectReverse(x1, y1, -1, 1);
            }

            Cls_Can_Position();//clear tips

            if (MyColor == 1)
            {
                //tell white play
                MyColor = 2;
                toolStripStatusLabel1.Text = "White play!";
            }
            else
            {
                //tell black play
                MyColor = 1;
                toolStripStatusLabel1.Text = "Black play!";
            }

            Show_Can_Position();//show tips
            if (Show_Can_Num() == 0)
            {
                MessageBox.Show("Tips", "No place to go the other side! Please continue.");
                if (MyColor == 1)
                {
                    MyColor = 2;
                    toolStripStatusLabel1.Text = "White continue.";
                }
                else
                {
                    MyColor = 1;
                    toolStripStatusLabel1.Text = "Black continue";
                }
                Show_Can_Position();//show tips
            }
            //check whether game end
            int whitenum = 0;
            int blacknum = 0;
            for (x = 1; x <= 8; x++)
            {
                for (y = 1; y <= 8; y++)
                {
                    if (Map[x, y] != 0)
                    {
                        n = n + 1;
                        if (Map[x, y] == 2)
                            whitenum += 1;
                        if (Map[x, y] == 1)
                            blacknum += 1;
                    }
                }
            }
            if (n == 64)//when map is full
            {
                if (blacknum > whitenum)
                {
                    MessageBox.Show("End, Black Won.", "Black:" + blacknum + "White:" + whitenum);
                }
                if (whitenum > blacknum)
                {
                    MessageBox.Show("End, White Won.", "Black:" + blacknum + "White:" + whitenum);
                }
                if (whitenum == blacknum)
                {
                    MessageBox.Show("Draw");
                }
                pictureBox1.Enabled = false;//game end, can not play
                button1.Enabled = true;//"start" button available
                return;
            }
            //when map is not full
            if (whitenum == 0)
            {
                MessageBox.Show("End, Black Won.", "Black:" + blacknum + "White:" + whitenum);
                pictureBox1.Enabled = false;//game end, can not play
                button1.Enabled = true;//"start" button available
            }
            if (blacknum == 0)
            {
                MessageBox.Show("End, White Won.", "Black:" + blacknum + "White:" + whitenum);
                pictureBox1.Enabled = false;//game end, can not play
                button1.Enabled = true;//"start" button available
            }
        }

        private void FanQi(int x, int y)
        {
            Graphics g = pictureBox1.CreateGraphics();
            Bitmap bitmap = new Bitmap("WhiteStone.png");

            //1 represent black
            //2 represent white
            if (Map[x, y] == 1)
            {
                Map[x, y] = 2;
                g.DrawImage(bitmap, (x - 1)*45 + 22, (y - 1)*45 + 22, 45, 45);
            }
            else
            {
                Map[x, y] = 1;
                bitmap = new Bitmap("BlackStone.png");
                g.DrawImage(bitmap, (x - 1)*45 + 22, (y - 1)*45 + 22, 45, 45);
            }
            Thread.Sleep(200);//delay 0.2 second
            listBox1.Items.Add(" (" + Convert.ToChar(x + 64).ToString() + "，" + y + ")was reversed.");
        }

        private bool Ismychess(int x, int y)
        {
            if (Map[x, y] == MyColor)
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //start game
            int x, y;
            Graphics g = pictureBox1.CreateGraphics();
            Bitmap bitmap = new Bitmap("WhiteStone.png");
            pictureBox1.Enabled = true;
            pictureBox1.Refresh();
            for (x = 1; x <= 8; x++)
            {
                for (y = 1; y <= 8; y++)
                {
                    Map[x, y] = 0;
                }
            }
            listBox1.Items.Clear();

            //first 4 pieces
            x = 4;
            y = 4;
            g.DrawImage(bitmap, (x - 1)*45 + 22, (y - 1)*45 + 22, 45, 45);
            x = 5;
            y = 5;
            g.DrawImage(bitmap, (x - 1)*45 + 22, (y - 1)*45 + 22, 45, 45);
            bitmap = new Bitmap("BlackStone.png");
            x = 5;
            y = 4;
            g.DrawImage(bitmap, (x - 1)*45 + 22, (y - 1)*45 + 22, 45, 45);
            x = 4;
            y = 5;
            g.DrawImage(bitmap, (x - 1)*45 + 22, (y - 1)*45 + 22, 45, 45);
            Map[4, 4] = 2;
            //0no
            //1balck
            //2white
            Map[5, 5] = 2;
            Map[4, 5] = 1;
            Map[5, 4] = 1;
            MyColor = 1;

            //my color is black
            toolStripStatusLabel1.Text = "Black First";
            Text = "Reversi Game";
            Show_Can_Position();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Show_Can_Position();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cls_Can_Position();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}