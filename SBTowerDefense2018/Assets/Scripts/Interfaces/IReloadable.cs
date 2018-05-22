/// <summary>
/// Classes, which implement this interface, can be reloaded.
/// </summary>
public interface IReloadable
{
    // Time required to reload.
    float ReloadTime { get; }
    // Reload method
    void Reload();
}