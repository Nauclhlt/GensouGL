namespace GensouGL.Core;

/// <summary>
/// Represents a RGBA color.
/// </summary>
public struct Color
{
    /// <summary>
    /// R(Red) component.
    /// </summary>
    public float R;
    /// <summary>
    /// G(Green) component.
    /// </summary>
    public float G;
    /// <summary>
    /// B(Blue) component.
    /// </summary>
    public float B;
    /// <summary>
    /// A(Alpha) component.
    /// </summary>
    public float A;

    /// <summary>
    /// Gets the R component as byte.
    /// </summary>
    public readonly byte Rb => (byte)(R * 255);
    /// <summary>
    /// Gets the G component as byte.
    /// </summary>
    public readonly byte Gb => (byte)(G * 255);
    /// <summary>
    /// Gets the B component as byte.
    /// </summary>
    public readonly byte Bb => (byte)(B * 255);
    /// <summary>
    /// Gets the A component as byte.
    /// </summary>
    public readonly byte Ab => (byte)(A * 255);

    /// <summary>
    /// Initializes <see cref="Color"/> using the specified RGB values. Alpha component will be 1(255 in byte).
    /// </summary>
    /// <param name="r">R component in byte.</param>
    /// <param name="g">G component in byte.</param>
    /// <param name="b">B component in byte.</param>
    public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

    /// <summary>
    /// Initializes <see cref="Color"/> using the specified RGBA values.
    /// </summary>
    /// <param name="r">R component in byte.</param>
    /// <param name="g">G component in byte.</param>
    /// <param name="b">B component in byte.</param>
    /// <param name="a">A component in byte.</param>
    public Color(byte r, byte g, byte b, byte a)
    {
        R = r / 255f;
        G = g / 255f;
        B = b / 255f;
        A = a / 255f;
    }

