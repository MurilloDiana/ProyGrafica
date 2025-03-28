using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK3DLetterU
{
    public class LetraU : Objeto3D
    {
        public LetraU(Vector3 posicion, Vector3 centro, float ancho = 1.0f, float altura = 1.2f, float profundidad = 1.0f)
        {
            Posicion = posicion;
            Centro = centro;
            Vertices = GenerarVertices(ancho, altura, profundidad);
        }

        private float[] GenerarVertices(float ancho, float altura, float profundidad)
        {
            float grosor = 0.2f;
            float alturaBase = 0.2f;

            float izquierda = Centro.X - ancho / 2;
            float derecha = Centro.X + ancho / 2;
            float arriba = Centro.Y + altura / 2;
            float abajo = Centro.Y - altura / 2;
            float frente = Centro.Z + profundidad / 2;
            float atras = Centro.Z - profundidad / 2;

            float izquierdaInterna = izquierda + grosor;
            float derechaInterna = derecha - grosor;
            float baseSuperior = abajo + alturaBase;

            return new float[] {
                // Prisma izquierdo
                izquierda, arriba, frente,
                izquierda, abajo, frente,
                izquierdaInterna, abajo, frente,
                izquierdaInterna, arriba, frente,

                izquierda, arriba, atras,
                izquierda, abajo, atras,
                izquierdaInterna, abajo, atras,
                izquierdaInterna, arriba, atras,

                izquierda, arriba, frente,
                izquierda, arriba, atras,
                izquierda, abajo, frente,
                izquierda, abajo, atras,
                izquierdaInterna, abajo, frente,
                izquierdaInterna, abajo, atras,
                izquierdaInterna, arriba, frente,
                izquierdaInterna, arriba, atras,

                // Prisma derecho
                derechaInterna, arriba, frente,
                derechaInterna, abajo, frente,
                derecha, abajo, frente,
                derecha, arriba, frente,

                derechaInterna, arriba, atras,
                derechaInterna, abajo, atras,
                derecha, abajo, atras,
                derecha, arriba, atras,

                derechaInterna, arriba, frente,
                derechaInterna, arriba, atras,
                derechaInterna, abajo, frente,
                derechaInterna, abajo, atras,
                derecha, abajo, frente,
                derecha, abajo, atras,
                derecha, arriba, frente,
                derecha, arriba, atras,

                // Base de la U
                izquierdaInterna, abajo, frente,
                izquierdaInterna, baseSuperior, frente,
                derechaInterna, baseSuperior, frente,
                derechaInterna, abajo, frente,

                izquierdaInterna, abajo, atras,
                izquierdaInterna, baseSuperior, atras,
                derechaInterna, baseSuperior, atras,
                derechaInterna, abajo, atras,

                izquierdaInterna, abajo, frente,
                izquierdaInterna, abajo, atras,
                izquierdaInterna, baseSuperior, frente,
                izquierdaInterna, baseSuperior, atras,
                derechaInterna, baseSuperior, frente,
                derechaInterna, baseSuperior, atras,
                derechaInterna, abajo, frente,
                derechaInterna, abajo, atras
            };
        }

        protected override void DibujarWireframe(Shader shader)
        {
            shader.SetVector4("color", WireframeColor);

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
    }
}