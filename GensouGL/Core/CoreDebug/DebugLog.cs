namespace GensouGL.Core;

public static class DebugLog
{
    private const int Capacity = 1024 * 1024;

    private static readonly StringBuilder _buffer;
    private static readonly StringWriter _writer;
    private static readonly List<string> _list;

    public static bool WriteToConsole { get; set; } = false;
    public static bool ColoredConsoleLog { get; set; } = true;

    internal static List<string> List => _list;

    static DebugLog()
    {
        _buffer = new();
        _writer = new(_buffer);
        _list = new();
    }

    private static void CheckState()
    {
        if(_buffer.Length > Capacity)
        {
            _buffer.Clear();
            _list.Clear();
        }
    }

    internal static void Info(string message)
    {
        if (WriteToConsole) ConsoleLog($"[INFO] {message}", ConsoleColor.White);
        _writer.Write($"[INFO] {message}");
        _list.Add($"[INFO] {message}");
    }

    internal static void Warn(string message)
    {
        if (WriteToConsole) ConsoleLog($"[WARN] {message}", ConsoleColor.Yellow);
        _writer.Write($"[WARN] {message}");
        _list.Add($"[WARN] {message}");
    }

    internal static void Error(string message)
    {
        if (WriteToConsole) ConsoleLog($"[ERROR] {message}", ConsoleColor.Red);
        _writer.Write($"[ERROR] {message}");
        _list.Add($"[ERROR] {message}");
    }

    public static void User(string message)
    {
        if (WriteToConsole) ConsoleLog($"[USER] {message}", ConsoleColor.Cyan);
        _writer.Write($"[USER] {message}");
        _list.Add($"[USER] {message}");
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