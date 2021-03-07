using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace os_lab1
{
    public partial class FormMain : Form
    {
        private Bitmap bmp;

        public Graphics gr;

        private int height; // высота 1 линии

        Pen[] processes = { new Pen(Color.Green, 2), new Pen(Color.Red, 2), new Pen(Color.Blue, 2) };

        public List<(int, int, int)> Threads; // ProcessId, ThreadId, oneIterationTime

        public List<(int, int, int, int)> markArray;//Список данных о метках, где заканчивается работа программы

        public List<(int, int, int, int)> marks;//Список данных о метках, где заканчивается работа программы

        public FormMain()
        {
            InitializeComponent();
            height = pictureBox.Height / 5;
            Threads = new List<(int, int, int)>();
            markArray = new List<(int, int, int, int)>();
            marks = new List<(int, int, int, int)>();
            marks.Add((1, 0, 1, height));
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            gr = Graphics.FromImage(bmp);
            Draw();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            SystemCore systemCore = new SystemCore(this);
            systemCore.Start();

            marks.Add(markArray.Last());
        }

        private void Draw()
        {           
            DrawMarking(gr);
            pictureBox.Image = bmp;
        }

        private void DrawMarking(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 3);
            for (int i = 0; i < pictureBox.Height / height; i++)
            {
                g.DrawLine(pen, 0, (i + 1) * height,  pictureBox.Width, (i + 1) * height);
            }
            g.DrawLine(pen, pictureBox.Width - 2, 0, pictureBox.Width - 2, pictureBox.Height);
        }

        public void DrawThread(Graphics g)
        {
            int tempWidth = 0;

            int tempHeight = 20;

            Pen mark = new Pen(Color.Magenta, 2);

            foreach (var thread in Threads)
            {
                g.DrawLine(processes[thread.Item1], tempWidth * 10 + 2, thread.Item2 * 25 + tempHeight, (tempWidth + thread.Item3) * 10, thread.Item2 * 25 + tempHeight);
                tempWidth += thread.Item3;

                if (tempWidth + 10 > pictureBox.Width / 10)
                {
                    tempWidth = 0;
                    tempHeight += height;
                }
            }

            foreach (var m in marks)
            {
                g.DrawLine(mark, m.Item1 + 2, m.Item2, m.Item3 + 2, m.Item4);
            }

            markArray.Add(( tempWidth * 10, tempHeight - 20, tempWidth * 10, tempHeight + 45 ));

            pictureBox.Image = bmp;
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            Threads = new List<(int, int, int)>();
            markArray = new List<(int, int, int, int)>();
            marks = new List<(int, int, int, int)>();
            marks.Add((1, 0, 1, height));
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            gr = Graphics.FromImage(bmp);
            Draw();
        }
    }
}
