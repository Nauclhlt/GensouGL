<blockquote>
So, in this Gensoukyo, you can't be bound by common sense!
</blockquote>

# 📘Introduction
**GensouGL** is a game dev library from Gensoukyo, which mainly wraps OpenTK(OpenGL and OpenAL), and provides the simple but flexible interface to develop a 2D game using C#.

---

GensouGL supports some specific features helpful for Touhou-like STG development.

---

# 🔍Overview
**GensouGL includes:**
- *A Simple rendering interface that require no OpenGL knowledge*
- *Instanced drawing for enabling a lot of bullets*
- *Beautiful post-processing effects*
- *etc...*

# ⚙️Requirements

.NET 10.0+

## depending on
- OpenTK 4.9.4
- NLayer 1.16.0
- SixLabors.ImageSharp 3.1.12
- SixLabors.ImageSharp.Drawing 2.1.7

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

Coming soon...