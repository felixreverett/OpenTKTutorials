#version 330
out vec4 FragColor;

//in vec4 vertexColor; //the input from the vertex shader
uniform vec4 ourColor;

void main()
{
    FragColor = ourColor;
}