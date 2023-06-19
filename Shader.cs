using OpenTK.Graphics.OpenGL4;

namespace OpenTKTutorials
{
    public class Shader : IDisposable
    {
        public readonly int Handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            //handles to the individual shaders
            int VertexShader;
            int FragmentShader;

            //loading the source shader code from files:
            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);

            //generate shaders and bind source shader code to them:
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            //compile shaders and check for errors:
            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);

            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            //link our shaders into a program runable on the GPU
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);

            }

            //detach the individual vertex and fragment shaders
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform1(location, value);
        }

    }
}
