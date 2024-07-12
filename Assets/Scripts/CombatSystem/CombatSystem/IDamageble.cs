public enum DamageType {
    Melee,
    Ranged
}

public interface IDamageble
{

    void TakeDamage(DamageContainer damage);

}