using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace InputDisplay
{
    public partial class FormDisplay : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private FormMain _formParent;
        private Graphics _bg;
        private Graphics _g;
        private const int _wndBaseHeight = 180;
        private const int _wndBaseWidth = 180;
        private const int _wndHeight = 180;
        private const int _wndWidth = 180;
        private static Point _joyCentre = new Point(_wndBaseWidth / 2, _wndBaseHeight / 2);
        private static Point _prevPos = new Point(_joyCentre.X, _joyCentre.Y);
        private const int _nFramesFade = 12;
        static volatile JoystickState joyState;
        private static List<JoyLineTrail> trailsLine = new List<JoyLineTrail>();
        private static List<JoyCircTrail> trailsCirc = new List<JoyCircTrail>();
        private static Vertices _vertices = new Vertices
        {
            bl = new Point(-55, 60),
            bm = new Point(0, 70),
            br = new Point(55, 60),
            mr = new Point(65, 0),
            tr = new Point(55, -60),
            tm = new Point(0, -70),
            tl = new Point(-55, -60),
            ml = new Point(-65, 0),
        };

        private struct Vertices
        {
            public Point bl, bm, br, mr, tr, tm, tl, ml;
        }

        public FormDisplay(FormMain parent)
        {
            _formParent = parent;
            InitializeComponent();
        }

        private void FormDisplay_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.Gray;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ClientSize = new Size(_wndWidth, _wndHeight);
            this.DoubleBuffered = true;
            this.BackColor = Color.BlanchedAlmond;
            this.TransparencyKey = Color.BlanchedAlmond;
            //DrawCircle(50, new Point(250, 250));
            timer.Interval = 1000 / 60;
            timer.Start();
        }

        /* Allow the Window to be moved by dragging anywhere */
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private void DrawCircle(int r, Point p, Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Red) , p.X - r, p.Y - r, r + r, r + r);
            g.FillEllipse(new SolidBrush(Color.FromArgb(180, Color.Red)), p.X - r / 1.5f, p.Y - r / 1.5f, r / 1.7f, r / 1.7f);
            g.FillEllipse(new SolidBrush(Color.FromArgb(240, 240, 240)), p.X - r / 1.5f, p.Y - r / 1.5f, r / 2, r / 2);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Point jPos = _joyCentre;
            DrawBackground(e.Graphics);
            DrawWndBorder(e.Graphics);
            DrawDispBorder(e.Graphics);
            joyState = _formParent.getJoyState();
            if (joyState == null)
            {
                DrawCircle(20, jPos, e.Graphics);

                return;
            }
            if (joyState.PointOfViewControllers[0] >= 0)
            {
                int x = (int)Math.Round(Math.Sin(joyState.PointOfViewControllers[0] / 100 * Math.PI / 180) * 100);
                int y = (int)Math.Round(Math.Cos(joyState.PointOfViewControllers[0] / 100 * Math.PI / 180) * 100);

                int xl = y == 0 ? 65 : 55;
                int yl = x == 0 ? 70 : 60;
                jPos.X += (Math.Sign(x) * xl);
                jPos.Y -= (Math.Sign(y) * yl); // Y is inversed

            }
                
            for (int i = 0; i < joyState.Buttons.Length; i++)
            {
                if (joyState.Buttons[i])
                {
                    trailsCirc.Add(new JoyCircTrail(i, jPos, 20, _nFramesFade, Color.IndianRed));
                }
            }

            if (jPos != _prevPos)
            {       
                trailsLine.Add(new JoyLineTrail(jPos, _prevPos, _nFramesFade));
                _prevPos = jPos;
            }
            DrawLineTrails(e.Graphics, trailsLine);
            DrawCircle(20, jPos, e.Graphics);
            DrawCircTrails(e.Graphics, trailsCirc);
            e.Dispose();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            joyState = _formParent.getJoyState();
            if (joyState == null)
            {
                return;
            }
            UpdateLineTrails(trailsLine);
            UpdateCircTrails(trailsCirc);
            Invalidate();
        }

        private void UpdateCircTrails(List<JoyCircTrail> t)
        {
            for (int i = t.Count - 1; i >= 0; i--)
            {
                t[i].Decrement();
            }

            for (int i = t.Count - 1; i >= 0; i--)
            {
                if (t[i].life <= 0)
                {
                    t.RemoveAt(i);
                }
            }
        }

        private void UpdateLineTrails(List<JoyLineTrail> t)
        {
            for (int i = t.Count - 1; i >= 0; i--)
            {
                t[i].Decrement();
            }

            for (int i = t.Count - 1; i >= 0; i--)
            {
                if (t[i].life <= 0)
                {
                    t.RemoveAt(i);
                }
            }
        }

        private void DrawLineTrail(Graphics g, JoyLineTrail t)
        {
            Pen p = new Pen(Brushes.DeepPink, 5);
            g.DrawLine(p, t.a, t.b);
        }

        private void DrawLineTrails(Graphics g, List<JoyLineTrail> t)
        {
            foreach (var r in t)
            {
                DrawLineTrail(g, r);
            }
        }

        private void DrawCircTrail(Graphics g, JoyCircTrail t)
        {
            Brush b = new SolidBrush(Color.FromArgb(255, t.color));
            int rr = t.r + 2;
            if (t.life < _nFramesFade - 2)
            {
                rr -= 1;
            }
            if (t.life < _nFramesFade - 4)
            {
                rr -= 1;
            }

            g.FillEllipse(b, t.a.X - rr, t.a.Y - rr , rr * 2, rr * 2);
            g.DrawString(t.id.ToString(), new Font("Arial", 30), new SolidBrush(Color.LightGoldenrodYellow), new Point(t.a.X - t.r + 2, t.a.Y - t.r));
        }

        private void DrawCircTrails(Graphics g, List<JoyCircTrail> t)
        {
            foreach (var r in t)
            {
                DrawCircTrail(g, r);
            }
        }

        private void DrawBackground(Graphics g)
        {
            Brush b = new SolidBrush(Color.FromArgb(0, Color.Red));
            g.FillRectangle(b, this.DisplayRectangle);
        }

        private void DrawWndBorder(Graphics g)
        {
            int brdWidth = 2;
            Pen p = new Pen(Brushes.Black, brdWidth);
            p.Alignment = PenAlignment.Inset;

            g.DrawRectangle(p, 0, 0, _wndWidth, _wndHeight);
        }

        private void DrawDispBorder(Graphics g)
        {
            Point[] points = new Point[]
            {
                new Point(_joyCentre.X - 55, _joyCentre.Y + 60),  // Bottom left
                new Point(_joyCentre.X, _joyCentre.Y + 70),       // Bottom middle
                new Point(_joyCentre.X + 55, _joyCentre.Y + 60),  // Bottom right
                new Point(_joyCentre.X + 65, _joyCentre.Y),       // Middle right
                new Point(_joyCentre.X + 55, _joyCentre.Y - 60),  // Top right
                new Point(_joyCentre.X, _joyCentre.Y - 70),       // Top middle
                new Point(_joyCentre.X - 55, _joyCentre.Y - 60),  // Top Left
                new Point(_joyCentre.X - 65, _joyCentre.Y),       // Middle Left
            };

            Brush bb = new SolidBrush(Color.FromArgb(128, 0xEE, 0x82, 0xEE));
            //g.FillPolygon(bb, points);

            // Draw the crosshair in the middle. Length 56
            Pen p = new Pen(Brushes.Beige, 1.5f);
            g.DrawLine(p, _joyCentre.X + 28, _joyCentre.Y + 0, _joyCentre.X - 28, _joyCentre.Y + 0);
            g.DrawLine(p, _joyCentre.X + 0, _joyCentre.Y + 28, _joyCentre.X + 0, _joyCentre.Y - 28);

            // Extend the crosshair using a smaller pen, length 60 (only length 5 shown)
            Pen p2 = new Pen(Brushes.Beige, 1);
            g.DrawLine(p2, _joyCentre.X + 30, _joyCentre.Y + 0, _joyCentre.X - 30, _joyCentre.Y + 0);
            g.DrawLine(p2, _joyCentre.X + 0, _joyCentre.Y + 30, _joyCentre.X + 0, _joyCentre.Y - 30);

            // Draw the outer lines. Draw smaller line inside large line to look like it has a border
            Pen pb = new Pen(Brushes.Beige, 5);
            Pen pbb = new Pen(Brushes.Black, 7);
            // Top and bottom lines. Corners @ (55, 60) and (-55, 60)
            g.DrawLine(pbb, _joyCentre.X + 0, _joyCentre.Y + 70, _joyCentre.X - 55, _joyCentre.Y + 60);
            g.DrawLine(pbb, _joyCentre.X + 0, _joyCentre.Y + 70, _joyCentre.X + 55, _joyCentre.Y + 60);
            g.DrawLine(pbb, _joyCentre.X + 0, _joyCentre.Y - 70, _joyCentre.X - 55, _joyCentre.Y - 60);
            g.DrawLine(pbb, _joyCentre.X + 0, _joyCentre.Y - 70, _joyCentre.X + 55, _joyCentre.Y - 60);
            g.DrawLine(pb, _joyCentre.X + 0, _joyCentre.Y + 70, _joyCentre.X - 55, _joyCentre.Y + 60);
            g.DrawLine(pb, _joyCentre.X + 0, _joyCentre.Y + 70, _joyCentre.X + 55, _joyCentre.Y + 60);
            g.DrawLine(pb, _joyCentre.X + 0, _joyCentre.Y - 70, _joyCentre.X - 55, _joyCentre.Y - 60);
            g.DrawLine(pb, _joyCentre.X + 0, _joyCentre.Y - 70, _joyCentre.X + 55, _joyCentre.Y - 60);

            // Left and Right lines. Corners @ (55, -60) and (-55, -60)
            g.DrawLine(pbb, _joyCentre.X + 65, _joyCentre.Y + 0, _joyCentre.X + 55, _joyCentre.Y - 60);
            g.DrawLine(pbb, _joyCentre.X + 65, _joyCentre.Y + 0, _joyCentre.X + 55, _joyCentre.Y + 60);
            g.DrawLine(pbb, _joyCentre.X - 65, _joyCentre.Y + 0, _joyCentre.X - 55, _joyCentre.Y - 60);
            g.DrawLine(pbb, _joyCentre.X - 65, _joyCentre.Y + 0, _joyCentre.X - 55, _joyCentre.Y + 60);
            g.DrawLine(pb, _joyCentre.X + 65, _joyCentre.Y + 0, _joyCentre.X + 55, _joyCentre.Y - 60);
            g.DrawLine(pb, _joyCentre.X + 65, _joyCentre.Y + 0, _joyCentre.X + 55, _joyCentre.Y + 60);
            g.DrawLine(pb, _joyCentre.X - 65, _joyCentre.Y + 0, _joyCentre.X - 55, _joyCentre.Y - 60);
            g.DrawLine(pb, _joyCentre.X - 65, _joyCentre.Y + 0, _joyCentre.X - 55, _joyCentre.Y + 60);

            Brush bc = new SolidBrush(Color.Beige);
            Pen pc = new Pen(Color.Black);

            int r = 7;
            g.FillEllipse(bc, _joyCentre.X - r, _joyCentre.Y + 70 - r,  2 * r, 2 * r);
            g.FillEllipse(bc, _joyCentre.X + 55 - r, _joyCentre.Y + 60 - r, 2 * r, 2 * r);
            g.FillEllipse(bc, _joyCentre.X - 55 - r, _joyCentre.Y + 60 - r, 2 * r, 2 * r);
            g.DrawEllipse(pc, _joyCentre.X - r, _joyCentre.Y + 70 - r, 2 * r, 2 * r);
            g.DrawEllipse(pc, _joyCentre.X + 55 - r, _joyCentre.Y + 60 - r, 2 * r, 2 * r);
            g.DrawEllipse(pc, _joyCentre.X - 55 - r, _joyCentre.Y + 60 - r, 2 * r, 2 * r);

            g.FillEllipse(bc, _joyCentre.X - 6, _joyCentre.Y - 70 - r, 2 * r, 2 * r);
            g.FillEllipse(bc, _joyCentre.X + 55 - r, _joyCentre.Y - 60 - r, 2 * r, 2 * r);
            g.FillEllipse(bc, _joyCentre.X - 55 - r, _joyCentre.Y - 60 - r, 2 * r, 2 * r);
            g.DrawEllipse(pc, _joyCentre.X - 6, _joyCentre.Y - 70 - r, 2 * r, 2 * r);
            g.DrawEllipse(pc, _joyCentre.X + 55 - r, _joyCentre.Y - 60 - r, 2 * r, 2 * r);
            g.DrawEllipse(pc, _joyCentre.X - 55 - r, _joyCentre.Y - 60 - r, 2 * r, 2 * r);

            g.FillEllipse(bc, _joyCentre.X + 65 - r, _joyCentre.Y - r, 12, 12);
            g.FillEllipse(bc, _joyCentre.X - 65 - r, _joyCentre.Y - r, 12, 12);
            g.DrawEllipse(pc, _joyCentre.X + 65 - r, _joyCentre.Y - r, 12, 12);
            g.DrawEllipse(pc, _joyCentre.X - 65 - r, _joyCentre.Y - r, 12, 12);

        }

        private void FormDisplay_DoubleClick(object sender, EventArgs e)
        {

        }
    }

    public class JoyCircTrail
    {
        public Point a;
        public int r;
        public int life;
        public Color color;
        public int id;
        public JoyCircTrail(int id, Point a, int r, int life, Color color)
        {
            this.id = id;
            this.a = a;
            this.r = r;
            this.life = life;
            this.color = color;
        }
        public void Decrement()
        {
            this.life -= 1;
        }
    }

    public class JoyLineTrail
    {
        public Point a;
        public Point b;
        public int life;

        public JoyLineTrail(Point a, Point b, int life)
        {
            this.a = a;
            this.b = b;
            this.life = life;
        }

        public void Decrement()
        {
            life -= 1;
        }
    }
}
