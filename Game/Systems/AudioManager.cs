using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Game.Systems;

public class AudioManager : IDisposable
{
    private SoundEffect _shootSfx;
    private SoundEffect _hitSfx;
    private SoundEffect _cureSfx;
    private SoundEffect _musicSfx;
    private SoundEffectInstance _musicInstance;

    // Candidate file names checked in order for each role.
    private static readonly string[] MusicNames  = { "music.wav", "bgm.wav", "background.wav", "ambient.wav" };
    private static readonly string[] ShootNames  = { "shoot.wav", "fire.wav", "gun.wav" };
    private static readonly string[] HitNames    = { "hit.wav", "damage.wav", "enemy_hit.wav" };
    private static readonly string[] CureNames   = { "cure.wav", "heal.wav" };

    public AudioManager()
    {
        string folder = FindAudioFolder();
        if (folder == null)
        {
            Console.WriteLine("[Audio] Content/Audio folder not found — no audio will play.");
            return;
        }

        var available = new List<string>(Directory.GetFiles(folder, "*.wav"));

        _musicSfx = LoadByNames(folder, MusicNames, available, "music",  out string musicFile);
        _shootSfx = LoadByNames(folder, ShootNames, available, "shoot",  out string shootFile);
        _hitSfx   = LoadByNames(folder, HitNames,   available, "hit",    out string hitFile);
        _cureSfx  = LoadByNames(folder, CureNames,  available, "cure",   out string cureFile);

        Console.WriteLine($"[Audio] music={musicFile ?? "none"}  shoot={shootFile ?? "none"}  hit={hitFile ?? "none"}  cure={cureFile ?? "none"}");
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public void StartMusic()
    {
        if (_musicSfx == null) return;
        _musicInstance?.Dispose();
        _musicInstance = _musicSfx.CreateInstance();
        _musicInstance.IsLooped = true;
        _musicInstance.Volume   = 0.35f;
        _musicInstance.Play();
    }

    public void StopMusic()
    {
        _musicInstance?.Stop();
        _musicInstance?.Dispose();
        _musicInstance = null;
    }

    public void PlayShoot() => _shootSfx?.Play(0.5f, 0f, 0f);
    public void PlayHit()   => _hitSfx?.Play(0.6f, 0f, 0f);

    // Falls back to PlayHit() if no dedicated cure sound was loaded.
    public void PlayCure()
    {
        if (_cureSfx != null)
            _cureSfx.Play(0.55f, 0.1f, 0f);
        else
            PlayHit();
    }

    public void Dispose()
    {
        StopMusic();
        _shootSfx?.Dispose();
        _hitSfx?.Dispose();
        _cureSfx?.Dispose();
        _musicSfx?.Dispose();
    }

    // ── Loading helpers ───────────────────────────────────────────────────────

    // Looks for the audio folder next to the executable, then three levels up
    // (covers both `dotnet publish` and `dotnet run` from bin/Debug/net9.0/).
    private static string FindAudioFolder()
    {
        string[] candidates =
        {
            Path.Combine(AppContext.BaseDirectory, "Content", "Audio"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Content", "Audio"),
        };
        foreach (var c in candidates)
        {
            string full = Path.GetFullPath(c);
            if (Directory.Exists(full)) return full;
        }
        return null;
    }

    // Tries names[] first; if none found, takes the first unused file from available[].
    private static SoundEffect LoadByNames(
        string folder,
        string[] names,
        List<string> available,
        string role,
        out string usedName)
    {
        // Try exact candidates.
        foreach (var name in names)
        {
            string path = Path.Combine(folder, name);
            if (File.Exists(path))
            {
                var sfx = TryLoad(path);
                if (sfx != null)
                {
                    available.Remove(path);
                    usedName = name;
                    return sfx;
                }
            }
        }

        // Fallback: grab any remaining .wav.
        if (available.Count > 0)
        {
            string path = available[0];
            available.RemoveAt(0);
            var sfx = TryLoad(path);
            if (sfx != null)
            {
                usedName = Path.GetFileName(path) + " (fallback for " + role + ")";
                Console.WriteLine($"[Audio] No '{role}' file found — using fallback: {Path.GetFileName(path)}");
                return sfx;
            }
        }

        usedName = null;
        return null;
    }

    private static SoundEffect TryLoad(string path)
    {
        try
        {
            using var stream = File.OpenRead(path);
            return SoundEffect.FromStream(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Audio] Failed to load '{path}': {ex.Message}");
            return null;
        }
    }
}
