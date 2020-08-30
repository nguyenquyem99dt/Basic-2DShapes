namespace MidtermProject
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openGLControl = new SharpGL.OpenGLControl();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.bt_Select = new System.Windows.Forms.Button();
            this.bt_Scanline = new System.Windows.Forms.Button();
            this.bt_FloodFill = new System.Windows.Forms.Button();
            this.bt_Color = new System.Windows.Forms.Button();
            this.bt_polygon = new System.Windows.Forms.Button();
            this.bt_hexagon = new System.Windows.Forms.Button();
            this.bt_pentagon = new System.Windows.Forms.Button();
            this.bt_rectangle = new System.Windows.Forms.Button();
            this.bt_ellipse = new System.Windows.Forms.Button();
            this.bt_circle = new System.Windows.Forms.Button();
            this.bt_triangle = new System.Windows.Forms.Button();
            this.bt_line = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.SuspendLayout();
            // 
            // openGLControl
            // 
            this.openGLControl.BackColor = System.Drawing.SystemColors.Control;
            this.openGLControl.DrawFPS = false;
            this.openGLControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.openGLControl.Location = new System.Drawing.Point(12, 79);
            this.openGLControl.Margin = new System.Windows.Forms.Padding(5);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(1600, 738);
            this.openGLControl.TabIndex = 1;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized_1);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw_1);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.Load += new System.EventHandler(this.openGLControl_Load);
            this.openGLControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseClick);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft PhagsPa", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(920, 32);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(113, 26);
            this.comboBox1.TabIndex = 13;
            this.comboBox1.Text = "Line Width";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // bt_Select
            // 
            this.bt_Select.Font = new System.Drawing.Font("Microsoft PhagsPa", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Select.Image = global::MidtermProject.Properties.Resources.click1;
            this.bt_Select.Location = new System.Drawing.Point(13, 11);
            this.bt_Select.Margin = new System.Windows.Forms.Padding(4);
            this.bt_Select.Name = "bt_Select";
            this.bt_Select.Size = new System.Drawing.Size(86, 63);
            this.bt_Select.TabIndex = 15;
            this.bt_Select.Text = "Select";
            this.bt_Select.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bt_Select.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Select.UseVisualStyleBackColor = true;
            this.bt_Select.Click += new System.EventHandler(this.bt_Select_Click);
            // 
            // bt_Scanline
            // 
            this.bt_Scanline.Font = new System.Drawing.Font("Microsoft PhagsPa", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Scanline.Image = global::MidtermProject.Properties.Resources.bucket;
            this.bt_Scanline.Location = new System.Drawing.Point(1258, 11);
            this.bt_Scanline.Margin = new System.Windows.Forms.Padding(4);
            this.bt_Scanline.Name = "bt_Scanline";
            this.bt_Scanline.Size = new System.Drawing.Size(80, 65);
            this.bt_Scanline.TabIndex = 14;
            this.bt_Scanline.Text = "Scanline";
            this.bt_Scanline.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Scanline.UseVisualStyleBackColor = true;
            this.bt_Scanline.Click += new System.EventHandler(this.bt_Scanline_Click);
            // 
            // bt_FloodFill
            // 
            this.bt_FloodFill.Font = new System.Drawing.Font("Microsoft PhagsPa", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_FloodFill.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_FloodFill.Image = global::MidtermProject.Properties.Resources.bucket;
            this.bt_FloodFill.Location = new System.Drawing.Point(1170, 11);
            this.bt_FloodFill.Margin = new System.Windows.Forms.Padding(4);
            this.bt_FloodFill.Name = "bt_FloodFill";
            this.bt_FloodFill.Size = new System.Drawing.Size(80, 65);
            this.bt_FloodFill.TabIndex = 12;
            this.bt_FloodFill.Text = "Flood Fill";
            this.bt_FloodFill.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_FloodFill.UseVisualStyleBackColor = true;
            this.bt_FloodFill.Click += new System.EventHandler(this.bt_FloodFill_MouseClick);
            // 
            // bt_Color
            // 
            this.bt_Color.Font = new System.Drawing.Font("Microsoft PhagsPa", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Color.Image = global::MidtermProject.Properties.Resources.color_picker;
            this.bt_Color.Location = new System.Drawing.Point(1479, 13);
            this.bt_Color.Margin = new System.Windows.Forms.Padding(4);
            this.bt_Color.Name = "bt_Color";
            this.bt_Color.Size = new System.Drawing.Size(87, 63);
            this.bt_Color.TabIndex = 11;
            this.bt_Color.Text = "Pick Color";
            this.bt_Color.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bt_Color.UseVisualStyleBackColor = true;
            this.bt_Color.Click += new System.EventHandler(this.bt_Color_MouseClick);
            // 
            // bt_polygon
            // 
            this.bt_polygon.BackColor = System.Drawing.SystemColors.Control;
            this.bt_polygon.Image = global::MidtermProject.Properties.Resources.polygon2;
            this.bt_polygon.Location = new System.Drawing.Point(838, 16);
            this.bt_polygon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_polygon.Name = "bt_polygon";
            this.bt_polygon.Size = new System.Drawing.Size(75, 59);
            this.bt_polygon.TabIndex = 10;
            this.bt_polygon.UseVisualStyleBackColor = false;
            this.bt_polygon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bt_polygon_MouseClick);
            // 
            // bt_hexagon
            // 
            this.bt_hexagon.BackColor = System.Drawing.SystemColors.Control;
            this.bt_hexagon.Image = global::MidtermProject.Properties.Resources.hexagon2;
            this.bt_hexagon.Location = new System.Drawing.Point(757, 17);
            this.bt_hexagon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_hexagon.Name = "bt_hexagon";
            this.bt_hexagon.Size = new System.Drawing.Size(75, 59);
            this.bt_hexagon.TabIndex = 8;
            this.bt_hexagon.UseVisualStyleBackColor = false;
            this.bt_hexagon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bt_hexagon_MouseClick);
            // 
            // bt_pentagon
            // 
            this.bt_pentagon.BackColor = System.Drawing.SystemColors.Control;
            this.bt_pentagon.Image = global::MidtermProject.Properties.Resources.pentagon2;
            this.bt_pentagon.Location = new System.Drawing.Point(676, 17);
            this.bt_pentagon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_pentagon.Name = "bt_pentagon";
            this.bt_pentagon.Size = new System.Drawing.Size(75, 59);
            this.bt_pentagon.TabIndex = 7;
            this.bt_pentagon.UseVisualStyleBackColor = false;
            this.bt_pentagon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bt_pentagon_MouseClick);
            // 
            // bt_rectangle
            // 
            this.bt_rectangle.BackColor = System.Drawing.SystemColors.Control;
            this.bt_rectangle.Image = global::MidtermProject.Properties.Resources.rectangle2;
            this.bt_rectangle.Location = new System.Drawing.Point(595, 16);
            this.bt_rectangle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_rectangle.Name = "bt_rectangle";
            this.bt_rectangle.Size = new System.Drawing.Size(75, 59);
            this.bt_rectangle.TabIndex = 6;
            this.bt_rectangle.UseVisualStyleBackColor = false;
            this.bt_rectangle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bt_rectangle_MouseClick);
            // 
            // bt_ellipse
            // 
            this.bt_ellipse.BackColor = System.Drawing.SystemColors.Control;
            this.bt_ellipse.Image = global::MidtermProject.Properties.Resources.oval2;
            this.bt_ellipse.Location = new System.Drawing.Point(514, 15);
            this.bt_ellipse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_ellipse.Name = "bt_ellipse";
            this.bt_ellipse.Size = new System.Drawing.Size(75, 59);
            this.bt_ellipse.TabIndex = 5;
            this.bt_ellipse.UseVisualStyleBackColor = false;
            this.bt_ellipse.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bt_ellipse_MouseClick);
            // 
            // bt_circle
            // 
            this.bt_circle.BackColor = System.Drawing.SystemColors.Control;
            this.bt_circle.Image = global::MidtermProject.Properties.Resources.circle2;
            this.bt_circle.Location = new System.Drawing.Point(433, 16);
            this.bt_circle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_circle.Name = "bt_circle";
            this.bt_circle.Size = new System.Drawing.Size(75, 58);
            this.bt_circle.TabIndex = 4;
            this.bt_circle.UseVisualStyleBackColor = false;
            this.bt_circle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bt_circle_MouseClick);
            // 
            // bt_triangle
            // 
            this.bt_triangle.BackColor = System.Drawing.SystemColors.Control;
            this.bt_triangle.Image = global::MidtermProject.Properties.Resources.triangle2;
            this.bt_triangle.Location = new System.Drawing.Point(352, 15);
            this.bt_triangle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_triangle.Name = "bt_triangle";
            this.bt_triangle.Size = new System.Drawing.Size(75, 59);
            this.bt_triangle.TabIndex = 3;
            this.bt_triangle.UseVisualStyleBackColor = false;
            this.bt_triangle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bt_triangle_MouseClick);
            // 
            // bt_line
            // 
            this.bt_line.BackColor = System.Drawing.SystemColors.Control;
            this.bt_line.Image = global::MidtermProject.Properties.Resources.line2;
            this.bt_line.Location = new System.Drawing.Point(270, 16);
            this.bt_line.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bt_line.Name = "bt_line";
            this.bt_line.Size = new System.Drawing.Size(76, 58);
            this.bt_line.TabIndex = 2;
            this.bt_line.UseVisualStyleBackColor = false;
            this.bt_line.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bt_line_MouseClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1077, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(65, 22);
            this.textBox1.TabIndex = 16;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1579, 690);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bt_Select);
            this.Controls.Add(this.bt_Scanline);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.bt_FloodFill);
            this.Controls.Add(this.bt_Color);
            this.Controls.Add(this.bt_polygon);
            this.Controls.Add(this.bt_hexagon);
            this.Controls.Add(this.bt_pentagon);
            this.Controls.Add(this.bt_rectangle);
            this.Controls.Add(this.bt_ellipse);
            this.Controls.Add(this.bt_circle);
            this.Controls.Add(this.bt_triangle);
            this.Controls.Add(this.bt_line);
            this.Controls.Add(this.openGLControl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.Button bt_line;
        private System.Windows.Forms.Button bt_triangle;
        private System.Windows.Forms.Button bt_circle;
        private System.Windows.Forms.Button bt_ellipse;
        private System.Windows.Forms.Button bt_rectangle;
        private System.Windows.Forms.Button bt_pentagon;
        private System.Windows.Forms.Button bt_hexagon;
        private System.Windows.Forms.Button bt_polygon;
        private System.Windows.Forms.Button bt_Color;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button bt_FloodFill;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button bt_Scanline;
        private System.Windows.Forms.Button bt_Select;
        private System.Windows.Forms.TextBox textBox1;
    }
}

