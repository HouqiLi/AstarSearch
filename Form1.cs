using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstarSearch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        enum co_attr
        {
            WALL,
            START,
            DIST,
        }

        co_attr mouse_choice = co_attr.WALL;

        //byte[,] astar_map = new byte[10, 10]
        //{
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        //};

        cbtn[,] astar_btn = new cbtn[20, 20];   //按钮
        byte[,] astar_array = new byte[20, 20]; //区分坐标是否是墙,0为墙

        AstarPoint pa = new AstarPoint();
        AstarPoint pb = new AstarPoint();

        //初始化所有按钮和数组
        void init()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    astar_array[i, j] = 1;      //非墙
                    astar_btn[i, j].BackColor = Color.Transparent;
                }
            }
        }

        //自定义按钮类继承Button
        public class cbtn : Button
        {
            int x, y;
            public int X
            {
                set { x = value; }
                get { return x; }
            }
            public int Y
            {
                set { y = value; }
                get { return y; }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    astar_btn[i, j] = new cbtn();
                    astar_btn[i, j].X = i;
                    astar_btn[i, j].Y = j;
                    astar_btn[i, j].Size = new Size(20, 20);
                    astar_btn[i, j].Location = new Point(i * 20, j * 20);
                    this.Controls.Add(astar_btn[i, j]);
                    astar_btn[i, j].MouseDown += Form1_MouseDown;
                }
            }
        }
        
        //格子按下标记起点or终点or墙
        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            cbtn bb = (cbtn)sender;
            int x = bb.X;       //对应于数组的列
            int y = bb.Y;       //对应于数组的行
            if (mouse_choice == co_attr.WALL)
            {
                astar_btn[x, y].BackColor = Color.Black;
                astar_array[y, x] = 0;  //墙标记
            }
            if (mouse_choice == co_attr.START)
            {
                astar_btn[x, y].BackColor = Color.Green;
                pa.x = x;
                pa.y = y;
            }
            if (mouse_choice == co_attr.DIST)
            {
                astar_btn[x, y].BackColor = Color.Red;
                pb.x = x;
                pb.y = y;
            }
        }

        //初始化
        private void button2_Click(object sender, EventArgs e)
        {
            init();
        }

        //标记起点
        private void button3_Click(object sender, EventArgs e)
        {
            mouse_choice = co_attr.START;
        }

        //标记终点
        private void button4_Click(object sender, EventArgs e)
        {
            mouse_choice = co_attr.DIST;
        }

        //标记墙
        private void button5_Click(object sender, EventArgs e)
        {
            mouse_choice = co_attr.WALL;
        }

        //寻路
        private void button6_Click(object sender, EventArgs e)
        {
            AstarAlgo algo = new AstarAlgo(astar_array);
            List<AstarPoint> plist = new List<AstarPoint>();
            plist = algo.astar_find_way(pa, pb);

            foreach (AstarPoint p in plist)
            {
                astar_btn[p.x, p.y].BackColor = Color.Yellow;
            }
            astar_btn[pb.x, pb.y].BackColor = Color.Red;
        }
    }
}
