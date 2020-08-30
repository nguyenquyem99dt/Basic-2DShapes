using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using System.Windows.Forms;

namespace MidtermProject
{  
    // Lop hinh hoc
    class Shape
    {
        //Độ dày nét vẽ
        public float _lineWidth = 1.5f;

        //Hệ số để chuyển nhanh từ độ sang radian
        public double degree2rad = Math.PI / 180;

        //Hàm tính khoảng cách giữa 2 điểm kiểu Point
        public double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        //Hàm tính khoảng cách giữa hai điểm
        //Truyền vào tọa độ x,y của mỗi điểm
        public double Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        //Hàm vẽ hình (ảo)
        public virtual void Draw(OpenGL gl, Point p1, Point p2)
        { }
        public virtual void DrawPolygon(OpenGL gl, Point pStart, Point pMid, Point pEnd)
        { }
        public virtual void DrawPolygon(OpenGL gl, List<Point> listPoints) { }
    
    }

    //Lớp đoạn thẳng
    class Line : Shape
    {   
        //Hàm vẽ đoạn thằng từ 2 điểm
        public override void Draw(OpenGL gl, Point p1, Point p2)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(p1.X, gl.RenderContextProvider.Height - p1.Y);
            gl.Vertex(p2.X, gl.RenderContextProvider.Height - p2.Y);
            gl.End();
            gl.Flush();
        }
    }

    //Lớp tam giác
    class Triangle : Shape
    {
        /*Hàm vẽ tam giác đều dùng phương pháp quay điểm.
         Tam giác nội tiếp đường tròn C(O, r).
         Mỗi cạnh tam giác chắn cung 120 độ đường tròn C.
         */
        public override void Draw(OpenGL gl, Point p1, Point p2)
        {
            //Thay đổi tọa độ pStart, pEnd lưu vào p1, p2
            //Để khi vào vòng lặp không cần dùng lại RenderContextProvider.Height
            Point _p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
            Point _p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

            //Tam giác đều nội tiếp hình vuông
            //Bán kính là nửa cạnh hình vuông
            //Distance(p1,p2) là đường chéo hình vuông => r = ....
            double r = Distance(p1, p2) / (2 * Math.Sqrt(2));

            //Tâm đường tròn là trung điểm pStart - pEnd
            int xCenter = (_p1.X + _p2.X) / 2;
            int yCenter = (_p1.Y + _p2.Y) / 2;

            //Xét tại tâm O
            int x = 0;
            int y = (int)r;

            //Vẽ, mỗi lần xoay 120 độ để lấy đỉnh tam giác
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int i = 0; i < 360; i += 120)
            {
                double angle = i * degree2rad;
                gl.Vertex(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle), yCenter + x * Math.Sin(angle) + y * Math.Cos(angle));
            }
            gl.End();
            gl.Flush();

        }     
    }

    //Lớp hình chữ nhật
    class Rectangle : Shape
    {   
        //Hàm vẽ hình chữ nhật
        public override void Draw(OpenGL gl, Point p1, Point p2)
        {
            gl.Begin(OpenGL.GL_LINE_LOOP);
            gl.Vertex(p1.X, gl.RenderContextProvider.Height - p1.Y);
            gl.Vertex(p1.X, gl.RenderContextProvider.Height - p2.Y);
            gl.Vertex(p2.X, gl.RenderContextProvider.Height - p2.Y);
            gl.Vertex(p2.X, gl.RenderContextProvider.Height - p1.Y);
            gl.End();
            gl.Flush();          
        }
    }

    //Lớp vẽ đường tròn bằng thuật toán mid point
    class Circle : Shape
    {
        //Hàm vẽ 8 điểm đối xứng
        public void Put8Pixels(OpenGL gl, Point center, int x, int y)
        {
            gl.PointSize(_lineWidth);
            gl.Begin(OpenGL.GL_POINTS);
            gl.Vertex(center.X + x, center.Y + y);
            gl.Vertex(center.X+ y, center.Y - x);
            gl.Vertex(center.X + y,center.Y + x);
            gl.Vertex(center.X + x, center.Y - y);
            gl.Vertex(center.X - x, center.Y - y);
            gl.Vertex(center.X - y, center.Y - x);
            gl.Vertex(center.X - y, center.Y + x);
            gl.Vertex(center.X - x, center.Y + y);
            gl.End();
            gl.Flush();
        }
        //Hàm vẽ đường tròn bằng mid point
        public override void Draw(OpenGL gl, Point p1, Point p2)
        {
            //Thay đổi tọa độ pStart, pEnd lưu vào p1, p2
            //Để khi vào vòng lặp không cần dùng lại RenderContextProvider.Height
            Point _p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
            Point _p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

            //Tam giác đều nội tiếp hình vuông
            //Bán kính là nửa cạnh hình vuông
            //Distance(p1,p2) là đường chéo hình vuông => r = ....
            double r = Distance(p1, p2) / (2 * Math.Sqrt(2));

            //Tâm đường tròn là trung điểm pStart - pEnd
            int xCenter = (_p1.X + _p2.X) / 2;
            int yCenter = (_p1.Y + _p2.Y) / 2;
            Point center = new Point(xCenter, yCenter);

            //Xét tại tâm O
            int x = 0;
            int y = (int)r;
            // Lượng giá
            int p = (int)(5 / 4 - r);

            //Theo thuật toán vẽ đường tròn đã học
            while(x<y)
            {               
                if (p < 0)
                    p += 2 * x + 3;
                else
                {                  
                    p += 2 * (x - y) + 5;
                    y--;

                }
                x++;
                Put8Pixels(gl,center,x,y);
            }

        }
    }

    //Lớp vẽ ellipse
    class Ellipse : Shape
    {      
        public override void Draw(OpenGL gl, Point p1, Point p2)
        {
            double rx, ry, centerX, centerY;

            //Lấy giá trị bán kính lớn, bé
            rx = Math.Abs(p2.X - p1.X) / 2;
            ry = Math.Abs(p2.Y - p1.Y) / 2;

            //Tính tâm
            centerX = (p1.X + p2.X) / 2;
            centerY = (p1.Y + p2.Y) / 2;

            //Vẽ ellips theo kiểu xoay điểm
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int i = 0; i < 360; i++)
            {
                double angle = degree2rad * i;//Đổi sang radian
                //Áp dụng công thức để tính tọa độ x, y
                double x = centerX + Math.Abs(rx) * Math.Cos(angle);
                double y = centerY + Math.Abs(ry) * Math.Sin(angle);
                gl.Vertex(x, gl.RenderContextProvider.Height - y);//Vẽ
            }
            gl.End();
            gl.Flush();
        }
        
    }

    class Pentagon : Shape
    {
        public override void Draw(OpenGL gl, Point p1, Point p2)
        {
            //Thay đổi tọa độ pStart, pEnd lưu vào p1, p2
            //Để khi vào vòng lặp không cần dùng lại RenderContextProvider.Height
            Point _p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
            Point _p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

            //Ngũ giác đều nội tiếp  hình vuông
            //Bán kính đường tròn ngoại tiếp ngũ giác là nửa cạnh hình vuông
            //Distance(p1,p2) là đường chéo hình vuông => r = ....
            double r = Distance(p1, p2) / (2 * Math.Sqrt(2));

            //Tâm đường tròn là trung điểm pStart - pEnd
            int xCenter = (_p1.X + _p2.X) / 2;
            int yCenter = (_p1.Y + _p2.Y) / 2;

            //Xét tại tâm O
            int x = 0;
            int y = (int)r;

            //Vẽ, mỗi lần xoay 120 độ để lấy đỉnh tam giác
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int i = 0; i < 360; i += 72)
            {
                double angle = i * degree2rad;
                gl.Vertex(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle), yCenter + x * Math.Sin(angle) + y * Math.Cos(angle));
            }
            gl.End();
            gl.Flush();
        }
    }

    class Hexagon : Shape
    {
       
        public override void Draw(OpenGL gl, Point p1, Point p2)
        {
            //Thay đổi tọa độ pStart, pEnd lưu vào p1, p2
            //Để khi vào vòng lặp không cần dùng lại RenderContextProvider.Height
            Point _p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
            Point _p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

            //Ngũ giác đều nội tiếp hình vuông
            //Bán kính đường tròn ngoại tiếp ngũ giác là nửa cạnh hình vuông
            //Distance(p1,p2) là đường chéo hình vuông => r = ....
            double r = Distance(p1, p2) / (2 * Math.Sqrt(2));

            //Tâm đường tròn là trung điểm pStart - pEnd
            int xCenter = (_p1.X + _p2.X) / 2;
            int yCenter = (_p1.Y + _p2.Y) / 2;

            //Xét tại tâm O
            int x = 0;
            int y = (int)r;

            //Vẽ, mỗi lần xoay 120 độ để lấy đỉnh tam giác
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int i = 0; i < 360; i += 60)
            {
                double angle = i * degree2rad;
                gl.Vertex(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle), yCenter + x * Math.Sin(angle) + y * Math.Cos(angle));
            }
            gl.End();
            gl.Flush();
        }
    }

   
}

