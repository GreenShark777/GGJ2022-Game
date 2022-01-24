/// <summary>
/// Interfaccia per tutti gli script che hanno bisogno che un ground check gli dica di essere per terra
/// </summary>
public interface INeedGroundCheck
{
    /// <summary>
    /// Comunica che si è toccata terra o meno
    /// </summary>
    void HasLanded(bool landed = true);

}
