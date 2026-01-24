namespace GensouGL.Graphics;

internal interface IDrawCommand3D
{
    void Prepare();
    void Finish();
    void MakeDrawCall();
    Shader GetShader(LightingMode lightingMode);
    Shader GetDepthShader();
    Material Material { get; }
    Transform3D Transform { get; }
}