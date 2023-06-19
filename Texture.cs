using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

// I'm not going to lie, all of this code feels pretty horrible and there's a much better
// way of doing it that I've already learnt through a separate tutorial. That being said,
// the setup of this better version takes a little longer, so I will humour this tutorial for now.

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

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            //bind handle
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);

            StbImage.stbi_set_flip_vertically_on_load(1);

        }
    }
}
