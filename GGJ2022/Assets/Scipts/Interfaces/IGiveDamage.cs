/// <summary>
/// Interfaccia per gli script che fanno danno in certe condizioni
/// </summary>
public interface IGiveDamage
{
    /// <summary>
    /// Ritorna il danno che fa subire
    /// </summary>
    /// <returns></returns>
    float GiveDamage();
    /// <summary>
    /// Comunica se questo script di danno proviene da un nemico o trappola
    /// </summary>
    /// <returns></returns>
    bool IsEnemyOrTrapAttack();

}
