using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTK3DLetterU
{
    public class Program : GameWindow
    {
        private Shader _shader;
        private LetraU _letraU;
        private float _rotationAngle;

        public Program(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Crear shader
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

            _shader = new Shader(vertexShaderSource, fragmentShaderSource);

            // Crear letra U
            _letraU = new LetraU(
                posicion: Vector3.Zero,
                centro: Vector3.Zero);
            _letraU.Inicializar();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();

            _rotationAngle += (float)args.Time * 50.0f;

            Matrix4 view = Matrix4.LookAt(
                new Vector3(2, 2, 2),
                Vector3.Zero,
                Vector3.UnitY);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45),
                Size.X / (float)Size.Y,
                0.1f, 100.0f);

            _letraU.Dibujar(_shader, view, projection, _rotationAngle);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            _letraU.Dispose();
            _shader.Dispose();
            base.OnUnload();
        }

        public static void Main(string[] args)
        {
            using (Program program = new Program(800, 600, "Letra U en 3D con Clases"))
            {
                program.Run();
            }
        }
    }
}