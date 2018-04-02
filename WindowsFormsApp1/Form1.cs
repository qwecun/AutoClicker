using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Boolean success_a = Form1.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 0x0000, 0x42);//Set hotkey as 'b'
            int FirstHotkeyId = 1;
            int FirstHotKeyKey = (int)Keys.Right;
            Boolean A_Registered = RegisterHotKey(this.Handle, FirstHotkeyId, 0x0000, FirstHotKeyKey);

            int SecondHotkeyId = 2;
            int SecondHotKeyKey = (int)Keys.Left;
            Boolean B_Registered = RegisterHotKey(this.Handle, SecondHotkeyId, 0x0000, SecondHotKeyKey);

            radioButton1.Checked = true;
            radioButton3.Checked = true;

        }

        private int x, y;
        private bool control = false;

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        //Press Space for Running/Stopping auto clicker;Press Enter for recording Positions
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();
                switch (id)
                {
                    case 1:
                        control = !control;
                        break;
                    case 2:
                        if (radioButton1.Checked == true)
                        {
                            textBox1.Text = x.ToString();
                            textBox2.Text = y.ToString();
                            radioButton1.Checked = false;
                            radioButton2.Checked = true;
                        }
                        else
                        {
                            textBox4.Text = x.ToString();
                            textBox5.Text = y.ToString();
                            radioButton1.Checked = true;
                            radioButton2.Checked = false;
                        }
                        break;
                }

            }
            base.WndProc(ref m);
        }
        public static void LeftClick(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = 1;
            timer2.Start();
            timer2.Interval = Int32.Parse(textBox3.Text);

           // MessageBox.Show();
            KeyPreview = true;

        }
        //Switch Stopping or Running
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                control = false;
            }
        }

        //Looking for the position of cursor
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            x = Cursor.Position.X;
            y = Cursor.Position.Y;

            label1.Text = x + "," + y;
        }
        int counter = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {

            if (control == true)//Checking clicker status
            {
                if (radioButton3.Checked)//One position
                {
                    int movX = Int32.Parse(textBox1.Text);
                    int movY = Int32.Parse(textBox2.Text);
                    LeftClick(movX, movY);
                }
                else//Two position
                {
                    if (counter % 2 == 0)//Swap clicking for each interval
                    {
                        int movX = Int32.Parse(textBox1.Text);
                        int movY = Int32.Parse(textBox2.Text);
                        LeftClick(movX, movY);
                    }
                    else 
                    {
                        int movX = Int32.Parse(textBox4.Text);
                        int movY = Int32.Parse(textBox5.Text);
                        LeftClick(movX, movY);
                    }
                    counter++;
                }

            }
        }

        //Display first Position and hide second
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                radioButton2.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                radioButton1.Checked = true;
            }
        }

        //Display first and second
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                radioButton2.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
            }
        }

        //Update interval of click speed
        private void button1_Click(object sender, EventArgs e)
        {
            timer2.Interval = Int32.Parse(textBox3.Text);
        }


    }
}
