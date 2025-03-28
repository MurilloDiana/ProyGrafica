using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK3DLetterU
{
    public class Shader
    {
        private readonly int _handle;

        public Shader(string vertexShaderSource, string fragmentShaderSource)
        {
            // Compilar vertex shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            // Compilar fragment shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            // Crear programa shader
            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);
            GL.LinkProgram(_handle);

            // Limpiar recursos
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void SetVector4(string name, Vector4 vector)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.Uniform4(location, vector);
        }

        public void Dispose()
        {
            GL.DeleteProgram(_handle);
        }
    }
}