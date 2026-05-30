using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;
using Game.Entities;
using Game.Systems;
using Game.UI;
using Game.World;

namespace Game.Scenes;

public class GameplayScene : IScene
{
    private readonly SceneManager _sceneManager;
    private Level _level;
    private Player _player;
    private List<Enemy> _enemies;
    private List<Npc> _npcs;
    private List<Projectile> _projectiles;
    private List<NotePickup> _notePickups;
    private Door _door;
    private Camera2D _camera;
    private CombatSystem _combat;
    private InfectionSystem _infection;
    private DialogueSystem _dialogue;
    private NoteSystem _notes;
    private HealthBar _healthBar;
    private InfectionBar _infectionBar;
    private DialogueBox _dialogueBox;
    private NotePanel _notePanel;
    private AudioManager _audio;

    public GameplayScene(SceneManager sceneManager) => _sceneManager = sceneManager;

    public void Load()
    {
        _audio = new AudioManager();
        _level = new Level();
        _camera = new Camera2D();
        _enemies = new List<Enemy>();
        _npcs = new List<Npc>();
        _projectiles = new List<Projectile>();
        _notePickups = new List<NotePickup>();
        _infection = new InfectionSystem();
        _dialogue = new DialogueSystem();
        _notes = new NoteSystem();

        _player = new Player(_level.PlayerSpawn);
        _player.OnShoot += (pos, right, isCure) =>
        {
            _projectiles.Add(new Projectile(pos, right, isCure));
            _audio.PlayShoot();
        };

        _dialogue.OnComplete += () =>
        {
            if (!_player.CureUnlocked)
            {
                _player.CureUnlocked = true;
                _infection.Increase(0.15f);
            }
        };

        foreach (var (pos, isMutant) in _level.EnemySpawns)
        {
            if (isMutant)
            {
                var mutant = new MutantEnemy(pos);
                mutant.OnShoot += (shootPos, right) =>
                    _projectiles.Add(new Projectile(shootPos, right, false, true));
                _enemies.Add(mutant);
            }
            else
            {
                _enemies.Add(new Enemy(pos));
            }
        }

        _door = new Door(_level.DoorPosition);

        foreach (var pos in _level.NpcSpawns)
            _npcs.Add(new Npc(pos, _dialogue));

        for (int i = 0; i < _level.NoteSpawns.Count; i++)
        {
            var (pos, idx) = _level.NoteSpawns[i];
            string text = idx < NoteData.Notes.Length ? NoteData.Notes[idx] : $"Note {idx + 1}";
            _notePickups.Add(new NotePickup(pos, text));
        }

        _combat = new CombatSystem(_player, _enemies, _projectiles, _infection);
        _combat.OnEnemyKilled += () => _audio.PlayHit();
        _combat.OnEnemyCured  += () => _audio.PlayCure();

        int hudY = GameConstants.WindowHeight - 50;
        _healthBar = new HealthBar(new Vector2(20, hudY));
        _infectionBar = new InfectionBar(new Vector2(260, hudY));
        _dialogueBox = new DialogueBox(_dialogue);
        _notePanel = new NotePanel(_notes);

        _audio.StartMusic();
    }

    public void Unload() { _audio?.StopMusic(); _audio?.Dispose(); }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Game1.Input.IsPressed(Keys.Escape) || Game1.Input.IsPressed(Keys.P))
        {
            _sceneManager.PushOverlay(new PauseScene(_sceneManager));
            return;
        }

        if (Game1.Input.IsPressed(Keys.Tab))
            _notes.Toggle();

        if (_dialogue.IsActive)
        {
            _dialogue.Update(gameTime);
            return;
        }

        _player.Update(gameTime);
        _player.ApplyPhysics(_level.TileMap, dt);

        foreach (var enemy in _enemies)
        {
            enemy.Update(gameTime);
            enemy.ApplyPhysics(_level.TileMap, dt);
            if (enemy is MutantEnemy mutantEnemy)
                mutantEnemy.TryShoot(_player.Position);
        }

        foreach (var npc in _npcs)
            npc.ApplyPhysics(_level.TileMap, dt);

        foreach (var proj in _projectiles)
        {
            proj.Update(gameTime);
            proj.CheckTileCollision(_level.TileMap);
        }

        foreach (var npc in _npcs)
            npc.TryTalk(_player.Bounds);

        foreach (var note in _notePickups)
            note.TryCollect(_player.Bounds, _notes);

        bool allHandled = _enemies.TrueForAll(e => !e.IsActive || e.IsCured);
        if (allHandled && !_door.IsUnlocked)
            _door.Unlock();

        _combat.Update(gameTime);

        _projectiles.RemoveAll(p => !p.IsActive);

        _camera.Follow(
            _player.Position + new Vector2(_player.Width / 2f, _player.Height / 2f),
            _level.WorldWidth, _level.WorldHeight);

        if (_player.State == PlayerState.Dead)
        {
            _sceneManager.ChangeScene(new GameOverScene(_sceneManager));
            return;
        }

        if (_door.IsUnlocked && _player.Bounds.Intersects(_door.Bounds))
        {
            _sceneManager.ChangeScene(new EndingScene(_sceneManager, _combat.KilledCount, _combat.CuredCount));
            return;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(transformMatrix: _camera.Transform);
        _level.Draw(spriteBatch);
        _door.Draw(spriteBatch);
        foreach (var note in _notePickups) note.Draw(spriteBatch);
        foreach (var enemy in _enemies) enemy.Draw(spriteBatch);
        foreach (var npc in _npcs) npc.Draw(spriteBatch);
        foreach (var proj in _projectiles) proj.Draw(spriteBatch);
        _player.Draw(spriteBatch);
        spriteBatch.End();

        spriteBatch.Begin();
        _healthBar.Draw(spriteBatch, _player.Health, GameConstants.PlayerMaxHealth);
        _infectionBar.Draw(spriteBatch, _infection.InfectionLevel);
        DrawModeIndicator(spriteBatch);
        spriteBatch.DrawString(AssetManager.Font, "Tab: Notes   Z/Ctrl: Shoot   Q: Mode   ESC: Pause",
            new Vector2(20, GameConstants.WindowHeight - 75), Color.DarkGray);
        _dialogueBox.Draw(spriteBatch);
        _notePanel.Draw(spriteBatch);
        spriteBatch.End();
    }

    private void DrawModeIndicator(SpriteBatch spriteBatch)
    {
        if (!_player.CureUnlocked)
        {
            spriteBatch.DrawString(AssetManager.Font, "Mode: KILL",
                new Vector2(GameConstants.WindowWidth - 160, GameConstants.WindowHeight - 50), Color.Orange);
            return;
        }

        bool isKill = _player.Mode == ShootMode.Kill;
        string label = isKill ? "Mode: KILL [Q]" : "Mode: CURE [Q]";
        Color c = isKill ? Color.Orange : Color.Cyan;
        spriteBatch.DrawString(AssetManager.Font, label,
            new Vector2(GameConstants.WindowWidth - 180, GameConstants.WindowHeight - 50), c);
    }
}
