using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using System.Windows.Forms;
using System.Collections;


namespace MidtermProject
{
    class Fill
    {
        public Point seed;
        public Color oldColor;
        public Color newColor;
        public virtual void FillColor(OpenGL gl) { }
        public virtual void FillColor(OpenGL gl,ET edgeTable) { }
        public virtual void ApplyFill(OpenGL gl, Point p) { }
        public virtual void ApplyFill(OpenGL gl, ET edgeTable) { }
        public void putPixel(OpenGL gl, Point p)
        {
            //Lấy từng thành phần màu
            Byte[] pixels = new Byte[4];
            pixels[0] = newColor.R;
            pixels[1] = newColor.G;
            pixels[2] = newColor.B;
            pixels[3] = newColor.A;
            gl.RasterPos(p.X, gl.RenderContextProvider.Height - p.Y);
            gl.DrawPixels(1, 1, OpenGL.GL_RGBA, pixels);
            gl.Flush();
        }
        public void getPixel(OpenGL gl, Point p, out Byte[] color)
        {
            color = new Byte[4 * 1 * 1]; // Components * width * height (RGBA)
            gl.ReadPixels(p.X, gl.RenderContextProvider.Height - p.Y, 1, 1, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, color);
        }
    }

    class FloodFill : Fill
    {
        public override void ApplyFill(OpenGL gl, Point pFill)
        {
            seed = pFill;
            FillColor(gl);
        }
        public override void FillColor(OpenGL gl)
        {
            Byte[] pixel = new Byte[4];
            //Lấy điểm hạt giống
            getPixel(gl,seed, out pixel);
            oldColor = Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);

            //Tránh lặp vô hạn khi màu cũ giống màu mới
            if (newColor == oldColor) return;

            //Mảng index để truy cập các lân cận 4
            int[] dx = new int[] { 0, 1, 0, -1 };
            int[] dy = new int[] { -1, 0, 1, 0 };

            //Tạo stack và push điểm khởi đầu vào stack
            Stack<Point> s = new Stack<Point>();
            s.Push(seed);