    /// <summary>
    /// Initializes <see cref="Color"/> using the specified RGBA values.
    /// </summary>
    /// <param name="r">R component.</param>
    /// <param name="g">G component.</param>
    /// <param name="b">B component.</param>
    /// <param name="a">A component.</param>
    public Color(float r, float g, float b, float a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    /// <summary>
    /// Returns a <see cref="Color"/> with the current RGB values, and the specified Alpha value.
    /// </summary>
    /// <param name="a">New alpha.</param>
    /// <returns>Color with the new alpha value.</returns>
    public readonly Color WithAlpha(float a)
    {
        return new Color(R, G, B, a);
    }

    /// <summary>
    /// Returns a color representing the dot product of the current color and the specified color.
    /// </summary>
    /// <param name="color">Target color.</param>
    /// <returns>Color representing the dot product.</returns>
    public readonly Color Dot(Color color)
    {
        return new(R * color.R, G * color.G, B * color.B, A * color.A);
    }

    /// <summary>
    /// Returns the size of the vector (R, G, B).
    /// </summary>
    /// <returns>|(R, G, B)|</returns>
    public readonly float Size()
    {
        return MathF.Sqrt(R * R + G * G + B * B);
    }

    /// <summary>
    /// Initializes <see cref="Color"/> from the specified hex integer. Each component uses 8 bits(1 byte).
    /// </summary>
    /// <param name="hex">Source integer.</param>
    /// <returns>Constructed Color.</returns>
    public static Color FromHex(uint hex)
    {
        byte r = (byte)((hex >> 16) & 0xFF);
        byte g = (byte)((hex >> 8) & 0xFF);
        byte b = (byte)(hex & 0xFF);
        return new Color(r, g, b);
    }

    internal readonly OpenTK.Mathematics.Color4 ToOpenTKColor4()
    {
        return new OpenTK.Mathematics.Color4(R, G, B, A);
    }

    #region Default Colors

    ///<summary>
    /// Transparent color.
    ///</summary>
    public static readonly Color Transparent = new Color(0f, 0f, 0f, 0f);
    ///<summary>
    /// AliceBlue color.
    ///</summary>
    public static readonly Color AliceBlue = new Color(240, 248, 255);
    ///<summary>
    /// AntiqueWhite color.
    ///</summary>
    public static readonly Color AntiqueWhite = new Color(250, 235, 215);
    ///<summary>
    /// Aqua color.
    ///</summary>
    public static readonly Color Aqua = new Color(0, 255, 255);
    ///<summary>
    /// Aquamarine color.
    ///</summary>
    public static readonly Color Aquamarine = new Color(127, 255, 212);
    ///<summary>
    /// Azure color.
    ///</summary>
    public static readonly Color Azure = new Color(240, 255, 255);
    ///<summary>
    /// Beige color.
    ///</summary>
    public static readonly Color Beige = new Color(245, 245, 220);
    ///<summary>
    /// Bisque color.
    ///</summary>
    public static readonly Color Bisque = new Color(255, 228, 196);
    ///<summary>
    /// Black color.
    ///</summary>
    public static readonly Color Black = new Color(0, 0, 0);
    ///<summary>
    /// BlanchedAlmond color.
    ///</summary>
    public static readonly Color BlanchedAlmond = new Color(255, 235, 205);
    ///<summary>
    /// Blue color.
    ///</summary>
    public static readonly Color Blue = new Color(0, 0, 255);
    ///<summary>
    /// BlueViolet color.
    ///</summary>
    public static readonly Color BlueViolet = new Color(138, 43, 226);
    ///<summary>
    /// Brown color.
    ///</summary>
    public static readonly Color Brown = new Color(165, 42, 42);
    ///<summary>
    /// BurlyWood color.
    ///</summary>
    public static readonly Color BurlyWood = new Color(222, 184, 135);
    ///<summary>
    /// CadetBlue color.
    ///</summary>
    public static readonly Color CadetBlue = new Color(95, 158, 160);
    ///<summary>
    /// Chartreuse color.
    ///</summary>
    public static readonly Color Chartreuse = new Color(127, 255, 0);
    ///<summary>
    /// Chocolate color.
    ///</summary>
    public static readonly Color Chocolate = new Color(210, 105, 30);
    ///<summary>
    /// Coral color.
    ///</summary>
    public static readonly Color Coral = new Color(255, 127, 80);
    ///<summary>
    /// CornflowerBlue color.
    ///</summary>
    public static readonly Color CornflowerBlue = new Color(100, 149, 237);
    ///<summary>
    /// Cornsilk color.
    ///</summary>
    public static readonly Color Cornsilk = new Color(255, 248, 220);
    ///<summary>
    /// Crimson color.
    ///</summary>
    public static readonly Color Crimson = new Color(220, 20, 60);
    ///<summary>
    /// Cyan color.
    ///</summary>
    public static readonly Color Cyan = new Color(0, 255, 255);
    ///<summary>
    /// DarkBlue color.
    ///</summary>
    public static readonly Color DarkBlue = new Color(0, 0, 139);
    ///<summary>
    /// DarkCyan color.
    ///</summary>
    public static readonly Color DarkCyan = new Color(0, 139, 139);
    ///<summary>
    /// DarkGoldenrod color.
    ///</summary>
    public static readonly Color DarkGoldenrod = new Color(184, 134, 11);
    ///<summary>
    /// DarkGray color.
    ///</summary>
    public static readonly Color DarkGray = new Color(169, 169, 169);
    ///<summary>
    /// DarkGreen color.
    ///</summary>
    public static readonly Color DarkGreen = new Color(0, 100, 0);
    ///<summary>
    /// DarkKhaki color.
    ///</summary>
    public static readonly Color DarkKhaki = new Color(189, 183, 107);
    ///<summary>
    /// DarkMagenta color.
    ///</summary>
    public static readonly Color DarkMagenta = new Color(139, 0, 139);
    ///<summary>
    /// DarkOliveGreen color.
    ///</summary>
    public static readonly Color DarkOliveGreen = new Color(85, 107, 47);
    ///<summary>
    /// DarkOrange color.
    ///</summary>
    public static readonly Color DarkOrange = new Color(255, 140, 0);
    ///<summary>
    /// DarkOrchid color.
    ///</summary>
    public static readonly Color DarkOrchid = new Color(153, 50, 204);
    ///<summary>
    /// DarkRed color.
    ///</summary>
    public static readonly Color DarkRed = new Color(139, 0, 0);
    ///<summary>
    /// DarkSalmon color.
    ///</summary>
    public static readonly Color DarkSalmon = new Color(233, 150, 122);
    ///<summary>
    /// DarkSeaGreen color.
    ///</summary>
    public static readonly Color DarkSeaGreen = new Color(143, 188, 143);
    ///<summary>
    /// DarkSlateBlue color.
    ///</summary>
    public static readonly Color DarkSlateBlue = new Color(72, 61, 139);
    ///<summary>
    /// DarkSlateGray color.
    ///</summary>
    public static readonly Color DarkSlateGray = new Color(47, 79, 79);
    ///<summary>
    /// DarkTurquoise color.
    ///</summary>
    public static readonly Color DarkTurquoise = new Color(0, 206, 209);
    ///<summary>
    /// DarkViolet color.
    ///</summary>
    public static readonly Color DarkViolet = new Color(148, 0, 211);
    ///<summary>
    /// DeepPink color.
    ///</summary>
    public static readonly Color DeepPink = new Color(255, 20, 147);
    ///<summary>
    /// DeepSkyBlue color.
    ///</summary>
    public static readonly Color DeepSkyBlue = new Color(0, 191, 255);
    ///<summary>
    /// DimGray color.
    ///</summary>
    public static readonly Color DimGray = new Color(105, 105, 105);
    ///<summary>
    /// DodgerBlue color.
    ///</summary>
    public static readonly Color DodgerBlue = new Color(30, 144, 255);
    ///<summary>
    /// Firebrick color.
    ///</summary>
    public static readonly Color Firebrick = new Color(178, 34, 34);
    ///<summary>
    /// FloralWhite color.
    ///</summary>
    public static readonly Color FloralWhite = new Color(255, 250, 240);
    ///<summary>
    /// ForestGreen color.
    ///</summary>
    public static readonly Color ForestGreen = new Color(34, 139, 34);
    ///<summary>
    /// Fuchsia color.
    ///</summary>
    public static readonly Color Fuchsia = new Color(255, 0, 255);
    ///<summary>
    /// Gainsboro color.
    ///</summary>
    public static readonly Color Gainsboro = new Color(220, 220, 220);
    ///<summary>
    /// GhostWhite color.
    ///</summary>
    public static readonly Color GhostWhite = new Color(248, 248, 255);
    ///<summary>
    /// Gold color.
    ///</summary>
    public static readonly Color Gold = new Color(255, 215, 0);
    ///<summary>
    /// Goldenrod color.
    ///</summary>
    public static readonly Color Goldenrod = new Color(218, 165, 32);
    ///<summary>
    /// Gray color.
    ///</summary>
    public static readonly Color Gray = new Color(128, 128, 128);
    ///<summary>
    /// Green color.
    ///</summary>
    public static readonly Color Green = new Color(0, 128, 0);
    ///<summary>
    /// GreenYellow color.
    ///</summary>
    public static readonly Color GreenYellow = new Color(173, 255, 47);
    ///<summary>
    /// Honeydew color.
    ///</summary>
    public static readonly Color Honeydew = new Color(240, 255, 240);
    ///<summary>
    /// HotPink color.
    ///</summary>
    public static readonly Color HotPink = new Color(255, 105, 180);
    ///<summary>
    /// IndianRed color.
    ///</summary>
    public static readonly Color IndianRed = new Color(205, 92, 92);
    ///<summary>
    /// Indigo color.
    ///</summary>
    public static readonly Color Indigo = new Color(75, 0, 130);
    ///<summary>
    /// Ivory color.
    ///</summary>
    public static readonly Color Ivory = new Color(255, 255, 240);
    ///<summary>
    /// Khaki color.
    ///</summary>
    public static readonly Color Khaki = new Color(240, 230, 140);
    ///<summary>
    /// Lavender color.
    ///</summary>
    public static readonly Color Lavender = new Color(230, 230, 250);
    ///<summary>
    /// LavenderBlush color.
    ///</summary>
    public static readonly Color LavenderBlush = new Color(255, 240, 245);
    ///<summary>
    /// LawnGreen color.
    ///</summary>
    public static readonly Color LawnGreen = new Color(124, 252, 0);
    ///<summary>
    /// LemonChiffon color.
    ///</summary>
    public static readonly Color LemonChiffon = new Color(255, 250, 205);
    ///<summary>
    /// LightBlue color.
    ///</summary>
    public static readonly Color LightBlue = new Color(173, 216, 230);
    ///<summary>
    /// LightCoral color.
    ///</summary>
    public static readonly Color LightCoral = new Color(240, 128, 128);
    ///<summary>
    /// LightCyan color.
    ///</summary>
    public static readonly Color LightCyan = new Color(224, 255, 255);
    ///<summary>
    /// LightGoldenrodYellow color.
    ///</summary>
    public static readonly Color LightGoldenrodYellow = new Color(250, 250, 210);
    ///<summary>
    /// LightGreen color.
    ///</summary>
    public static readonly Color LightGreen = new Color(144, 238, 144);
    ///<summary>
    /// LightGray color.
    ///</summary>
    public static readonly Color LightGray = new Color(211, 211, 211);
    ///<summary>
    /// LightPink color.
    ///</summary>
    public static readonly Color LightPink = new Color(255, 182, 193);
    ///<summary>
    /// LightSalmon color.
    ///</summary>
    public static readonly Color LightSalmon = new Color(255, 160, 122);
    ///<summary>
    /// LightSeaGreen color.
    ///</summary>
    public static readonly Color LightSeaGreen = new Color(32, 178, 170);
    ///<summary>
    /// LightSkyBlue color.
    ///</summary>
    public static readonly Color LightSkyBlue = new Color(135, 206, 250);
    ///<summary>
    /// LightSlateGray color.
    ///</summary>
    public static readonly Color LightSlateGray = new Color(119, 136, 153);
    ///<summary>
    /// LightSteelBlue color.
    ///</summary>
    public static readonly Color LightSteelBlue = new Color(176, 196, 222);
    ///<summary>
    /// LightYellow color.
    ///</summary>
    public static readonly Color LightYellow = new Color(255, 255, 224);
    ///<summary>
    /// Lime color.
    ///</summary>
    public static readonly Color Lime = new Color(0, 255, 0);
    ///<summary>
    /// LimeGreen color.
    ///</summary>
    public static readonly Color LimeGreen = new Color(50, 205, 50);
    ///<summary>
    /// Linen color.
    ///</summary>
    public static readonly Color Linen = new Color(250, 240, 230);
    ///<summary>
    /// Magenta color.
    ///</summary>
    public static readonly Color Magenta = new Color(255, 0, 255);
    ///<summary>
    /// Maroon color.
    ///</summary>
    public static readonly Color Maroon = new Color(128, 0, 0);
    ///<summary>
    /// MediumAquamarine color.
    ///</summary>
    public static readonly Color MediumAquamarine = new Color(102, 205, 170);
    ///<summary>
    /// MediumBlue color.
    ///</summary>
    public static readonly Color MediumBlue = new Color(0, 0, 205);
    ///<summary>
    /// MediumOrchid color.
    ///</summary>
    public static readonly Color MediumOrchid = new Color(186, 85, 211);
    ///<summary>
    /// MediumPurple color.
    ///</summary>
    public static readonly Color MediumPurple = new Color(147, 112, 219);
    ///<summary>
    /// MediumSeaGreen color.
    ///</summary>
    public static readonly Color MediumSeaGreen = new Color(60, 179, 113);
    ///<summary>
    /// MediumSlateBlue color.
    ///</summary>
    public static readonly Color MediumSlateBlue = new Color(123, 104, 238);
    ///<summary>
    /// MediumSpringGreen color.
    ///</summary>
    public static readonly Color MediumSpringGreen = new Color(0, 250, 154);
    ///<summary>
    /// MediumTurquoise color.
    ///</summary>
    public static readonly Color MediumTurquoise = new Color(72, 209, 204);
    ///<summary>
    /// MediumVioletRed color.
    ///</summary>
    public static readonly Color MediumVioletRed = new Color(199, 21, 133);
    ///<summary>
    /// MidnightBlue color.
    ///</summary>
    public static readonly Color MidnightBlue = new Color(25, 25, 112);
    ///<summary>
    /// MintCream color.
    ///</summary>
    public static readonly Color MintCream = new Color(245, 255, 250);
    ///<summary>
    /// MistyRose color.
    ///</summary>
    public static readonly Color MistyRose = new Color(255, 228, 225);
    ///<summary>
    /// Moccasin color.
    ///</summary>
    public static readonly Color Moccasin = new Color(255, 228, 181);
    ///<summary>
    /// NavajoWhite color.
    ///</summary>
    public static readonly Color NavajoWhite = new Color(255, 222, 173);
    ///<summary>
    /// Navy color.
    ///</summary>
    public static readonly Color Navy = new Color(0, 0, 128);
    ///<summary>
    /// OldLace color.
    ///</summary>
    public static readonly Color OldLace = new Color(253, 245, 230);
    ///<summary>
    /// Olive color.
    ///</summary>
    public static readonly Color Olive = new Color(128, 128, 0);
    ///<summary>
    /// OliveDrab color.
    ///</summary>
    public static readonly Color OliveDrab = new Color(107, 142, 35);
    ///<summary>
    /// Orange color.
    ///</summary>
    public static readonly Color Orange = new Color(255, 165, 0);
    ///<summary>
    /// OrangeRed color.
    ///</summary>
    public static readonly Color OrangeRed = new Color(255, 69, 0);
    ///<summary>
    /// Orchid color.
    ///</summary>
    public static readonly Color Orchid = new Color(218, 112, 214);
    ///<summary>
    /// PaleGoldenrod color.
    ///</summary>
    public static readonly Color PaleGoldenrod = new Color(238, 232, 170);
    ///<summary>
    /// PaleGreen color.
    ///</summary>
    public static readonly Color PaleGreen = new Color(152, 251, 152);
    ///<summary>
    /// PaleTurquoise color.
    ///</summary>
    public static readonly Color PaleTurquoise = new Color(175, 238, 238);
    ///<summary>
    /// PaleVioletRed color.
    ///</summary>
    public static readonly Color PaleVioletRed = new Color(219, 112, 147);
    ///<summary>
    /// PapayaWhip color.
    ///</summary>
    public static readonly Color PapayaWhip = new Color(255, 239, 213);
    ///<summary>
    /// PeachPuff color.
    ///</summary>
    public static readonly Color PeachPuff = new Color(255, 218, 185);
    ///<summary>
    /// Peru color.
    ///</summary>
    public static readonly Color Peru = new Color(205, 133, 63);
    ///<summary>
    /// Pink color.
    ///</summary>
    public static readonly Color Pink = new Color(255, 192, 203);
    ///<summary>
    /// Plum color.
    ///</summary>
    public static readonly Color Plum = new Color(221, 160, 221);
    ///<summary>
    /// PowderBlue color.
    ///</summary>
    public static readonly Color PowderBlue = new Color(176, 224, 230);
    ///<summary>
    /// Purple color.
    ///</summary>
    public static readonly Color Purple = new Color(128, 0, 128);
    ///<summary>
    /// RebeccaPurple color.
    ///</summary>
    public static readonly Color RebeccaPurple = new Color(102, 51, 153);
    ///<summary>
    /// Red color.
    ///</summary>
    public static readonly Color Red = new Color(255, 0, 0);
    ///<summary>
    /// RosyBrown color.
    ///</summary>
    public static readonly Color RosyBrown = new Color(188, 143, 143);
    ///<summary>
    /// RoyalBlue color.
    ///</summary>
    public static readonly Color RoyalBlue = new Color(65, 105, 225);
    ///<summary>
    /// SaddleBrown color.
    ///</summary>
    public static readonly Color SaddleBrown = new Color(139, 69, 19);
    ///<summary>
    /// Salmon color.
    ///</summary>
    public static readonly Color Salmon = new Color(250, 128, 114);
    ///<summary>
    /// SandyBrown color.
    ///</summary>
    public static readonly Color SandyBrown = new Color(244, 164, 96);
    ///<summary>
    /// SeaGreen color.
    ///</summary>
    public static readonly Color SeaGreen = new Color(46, 139, 87);
    ///<summary>
    /// SeaShell color.
    ///</summary>
    public static readonly Color SeaShell = new Color(255, 245, 238);
    ///<summary>
    /// Sienna color.
    ///</summary>
    public static readonly Color Sienna = new Color(160, 82, 45);
    ///<summary>
    /// Silver color.
    ///</summary>
    public static readonly Color Silver = new Color(192, 192, 192);
    ///<summary>
    /// SkyBlue color.
    ///</summary>
    public static readonly Color SkyBlue = new Color(135, 206, 235);
    ///<summary>
    /// SlateBlue color.
    ///</summary>
    public static readonly Color SlateBlue = new Color(106, 90, 205);
    ///<summary>
    /// SlateGray color.
    ///</summary>
    public static readonly Color SlateGray = new Color(112, 128, 144);
    ///<summary>
    /// Snow color.
    ///</summary>
    public static readonly Color Snow = new Color(255, 250, 250);
    ///<summary>
    /// SpringGreen color.
    ///</summary>
    public static readonly Color SpringGreen = new Color(0, 255, 127);
    ///<summary>
    /// SteelBlue color.
    ///</summary>
    public static readonly Color SteelBlue = new Color(70, 130, 180);
    ///<summary>
    /// Tan color.
    ///</summary>
    public static readonly Color Tan = new Color(210, 180, 140);
    ///<summary>
    /// Teal color.
    ///</summary>
    public static readonly Color Teal = new Color(0, 128, 128);
    ///<summary>
    /// Thistle color.
    ///</summary>
    public static readonly Color Thistle = new Color(216, 191, 216);
    ///<summary>
    /// Tomato color.
    ///</summary>
    public static readonly Color Tomato = new Color(255, 99, 71);
    ///<summary>
    /// Turquoise color.
    ///</summary>
    public static readonly Color Turquoise = new Color(64, 224, 208);
    ///<summary>
    /// Violet color.
    ///</summary>
    public static readonly Color Violet = new Color(238, 130, 238);
    ///<summary>
    /// Wheat color.
    ///</summary>
    public static readonly Color Wheat = new Color(245, 222, 179);
    ///<summary>
    /// White color.
    ///</summary>
    public static readonly Color White = new Color(255, 255, 255);
    ///<summary>
    /// WhiteSmoke color.
    ///</summary>
    public static readonly Color WhiteSmoke = new Color(245, 245, 245);
    ///<summary>
    /// Yellow color.
    ///</summary>
    public static readonly Color Yellow = new Color(255, 255, 0);
    ///<summary>
    /// YellowGreen color.
    ///</summary>
    public static readonly Color YellowGreen = new Color(154, 205, 50);

    #endregion
}