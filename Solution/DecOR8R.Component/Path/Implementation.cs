namespace DecOR8R.Component.Path;

public class Implementation
{
    private Implementation() { }
    public static Implementation Instance { get; } = new();

    public string Decorate(string path)
    {
        return path;
    }
}
