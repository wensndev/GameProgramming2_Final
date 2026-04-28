namespace Game.Data;

public static class GameConstants
{
    public const int WindowWidth = 1280;
    public const int WindowHeight = 720;
    public const int TileSize = 32;

    public const float Gravity = 900f;
    public const float PlayerSpeed = 180f;
    public const float JumpForce = -550f;
    public const float ProjectileSpeed = 420f;

    public const int PlayerMaxHealth = 100;
    public const float ShootCooldown = 0.4f;
    public const float ContactDamageCooldown = 1.0f;
    public const int EnemyContactDamage = 10;
    public const int ProjectileDamage = 25;
}
