using GensouGL.Core;
using GensouGL.GameDev;
using GensouGL.Graphics;
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
    private List<Bullet> _bullets = new();
    private Texture _image;
    private MemoryAudio _testAudio;
    private StreamingAudio _testStream;

    public override void OnInitialize()
    {
        DebugLog.User("TestScene initialized.");

        _br = CreateBatchBulletRenderer(Screen.Default);
        _qr = CreateQuickRenderer(Screen.Default);

        _image = new(@"C:\Users\PC_User\Downloads\red.png");
        _image.ResizeMode = TextureResizeMode.NearestNeighbor;
        RegisterPersistentResource(_image);

        //_testAudio = MemoryAudio.FromMp3(@"C:\Users\PC_User\Downloads\[MV] SECRET NUMBER(시크릿넘버)  Got That Boom.mp3");
        _testAudio = MemoryAudio.FromWave(@"C:\Users\PC_User\Downloads\nc434502_レーザー発射音.wav");
        _testAudio.Loop = true;
        RegisterPersistentResource(_testAudio);

        _testStream = StreamingAudio.FromMp3(@"C:\Users\PC_User\Downloads\[MV] SECRET NUMBER(시크릿넘버)  Got That Boom.mp3");
        //_testStream = StreamingAudio.FromWave(@"C:\Users\PC_User\Downloads\nc434502_レーザー発射音.wav");
        _testStream.Loop = true;
        _testStream.LoopStart = 10f;
        _testStream.LoopEnd = 20f;
        RegisterPersistentResource(_testStream);
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

        if (Input.IsKeyDown(Key.Enter))
        {
            float cx = 960f * Randomizer.NextFloat();
            float cy = 540f * Randomizer.NextFloat();

            GVector2 v = new GVector2(0, 220f);
            for (int i = 0; i < 12; i++)
            {
                _bullets.Add(new Bullet(new GVector2(cx, cy) - v.Normalize() * 80f, v));
                v = v.Rotate(MathF.PI * 2 / 12);
            }
        }

        if (Input.IsKeyPressed(Key.A))
        {
            _testStream.Play();
        }

        if (Input.IsKeyPressed(Key.B))
        {
            _testStream.Stop();
        }

        if (Input.IsKeyPressed(Key.C))
        {
            _testStream.Seek(10f);
        }
        
        if (Input.IsKeyPressed(Key.Escape))
        {
            ExitGame();
        }
    }

    public override void Render(float deltaTime)
    {
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
        br.Draw(_position.X, _position.Y, 12, 12, Color.White, texture);
    }
}