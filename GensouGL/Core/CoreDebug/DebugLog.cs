namespace GensouGL.Core;

/// <summary>
/// Handles the debug logs.
/// </summary>
/// <remarks>
/// To print something in the debug log, use User(string).
/// </remarks>
public static class DebugLog
{
    private const int Capacity = 1024 * 1024;

    private static readonly StringBuilder _buffer;
    private static readonly StringWriter _writer;
    private static readonly List<string> _list;

    /// <summary>
    /// Gets or sets whether the debug log is enabled.
    /// </summary>
    public static bool Enabled { get; set; } = true;
    /// <summary>
    /// Gets or sets whether the logs should be also written to the console.
    /// </summary>
    public static bool WriteToConsole { get; set; } = false;
    /// <summary>
    /// Gets or sets whether the console logs are colored.
    /// </summary>
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
        CheckState();
        if (WriteToConsole) ConsoleLog($"[INFO] {message}", ConsoleColor.White);
        _writer.Write($"[INFO] {message}");
        _list.Add($"[INFO] {message}");
    }

    internal static void Warn(string message)
    {
        CheckState();
        if (WriteToConsole) ConsoleLog($"[WARN] {message}", ConsoleColor.Yellow);
        _writer.Write($"[WARN] {message}");
        _list.Add($"[WARN] {message}");
    }

    internal static void Error(string message)
    {
        CheckState();
        if (WriteToConsole) ConsoleLog($"[ERROR] {message}", ConsoleColor.Red);
        _writer.Write($"[ERROR] {message}");
        _list.Add($"[ERROR] {message}");
    }

    /// <summary>
    /// Prints the specified message to the debug log.
    /// </summary>
    /// <param name="message">Message.</param>
    public static void User(string message)
    {
        CheckState();
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

    /// <summary>
    /// Clears the logs.
    /// </summary>
    public static void Clear()
    {
        _buffer.Clear();
        _list.Clear();
    }
    
    /// <summary>
    /// Saves the contents of the logs to the specified file.
    /// </summary>
    /// <param name="filename">Target file name.</param>
    public static void SaveToFile(string filename)
    {
        File.WriteAllText(filename, _buffer.ToString());
    }
}