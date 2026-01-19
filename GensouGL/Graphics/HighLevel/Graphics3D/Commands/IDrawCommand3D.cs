namespace GensouGL.Graphics;

internal interface IDrawCommand3D
{
    void MakeDrawCall();
    Shader GetShader(LightingMode lightingMode);
    Material Material { get; }
}