            //Lặp đến khi stack rỗng
            while (s.Count != 0)
            {
                //Lấy điểm từ stack
                Point p = s.Pop();
                //Tô màu cho điểm đó
                putPixel(gl, p);
                //Truy xuất lân cận 4 của điểm hiện hành
                for (int i = 0; i < 4; i++)
                {
                    //Lấy điểm lân cận
                    Point pNeighbor = new Point(p.X + dx[i], p.Y + dy[i]);

                    //Lấy giá trị pixel của điểm đó
                    Byte[] neighbor_color;
                    getPixel(gl, pNeighbor, out neighbor_color);

                    //Nếu điểm đó chưa tô thì thêm vào stack
                    if (neighbor_color[0] == oldColor.R && neighbor_color[1] == oldColor.G && neighbor_color[2] == oldColor.B
                       && neighbor_color[3] == oldColor.A)
                        s.Push(pNeighbor);
                }

            }
        }
    }

    class ScanLine : Fill
    {
        public override void ApplyFill(OpenGL gl, ET edgeTable)
        {
            FillColor(gl, edgeTable);
        }
        public override void FillColor(OpenGL gl,ET edgeTable)
        {
            //Lấy bảng ET
            Hashtable hashTable = new Hashtable();
            hashTable = edgeTable.GetET();

            //Lấy max key, min key
            int minKey = edgeTable.GetMinKey();
            int maxKey = edgeTable.GetMaxKey();

            //Khởi tạo linked list beg List để lưu các cạnh
            LinkedList<ALE> begList = new LinkedList<ALE>();

            for (int i = minKey; i < maxKey; i++)
            {
                //Thêm các cạnh vào begList
                if (hashTable.ContainsKey(i))
                    foreach (ALE edge in (LinkedList<ALE>)hashTable[i])
                        begList.AddLast(edge);

                //Lưu cạnh theo x intersect
                var saveBegList = ((LinkedList<ALE>)begList).OrderBy(edge => edge.GetXIntersect());

                //Vẽ các đường thẳng (scanline)
                var node = begList.First;
                while (node!=null&&node.Next!=null)
                {
                    var nextNode = node.Next;
                    var afterNextNode = nextNode.Next;

                    //Lấy điểm và vẽ
                    Point p1 = new Point((int)node.Value.GetXIntersect(), i);
                    Point p2 = new Point((int)nextNode.Value.GetXIntersect(), i);
                    gl.Color(newColor.R / 255.0, newColor.G / 255.0, newColor.B / 255.0);
                    gl.Begin(OpenGL.GL_LINES);                    
                    gl.Vertex(p1.X, p1.Y);
                    gl.Vertex(p2.X, p2.Y);
                    gl.End();
                    node = afterNextNode;
                }

                //Xóa các cạnh có yUpper = i
                var node1 = begList.First;
                while(node1!=null)
                {
                    var nextNode = node1.Next;
                    if(node1.Value.GetYUpper()==i)
                        begList.Remove(node1);
                    node1 = nextNode;                   
                }

                //Cập nhật lại x intersect
                foreach (ALE e in begList)
                    e.UpdateXIntersect();
            }
            gl.Flush();
        }
    }

    //Lớp Active Edge List
    class ALE
    {
        int _yUpper;
        float _xIntersect;
        float _reciSlope;
        public int GetYUpper()
        {
            return _yUpper;
        }
        public float GetXIntersect()
        {
            return _xIntersect;
        }
        public float GetReciSlope()
        {
            return _reciSlope;
        }
        public void UpdateXIntersect()
        {
            _xIntersect += _reciSlope;
        }
        public ALE(int yUpper, float xIntersect, float reciSlope)
        {
            _yUpper = yUpper;
            _xIntersect = xIntersect;
            _reciSlope = reciSlope;
        }
    }

    //Lớp Edge Table
    class ET
    {
        Hashtable _egdeTable = new Hashtable();
        int minKey = 99999, maxKey = -1;
        public Hashtable GetET()
        {
            return _egdeTable;
        }
        public int GetMinKey()
        {
            return minKey;
        }
        public int GetMaxKey()
        {
            return maxKey;
        }
        //Hệ số góc của một cạnh p1p2
        private float SlopeCoefficient(FloatPoint p1, FloatPoint p2)        
        {
            //Nếu x bằng nhau thì bằng 0.
            if (p1.X == p2.X)
                return 0;
            //Ngược lại, tính theo công thức hệ số góc
            else
                return (p2.Y - p1.Y) / (p2.X - p1.X);
        }

        //Kiểm tra giao điểm có phải là cực trị hay không
        private bool IsExtreme(FloatPoint p1, FloatPoint pIntersection,FloatPoint p2)
        {
            //Nếu giao điểm là cực đại hoặc cực tiểu (cực trị)
            if (pIntersection.Y > p1.Y && pIntersection.Y > p2.Y
                || pIntersection.Y < p1.Y && pIntersection.Y < p2.Y)
                return true;
            return false;
        }

        public void EdgeData(List<FloatPoint>listPoint)
        {
            int n = listPoint.Count;
            for(int i=0;i<n;i++)
            {
                //Khởi tạo 3 điểm liền kề
                FloatPoint previousPoint, currentPoint, nextPoint;

                //Điểm hiện tại
                currentPoint = listPoint[i];

                //Nếu đang ở đầu list, thì điểm trước điểm hiện tại là điểm cuối list
                if (i - 1 < 0)
                    previousPoint = listPoint[n - 1];
                else//Ngược lại thì là điểm ở vị trí trước nó
                    previousPoint = listPoint[i - 1];

                //Nếu đang ở cuối list thì điểm kế tiếp của điểm hiện tại là điểm đầu list
                if (i + 1 > n - 1)
                    nextPoint = listPoint[0];
                else//Ngược lại thì là điểm ở vị trí liền sau
                    nextPoint = listPoint[i + 1];

                //Tính hệ số góc
                float reciSlope = SlopeCoefficient(currentPoint, nextPoint);

                //Cạnh song song Ox
                if (reciSlope == 0 && currentPoint.X != nextPoint.X)
                    continue;

                //Nghịch đảo hệ số góc, mặc định là 0 cho trường hợp cạnh song song Oy (có hsg = 0)
                float reciSlopeInverse = 0;

                //Nếu khác 0 thì lấy nghịch đảo
                if (reciSlope != 0)
                    reciSlopeInverse = 1 / reciSlope;

                //Tính yUpper
                int yUpper;

                //Nếu đỉnh sau có y lớn hơn đỉnh trước thì xét nó
                if(nextPoint.Y>currentPoint.Y)
                {
                    yUpper = (int)nextPoint.Y;
                    FloatPoint afterNextPoint;

                    //Nếu điểm liền sau của nextPoint ra khỏi danh sách thì lấy về đầu danh sách
                    if (i + 2 > n - 1)
                        afterNextPoint = listPoint[i + 2 - n];

                    //Nếu không lấy theo index bình thường
                    else afterNextPoint = listPoint[i + 2];

                    //Nếu không phải cực trị thì giảm yUpper
                    if (!IsExtreme(currentPoint, nextPoint, afterNextPoint))
                        yUpper--;

                }
                //Ngược lại, xét currentPoint
                else
                {
                    yUpper = (int)currentPoint.Y;
                    if (!IsExtreme(previousPoint, currentPoint, nextPoint))
                        yUpper--;
                }

                //Cập nhật max key
                if (yUpper > maxKey)
                    maxKey = yUpper - 1;

                //Tinh xIntersect
                float xIntersect = currentPoint.X;
                if (nextPoint.Y < currentPoint.Y)
                    xIntersect = nextPoint.X;

                //Tính minKey: là key để lưu cạnh vào bảng ET
                int yMin = (int)currentPoint.Y;
                if (nextPoint.Y < yMin)
                    yMin = (int)nextPoint.Y;
                if (yMin < minKey)
                    minKey = yMin;

                //Lưu cạnh vừa tính được vào ET
                ALE newEdge = new ALE(yUpper, xIntersect,reciSlopeInverse);

                //Nếu tại vị trí yMin đã có một linked list thì thêm cạnh mới vào sau linked list
                if (_egdeTable.ContainsKey(yMin))
                    ((LinkedList<ALE>)_egdeTable[yMin]).AddLast(newEdge);
                //Nếu chưa có
                else
                {
                    //Thì khởi tạo edge list mới
                    LinkedList<ALE> newEdgeList = new LinkedList<ALE>();
                    //Thêm cạnh vào edge list
                    newEdgeList.AddLast(newEdge);
                    //Đưa edge list vào ET
                    _egdeTable.Add(yMin, newEdgeList);
                }
            }
        }


       

    }
    //Struct Điểm lưu tọa độ kiểu thực
    public struct FloatPoint
    {
        public float X;
        public float Y;
        public FloatPoint(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
