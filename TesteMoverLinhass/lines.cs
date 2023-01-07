using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteMoverLinhass
{
    public class lines
    {
        public List<Point> point_line = new List<Point>();
        public GraphicsPath path_line = new GraphicsPath();

        //new
        public GraphicsPath outline = new GraphicsPath();
        //new

        private void line_action(Point e)
        {
            point_line.Add(e);
            path_line.AddLines(point_line.ToArray());
        }

        public GraphicsPath arrow_path_line = new GraphicsPath();
        public Pen pen = new Pen(Brushes.Black, 2);

        public void draw_arrow()
        {
            int number_of_points = this.point_line.Count - 1;
            Point last_point = point_line.ElementAt<Point>(number_of_points);

            //new
            outline = (GraphicsPath)path_line.Clone();

            Pen pen = new Pen(Brushes.Red, 10);
            outline.Widen(pen);
            //new

        }

    }
}
