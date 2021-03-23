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

        private int height; // высота 1 линии разметки

        private Brush BrushStop = new SolidBrush(Color.Red);

        private Brush BrushPause = new SolidBrush(Color.Blue);

        private Pen PenRepeat = new Pen(Color.Gold, 2);

        private Pen[] processes = { new Pen(Color.Red, 2), new Pen(Color.Green, 2), new Pen(Color.Blue, 2), new Pen(Color.Black, 2), 
            new Pen(Color.Purple, 2), new Pen(Color.Magenta, 2), new Pen(Color.Lime, 2),
            new Pen(Color.Salmon, 2), new Pen(Color.Indigo, 2), new Pen(Color.Gold, 2)
        };

        public List<Thread> Threads;

        public List<(int, int, int, int)> markArray; //Список данных о метках, где заканчивается работа программы

        private List<(int, int, int, int)> marks; //Список данных о метках, где заканчивается работа программы

        private List<(int, int, int, int)> ProcessPause; //Список данных о метках, где останавливается процесс

        private List<(int, int, int, int)> ProcessStop; //Список данных о метках, где заканчивается процесс

        private List<(int, int, int, int)> ProcessRepeat; //Список данных о метках, где продолжается процесс

        private void Initial()
        {
            Threads = new List<Thread>();
            markArray = new List<(int, int, int, int)>();
            marks = new List<(int, int, int, int)>();
            marks.Add((1, 0, 1, height));
            ProcessStop = new List<(int, int, int, int)>();
            ProcessPause = new List<(int, int, int, int)>();
            ProcessRepeat = new List<(int, int, int, int)>();
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            gr = Graphics.FromImage(bmp);
            Draw();
        }

        public FormMain()
        {
            InitializeComponent();
            height = pictureBox.Height / 6;
            Initial();
            DrawDescription();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Initial();
            SystemCore systemCore = new SystemCore(this);
            systemCore.Start();

            marks.Add(markArray.Last());
            TrueFalse();
            DrawThread(gr);
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
                g.DrawLine(pen, 0, (i + 1) * height, pictureBox.Width, (i + 1) * height);
            }
            g.DrawLine(pen, pictureBox.Width - 2, 0, pictureBox.Width - 2, pictureBox.Height);
        }

        public void DrawThread(Graphics g)
        {
            int tempWidth = 0;
            int tempHeight = 15;

            Pen mark = new Pen(Color.Magenta, 2);
          
            foreach (var thread in Threads)
            {
                int h = thread.ThreadId * 18 + tempHeight; // регулируем высоту переменной

                g.DrawLine(processes[thread.ProcessId], tempWidth * 10 + 2, h, (tempWidth + thread.ThreadOneIterationTime) * 10, h);

                int markWidth = (tempWidth + thread.ThreadOneIterationTime) * 10 - 13; // координата x для отметок окончания процесса
                if (thread.Status == ThreadStatusEnum.Stop)
                {
                    ProcessStop.Add((markWidth, h - 10, 8, 10));
                }
                else if (thread.Status == ThreadStatusEnum.Pause)
                {
                    ProcessPause.Add((markWidth, h - 10, 8, 10));
                }
                else if (thread.Status == ThreadStatusEnum.Repeat)
                {
                    ProcessRepeat.Add((tempWidth * 10 + 2, h - 8, tempWidth * 10 + thread.ThreadOneIterationTime * 4, h - 8));
                }

                tempWidth += thread.ThreadOneIterationTime;
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

            foreach (var e in ProcessStop)
            {
                DrawStop(g, BrushStop, e.Item1, e.Item2, e.Item3, e.Item4);

            }

            foreach (var e in ProcessPause)
            {
                DrawPause(g, BrushPause, e.Item1, e.Item2, e.Item3, e.Item4);
            }

            foreach (var e in ProcessRepeat)
            {
                DrawRepeat(g, PenRepeat, e.Item1, e.Item2, e.Item3, e.Item4, 4);
            }
            pictureBox.Image = bmp;
        }

        private void DrawDescription()
        {
            Bitmap bm = new Bitmap(pictureBoxPause.Width, pictureBoxPause.Height);
            Graphics g = Graphics.FromImage(bm);
            DrawPause(g, BrushPause, 0, 0, pictureBoxPause.Width - 10, pictureBoxPause.Height - 10);
            pictureBoxPause.Image = bm;

            bm = new Bitmap(pictureBoxStop.Width, pictureBoxStop.Height);
            g = Graphics.FromImage(bm);
            DrawStop(g, BrushStop, 0, 0, pictureBoxStop.Width - 10, pictureBoxStop.Height - 10);
            pictureBoxStop.Image = bm;

            bm = new Bitmap(pictureBoxRepeat.Width, pictureBoxRepeat.Height);
            g = Graphics.FromImage(bm);
            DrawRepeat(g, PenRepeat, 0, pictureBoxRepeat.Height / 2, pictureBoxRepeat.Width, pictureBoxRepeat.Height / 2, pictureBoxRepeat.Height / 3);
            pictureBoxRepeat.Image = bm;
        }

        private void DrawPause(Graphics graphic, Brush brush, int w1, int h1, int w2, int h2)
        {
            graphic.FillRectangle(brush, w1 + 2, h1, w2 + 2, h2);
        }

        private void DrawStop(Graphics graphic, Brush brush, int w1, int h1, int w2, int h2)
        {
            graphic.FillEllipse(brush, w1 + 2, h1, w2 + 2, h2);
        }

        private void DrawRepeat(Graphics graphic, Pen pen, int w1, int h1, int w2, int h2, int triengleh)
        {
            float width = w1 + Convert.ToSingle((w2 - w1) * 0.8);
            graphic.DrawLine(pen, w1, h1, width, h2);

            // рисуем треугольник
            graphic.DrawLine(pen, width, h1 - triengleh, width, h1 + triengleh);
            graphic.DrawLine(pen, width, h1 - triengleh, w2, h2);
            graphic.DrawLine(pen, width, h1 + triengleh, w2, h2);
        }

        #region logic
        public void AddMark()
        {
            int tempWidth = 0;

            int tempHeight = 20;

            foreach (var thread in Threads)
            {
                tempWidth += thread.ThreadOneIterationTime;


                if (tempWidth + 10 > pictureBox.Width / 10)
                {
                    tempWidth = 0;
                    tempHeight += height;
                }

            }

            markArray.Add((tempWidth * 10, tempHeight - 20, tempWidth * 10, tempHeight + 45));
        }

        private void TrueFalse()
        {
            int n = Threads.Count;
            for (int i = 0; i < n; ++i)
            {
                var thread = Threads[i];
                if (i == n - 1)
                {
                    Threads[i].Status = ThreadStatusEnum.Stop;
                }
                else if (Threads[i].ProcessId != Threads[i + 1].ProcessId)
                {
                    bool flag = true;
                    for (int j = i + 1; j < n; ++j)
                    {
                        if (Threads[i].ProcessId == Threads[j].ProcessId)
                        {
                            Threads[i].Status = ThreadStatusEnum.Pause;
                            Threads[j].Status = ThreadStatusEnum.Repeat;
                            flag = false;
                            break;
                        }
                    }
                    if (flag) { thread.Status = ThreadStatusEnum.Stop; }
                }
            }
        }
        #endregion
    }
}
