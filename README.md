<blockquote>
So, in this Gensoukyo, common sense doesn't apply!
</blockquote>

# 📘Introduction
**GensouGL** is a game dev library from Gensoukyo, which mainly wraps OpenTK(OpenGL and OpenAL), and provides the simple but flexible interface to develop a 2D or 3D game using C#.

GensouGL supports some specific features helpful for Touhou-like STG development.

# 🔍Overview
**GensouGL includes:**
- *A Simple rendering interface. No OpenGL knowledge is required.*
- *Instanced drawing for enabling a lot of bullets*
- *Post-processing effects*
- *etc...*

# ⚙️Requirements

.NET 10.0+

## depending on
- OpenTK 4.9.4+
- NLayer 1.16.0+
- SixLabors.ImageSharp 3.1.12+
- SixLabors.ImageSharp.Drawing 2.1.7+
- AssimpNet 4.1.0+

# 🛠️Installation

1. Install via nuget

# 🚀Quick Start

### Creating an Empty Window

After installing GensouGL, change Program.cs as follows to get an empty game window.

```C#
Gensou.Create()
      .WithTitle("GensouGL Game")
      .WithWindowSize(640, 480)
      .WithScreenSize(640, 480)
      .WithFps(60.0)
      .WithInitialScene(Scene.Empty)
      .Run();
```

using directives:
```C#
using GensouGL.Core;
```

### Creating a scene

Inherit `Scene`, and override some virtual methods to create one.

```C#
class MyScene : Scene
{
    public override void OnEnter()
    {
        // Set background color to red when entering this scene.
        BackgroundColor = Color.Red;
    }

    public override void Update(float deltaTime)
    {
        // Update logic
    }

    public override void Render(float deltaTime)
    {
        // Rendering logic
    }
}
```

By passing the instance of `MyScene` to WithInitialScene(), you will get a red window.

### Rendering textures

All rendering processes should be done in `Scene.Render(float)`. The easiest way to render textures is to use `QuickRenderer2D`.
This renderer supports *quick* rendering, which means the renderer immediately flushes data and makes a draw call for each objects.

```C#
class MyScene : Scene
{
    private QuickRenderer2D _renderer;
    private Texture _texture;

    public override void OnInitialize()
    {
        // Initialization method, which is called when the scene is first registered to the scene manager.
        
        // Screen.Default: Default render target(game window buffer).
        _renderer = new(Screen.Default);
        // Specify the path to your image in the constructor parameter.
        _texture = new(@"\images\character1.png");

        // Register resource objects using this method to make them disposed of when at the end of the game.
        RegisterPersistentResource(_texture);
    }

    public override void Render(float deltaTime)
    {
        // Render the texture to fill the entire screen.
        _renderer.DrawTexture(ScreenBounds, _texture);
    }
}
```

### Rendering text

Coming soon...

### Scene Navigation

To navigate to other scenes, use `Scene.SceneNavigator`.

```C#
SceneNavigator.Navigate(MyScene2.Instance);
```