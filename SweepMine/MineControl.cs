using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SweepMine
{
    public partial class MineControl : UserControl
    {
        enum ButtonStatus
        {
            Initial,
            Pressed,
            Flag,
            QuestionMark,
            Unseal
        }

        public int Value;
        ButtonStatus BtStatus;
        public int MouseStatus;
        public int LocationX;
        public int LocationY;

        public bool IsUnseal
        {
            get
            {
                if (BtStatus == ButtonStatus.Unseal)
                    return true;
                else
                    return false;
            }
        }
        public bool IsFlag
        {
            get
            {
                if (BtStatus == ButtonStatus.Flag)
                    return true;
                else
                    return false;
            }
        }
        public MineControl()
        {
            InitializeComponent();
            BtStatus = ButtonStatus.Initial;
            MouseStatus = 0;
        }

        private void MineControl_Load(object sender, EventArgs e)
        {

        }

        private void MineControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            switch (BtStatus)
            {
                case ButtonStatus.Initial:
                    g.DrawImage(Properties.Resources.bt, 0, 0); break;
                case ButtonStatus.Pressed:
                    g.DrawImage(Properties.Resources.empty, 0, 0); break;
                case ButtonStatus.QuestionMark:
                    g.DrawImage(Properties.Resources.questionmark, 0, 0); break;
                case ButtonStatus.Flag:
                    g.DrawImage(Properties.Resources.flag1, 0, 0); break;
                case ButtonStatus.Unseal:
                    switch (Value)
                    {
                        case 0:
                            g.DrawImage(Properties.Resources.empty, 0, 0); break;
                        case 1:
                            g.DrawImage(Properties.Resources._1, 0, 0); break;
                        case 2:
                            g.DrawImage(Properties.Resources._2, 0, 0); break;
                        case 3:
                            g.DrawImage(Properties.Resources._3, 0, 0); break;
                        case 4:
                            g.DrawImage(Properties.Resources._4, 0, 0); break;
                        case 5:
                            g.DrawImage(Properties.Resources._5, 0, 0); break;
                        case 6:
                            g.DrawImage(Properties.Resources._6, 0, 0); break;
                        case 7:
                            g.DrawImage(Properties.Resources._7, 0, 0); break;
                        case 8:
                            g.DrawImage(Properties.Resources._8, 0, 0); break;
                        case 9:
                            g.DrawImage(Properties.Resources.mine, 0, 0); break;
                    }
                    break;
            }
        }

        public void Press()
        {
            if (BtStatus == ButtonStatus.Initial)
            {
                BtStatus = ButtonStatus.Pressed;
                this.Refresh();
            }
        }

        public void UnPress()
        {
            if (BtStatus == ButtonStatus.Pressed)
            {
                BtStatus = ButtonStatus.Initial;
                this.Refresh();
            }
        }

        public void PutFlag()
        {
            if (BtStatus == ButtonStatus.Initial)
            {
                BtStatus = ButtonStatus.Flag;
                this.Refresh();
            }
            else if (BtStatus == ButtonStatus.Flag)
            {
                BtStatus = ButtonStatus.QuestionMark;
                this.Refresh();
            }
            else if (BtStatus == ButtonStatus.QuestionMark)
            {
                BtStatus = ButtonStatus.Initial;
                this.Refresh();
            }
        }

        public void Unseal()
        {
            if (BtStatus == ButtonStatus.Pressed || BtStatus == ButtonStatus.Initial || BtStatus == ButtonStatus.QuestionMark)
            {
                BtStatus = ButtonStatus.Unseal;
                this.Refresh();
            }
        }

        public EventHandler MouseDownEvent;
        public EventHandler MouseUpEvent;
        private void MineControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.MouseStatus |= 1;
            if (e.Button == MouseButtons.Right)
                this.MouseStatus |= 2;
            if (MouseDownEvent != null)
                MouseDownEvent(sender, e);
        }

        private void MineControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseUpEvent != null)
                MouseUpEvent(sender, e);
            if (e.Button == MouseButtons.Left)
                this.MouseStatus &= ~1;
            if (e.Button == MouseButtons.Right)
                this.MouseStatus &= ~2;
        }
    }
}
