/// <summary>
/// Interfaccia per tutti gli script degli oggetti che subiscono danni
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Fa in modo che subisca danno
    /// </summary>
    /// <param name="dmgTaken"></param>
    void TakeDmg(int dmgTaken);

}
