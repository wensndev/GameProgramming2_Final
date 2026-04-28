using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Game.Data;
using Game.Entities;

namespace Game.Systems;

public class CombatSystem
{
    private readonly Player _player;
    private readonly List<Enemy> _enemies;
    private readonly List<Projectile> _projectiles;
    private readonly InfectionSystem _infection;

    public int KilledCount { get; private set; }
    public int CuredCount { get; private set; }

    private float _contactCooldown;

    public CombatSystem(Player player, List<Enemy> enemies,
        List<Projectile> projectiles, InfectionSystem infection)
    {
        _player = player;
        _enemies = enemies;
        _projectiles = projectiles;
        _infection = infection;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _contactCooldown -= dt;

        foreach (var enemy in _enemies)
        {
            if (!enemy.IsActive || enemy.IsCured) continue;
            if (!_player.Bounds.Intersects(enemy.Bounds)) continue;
            if (_contactCooldown > 0) continue;

            _player.TakeDamage(GameConstants.EnemyContactDamage);
            _contactCooldown = GameConstants.ContactDamageCooldown;
        }

        foreach (var proj in _projectiles)
        {
            if (!proj.IsActive) continue;

            if (proj.IsEnemyShot)
            {
                if (_player.Bounds.Intersects(proj.Bounds))
                {
                    _player.TakeDamage(GameConstants.ProjectileDamage);
                    proj.IsActive = false;
                }
                continue;
            }

            foreach (var enemy in _enemies)
            {
                if (!enemy.IsActive || enemy.IsCured) continue;
                if (!proj.Bounds.Intersects(enemy.Bounds)) continue;

                proj.IsActive = false;

                if (proj.IsCure)
                {
                    enemy.Cure();
                    CuredCount++;
                    _infection.Decrease(0.1f);
                }
                else
                {
                    enemy.TakeDamage(GameConstants.ProjectileDamage);
                    if (!enemy.IsActive)
                    {
                        KilledCount++;
                        _infection.Increase(0.05f);
                    }
                }
                break;
            }
        }
    }
}
