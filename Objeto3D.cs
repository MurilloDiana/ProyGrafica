using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK3DLetterU
{
    public abstract class Objeto3D
    {
        protected int VertexBufferObject;
        protected int VertexArrayObject;

        public Vector3 Posicion { get; set; }
        public Vector3 Centro { get; set; }
        public Vector4 Color { get; set; } = new Vector4(0.96f, 0.87f, 0.7f, 1.0f);
        public Vector4 WireframeColor { get; set; } = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        public float[] Vertices { get; protected set; }

        public virtual void Inicializar()
        {
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        public virtual void Dibujar(Shader shader, Matrix4 view, Matrix4 projection, float anguloRotacion = 0)
        {
            Matrix4 model = Matrix4.CreateTranslation(Posicion - Centro) *
                          Matrix4.CreateRotationY(MathHelper.DegreesToRadians(anguloRotacion)) *
                          Matrix4.CreateTranslation(Centro);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.BindVertexArray(VertexArrayObject);
            DibujarWireframe(shader);
        }

        protected abstract void DibujarWireframe(Shader shader);

        public virtual void Dispose()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
        }
    }
}