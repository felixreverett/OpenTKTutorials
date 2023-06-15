#version 330 core
layout (location = 0) in vec3 aPosition;

//out vec4 vertexColor; //this tells the fragment shader a colour output

void main()
{
    gl_Position = vec4(aPosition, 1.0);
	//vertexColor = vec4(0.4, 0.0, 0.0, 1.0);
}