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
using Tao.OpenGl;
using ClearBufferMask = OpenTK.Graphics.ClearBufferMask;
using GL = OpenTK.Graphics.GL;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace machinevision
{


    public partial class Main_form : Form
    {

        private GLControl glControl;
        private float rotationX = 0.0f;
        private float rotationY = 0.0f;
        private Point lastMousePos;


        public static List<Vector3> Vertices { get; } = new List<Vector3>();
        public static List<int> Indices { get; } = new List<int>();

       
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

                // 투영 행렬 설정
                GL.MatrixMode(OpenTK.Graphics.MatrixMode.Projection);
                GL.LoadIdentity();
                OpenTK.Graphics.Glu.Perspective(45.0f, glControl.Width / (float)glControl.Height, 0.1f, 100.0f);
            };

            // OpenGL 렌더링
            glControl.Paint += (sender, e) =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                // 뷰 행렬 설정 (카메라 위치와 방향)
                GL.MatrixMode(OpenTK.Graphics.MatrixMode.Modelview);
                GL.LoadIdentity();

                // 정육면체의 8개 꼭짓점 좌표 (예제)
                Vector3[] cubeVertices = new Vector3[]
                {
                    new Vector3(-1.0f, -1.0f, -1.0f),
                    new Vector3(1.0f, -1.0f, -1.0f),
                    new Vector3(1.0f, 1.0f, -1.0f),
                    new Vector3(-1.0f, 1.0f, -1.0f),
                    new Vector3(-1.0f, -1.0f, 1.0f),
                    new Vector3(1.0f, -1.0f, 1.0f),
                    new Vector3(1.0f, 1.0f, 1.0f),
                    new Vector3(-1.0f, 1.0f, 1.0f)
                };

                // 꼭짓점들의 평균을 계산하여 정육면체 중심 위치를 찾습니다.
                Vector3 cubeCenter = Vector3.Zero;

                foreach (Vector3 vertex in cubeVertices)
                {
                    cubeCenter += vertex;
                }
                float distanceFromCenter = 5.0f;

                cubeCenter /= cubeVertices.Length;

                // 1. 정사각형의 아래쪽 왼쪽 모서리 좌표 계산
                Vector3 squareBottomLeftCorner = new Vector3(-1.0f, -1.0f, 0.0f); // 예제 좌표, 필요에 따라 변경

                // 2. 카메라 위치 설정 (정사각형의 아래쪽 왼쪽 모서리에서 적절한 거리에 배치)
                Vector3 cameraPosition = squareBottomLeftCorner + new Vector3(0.0f, 0.0f, -distanceFromCenter);

                // 3. 카메라가 아래쪽 왼쪽 모서리를 바라보도록 시선 방향 조정
                Vector3 targetPosition = squareBottomLeftCorner; // 정사각형 모서리를 바라보도록 설정
                Vector3 cameraDirection = targetPosition - cameraPosition;
                cameraDirection.Normalize();


             

                GL.Translate(cameraPosition); // 카메라 위치 조정

                // 3D 정육면체 그리기
                GL.Begin((OpenTK.Graphics.BeginMode)PrimitiveType.Quads);
                GL.Color3(Color.Red);

                // 앞면
                GL.Vertex3(-1.0f, -1.0f, -1.0f);
                GL.Vertex3(1.0f, -1.0f, -1.0f);
                GL.Vertex3(1.0f, 1.0f, -1.0f);
                GL.Vertex3(-1.0f, 1.0f, -1.0f);

                // 뒷면
                GL.Vertex3(-1.0f, -1.0f, 1.0f);
                GL.Vertex3(1.0f, -1.0f, 1.0f);
                GL.Vertex3(1.0f, 1.0f, 1.0f);
                GL.Vertex3(-1.0f, 1.0f, 1.0f);

                // 왼쪽 면
                GL.Vertex3(-1.0f, -1.0f, -1.0f);
                GL.Vertex3(-1.0f, 1.0f, -1.0f);
                GL.Vertex3(-1.0f, 1.0f, 1.0f);
                GL.Vertex3(-1.0f, -1.0f, 1.0f);

                // 오른쪽 면
                GL.Vertex3(1.0f, -1.0f, -1.0f);
                GL.Vertex3(1.0f, 1.0f, -1.0f);
                GL.Vertex3(1.0f, 1.0f, 1.0f);
                GL.Vertex3(1.0f, -1.0f, 1.0f);

                // 위쪽 면
                GL.Vertex3(-1.0f, 1.0f, -1.0f);
                GL.Vertex3(1.0f, 1.0f, -1.0f);
                GL.Vertex3(1.0f, 1.0f, 1.0f);
                GL.Vertex3(-1.0f, 1.0f, 1.0f);

                // 아래쪽 면
                GL.Vertex3(-1.0f, -1.0f, -1.0f);
                GL.Vertex3(1.0f, -1.0f, -1.0f);
                GL.Vertex3(1.0f, -1.0f, 1.0f);
                GL.Vertex3(-1.0f, -1.0f, 1.0f);

                GL.End();


                glControl.SwapBuffers();
            };

          

        


            panel_opengl.Controls.Add(glControl);
        }









        private void openfile_Click(object sender, EventArgs e)
        {
            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(appDirectory, @"..\..\..\mydiary\data\visionboard_7.txt");
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "오브젝트 파일 (*.obj)|*.obj";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string imagePath = openFileDialog.FileName;
                        

                    }

                }
            }
            catch { }

        }

        


    }




}
