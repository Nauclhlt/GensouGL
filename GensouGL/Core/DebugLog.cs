namespace GensouGL.Core;

public static class DebugLog
{
    private static readonly StringBuilder _buffer;
    private static readonly StringWriter _writer;

    public static bool WriteToConsole { get; set; } = false;
    public static bool ColoredConsoleLog { get; set; } = true;

    static DebugLog()
    {
        _buffer = new();
        _writer = new(_buffer);
    }

    internal static void Info(string message)
    {
        if (WriteToConsole) ConsoleLog($"[INFO] {message}", ConsoleColor.White);
        _writer.Write($"[INFO] {message}");
    }

    internal static void Warn(string message)
    {
        if (WriteToConsole) ConsoleLog($"[WARN] {message}", ConsoleColor.Yellow);
        _writer.Write($"[WARN] {message}");
    }

    internal static void Error(string message)
    {
        if (WriteToConsole) ConsoleLog($"[ERROR] {message}", ConsoleColor.Red);
        _writer.Write($"[ERROR] {message}");
    }

    public static void User(string message)
    {
        if (WriteToConsole) ConsoleLog($"[USER] {message}", ConsoleColor.Cyan);
        _writer.Write($"[USER] {message}");
    }

    private static void ConsoleLog(string message, ConsoleColor color)
    {
        if (ColoredConsoleLog)
        {
            ConsoleColor cache = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = cache;
        }
        else
        {
            Console.WriteLine(message);
        }
    }

    public static void Clear()
    {
        _buffer.Clear();
    }

    public static void SaveToFile(string filename)
    {
        File.WriteAllText(filename, _buffer.ToString());
    }
}