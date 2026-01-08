namespace GensouGL.Core;

/// <summary>
/// Representing a listener class to buffer input text.
/// </summary>
/// <remarks>
/// This class does not provide any public constructors. To receive user input using <see cref="TextInputListener"/>, create an instance using <see cref="InputService.CreateTextInputListener()"/>.
/// Use <see cref="InputService.DestroyTextInputListener(TextInputListener)"/> to dispose of unnecessary TextInputListener.
/// </remarks>
public sealed class TextInputListener
{
    private StringBuilder _buffer;

    /// <summary>
    /// Gets the underlying buffer.
    /// </summary>
    public StringBuilder Buffer => _buffer;
    /// <summary>
    /// Gets if the buffer contains any data.
    /// </summary>
    public bool ContainsAny => _buffer.Length > 0;
    /// <summary>
    /// Gets the buffered text.
    /// </summary>
    public string BufferedText => _buffer.ToString();

    internal TextInputListener()
    {
        _buffer = new StringBuilder();
    }

    internal void Send(string text)
    {
        _buffer.Append(text);
    }

    /// <summary>
    /// Tries to read the buffered text and clear the buffer.
    /// </summary>
    /// <param name="text">Resulting text. If there was no data left to be read, null.</param>
    /// <returns>Flag representing whether the buffer had any data to be read. When false, text is null.</returns>
    public bool Pop(out string text)
    {
        if (ContainsAny)
        {
            text = BufferedText;
            ClearBuffer();
            return true;
        }
        else
        {
            text = null;
            return false;
        }
    }

    /// <summary>
    /// Inputs the specified text to the buffer.
    /// </summary>
    /// <param name="text">Text to input.</param>
    public void Input(string text)
    {
        _buffer.Append(text);
    }

    /// <summary>
    /// Clears the buffer.
    /// </summary>
    public void ClearBuffer()
    {
        _buffer.Clear();
    }
}