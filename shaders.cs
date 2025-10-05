using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

public class ShaderLoader
{
    public int ProgramId { get; private set; }

    public ShaderLoader(string vertexPath, string fragmentPath)
    {
        string vertexShaderSource = File.ReadAllText(vertexPath);
        string fragmentShaderSource = File.ReadAllText(fragmentPath);

        int vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
        int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

        ProgramId = GL.CreateProgram();

        GL.AttachShader(ProgramId, vertexShader);
        GL.AttachShader(ProgramId, fragmentShader);

        GL.LinkProgram(ProgramId);

        GL.GetProgram(ProgramId, GetProgramParameterName.LinkStatus, out int status);
        if (status == 0)
        {
            string infoLog = GL.GetProgramInfoLog(ProgramId);
            throw new Exception($"Program linking failed: {infoLog}");
        }

        GL.DetachShader(ProgramId, vertexShader);
        GL.DetachShader(ProgramId, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    private int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
        if (status == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"{type} compilation failed: {infoLog}");
        }

        return shader;
    }

    public void Use()
    {
        GL.UseProgram(ProgramId);
    }

    public void Delete()
    {
        GL.DeleteProgram(ProgramId);
    }
}
