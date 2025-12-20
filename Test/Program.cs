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
    private Texture _backgroundImage;
    private List<Bullet> _bullets = new();
    private Texture _image;
    private Texture _particleImage;
    private BloomEffect _bloom;
    private TextInputListener _listener;
    private StreamingAudio _streamingAudio;
    private ParticleSystem _particleSystem;

    public override void OnInitialize()
    {
        DebugLog.User("TestScene initialized.");
        
        _br = CreateBatchBulletRenderer(Screen.Default);
        _qr = CreateQuickRenderer(Screen.Default);

        _image = new(@"C:\Users\PC_User\Downloads\red.png");
        _image.ResizeMode = TextureResizeMode.NearestNeighbor;
        RegisterPersistentResource(_image);

        _particleImage = new(@"C:\Users\PC_User\Downloads\testpart.png");
        RegisterPersistentResource(_particleImage);

        _backgroundImage = new(@"C:\Users\PC_User\OneDrive\画像\Screenshots\スクリーンショット 2025-03-30 142858.png");
        RegisterPersistentResource(_backgroundImage);

        _bloom = new(0.4f, 1.0f, 0.3f, 3.0f);

        _listener = Input.CreateTextInputListener();
        
        InGameDebug.Enable();

        

        _particleSystem = new(_br, _particleImage, BlendMode.Add);


        _streamingAudio = StreamingAudio.FromWave(@"C:\Users\PC_User\Downloads\nc434502_レーザー発射音.wav");
        RegisterPersistentResource(_streamingAudio);
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

        _particleSystem.Update(deltaTime);

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

        if (Input.IsKeyPressed(Key.A))
        {
            _streamingAudio.Play();
        }

        if (Input.IsKeyPressed(Key.B))
        {
            _streamingAudio.Stop();
        }

        if (Input.IsKeyPressed(Key.D))
        {
            InGameDebug.ShowOrHideOverlay();
        }

        if (Input.IsKeyPressed(Key.L))
        {
            RandomCircularEmitter emitter = new(
                new ParticleSettings()
                {
                    Speed = (300f, 350f),
                    Accel = (15f, 30f),
                    Rotation = (0, MathF.PI),
                    Life = (0.2f, 0.4f),
                    Size = (16, 20f),
                    ShootAngle = (0f, MathF.PI * 2),
                    StartColor = (Color.White, Color.White),
                    EndColor = (Color.White.WithAlpha(0), Color.White.WithAlpha(0)),
                    Texture = _particleImage
                }, ScreenCenter, 1f, 2, 32);
            _particleSystem.Emit(emitter);
        }

        _elapsed += deltaTime;

        if (_listener.Pop(out string content))
        {
            DebugLog.User(content);
        }
    }

    public override void Render(float deltaTime)
    {
        _qr.DrawTexture(ScreenBounds, _backgroundImage);

        using (_br.BeginPostProc(_bloom))
        {
            _br.Begin(_image);
            for (int i = 0; i < _bullets.Count; i++)
            {
                _bullets[i].Render(_br, _image);
            }
            _br.End();
        }

        Point mp = Input.GetMousePosition();

        _particleSystem.Render();
        //_br.Begin(_image);
        //_br.Draw(mp.X, mp.Y, 128, 128, 0f, Color.Yellow, _image);
        //_br.End();
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