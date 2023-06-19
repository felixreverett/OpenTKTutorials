using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace OpenTKTutorials
{
    public class Game : GameWindow
    {
        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        
        private Shader _shader;
        private Texture _texture;
        private Texture _texture2;

        private readonly float[] _vertices =
        {
            //Position          Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices = {  // note that we start from 0!
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
        };

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        //This function runs ONCE on load. Any initialisation-related code goes here
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _shader = new Shader("../../../Resources/Shaders/textureShader.vert", "../../../Resources/Shaders/textureShader.frag");
            _shader.Use();

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexLocation);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            //generate element buffer object
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _texture = Texture.LoadFromFile(@"../../../Resources/Textures/container.jpg");
            _texture.Use(TextureUnit.Texture0);

            _texture2 = Texture.LoadFromFile(@"../../../Resources/Textures/awesomeface.png");

            _shader.SetInt("texture1", 0);
            _shader.SetInt("texture2", 1);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindVertexArray(_vertexArrayObject);

            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);
            _shader.Use();

            //draw the elements (two triangles to make a rectangle)
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        //Runs every time the window gets resized
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        //runs when unloading
        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            _shader.Dispose();
            base.OnUnload();
        }

    }
}
