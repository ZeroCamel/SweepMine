using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace SweepMine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const int ButtonSize = 16;
        int X, Y;
        MineControl[] matrix = null;
        int[,] g = null;
        private int CountNeighbours(int i, int j, int x, int y)
        {
            int res = 0;
            for (int ni = i - 1; ni <= i + 1; ni++)
            {
                if (ni < 0 || ni >= x) continue;
                for (int nj = j - 1; nj <= j + 1; nj++)
                {
                    if (nj < 0 || nj >= y) continue;
                    if (g[ni, nj] == 9) res++;
                }
            }
            return res;
        }

        private void GenerateMap(int x, int y, int mines)
        {
            g = new int[x, y];
            Random r = new Random();
            //布雷
            for (int i = 0; i < mines; i++)
            {
                int mx = r.Next()%x;
                int my = r.Next()%y;
                g[mx, my] = 9; //9 - mine
            }
            //计算其余格子的数值
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (g[i,j] == 0)
                    {
                        g[i, j] = CountNeighbours(i, j, x, y);
                    }
                }
            }
        }

        private void FloodFill(int i, int j)
        {
            if (i < 0 || i >= X || j < 0 || j >= Y) return;
            if (g[i, j] == 0)
            {
                g[i, j] = -1;
                FloodFill(i - 1, j);
                FloodFill(i, j - 1);
                FloodFill(i + 1, j);
                FloodFill(i, j + 1);
                FloodFill(i - 1, j - 1);
                FloodFill(i - 1, j + 1);
                FloodFill(i + 1, j - 1);
                FloodFill(i + 1, j + 1);
            }
            matrix[i * Y + j].Unseal();
        }

        private void CleanUp()
        {
            if (matrix != null)
            {
                for (int i = 0; i < matrix.Length; i++)
                {
                    matrix[i].Dispose();
                }
            }
        }

        private void MineMouseDown(object sender, EventArgs e)
        {
            MineControl btn = (MineControl)sender;
            switch (btn.MouseStatus)
            {
                case 1: //left
                    btn.Press();
                    break;
                case 2: //right
                    btn.PutFlag();
                    break;
                case 3: //both
                    //按下周围9个键
                    for (int i = -1; i <= 1; i++)
                    {
                        if (i + btn.LocationX < 0 || i + btn.LocationX >= X) continue;
                        for (int j = -1; j <= 1; j++)
                        {
                            if (j + btn.LocationY < 0 || j + btn.LocationY >= Y) continue;
                            matrix[(i + btn.LocationX) * Y + j + btn.LocationY].Press();
                        }
                    }
                    break;
            }
        }

        private bool CheckCondition(int x, int y)
        {
            //左右键双击时,当且仅当周围的flag数和当前格子的数值相等,
            //才能同时翻开9格
            if (!matrix[x * Y + y].IsUnseal) return false;
            int cnt=0;
            for (int i = -1; i <= 1; i++)
            {
                if (i + x < 0 || i + x >= X) continue;
                for (int j = -1; j <= 1; j++)
                {
                    if (j + y < 0 || j + y >= Y) continue;
                    if (i == 0 && j == 0) continue;
                    if (matrix[(i + x) * Y + j + y].IsFlag) 
                        cnt++;
                }
            }
            if (cnt == matrix[x * Y + y].Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MineMouseUp(object sender, EventArgs e)
        {
            MineControl btn = (MineControl)sender;
            switch (btn.MouseStatus)
            {
                case 1: //left
                    btn.Unseal();
                    if (btn.Value == 0)
                        FloodFill(btn.LocationX, btn.LocationY);
                    break;
                case 2: //right
                    //do nothing
                    break;
                case 3: //both
                    bool doUnseal = false;
                    //检查是否符合翻开的条件
                    if(CheckCondition(btn.LocationX,btn.LocationY))
                        doUnseal = true;
                    for (int i = -1; i <= 1; i++)
                    {
                        if (i + btn.LocationX < 0 || i + btn.LocationX >= X) continue;
                        for (int j = -1; j <= 1; j++)
                        {
                            if (j + btn.LocationY < 0 || j + btn.LocationY >= Y) continue;
                            MineControl curbtn =matrix[(i + btn.LocationX) * Y + j + btn.LocationY];
                            if (doUnseal)
                            {
                                curbtn.Unseal();
                                if (curbtn.Value == 0)
                                    FloodFill(curbtn.LocationX, curbtn.LocationY);
                            }
                            else
                            {
                                curbtn.UnPress();
                            }
                        }
                    }
                    break;
            }
        }

        private void InitializeGame(int x, int y, int mines)
        {

            CleanUp();
            this.X = x; this.Y = y;
            //布雷
            GenerateMap(x, y, mines);
            matrix = new MineControl[x * y];
            //初始化MineControl对象数组
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    MineControl currentbtn = new MineControl();
                    currentbtn.Width = ButtonSize;
                    currentbtn.Height = ButtonSize;
                    currentbtn.Top = 25 + ButtonSize * i;
                    currentbtn.Left =  + ButtonSize * j;
                    currentbtn.Visible = true;
                    currentbtn.Value = g[i, j];
                    currentbtn.MouseDownEvent += MineMouseDown;
                    currentbtn.MouseUpEvent += MineMouseUp;
                    currentbtn.LocationX = i;
                    currentbtn.LocationY = j;
                    this.Controls.Add(currentbtn);
                    matrix[i * y + j] = currentbtn;
                }
            }
            this.Width = 10 + ButtonSize * y;
            this.Height = 45 + ButtonSize * x + ButtonSize;
            this.Refresh();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeGame(10,10,10);
        }

    }
}