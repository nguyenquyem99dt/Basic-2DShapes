using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL.WinForms;
using System.Collections;
using System.Diagnostics;


namespace MidtermProject
{
    public partial class Form1 : Form
    {
        
        double degree2rad = Math.PI / 180;//Hệ số chuyển nhanh từ độ sang radian
        Color colorUserColor;//Màu người dùng chọn
        List<Data> listData = new List<Data>();//List data để lưu các object đã thao tác  
        TypeAction typeAction;//Kiểu thao tác
        float size = 1.5f;//Độ dày nét vẽ, khởi tạo 1.5
        Point pStart= new Point(-1, -1);//Điểm đầu khi click chuột
        Point pEnd, pMid, pSelect;//Các điểm cuối, điểm giữa (hỗ trợ vẽ polygon), để chọn (tô màu)
        bool drawing = false;//Cờ đánh dấu vẽ
        bool rightClicked = false;//Cờ đánh dấu chuột phải đã được click
        bool selected = false;//Cờ đánh dấu người dùng chọn chức năng select để hiện control point của đối tượng

        //Hỗ trợ hiển thị thời gian
        Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        Stopwatch watchPoly = System.Diagnostics.Stopwatch.StartNew();
        string startTime;
        string timeDraw;

        public Form1()
        {
            InitializeComponent();
            colorUserColor = Color.White;//Khởi tạo mặc định màu vẽ là màu trắng
            //Một số lựa chọn độ dày nét vẽ
            comboBox1.Items.Add("1.5");
            comboBox1.Items.Add("2.0");
            comboBox1.Items.Add("2.5");
            comboBox1.Items.Add("3.0");
            comboBox1.Items.Add("3.5");
            comboBox1.Items.Add("4.0");
            comboBox1.Items.Add("4.5");
            comboBox1.Items.Add("5.0");
            comboBox1.Items.Add("5.5");
            comboBox1.Items.Add("6.0");
        }

