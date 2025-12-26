using GensouGL.Core;
using GensouGL.GameDev;
using GensouGL.Graphics;
using GensouGL.Graphics.Particles;
using GensouGL.Helpers;
using GensouGL.IO;

DebugLog.WriteToConsole = true;
Gensou.Create()
    .WithTitle("GensouGL Test")
    .WithWindowSize(960, 540)
    .WithScreenSize(960, 540)
    .WithFps(120.0)
    .WithInitialScene(new TestScene())
    .Run();

class TestScene : Scene
{
    private float _elapsed = 0.0f;

    private BatchBulletRenderer _br;
    private QuickRenderer _qr;
    private BatchTextRenderer _tr;
    private Texture _backgroundImage;
    private List<Bullet> _bullets = new();
    private Texture _image;
    private BloomEffect _bloom;
    private TextFont _font;

    public override void OnInitialize()
    {
        DebugLog.User("TestScene initialized.");
        
        _br = CreateBatchBulletRenderer(Screen.Default);
        _qr = CreateQuickRenderer(Screen.Default);
        
        _image = new(@"C:\Users\PC_User\Downloads\red.png");
        _image.ResizeMode = TextureResizeMode.NearestNeighbor;
        RegisterPersistentResource(_image);
        
        _backgroundImage = new(@"C:\Users\PC_User\Downloads\20231104200836.png");
        RegisterPersistentResource(_backgroundImage);

        _bloom = new(0.4f, 1.0f, 0.3f, 3.0f);

        //FontSource src = new FontSource(@"C:\Users\PC_User\Downloads\makinas4\makinas4\Makinas-4-Flat.otf");
        //_font = new(@"C:\Users\PC_User\Downloads\makinas4\makinas4\Makinas-4-Flat.otf", 70, FontSettings.Regular.Outlined(2f));
        _font = new(@"C:\Users\PC_User\Downloads\MS-Gothic\MSGOTHIC.TTF", 46, FontSettings.Regular.Outlined(2f));
        _tr = CreateBatchTextRenderer(Screen.Default);

        InGameDebug.Enable();
    }

    public override void OnEnter()
    {
        DebugLog.User("Entered TestScene.");
        BackgroundColor = Color.SkyBlue;
    }

    public override void OnLeave()
    {
        DebugLog.User("Left TestScene.");
    }

    public override void Update(float deltaTime)
    {
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            if (_bullets[i].KillFlag)
            {
                _bullets[i] = _bullets[^1];
                _bullets.RemoveAt(_bullets.Count - 1);
            }
            else
            {
                _bullets[i].Update(deltaTime);
            }
        }

        if (Input.IsKeyDown(Key.Enter))
        {
            float cx = 960f * Randomizer.NextFloat();
            float cy = 540f * Randomizer.NextFloat();

            GVector2 v = new GVector2(0, 220f);
            for (int i = 0; i < 12; i++)
            {
                _bullets.Add(new Bullet(new GVector2(cx, cy) + v.Normalize() * 40f, v.Rotate(GensouMathF.Rad(90))));
                v = v.Rotate(MathF.PI * 2 / 12);
            }
        }
        
        if (Input.IsKeyPressed(Key.Escape))
        {
            ExitGame();
        }

        

        if (Input.IsKeyPressed(Key.D))
        {
            InGameDebug.ShowOrHideOverlay();
        }

        _elapsed += deltaTime;
    }

    public override void Render(float deltaTime)
    {
        //_qr.DrawTexture(ScreenBounds, _backgroundImage);
        
        _br.Begin(_image);
        for (int i = 0; i < _bullets.Count; i++)
        {
            _bullets[i].Render(_br, _image);
        }
        _br.End();
    }
}

public sealed class Bullet
{
    public bool KillFlag { get; set; } = false;

    private GVector2 _position;
    private GVector2 _velocity;

    public Bullet(GVector2 pos,  GVector2 velocity)
    {
        _position = pos;
        _velocity= velocity;
    }

    public void Update(float deltaTime)
    {
        if (_position.X < -50 || _position.Y < -50 || _position.X >= 1000 || _position.Y >= 600)
        {
            KillFlag = true;
        }

        _position += _velocity * deltaTime;
    }

    public void Render(BatchBulletRenderer br, Texture texture)
    {
        br.Draw(_position.X, _position.Y, 12, 12, 0f, Color.White, texture);
    }
}

public sealed class TestOverlay : Overlay
{
    private QuickRenderer _qr;

    public TestOverlay(QuickRenderer qr)
    {
        _qr = qr;
    }

    public override void FocusUpdate(float deltaTime)
    {
        if (Input.IsKeyPressed(Key.C))
        {
            Close();
        }
    }

    public override void Render(float deltaTime)
    {
        _qr.DrawRect(ScreenBounds.Center(400, 400), Color.DarkSlateBlue.WithAlpha(140));
        _qr.DrawWireRect(ScreenBounds.Center(400, 400), Color.White);

    }
}