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
            //this.SetStyle(ControlStyles.ResizeRedraw, true);

            Size = new Size(800, 400);
            this.DoubleBuffered = true;

            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            pic.Image = bm;

            Invalidate();

            
            
        }

        Bitmap bm;
        Graphics g;

        Point px, py;
        Pen p = new Pen(Color.Black, 1);
        Pen erase = new Pen(Color.White, 10);
        int index;

        int x, y, sX, sY, cX, cY;

        public int selected = 0;
        public bool is_selected = false;

        

        //protected override void OnPaint(PaintEventArgs e){}

        MouseButtons m = MouseButtons.Left;
        public RectangleF selected_recatngle = new RectangleF();

        public int oldX, oldY;
        public string selected_type = "";
        public bool continous_select = false;
        public int selected_line;
        public bool skip_checking_lines = false;

        bool first = true;

        bool paint = false;

        public int line_counter = 0;
        public List<lines> lines_list = new List<lines>();

        public static int q = 0;

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics gx = e.Graphics;
            //System.Diagnostics.Debug.WriteLine(lines_list.Count);

            if (pontosClicados >= 1)
            {
                //System.Diagnostics.Debug.WriteLine(x);
                gx.DrawLine(p, cX, cY, x, y);

            }

            foreach (lines l in lines_list)
            {
                //new
                if (super_form.super_action.Equals(action.move))
                {
 
                    Pen selection_pen_line = new Pen(Brushes.Blue, 1); //muda o contorno de mover 
                    gx.DrawPath(selection_pen_line, l.outline);

                }
                //new
                if (super_form.super_action.Equals(action.line))
                {
                    g.DrawPath(l.pen, l.path_line);
                    g.FillPath(Brushes.Black, l.arrow_path_line);
                }

                

                gx.DrawPath(l.pen, l.path_line);
                gx.FillPath(Brushes.Black, l.arrow_path_line);
                
            }
            //Application.DoEvents();
            //pic.Refresh();
            Invalidate();
        }

        public bool first_point_in_line = true;
        private Color xcc;
        private int pontosClicados = 0;

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (super_form.super_action.Equals(action.TwoPoint))
            {

                line_action(e);
            }

            if (super_form.super_action.Equals(action.move))
            {
                skip_checking_lines = false;

                int counter = 0;


                first = false;
            }

            if (super_form.super_action.Equals(action.bucket)) {
                Point point = set_point(pic, e.Location);
                Fill(bm, point.X, point.Y, Color.Goldenrod);
            }
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;

            py = e.Location;
            cX = e.X;
            cY = e.Y;
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            //Ferramenta Pincel
            if (super_form.super_action.Equals(action.line))
            {

                super_form.super_action = action.none;
                super_acao = acao.ending;
                System.Diagnostics.Debug.WriteLine("En");
                super_form.sf.finalizacao();
            }

            //Ferramenta Linha
            if (super_form.super_action.Equals(action.TwoPoint)){
                pontosClicados++;
                //System.Diagnostics.Debug.WriteLine(pontosClicados);
            }

            if (pontosClicados == 2)
            {
                pontosClicados = 0;
                if (super_form.super_action.Equals(action.TwoPoint))
                {

                    super_form.super_action = action.none;
                    super_acao = acao.ending;
                    System.Diagnostics.Debug.WriteLine("En");
                    super_form.sf.finalizacao();
                }
            }

        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            

            if (paint){

                if (super_form.super_action.Equals(action.line))
                {

                    line_action(e);
                }

                if (super_form.super_action.Equals(action.eraser)) {
                    px = e.Location;
                    g.DrawLine(erase, px, py);
                    py = px;
                }

            }

            if (first && e.Button == m && super_form.super_action.Equals(action.eraser)) 
            {

                if (!skip_checking_lines)
                {
                    int line_move_counter = 0;
                    Point with_offset = new Point(e.X - 10, e.Y - 10);
                    foreach (lines Line in lines_list)
                    {

                        if (Line.outline.IsVisible(with_offset) || Line.arrow_path_line.IsVisible(with_offset))
                        {

                            selected_line = line_move_counter;

                            System.Diagnostics.Debug.WriteLine(selected_line); //Faz com que o que esteja desenhado na tela fique branco para poder mover(dar impressao que não tinha nada ali) (Gambiarra)
                            
                            lines_list.RemoveAt(selected_line);
                            line_counter--;
                            xcc = Line.pen.Color;
                            Line.pen.Color = Color.White;
                            g.DrawPath(Line.pen, Line.path_line);
                            g.FillPath(Brushes.Black, Line.arrow_path_line);
                            Line.pen.Color = xcc;

                            break;
                        }

                        line_move_counter++;
                    }

                }
                pic.Refresh();
                first = false;

            }

            skip_checking_lines = false;

            if (first && e.Button == m && super_form.super_action.Equals(action.move)) //Mexer para so mover quando clicar no unica linha (inves de todos ficarem selecionados)
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

                            System.Diagnostics.Debug.WriteLine("Carregou."); //Faz com que o que esteja desenhado na tela fique branco para poder mover(dar impressao que não tinha nada ali) (Gambiarra)
                            xcc = Line.pen.Color;
                            Line.pen.Color = Color.White;
                            g.DrawPath(Line.pen, Line.path_line);
                            g.FillPath(Brushes.Black, Line.arrow_path_line);
                            Line.pen.Color = xcc;
                            q = 1;
                            break;
                        }

                        else
                        {
                            
                            is_selected = false;
                        }
                        line_move_counter++;
                    }

                }
                pic.Refresh();
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

                if (q == 1) //Para entrar quando ele depois de mover poder usar o Balde de Tinta
                {
                    var selected_line_from_list = lines_list[selected_line];
                    System.Diagnostics.Debug.WriteLine("Saiu.");
                    g.DrawPath(selected_line_from_list.pen, selected_line_from_list.path_line);
                    g.FillPath(Brushes.Black, selected_line_from_list.arrow_path_line);
                    q = 0;
                }
            }
            pic.Refresh();
            Invalidate();


            //new
            oldX = e.X;
            oldY = e.Y;

            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY;
        }

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
            System.Diagnostics.Debug.WriteLine("Linha");
            System.Diagnostics.Debug.WriteLine(line_counter - 1);
            current_line = lines_list.ElementAt<lines>(line_counter - 1);

            Point Point_line = new Point();
            Point_line.X = e.Location.X;
            Point_line.Y = e.Location.Y;
            current_line.point_line.Add(Point_line);

            GraphicsPath a = new GraphicsPath();
            current_line.path_line = a;
            current_line.path_line.AddLines(current_line.point_line.ToArray());

            pic.Refresh();
            Invalidate();
        }


        static Point set_point(PictureBox pb, Point pt)
        {

            float px = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * px), (int)(pt.Y * pY));
        }

        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {

            Color cx = bm.GetPixel(x, y);
            //System.Diagnostics.Debug.WriteLine(cx);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        public void Fill(Bitmap bm, int x, int y, Color new_clr)
        {

            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_color == new_clr)
            {
                return;
            }

            while (pixel.Count > 0)
            {

                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {

                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
                }
            }


        }

    }
}