        private void openGLControl_OpenGLInitialized_1(object sender, EventArgs e)
        {
            // Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;
            // Set the clear color.
            gl.ClearColor(0, 0, 0, 0);
            // Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            // Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;
            // Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
            // Create a perspective transformation.
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);
            gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);
        }

        private void openGLControl_OpenGLDraw_1(object sender, RenderEventArgs args)
        {
            if (drawing == true)
            {
                // Get the OpenGL object.
                OpenGL gl = openGLControl.OpenGL;
                Shape shape = new Shape();
                // Clear the color and depth buffer
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);     
                
                //Hàm vẽ lại các dữ liệu (object) đã được vẽ trước đó
                DrawFromData(gl);
               
                gl.Color(colorUserColor.R / 255.0, colorUserColor.G / 255.0, colorUserColor.B / 255.0);//Màu vẽ
                gl.LineWidth(size);//Độ dày nét vẽ

                //Vẽ đoạn thằng
                if (typeAction == TypeAction.line)
                {
                    shape = new Line();
                    shape.Draw(gl, pStart, pEnd);
                }
                //Vẽ tam giác đều
                else if (typeAction == TypeAction.triangle)
                {
                    shape = new Triangle();
                    shape.Draw(gl, pStart, pEnd);
                }
                //Vẽ hình chữ nhật
                else if (typeAction == TypeAction.rectangle)
                {
                    shape = new Rectangle();
                    shape.Draw(gl, pStart, pEnd);
                }
                //Vẽ đường tròn
                else if (typeAction == TypeAction.circle)
                {
                    shape = new Circle();
                    shape._lineWidth = size;
                    shape.Draw(gl, pStart, pEnd);
                }
                //Vẽ ellipse
                else if (typeAction == TypeAction.ellipse)
                {
                    shape = new Ellipse();
                    shape.Draw(gl, pStart, pEnd);
                }
                //Vẽ ngũ giác đều
                else if (typeAction == TypeAction.pentagon)
                {
                    shape = new Pentagon();
                    shape.Draw(gl, pStart, pEnd);
                }
                //Vẽ lục giác đều
                else if (typeAction == TypeAction.hexagon)
                {
                    shape = new Hexagon();
                    shape.Draw(gl, pStart, pEnd);
                }
                //Vẽ polygon
                else if (typeAction == TypeAction.polygon)
                    DrawPolygon(gl);                            
            }
        }

        //Hàm vẽ đa giác
        private void DrawPolygon(OpenGL gl)
        {
            Shape shape = new Line();
            //Nếu chuột phải đã được click
            if (rightClicked)
            {
                //Vẽ đường thẳng dựa vào pStart, pEnd (nối điểm cuối cạnh đang vẽ với điểm đầu của cạnh đầu tiên)
                shape.Draw(gl, pStart, pEnd);
                //Vẽ control points
                DrawControlPoints(listData[listData.Count - 1]._controlPoints, typeAction);
                //Reset lại các cờ
                drawing = false;
                rightClicked = false;

                //Reset lại các điểm
                pStart = new Point(-1, -1);
                pEnd = new Point(-1, -1);
                pMid = new Point(-1, -1);

                //Thời gian
                watchPoly.Stop();
                string[] seconds = watchPoly.Elapsed.TotalSeconds.ToString().Split('.');
                string[] milliseconds = watchPoly.Elapsed.TotalMilliseconds.ToString().Split('.');
                timeDraw = seconds[0].ToString() + "." + milliseconds[0].ToString() + 's';
                textBox1_TextChanged(new object(), new System.EventArgs());
            }
            //Ngược lại, vẽ đường thằng dựa vào pMid và pEnd
            else
                shape.Draw(gl, pMid, pEnd);
        }

        //Hàm vẽ đa giác có truyền tham số là 1 danh sách điểm
        private void DrawPolygon(OpenGL gl, List<Point>listPoints)
        {
            //Nếu số điểm trong list
            int n = listPoints.Count;
            Shape shape = new Line();

            //Nhiều hơn 1 điểm thì mới vẽ được
            if(n>1)
            {
                //Lặp qua các điểm trong list, vẽ đường thằng nối 2 điểm kề nhau
                for (int i = 0; i < n - 1; i++)
                    shape.Draw(gl, listPoints[i], listPoints[i + 1]);
            }
        }

        //Hàm vẽ từ data lưu trữ (các object đã được vẽ)
        private void DrawFromData(OpenGL gl)
        {
            Shape shape = new Shape();
            foreach(Data data in listData)
            {
                Point p1, p2;
                gl.Color(data._color.R / 255.0, data._color.G / 255.0, data._color.B / 255.0);
                gl.LineWidth(data._lineWidth);

                //Nếu là chức năng chọn hình
                if (data._type == TypeAction.select)
                {
                    //Nếu đã chọn
                    if (selected)
                    {
                        //Lấy đối tượng được chọn thông qua điểm chọn
                        Data shapeSelect = SelectShape(pSelect);
                        //Vẽ các control points
                        DrawControlPoints(shapeSelect._controlPoints, shapeSelect._type);
                    }
                }
                //Flood fill
                else if (data._type == TypeAction.floodFill)
                {
                    //Lấy điểm control point đầu tiên (cũng chính là pSeed, là điểm mà người dùng click chuột sau khi chọn button flood fill)
                    p1 = data._controlPoints[0];
                    Fill f = new FloodFill();//Khởi tạo                                       
                    f.newColor = data._color;//Màu tô
                    f.ApplyFill(gl, p1);//Tô
                }
                //Scan line
                else if (data._type == TypeAction.scanLines)
                {
                    p1 = data._controlPoints[0];//Lấy điểm chọn hình
                    Fill f = new ScanLine();//Khởi tạo hàm tô
                    ET edgeTable = new ET();//Khởi tạo bảng ET
                    Data shapeFill = SelectShape(p1);//Lấy đối tượng được chọn

                    //Nếu chọn được đối tượng
                    if (!shapeFill.isEmpty())
                    {
                        //Lấy các đỉnh của đối tượng
                        List<Point> listVertices = AllVertices(shapeFill);

                        //Chuyển các đỉnh sang dạng điểm có tọa độ thực
                        List<FloatPoint> listVerticesFloat = new List<FloatPoint>();
                        foreach (var p in listVertices)
                            listVerticesFloat.Add(new FloatPoint(p.X, p.Y));
                        //Lấy các đỉnh đó đưa vào bảng ET
                        edgeTable.EdgeData(listVerticesFloat);
                        f.newColor = data._color;//Màu tô
                        f.ApplyFill(gl, edgeTable);//Áp dụng tô
                    }
                }
                //Chức năng vẽ polygon
                else if (data._type == TypeAction.polygon)
                {                 
                    //Gọi hàm vẽ polygon, truyền vào control points
                    DrawPolygon(gl, data._controlPoints);
                }

                //Các chức năng còn lại (Các hình thông thường)
                else
                {
                    //Lấy 2 điểm control points (cũng chính là pStart pEnd khi thao tác chuột)
                    p1 = data._controlPoints[0];
                    p2 = data._controlPoints[1];

                    //Chức năng vẽ đường thẳng
                    if (data._type == TypeAction.line)
                    {
                        shape = new Line();
                        shape.Draw(gl, p1, p2);
                    }
                    //Chức năng vẽ tam giác
                    else if (data._type == TypeAction.triangle)
                    {
                        shape = new Triangle();
                        shape.Draw(gl, p1, p2);
                    }
                    //Chức năng vẽ hình chữ nhật
                    else if (data._type == TypeAction.rectangle)
                    {
                        shape = new Rectangle();
                        shape.Draw(gl, p1, p2);
                    }
                    //Chức năng vẽ hình tròn
                    else if (data._type == TypeAction.circle)
                    {
                        shape = new Circle();
                        shape.Draw(gl, p1, p2);
                    }
                    //Chức năng vẽ ellipse
                    else if (data._type == TypeAction.ellipse)
                    {
                        shape = new Ellipse();
                        shape.Draw(gl, p1, p2);
                    }
                    //Chức năng vẽ ngũ giác đều
                    else if (data._type == TypeAction.pentagon)
                    {
                        shape = new Pentagon();
                        shape.Draw(gl, p1, p2);
                    }
                    //Chức năng vẽ lục giác đều
                    else if (data._type == TypeAction.hexagon)
                    {
                        shape = new Hexagon();
                        shape.Draw(gl, p1, p2);
                    }
                }
                
            }
        }

        //Hàm làm tròn
        int Round(double x)
        {
            return (int)(x + 0.5);
        }

        private void update_pStart_pEnd(TypeAction type)
        {
            if(type==TypeAction.triangle|| type == TypeAction.circle ||type==TypeAction.pentagon||type==TypeAction.hexagon)
            {
                //Lấy độ dãn của X
                int stretchX = Math.Abs(pEnd.X - pStart.X);
              
                // Tính vị trí tương đối giữa x, y
                int dx = pEnd.X - pStart.X;
                int dy = pEnd.Y - pStart.Y;

                if (dx > 0 && dy > 0)
                    pEnd = new Point(pStart.X + stretchX, pStart.Y + stretchX);
                else if (dx > 0 && dy <= 0)
                    pEnd = new Point(pStart.X + stretchX, pStart.Y - stretchX);
                else if (dx <= 0 && dy > 0)
                    pEnd = new Point(pStart.X - stretchX, pStart.Y + stretchX);
                else
                    pEnd = new Point(pStart.X - stretchX, pStart.Y - stretchX);
            }
        }
        //Tạo list control point
        private List<Point> createListControlPoints(List<Point>listControlPoints, TypeAction type)
        {
            List<Point> newListControlPoints = new List<Point>();

            try
            {
                //Nếu không phải polygon
                if (type != TypeAction.polygon)
                {
                    //Lấy 2 điểm đầu trong list control points (pStart, pEnd)
                    Point p1 = new Point(listControlPoints[0].X, listControlPoints[0].Y);
                    Point p2 = new Point(listControlPoints[1].X, listControlPoints[1].Y);
                    //Nếu là vẽ đường thẳng
                    if (type == TypeAction.line)
                    {
                        //Thêm 2 điểm vào list
                        newListControlPoints.Add(p1);
                        newListControlPoints.Add(p2);
                    }
                    //Nếu là tam giác
                    else if (type == TypeAction.triangle)
                    {
                        OpenGL gl = openGLControl.OpenGL;

                        // Cập nhật lại tọa độ điểm start end
                        p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
                        p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

                        double r = Distance(p1, p2) / (2 * Math.Sqrt(2));

                        //Tâm đường tròn là trung điểm pStart - pEnd
                        int xCenter = (p1.X + p2.X) / 2;
                        int yCenter = (p1.Y + p2.Y) / 2;

                        //Gốc tọa độ
                        int x = 0;
                        int y = (int)r;

                        //Tính lại các đỉnh của tam giác đã vẽ để hiển thị control point cho chính xác
                        //Đỉnh thứ 1 == đỉnh trên cùng
                        double angle = 0;
                        Point vertex1 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                        , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Đỉnh thứ 2 == đỉnh dưới bên trái
                        angle = 120 * degree2rad;
                        Point vertex2 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                                , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Đỉnh thứ 3 == đỉnh dưới bên phải
                        angle = 240 * degree2rad;
                        Point vertex3 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                                , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Cập nhật lại y của các đỉnh
                        vertex1.Y = gl.RenderContextProvider.Height - vertex1.Y;
                        vertex2.Y = gl.RenderContextProvider.Height - vertex2.Y;
                        vertex3.Y = gl.RenderContextProvider.Height - vertex3.Y;

                        //Tạo các điểm control
                        Point ctrlP1 = new Point(vertex3.X, vertex1.Y);
                        Point ctrlP2 = vertex1;
                        Point ctrlP3 = new Point(vertex2.X, vertex1.Y);
                        Point ctrlP4 = new Point(vertex2.X, (vertex1.Y + vertex2.Y) / 2);
                        Point ctrlP5 = vertex2;
                        Point ctrlP6 = new Point(vertex1.X, vertex2.Y);
                        Point ctrlP7 = vertex3;
                        Point ctrlP8 = new Point(vertex3.X, ctrlP4.Y);

                        //Thêm control points vào danh sách
                        newListControlPoints.Add(ctrlP1); 
                        newListControlPoints.Add(ctrlP2); 
                        newListControlPoints.Add(ctrlP3); 
                        newListControlPoints.Add(ctrlP4); 
                        newListControlPoints.Add(ctrlP5); 
                        newListControlPoints.Add(ctrlP6); 
                        newListControlPoints.Add(ctrlP7); 
                        newListControlPoints.Add(ctrlP8);
                    }

                    //Nếu là hình chữ nhật
                    else if (type == TypeAction.rectangle)
                    {
                        //Tạo các control points
                        Point ctrlP1 = p1;
                        Point ctrlP2 = new Point(p1.X, (p1.Y + p2.Y) / 2);
                        Point ctrlP3 = new Point(p1.X, p2.Y);
                        Point ctrlP4 = new Point((p1.X + p2.X) / 2, p2.Y);
                        Point ctrlP5 = p2;
                        Point ctrlP6 = new Point(p2.X, ctrlP2.Y);
                        Point ctrlP7 = new Point(p2.X, p1.Y);
                        Point ctrlP8 = new Point(ctrlP4.X, p1.Y);

                        //Thêm vào list
                        newListControlPoints.Add(ctrlP1);
                        newListControlPoints.Add(ctrlP2);
                        newListControlPoints.Add(ctrlP3);
                        newListControlPoints.Add(ctrlP4);
                        newListControlPoints.Add(ctrlP5);
                        newListControlPoints.Add(ctrlP6);
                        newListControlPoints.Add(ctrlP7);
                        newListControlPoints.Add(ctrlP8);

                    }
                    //Nếu là đường tròn
                    else if (type == TypeAction.circle)
                    {
                        //Tạo các control points
                        Point ctrlP1 = p1;
                        Point ctrlP2 = new Point(p1.X, (p1.Y + p2.Y) / 2);
                        Point ctrlP3 = new Point(p1.X, p2.Y);
                        Point ctrlP4 = new Point((p1.X + p2.X) / 2, p2.Y);
                        Point ctrlP5 = p2;
                        Point ctrlP6 = new Point(p2.X, ctrlP2.Y);
                        Point ctrlP7 = new Point(p2.X, p1.Y);
                        Point ctrlP8 = new Point(ctrlP4.X, p1.Y);

                        //Thêm vào list
                        newListControlPoints.Add(ctrlP1);
                        newListControlPoints.Add(ctrlP2);
                        newListControlPoints.Add(ctrlP3);
                        newListControlPoints.Add(ctrlP4);
                        newListControlPoints.Add(ctrlP5);
                        newListControlPoints.Add(ctrlP6);
                        newListControlPoints.Add(ctrlP7);
                        newListControlPoints.Add(ctrlP8);
                    }

                    //Nếu là ellipse
                    else if (type == TypeAction.ellipse)
                    {
                       //Tính các control points
                        Point ctrlP1 = p1;
                        Point ctrlP2 = new Point(p1.X, (p1.Y + p2.Y) / 2);
                        Point ctrlP3 = new Point(p1.X, p2.Y);
                        Point ctrlP4 = new Point((p1.X + p2.X) / 2, p2.Y);
                        Point ctrlP5 = p2;
                        Point ctrlP6 = new Point(p2.X, ctrlP2.Y);
                        Point ctrlP7 = new Point(p2.X, p1.Y);
                        Point ctrlP8 = new Point(ctrlP4.X, p1.Y);
                        //Thêm vào list
                        newListControlPoints.Add(ctrlP1);
                        newListControlPoints.Add(ctrlP2);
                        newListControlPoints.Add(ctrlP3);
                        newListControlPoints.Add(ctrlP4);
                        newListControlPoints.Add(ctrlP5);
                        newListControlPoints.Add(ctrlP6);
                        newListControlPoints.Add(ctrlP7);
                        newListControlPoints.Add(ctrlP8);
                    }

                    //Nếu là ngũ giác đều
                    else if (type == TypeAction.pentagon)
                    {
                        OpenGL gl = openGLControl.OpenGL;

                        // Cập nhật lại tọa độ điểm start end
                        p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
                        p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

                        double r = Distance(p1, p2) / (2 * Math.Sqrt(2));

                        //Tâm đường tròn là trung điểm pStart - pEnd
                        int xCenter = (p1.X + p2.X) / 2;
                        int yCenter = (p1.Y + p2.Y) / 2;

                        //Gốc tọa độ
                        int x = 0;
                        int y = (int)r;

                        //Tính lại các đỉnh của ngũ giác đã vẽ để hiển thị control point cho chính xác
                        //Thứ tự ngược chiều kim đồng hồ, bắt đầu từ đỉnh trên cùng là đỉnh 1
                        //Đỉnh thứ 1 
                        double angle = 0;
                        Point vertex1 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                        , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Đỉnh thứ 2 
                        angle = 72 * degree2rad;
                        Point vertex2 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                                , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Đỉnh thứ 3 
                        angle = 144 * degree2rad;
                        Point vertex3 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                                , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));
                        //Đỉnh thứ 5
                        angle = 288 * degree2rad;
                        Point vertex5 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                                , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Cập nhật lại tọa độ y của các đỉnh
                        vertex1.Y = gl.RenderContextProvider.Height - vertex1.Y;
                        vertex2.Y = gl.RenderContextProvider.Height - vertex2.Y;
                        vertex3.Y = gl.RenderContextProvider.Height - vertex3.Y;
                        vertex5.Y = gl.RenderContextProvider.Height - vertex5.Y;

                        //Tính các control points
                        Point ctrlP1 = new Point(vertex2.X, vertex1.Y);
                        Point ctrlP2 = new Point(vertex2.X, (vertex1.Y + vertex3.Y) / 2);
                        Point ctrlP3 = new Point(vertex2.X, vertex3.Y);
                        Point ctrlP4 = new Point(vertex1.X, vertex3.Y);
                        Point ctrlP5 = new Point(vertex5.X, vertex3.Y);
                        Point ctrlP6 = new Point(vertex5.X, ctrlP2.Y);
                        Point ctrlP7 = new Point(vertex5.X, vertex1.Y);
                        Point ctrlP8 = vertex1;

                        //Thêm vào list
                        newListControlPoints.Add(ctrlP1);
                        newListControlPoints.Add(ctrlP2);
                        newListControlPoints.Add(ctrlP3);
                        newListControlPoints.Add(ctrlP4);
                        newListControlPoints.Add(ctrlP5);
                        newListControlPoints.Add(ctrlP6);
                        newListControlPoints.Add(ctrlP7);
                        newListControlPoints.Add(ctrlP8);
                    }

                    //Nếu là lục giác đều
                    else if (type == TypeAction.hexagon)
                    {
                        OpenGL gl = openGLControl.OpenGL;

                        // Cập nhật lại tọa độ điểm start end
                        p1 = new Point(p1.X, gl.RenderContextProvider.Height - p1.Y);
                        p2 = new Point(p2.X, gl.RenderContextProvider.Height - p2.Y);

                        double r = Distance(p1, p2) / (2 * Math.Sqrt(2));

                        //Tâm đường tròn là trung điểm pStart - pEnd
                        int xCenter = (p1.X + p2.X) / 2;
                        int yCenter = (p1.Y + p2.Y) / 2;

                        //Gốc tọa độ
                        int x = 0;
                        int y = (int)r;

                        //Tính lại các đỉnh của ngũ giác đã vẽ để hiển thị control point cho chính xác
                        //Đỉnh thứ 1 
                        double angle = 0;
                        Point vertex1 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                        , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Đỉnh thứ 2 
                        angle = 60 * degree2rad;
                        Point vertex2 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                                , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));


                        //Đỉnh thứ 4
                        angle = 180 * degree2rad;
                        Point vertex4 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                                , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Đỉnh thứ 5
                        angle = 240 * degree2rad;
                        Point vertex5 = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle))
                                , Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));

                        //Cập nhật lại tọa độ y của các đỉnh
                        vertex1.Y = gl.RenderContextProvider.Height - vertex1.Y;
                        vertex2.Y = gl.RenderContextProvider.Height - vertex2.Y;
                        vertex4.Y = gl.RenderContextProvider.Height - vertex4.Y;
                        vertex5.Y = gl.RenderContextProvider.Height - vertex5.Y;

                        //Tạo các điểm control
                        Point ctrlP1 = new Point(vertex2.X, vertex1.Y);
                        Point ctrlP2 = new Point(vertex2.X, (vertex4.Y + vertex1.Y) / 2);
                        Point ctrlP3 = new Point(vertex2.X, vertex4.Y);
                        Point ctrlP4 = vertex4;
                        Point ctrlP5 = new Point(vertex5.X, vertex4.Y);
                        Point ctrlP6 = new Point(vertex5.X, ctrlP2.Y);
                        Point ctrlP7 = new Point(vertex5.X, vertex1.Y);
                        Point ctrlP8 = vertex1;

                        //Thêm vào list
                        newListControlPoints.Add(ctrlP1);
                        newListControlPoints.Add(ctrlP2);
                        newListControlPoints.Add(ctrlP3);
                        newListControlPoints.Add(ctrlP4);
                        newListControlPoints.Add(ctrlP5);
                        newListControlPoints.Add(ctrlP6);
                        newListControlPoints.Add(ctrlP7);
                        newListControlPoints.Add(ctrlP8);
                    }
                }
                //Ngược lại, nếu là vẽ polygon
                else
                {
                    //Tính các giá trị x, y min max
                    //Khởi tạo là tọa độ của control point đầu tiên trong list
                    int xMin = listControlPoints[0].X;
                    int xMax = listControlPoints[0].X;
                    int yMin = listControlPoints[0].Y;
                    int yMax = listControlPoints[0].Y;
                    int n = listControlPoints.Count;

                    //Lặp qua các control points còn lại
                    for (int i = 1; i < n; i++)
                    {
                        //Cập nhật
                        if (xMin > listControlPoints[i].X)
                            xMin = listControlPoints[i].X;
                        if (xMax < listControlPoints[i].X)
                            xMax = listControlPoints[i].X;
                        if (yMin > listControlPoints[i].Y)
                            yMin = listControlPoints[i].Y;
                        if (yMax < listControlPoints[i].Y)
                            yMax = listControlPoints[i].Y;
                    }
                    //Lấy điểm "giữa"
                    int xMid = Round((xMax + xMin) / 2);
                    int yMid = Round((yMax + yMin) / 2);

                    //Tạo điểm dựa vào tọa độ đã tính và thêm vào list
                    newListControlPoints.Add(new Point(xMin, yMin));
                    newListControlPoints.Add(new Point(xMax, yMax));
                    newListControlPoints.Add(new Point(xMin, yMax));
                    newListControlPoints.Add(new Point(xMax, yMin));
                    newListControlPoints.Add(new Point(xMid, yMax));                  
                    newListControlPoints.Add(new Point(xMax, yMid));                   
                    newListControlPoints.Add(new Point(xMid, yMin));                   
                    newListControlPoints.Add(new Point(xMin, yMid));

                }
            }
            catch { }
            return newListControlPoints;//Trả về list control points đã xây dựng

        }

        //Vẽ các control point trong list
        private void DrawControlPoints(List<Point> listControlPoints, TypeAction type)
        {
            OpenGL gl = openGLControl.OpenGL;
            Color colorControlPoints = Color.Cyan;//Màu của điểm control
            gl.Color(colorControlPoints.R / 255.0, colorControlPoints.G / 255.0, colorControlPoints.B / 255.0);

            //Tạo list control points từ list control points ban đầu (dữ liệu thô từ pStart pEnd)
            List<Point> newListControlPoints = createListControlPoints(listControlPoints, type);

            //Chỉ áp dụng với các hình
            if (type != TypeAction.floodFill && type != TypeAction.scanLines&&type!=TypeAction.select)
            {
                //Lặp qua danh sách và vẽ
                foreach (var ctrlPoint in newListControlPoints)
                {
                    gl.PointSize(6);
                    gl.Begin(OpenGL.GL_POINTS);
                    gl.Vertex(ctrlPoint.X, gl.RenderContextProvider.Height - ctrlPoint.Y);
                    gl.End();
                    gl.Flush();
                }
            }
        }

       //Kiểu tác vụ cần thực hiện
        public enum TypeAction
        {
            select,//Chức năng chọn hình
            line,//Chức năng vẽ đường thẳng
            circle,//Chức năng vẽ đường tròn
            rectangle,//Chức năng vẽ hình chữ nhật
            ellipse,//Chức năng vẽ ellipse
            triangle,//Chức năng vẽ tam giác đều
            pentagon,//Chức năng vẽ ngũ giác đều
            hexagon,//Chức năng vẽ lục giác đều
            polygon,//Chức năng vẽ đa giác
            floodFill,//Chức năng tô loang
            scanLines//Chức năng tô theo dòng quét
        }
     
        //Struct Data dùng để lưu trữ các dữ liệu đã được thao tác.
        public struct Data
        {
            public List<Point> _controlPoints;//Lưu các control points
            public Color _color; //Lưu màu
            public TypeAction _type; //Kiểu tác vụ: vẽ line,...,tô,...
            public float _lineWidth; //Kích thước struct

          //Khởi tạo struct Data dựa vào 3 tham số màu sắc, kiểu tác vụ và kích thước
            public Data(Color color,TypeAction type, float lineWidth)
            {
                _controlPoints = new List<Point>();
                _color = color;
                _type = type;
                _lineWidth = lineWidth;
            }
            //Khởi tạo struct Data từ một Data khác có sẵn
            public Data(Data otherData)
            {
                _controlPoints = otherData._controlPoints;
                _color = otherData._color;
                _type = otherData._type;
                _lineWidth = otherData._lineWidth;
            }
            //Khởi tạo struct Data từ list control points và các tham số color, type, lineWidth
            public Data(List<Point>list, Color color, TypeAction type, int lineWidth)
            {
                _controlPoints = new List<Point>();
                _controlPoints.AddRange(list);
                _color = color;
                _type = type;
                _lineWidth = lineWidth;
            }
            //Kiểm tra rỗng
            public bool isEmpty()
            {
                if (_controlPoints == null)
                    return true;
                else
                    return false;
            }
        }

        //Hàm tính khoảng cách giữa 2 điểm kiểu Point
        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        
        //Hàm tính khoảng cách giữa hai điểm
        //Truyền vào tọa độ x,y của mỗi điểm
        private double Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        //Hàm chọn hình, truyền vào điểm chọn
        private Data SelectShape(Point pSeed)
        {
            Data shapeSelected = new Data(); //Khởi tạo    
            //Lặp qua list data
            foreach(Data data in listData)
            {
                Data dataAtMinIndex = new Data();
                int xMin=0, xMax=0, yMin=0, yMax=0;

                //Nếu là hình cơ bản bình thường
                if(data._type!=TypeAction.polygon&&data._type!=TypeAction.floodFill && data._type != TypeAction.scanLines
                    &&data._type!=TypeAction.select)
                {
                    //Lấy 2 điểm control points
                    Point p1 = data._controlPoints[0];
                    Point p2 = data._controlPoints[1];
                    //Tính xMin, xMax
                    if(p1.X<p2.X)
                    {
                        xMin = p1.X;
                        xMax = p2.X;
                    }
                    else
                    {
                        xMin = p2.X;
                        xMax = p1.X;
                    }
                    //Tính yMin, yMax
                    if(p1.Y< p2.Y)
                    {
                        yMin = p1.Y;
                        yMax = p2.Y;
                    }
                    else
                    {
                        yMin = p2.Y;
                        yMax = p1.Y;
                    }
                }
                //nếu là polygon
                else if(data._type==TypeAction.polygon)
                {
                    List<Point> listPoints = new List<Point>();
                    listPoints.AddRange(data._controlPoints);

                    //Tính x, y min max
                    xMin = listPoints[0].X;
                    xMax = listPoints[0].X;
                    yMin = listPoints[0].Y;
                    yMax = listPoints[0].Y;
                    int n = listPoints.Count;
                    for(int i=1;i<n;i++)
                    {
                        if (xMin > listPoints[i].X)
                            xMin = listPoints[i].X;
                        if (xMax < listPoints[i].X)
                            xMax = listPoints[i].X;
                        if (yMin > listPoints[i].Y)
                            yMin = listPoints[i].Y;
                        if (yMax < listPoints[i].Y)
                            yMax = listPoints[i].Y;
                    }
                }
                //Kiểm tra điểm chọn có nằm trong hình không
                bool isInside = IsInside(pSeed, xMin, xMax, yMin, yMax);
                double minDistance = -1;

                //Nếu có
                if(isInside)
                {
                    //Tính mid point
                    Point midPoint = new Point(Round((xMin + xMax) / 2.0), Round((yMin + yMax) / 2.0));

                    //Khoảng cách từ điểm mid đến điểm chọn
                    double distance = Distance(pSeed, midPoint);
                   
                    //Cập nhật lại min distance và object
                    if(minDistance==-1||minDistance>distance)
                    {
                        minDistance = distance;
                        dataAtMinIndex = data; 
                    }
                }
                //Nếu đối tượng không rỗng thì gán hình được chọn bằng đối tượng đó
                if (!dataAtMinIndex.isEmpty())
                    shapeSelected = dataAtMinIndex;               
            }
            return shapeSelected;//Trả về đối tượng đã được chọn
        }

        //Hàm kiểm tra một điểm có nằm trong hình không
        //Truyền vào điểm, các tọa độ min max
        private bool IsInside(Point pSeed, int xMin, int xMax, int yMin, int yMax)
        {
            //Điều kiện để điểm nằm trong hình
            if (pSeed.X <= xMax && pSeed.X >= xMin && pSeed.Y <= yMax && pSeed.Y >= yMin)
                return true;
            return false;
        }

        //Hàm các đỉnh của đối tượng
        private List<Point> AllVertices(Data data)
        {
            List<Point> vertices = new List<Point>();
            OpenGL gl = openGLControl.OpenGL;
            //Nếu không phải polygon
            if(data._type!=TypeAction.polygon)
            {
                //Lấy 2 điểm control point đầu tiên
                Point p1 = data._controlPoints[0];
                Point p2 = data._controlPoints[1];

                //Nếu là đường thẳng
                if (data._type==TypeAction.line)
                {
                    //Thì thêm hai điểm vào danh sách đỉnh
                    vertices.Add(new Point(p1.X, gl.RenderContextProvider.Height - p1.Y));
                    vertices.Add(new Point(p2.X, gl.RenderContextProvider.Height - p2.Y));
                }
                else if(data._type==TypeAction.triangle)
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

                    //Gốc tọa độ
                    int x = 0;
                    int y = (int)r;
                 
                    //mỗi lần xoay 120 độ để lấy đỉnh tam giác            
                    for (int i = 0; i < 360; i += 120)
                    {
                        //Tính đỉnh
                        double angle = i * degree2rad;
                        Point vertex = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle)),
                            Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));
                        vertices.Add(vertex);//Thêm vào danh sách
                    }                  
                }
                else if(data._type==TypeAction.circle)
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

                    //Gốc tọa độ
                    int x = 0;
                    int y = (int)r;

                    //mỗi lần xoay 120 độ để lấy đỉnh tam giác            
                    for (int i = 0; i < 360; i += 6)
                    {
                        //Tính đỉnh
                        double angle = i * degree2rad;
                        Point vertex = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle)),
                            Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));
                        vertices.Add(vertex);//Thêm vào danh sách
                    }
                }
                //Ellipse
                else if(data._type==TypeAction.ellipse)
                {
                    //Tính bán kính bé,  lớn, tâm
                    double rx, ry, centerX, centerY;
                    rx = Math.Abs(p2.X - p1.X) / 2;
                    ry = Math.Abs(p2.Y - p1.Y) / 2;
                    centerX = (p1.X + p2.X) / 2;
                    centerY = (p1.Y + p2.Y) / 2;
                    //Tính đỉnh
                    for (int i = 0; i < 360; i+=6)
                    {
                        double angle = degree2rad * i;
                        int x =Round( centerX + Math.Abs(rx) * Math.Cos(angle));
                        int y =Round( centerY + Math.Abs(ry) * Math.Sin(angle));
                        Point vertex = new Point(x, gl.RenderContextProvider.Height - y);
                        vertices.Add(vertex);//Thêm vào list
                    }
                }
                //Nếu là hình chữ nhật
                else if(data._type==TypeAction.rectangle)
                {   //Tính đỉnh và thêm vào list               
                    vertices.Add(new Point(p1.X, gl.RenderContextProvider.Height - p1.Y));
                    vertices.Add(new Point(p2.X, gl.RenderContextProvider.Height - p1.Y));
                    vertices.Add(new Point(p2.X, gl.RenderContextProvider.Height - p2.Y));
                    vertices.Add(new Point(p1.X, gl.RenderContextProvider.Height - p2.Y));
                }
                //Ngũ giác đều
                else if(data._type==TypeAction.pentagon)
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

                    //Gốc tọa độ
                    int x = 0;
                    int y = (int)r;

                    //mỗi lần xoay 120 độ để lấy đỉnh tam giác            
                    for (int i = 0; i < 360; i += 72)
                    {
                        //Tính đỉnh
                        double angle = i * degree2rad;
                        Point vertex = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle)),
                            Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));
                        vertices.Add(vertex);//Thêm vào list

                    }
                }
                //Lục giác đều
                else if(data._type==TypeAction.hexagon)
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

                    //Gốc tọa độ
                    int x = 0;
                    int y = (int)r;

                    //mỗi lần xoay 120 độ để lấy đỉnh tam giác            
                    for (int i = 0; i < 360; i += 60)
                    {
                        //Tính đỉnh
                        double angle = i * degree2rad;
                        Point vertex = new Point(Round(xCenter + x * Math.Cos(angle) - y * Math.Sin(angle)),
                            Round(yCenter + x * Math.Sin(angle) + y * Math.Cos(angle)));
                        vertices.Add(vertex);//Thêm vào list
                    }
                }
            }
            //Polygon
            else if (data._type == TypeAction.polygon)
            {
                //Duyệt qua danh sách control points
                foreach (var p in data._controlPoints)
                    vertices.Add(new Point(p.X, gl.RenderContextProvider.Height - p.Y));//Thêm vào list
            }
            return vertices;//Trả về list các đỉnh
        }
        //Hàm vẽ đoạn thằng
        private void DrawLine(OpenGL gl, Point p1, Point p2)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(p1.X, gl.RenderContextProvider.Height - p1.Y);
            gl.Vertex(p2.X, gl.RenderContextProvider.Height - p2.Y);
            gl.End();
            gl.Flush();
        }

        /*Hàm vẽ tam giác đều dùng phương pháp quay điểm.
         Tam giác nội tiếp đường tròn C(O, r).
         Mỗi cạnh tam giác chắn cung 120 độ đường tròn C.
         */
        private void DrawTriangle(OpenGL gl, Point p1, Point p2)
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

            //Gốc tọa độ
            int x = 0;
            int y = (int)r;

            //Vẽ, mỗi lần xoay 120 độ để lấy đỉnh tam giác
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for(int i=0; i<360; i+=120)
            {
                double angle = i * degree2rad;
                gl.Vertex(xCenter + x*Math.Cos(angle) - y * Math.Sin(angle), yCenter + x*Math.Sin(angle) + y * Math.Cos(angle));
            }
            gl.End();
            gl.Flush();

        }
      
        //Sự kiện nhấn chuột
        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            //Không phải polygon
            if (typeAction != TypeAction.polygon)
            {
                watch.Reset();
                watch.Start();
                //Lấy pStart tại vị trí chuột được down, khởi tạo pEnd = pStart
                pStart = e.Location;
                pEnd = pStart;
                //Ghi nhận đang vẽ
            }
            //ngược lại, nếu là polygon
            else
            {
                //Nếu ban đầu chưa click chuột (mới bắt đầu vẽ)
                if (pStart.X == -1)
                {
                    //Đồng hồ
                    watchPoly.Reset();
                    watchPoly.Start();
                    //Cập nhật điểm click
                    pStart = e.Location;
                    pEnd = pStart;
                    pMid = pStart;
                    //Tạo data
                    Data data = new Data(colorUserColor, typeAction, size);
                    data._controlPoints.Add(pStart);//Thêm vào control point list
                    listData.Add(data);//Thêm data vào list data
                }
                //Nếu đã click rồi (đang vẽ)
                else
                {
                    //Cho pMid là pEnd (của thao tác trước đó)
                    pMid = pEnd;
                    //Cập nhật lại pEnd
                    pEnd = e.Location;
                    //Thêm pEnd này vào list control point của data list
                    listData[listData.Count - 1]._controlPoints.Add(pEnd);
                }
               
            }
            drawing = true;//Bật cờ vẽ
        }

        //Sự kiện thả chuột
        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (typeAction != TypeAction.polygon)
            {             
                //Khởi tạo dữ liệu
                Data data = new Data(colorUserColor, typeAction, size);

                //Nếu chọn tô màu thì chỉ cần lấy điểm click đầu tiên làm điểm hạt giống
                if (typeAction == TypeAction.floodFill || typeAction == TypeAction.scanLines)
                    data._controlPoints.Add(pStart);

                else if (typeAction == TypeAction.select)
                {
                    pSelect = pStart;
                    selected = true;
                }
                //Ngược lại thì lấy cả 2 điểm
                else
                {
                    data._controlPoints.Add(pStart);
                    data._controlPoints.Add(pEnd);
                    DrawControlPoints(data._controlPoints, data._type);
                    drawing = false; //Kết thúc thao tác vẽ   

                    //Tính thời gian vẽ
                    watch.Stop();
                    string[] seconds = watch.Elapsed.TotalSeconds.ToString().Split('.');
                    string[] milliseconds = watch.Elapsed.TotalMilliseconds.ToString().Split('.');
                    timeDraw = seconds[0].ToString() + "." + milliseconds[0].ToString() + 's';
                    textBox1_TextChanged(new object(), new System.EventArgs());
                }
                //Lưu data vào listData
                listData.Add(data);
                pStart = new Point(-1, -1);
                pEnd = new Point(-1, -1);  
            }
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            //Nếu đang vẽ thì luôn cập nhật pEnd để nét vẽ đi theo chuột
            if (drawing == true)
            {
                pEnd = e.Location;
                update_pStart_pEnd(typeAction);
            }
        }

        
        private void openGLControl_MouseClick(object sender, MouseEventArgs e)
        {
            //Nếu click chuột phải và đang vẽ polygon
           if(e.Button==MouseButtons.Right&&typeAction==TypeAction.polygon)
            {
                rightClicked = true;//Bật cờ click chuột phải
                listData[listData.Count - 1]._controlPoints.Add(pStart);//Thêm pStart vào list control points
            }
        }
        
        //Các sự kiện chọn button vẽ hình, tô màu,...
        private void bt_line_MouseClick(object sender, MouseEventArgs e)
        {
            typeAction = TypeAction.line;
            drawing = false;
            selected = false;
        }

        private void bt_triangle_MouseClick(object sender, MouseEventArgs e)
        {
            typeAction = TypeAction.triangle;
            drawing = false;
            selected = false;
        }

        private void bt_circle_MouseClick(object sender, MouseEventArgs e)
        {
            typeAction = TypeAction.circle;
            drawing = false;
            selected = false;
           
        }

        private void bt_ellipse_MouseClick(object sender, MouseEventArgs e)
        {
            typeAction = TypeAction.ellipse;
            drawing = false;
            selected = false;
            
        }

        private void bt_rectangle_MouseClick(object sender, MouseEventArgs e)
        {
            typeAction = TypeAction.rectangle;
            drawing = false;
            selected = false;
            
        }

        private void bt_pentagon_MouseClick(object sender, MouseEventArgs e)
        {
            typeAction = TypeAction.pentagon;
            drawing = false;
            selected = false;
           
        }

        private void bt_hexagon_MouseClick(object sender, MouseEventArgs e)
        {
            typeAction = TypeAction.hexagon;
            drawing = false;
            selected = false;
            
        }

        private void bt_polygon_MouseClick(object sender, MouseEventArgs e)
        {
            typeAction = TypeAction.polygon;
            drawing = false;
            selected = false;
            
        }

        private void bt_Color_MouseClick(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                colorUserColor = colorDialog1.Color;
        }

        private void bt_Scanline_Click(object sender, EventArgs e)
        {
            typeAction = TypeAction.scanLines;
            selected = false;
          
        }

        private void bt_FloodFill_MouseClick(object sender, EventArgs e)
        {
            typeAction = TypeAction.floodFill;
            selected = false;        

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openGLControl_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = timeDraw;
        }

        private void bt_Select_Click(object sender, EventArgs e)
        {
            typeAction = TypeAction.select;
           
        }

        //Combo box để chọn độ dày nét vẽ
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == "1.5")
                size = 1.5f;
            else if (comboBox1.SelectedItem == "2.0")
                size = 2.0f;
            else if (comboBox1.SelectedItem == "2.5")
                size = 2.5f;
            else if (comboBox1.SelectedItem == "3.0")
                size = 3.0f;
            else if (comboBox1.SelectedItem == "3.5")
                size = 3.5f;
            else if (comboBox1.SelectedItem == "4.0")
                size = 4.0f;
            else if (comboBox1.SelectedItem == "4.5")
                size = 4.5f;
            else if (comboBox1.SelectedItem == "5.0")
                size = 5.0f;
            else if (comboBox1.SelectedItem == "5.5")
                size = 5.5f;
            else if (comboBox1.SelectedItem == "6.0")
                size = 6.0f;
        }
    }
}
