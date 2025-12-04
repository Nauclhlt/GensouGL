using GensouGL.Core;
using GensouGL.GameDev;
using GensouGL.Graphics;
using GensouGL.Helpers;
using GensouGL.IO;

DebugLog.WriteToConsole = true;
Gensou.Create()
    .WithTitle("GensouGL Test")
    .WithWindowSize(640, 480)
    .WithScreenSize(640, 480)
    .WithFps(60.0)
    .WithInitialScene(new TestScene())
    .Run();

class TestScene : Scene
{
    private float _elapsed = 0.0f;
    private bool _flg = false;

    private Texture _testImage;

    private QuickRenderer _qr;

    public override void OnInitialize()
    {
        DebugLog.User("TestScene initialized.");

        _qr = CreateQuickRenderer(Screen.Default);

        _testImage = new Texture(@"C:\Users\PC_User\Downloads\t9PP3kCh_400x400.jpg");
        RegisterPersistentResource(_testImage);
    }

    public override void OnEnter()
    {
        DebugLog.User("Entered TestScene.");
    }

    public override void OnLeave()
    {
        DebugLog.User("Left TestScene.");
    }

    public override void Update(float deltaTime)
    {
        _elapsed += deltaTime;
    }

    public override void Render(float deltaTime)
    {
        //_qr.DrawWireRect(245, 165, 150, 150, Color.Aqua);
        //_qr.DrawLine(320, 240, 640, 480, Color.Lime);
        //_qr.DrawTriangle(0, 0, 0, 240, 320, 0, Color.Red);
        //_qr.DrawWireTriangle(320, 480, 640, 480, 640, 240, Color.Magenta);
        //_qr.DrawEllipse(320 - 50, 240 - 50, 100, 100, Color.White, div: 10);

        _qr.DrawTexture(0, 0, _testImage.Width, _testImage.Height, _testImage, Color.White);
    }
}