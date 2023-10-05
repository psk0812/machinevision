using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using static machinevision.ObjLoader;


namespace machinevision
{


    public partial class Main_form : Form
    {
        private ObjLoader objLoader = new ObjLoader();

        private GLControl glControl;
        public List<Vector3> Vertices { get; } = new List<Vector3>();
        public List<int> Indices { get; } = new List<int>();


        public Main_form()
        {
            InitializeComponent();
            InitializeOpenGLControl();
        }

        private void InitializeOpenGLControl()
        {

            glControl = new GLControl();
            glControl.Dock = DockStyle.Fill;

            // OpenGL 초기화
            glControl.Load += (sender, e) =>
            {
                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            };

            // OpenGL 렌더링
            glControl.Paint += (sender, e) =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

                    // 여기서 OpenGL 그리기 코드를 추가하세요

                    glControl.SwapBuffers();
            };

            panel_opengl.Controls.Add(glControl);

        }

        public void LoadObjFile(string filePath)
        {
            Vertices.Clear();
            Indices.Clear();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    if (parts.Length > 0)
                    {
                        if (parts[0] == "v")
                        {

                            float x = float.Parse(parts[1]);
                            float y = float.Parse(parts[2]);
                            float z = float.Parse(parts[3]);
                            Vertices.Add(new Vector3(x, y, z));
                        }
                        else if (parts[0] == "f")
                        {

                            int v1 = int.Parse(parts[1].Split('/')[0]) - 1;
                            int v2 = int.Parse(parts[2].Split('/')[0]) - 1;
                            int v3 = int.Parse(parts[3].Split('/')[0]) - 1;
                            Indices.Add(v1);
                            Indices.Add(v2);
                            Indices.Add(v3);
                        }
                    }
                }
            }
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);

        //    try
        //    {
        //        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

        //        string filePath = Path.Combine(appDirectory, @"..\..\..\mydiary\data\visionboard_2.txt");
        //        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        //        {
        //            openFileDialog.Filter = "obj파일 (*.obj)|*.obj";
        //            if (openFileDialog.ShowDialog() == DialogResult.OK)
        //            {
        //                string imagePath = openFileDialog.FileName;

        //                objLoader.LoadObjFile(imagePath);
        //                // OpenGL 초기화
        //                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        //            }

        //        }
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.Message); }

        //}




    }


}
