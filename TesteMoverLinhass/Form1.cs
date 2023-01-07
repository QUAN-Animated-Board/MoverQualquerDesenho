using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TesteMoverLinhass
{
    public enum acao
    {
        non, ending
    }

    public partial class Form1 : Form
    {
        public static acao super_acao;

        public Form1()
        {
            InitializeComponent();
            Size = new Size(800, 400);
            this.DoubleBuffered = true;

            Invalidate();

            
            
        }

        public int selected = 0;
        public bool is_selected = false;

        

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics g = e.Graphics;

            foreach (lines l in lines_list)
            {
                //new
                if (super_form.super_action.Equals(action.move))
                {
                    Pen selection_pen_line = new Pen(Brushes.Blue, 1); //muda o contorno de mover 
                    g.DrawPath(selection_pen_line, l.outline);
                }
                //new

                g.DrawPath(l.pen, l.path_line);
                g.FillPath(Brushes.Black, l.arrow_path_line);
               
            }

           

        }

        MouseButtons m = MouseButtons.Left;
        public RectangleF selected_recatngle = new RectangleF();

        public int oldX, oldY;
        public string selected_type = "";
        public bool continous_select = false;
        public int selected_line;
        public bool skip_checking_lines = false;

        bool first = true;

        bool paint = false;


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if (super_form.super_action.Equals(action.line))
                {

                    line_action(e);
                }
            }

            skip_checking_lines = false;

            if (first && e.Button == m && super_form.super_action.Equals(action.move))
            {
                int counter = 0;

                if (!skip_checking_lines)
                {
                    int line_move_counter = 0;
                    Point with_offset = new Point(e.X - 10, e.Y - 10);
                    foreach (lines Line in lines_list)
                    {
                        if (continous_select == true)
                        {
                            selected_type = "line";
                            is_selected = true;
                        }

                        else if (Line.outline.IsVisible(with_offset) || Line.arrow_path_line.IsVisible(with_offset))
                        {
                            selected_type = "line";
                            is_selected = true;
                            continous_select = true;
                            selected_line = line_move_counter;
                            break;
                        }

                        else
                        {
                            is_selected = false;
                        }
                        line_move_counter++;
                    }

                }
                first = false;

            }

            if (is_selected && e.Button == m && super_form.super_action.Equals(action.move))
            {

                if (selected_type.Equals("line"))
                {
                    Matrix ma = new Matrix();
                    ma.Translate(e.X - oldX, e.Y - oldY);
                    var selected_line_from_list = lines_list[selected_line];
                    selected_line_from_list.path_line.Transform(ma);
                    selected_line_from_list.arrow_path_line.Transform(ma);
                    selected_line_from_list.outline.Transform(ma);
                }
                //new
            }

            else
            {
                first = true;

                //new
                continous_select = false;
                //new
            }

            Invalidate();


            //new
            oldX = e.X;
            oldY = e.Y;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            if (super_form.super_action.Equals(action.line))
            {
                super_form.super_action = action.none;
                super_acao = acao.ending;
                System.Diagnostics.Debug.WriteLine("En");
                super_form.sf.finalizacao();
            }
        }

        GraphicsPath inifilated_line = new GraphicsPath();
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (super_form.super_action.Equals(action.move))
            {
                skip_checking_lines = false;

                int counter = 0;

                
                first = false;
            }
        }

        public int line_counter = 0;
        public List<lines> lines_list = new List<lines>();

        public bool first_point_in_line = true;


        private void line_action(MouseEventArgs e)
        {
            //    if (first_point_in_line == true)
            if (first_point_in_line)
            {
                lines line = new lines();
                lines_list.Add(line);
                first_point_in_line = false;
                line_counter++;
            }
            lines current_line = new lines();
            current_line = lines_list.ElementAt<lines>(line_counter - 1);

            Point Point_line = new Point();
            Point_line.X = e.Location.X;
            Point_line.Y = e.Location.Y;
            current_line.point_line.Add(Point_line);

            GraphicsPath a = new GraphicsPath();
            current_line.path_line = a;
            current_line.path_line.AddLines(current_line.point_line.ToArray());

            Invalidate();
        }


    }
}