using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace OpenTKTutorials
{
    public class Texture
    {
        public readonly int _handle;

        public static Texture LoadFromFile(string filePath)
        {
            int glhandle = GL.GenTexture();

            //bind handle
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, glhandle);

            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(filePath))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return new Texture(glhandle);
        }

        public Texture(int glHandle)
        {
            _handle = glHandle;
        }

        public void Use(TextureUnit unit)
        {
            //bind handle
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);

            StbImage.stbi_set_flip_vertically_on_load(1);


        }
    }
}
