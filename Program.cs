using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace OpenTK3DLetterU
{
    public class Program : GameWindow
    {
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _shaderProgram;

        private float _rotationAngle = 0.0f;

        public Program(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f); 
            GL.Enable(EnableCap.DepthTest); 


            // Definir los vértices de la letra "U" con dos prismas y un cubo
            float[] vertices = {
                // Prisma 1 (prisma izquierdo)
                // Cara frontal
                -0.5f,  0.5f,  0.5f, 
                -0.5f, -0.5f,  0.5f, 
                -0.3f, -0.5f,  0.5f,
                -0.3f,  0.5f,  0.5f, 

                // Cara trasera
                -0.5f,  0.5f, -0.5f, 
                -0.5f, -0.5f, -0.5f, 
                -0.3f, -0.5f, -0.5f, 
                -0.3f,  0.5f, -0.5f, 

                // Conectar las caras (lados)
                -0.5f,  0.5f,  0.5f, 
                -0.5f,  0.5f, -0.5f, 
                -0.5f, -0.5f,  0.5f, 
                -0.5f, -0.5f, -0.5f, 
                -0.3f, -0.5f,  0.5f, 
                -0.3f, -0.5f, -0.5f, 
                -0.3f,  0.5f,  0.5f, 
                -0.3f,  0.5f, -0.5f, 

                // Prisma 2 (prisma derecho)
                // Cara frontal
                0.3f,  0.5f,  0.5f, 
                0.3f, -0.5f,  0.5f, 
                0.5f, -0.5f,  0.5f, 
                0.5f,  0.5f,  0.5f, 

                // Cara trasera
                0.3f,  0.5f, -0.5f, 
                0.3f, -0.5f, -0.5f, 
                0.5f, -0.5f, -0.5f, 
                0.5f,  0.5f, -0.5f, 

                // Conectar las caras (lados)
                0.3f,  0.5f,  0.5f, 
                0.3f,  0.5f, -0.5f, 
                0.3f, -0.5f,  0.5f, 
                0.3f, -0.5f, -0.5f, 
                0.5f, -0.5f,  0.5f, 
                0.5f, -0.5f, -0.5f, 
                0.5f,  0.5f,  0.5f, 
                0.5f,  0.5f, -0.5f, 

                // Cubo (base de la "U")
                // Cara frontal
                -0.3f, -0.5f,  0.5f, 
                -0.3f, -0.7f,  0.5f, 
                 0.3f, -0.7f,  0.5f, 
                 0.3f, -0.5f,  0.5f, 

                // Cara trasera
                -0.3f, -0.5f, -0.5f, 
                -0.3f, -0.7f, -0.5f, 
                 0.3f, -0.7f, -0.5f, 
                 0.3f, -0.5f, -0.5f, 

                // Conectar las caras (lados)
                -0.3f, -0.5f,  0.5f, 
                -0.3f, -0.5f, -0.5f, 
                -0.3f, -0.7f,  0.5f, 
                -0.3f, -0.7f, -0.5f, 
                 0.3f, -0.7f,  0.5f, 
                 0.3f, -0.7f, -0.5f, 
                 0.3f, -0.5f,  0.5f, 
                 0.3f, -0.5f, -0.5f  
            };

            
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            
            string vertexShaderSource = @"
                #version 330 core
                layout(location = 0) in vec3 aPosition;
                uniform mat4 model;
                uniform mat4 view;
                uniform mat4 projection;
                void main()
                {
                    gl_Position = projection * view * model * vec4(aPosition, 1.0);
                }
            ";

            string fragmentShaderSource = @"
                #version 330 core
                out vec4 FragColor;
                uniform vec4 color;
                void main()
                {
                    FragColor = color;
                }
            ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_shaderProgram);

            
            _rotationAngle += (float)args.Time * 50.0f; 
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotationAngle));

            
            Matrix4 view = Matrix4.LookAt(new Vector3(2, 2, 2), new Vector3(0, 0, 0), Vector3.UnitY);

            
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), Size.X / (float)Size.Y, 0.1f, 100.0f);

            
            int modelLocation = GL.GetUniformLocation(_shaderProgram, "model");
            int viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");

            GL.UniformMatrix4(modelLocation, false, ref model);
            GL.UniformMatrix4(viewLocation, false, ref view);
            GL.UniformMatrix4(projectionLocation, false, ref projection);

            
            int colorLocation = GL.GetUniformLocation(_shaderProgram, "color");
            GL.Uniform4(colorLocation, new Vector4(0.96f, 0.87f, 0.7f, 1.0f)); 

            GL.BindVertexArray(_vertexArrayObject);

           
            DrawFilledFaces();

            
            GL.Uniform4(colorLocation, new Vector4(0.0f, 0.0f, 0.0f, 1.0f)); 

           
            DrawWireframe();

            SwapBuffers();
        }

        private void DrawFilledFaces()
        {
            // Dibujar las caras rellenas usando triángulos
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4); 
            GL.DrawArrays(PrimitiveType.TriangleFan, 4, 4); 
            GL.DrawArrays(PrimitiveType.TriangleFan, 8, 4); 
            GL.DrawArrays(PrimitiveType.TriangleFan, 12, 4);
            GL.DrawArrays(PrimitiveType.TriangleFan, 16, 4);
            GL.DrawArrays(PrimitiveType.TriangleFan, 20, 4);
            GL.DrawArrays(PrimitiveType.TriangleFan, 24, 4);
            GL.DrawArrays(PrimitiveType.TriangleFan, 28, 4); 
            GL.DrawArrays(PrimitiveType.TriangleFan, 32, 4); 
            GL.DrawArrays(PrimitiveType.TriangleFan, 36, 4); 
            GL.DrawArrays(PrimitiveType.TriangleFan, 40, 4); 
            GL.DrawArrays(PrimitiveType.TriangleFan, 44, 4); 
        }

        private void DrawWireframe()
        {
            // Dibujar las líneas de las caras
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 4); 
            GL.DrawArrays(PrimitiveType.LineLoop, 4, 4); 
            GL.DrawArrays(PrimitiveType.Lines, 8, 8); 
            GL.DrawArrays(PrimitiveType.LineLoop, 16, 4);
            GL.DrawArrays(PrimitiveType.LineLoop, 20, 4);
            GL.DrawArrays(PrimitiveType.Lines, 24, 8); 
            GL.DrawArrays(PrimitiveType.LineLoop, 32, 4);
            GL.DrawArrays(PrimitiveType.LineLoop, 36, 4);
            GL.DrawArrays(PrimitiveType.Lines, 40, 8); 
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteProgram(_shaderProgram);

            base.OnUnload();
        }

        public static void Main(string[] args)
        {
            using (Program program = new Program(800, 600, "Letra U en 3D"))
            {
                program.Run();
            }
        }
    }
}