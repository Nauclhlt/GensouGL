using OpenTK.Audio.OpenAL;

namespace GensouGL.Core;

/// <summary>
/// Handles the in-game overlay debug.
/// </summary>
public static class InGameDebug
{
    private static bool _enabled = false;
    private static bool _overlayShown = false;

    /// <summary>
    /// Gets or sets whether the in-game debug is enabled.
    /// </summary>
    public static bool Enabled => _enabled;

    private static Game _game;
    private static TextFont _font1;
    private static TextFont _font2;
    private static QuickRenderer2D _quickRenderer;
    private static BatchTextRenderer2D _batchTextRenderer;

    // INFO
    private static string _glVersion;
    private static string _glVendor;
    private static string _glRenderer;
    private static string _alVersion;

    private static readonly Color Background = Color.Black.WithAlpha(0.6f);
    private static readonly Color BackgroundAccent = Color.DarkTurquoise.WithAlpha(0.2f);
    private static readonly Color Foreground = Color.LightGray;
    private static readonly Color ForegroundAccent = Color.HotPink;

    /// <summary>
    /// Enables and sets up the in-game debug.
    /// </summary>
    public static void Enable()
    {
        _enabled = true;

        // set up
        Assembly asm = typeof(Game).Assembly;
        using Stream ffStream = asm.GetManifestResourceStream("GensouGL.Core.CoreDebug.JetBrainsMono-Light.ttf");

        _font1 = new TextFont(ffStream, 20f);
        _font2 = new TextFont(ffStream, 26f);

        _quickRenderer = new QuickRenderer2D(Screen.Default);
        _batchTextRenderer = new BatchTextRenderer2D(Screen.Default);


        // info
        _glVersion = GL.GetString(StringName.Version);
        _glVendor = GL.GetString(StringName.Vendor);
        _glRenderer = GL.GetString(StringName.Renderer);
        _alVersion = AL.Get(ALGetString.Version);
    }

    internal static void Initialize(Game game)
    {
        _game = game;
    }

    internal static void Release()
    {
        if (!_enabled) return;

        // release resources

        _font1.Dispose();
        _font2.Dispose();
    }

    internal static void Update(float deltaTime)
    {
        if (!_enabled) return;
    }

    internal static void RenderDebugOverlay(float deltaTime)
    {
        if (!_enabled) return;

        static void drawWindow(float x, float y, float w, float h)
        {
            _quickRenderer.DrawRect(x, y, w, h, Background);
            _quickRenderer.DrawWireRect(x, y, w, h, Foreground);
        }

        if (_overlayShown)
        {
            // info window
            {
                string[] list1 = [
                $"FPS: {MathF.Round(1.0f / deltaTime)}",
                $"OpenGL version: {_glVersion}",
                $"OpenGL vendor: {_glVendor}",
                $"OpenGL renderer: {_glRenderer}",
                $"OpenAL version: {_alVersion}",
                $"Memory Usage: {Math.Round(Environment.WorkingSet / 1024.0 / 1024.0, 2)} MB"
                ];
                float maxWidth = list1.Max(x => _font1.WidthOf(x));
                drawWindow(10f, 10f, maxWidth + 20f, 200f);
                _batchTextRenderer.Begin(_font1);
                for (int i = 0; i < list1.Length; i++)
                {
                    _batchTextRenderer.Draw(15f, 15f + 30f * i, list1[i], Foreground);
                }
                _batchTextRenderer.End();
            }



            // log window
            {
                static Color logColor(string msg)
                {
                    return msg[1] switch
                    {
                        'I' => Foreground,
                        'W' => Color.Yellow,
                        'E' => Color.OrangeRed,
                        'U' => Color.DarkTurquoise,
                        _ => Foreground
                    };
                }
                int count = int.Min(DebugLog.List.Count, 5);
                int logWidth = 0;
                for (int i = 0; i < count; i++)
                {
                    logWidth = int.Max(logWidth, _font1.WidthOf(DebugLog.List[DebugLog.List.Count - 1 - i]));
                }
                logWidth += 20;

                drawWindow(10f, 220f, logWidth, 200f);
                _quickRenderer.DrawRect(10f, 220f, logWidth, 30f, BackgroundAccent);
                _batchTextRenderer.Begin(_font1);
                _batchTextRenderer.DrawCentered(10f, 220f, logWidth, 30f, "Log", Foreground);
                for (int i = 0; i < count; i++)
                {
                    _batchTextRenderer.Draw(15f, 250f + 30f * i, DebugLog.List[DebugLog.List.Count - count + i], logColor(DebugLog.List[DebugLog.List.Count - count + i]));
                }
                _batchTextRenderer.End();
            }
            
        }
    }

    /// <summary>
    /// Toggles the visibility of the debug overlay.
    /// </summary>
    public static void ToggleOverlayVisibility()
    {
        if (!_enabled)
        {
            DebugLog.Error("InGameDebug: In-Game debug is not enabled.");
            return;
        }

        _overlayShown = !_overlayShown;
    }
}