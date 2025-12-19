namespace GensouGL.Core;

public sealed class TextInputListener
{
    private StringBuilder _buffer;

    public StringBuilder Buffer => _buffer;
    public bool ContainsAny => _buffer.Length > 0;
    public string BufferedText => _buffer.ToString();

    internal TextInputListener()
    {
        _buffer = new StringBuilder();
    }

    internal void Send(string text)
    {
        _buffer.Append(text);
    }

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

    public void Input(string text)
    {
        _buffer.Append(text);
    }

    public void ClearBuffer()
    {
        _buffer.Clear();
    }
}