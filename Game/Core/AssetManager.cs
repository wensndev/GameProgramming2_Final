using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Core;

public static class AssetManager
{
    public static Texture2D Pixel      { get; private set; }
    public static SpriteFont Font      { get; private set; }
    public static Texture2D PlayerSheet { get; private set; }
    public static Texture2D EnemySheet  { get; private set; }
    public static Texture2D MutantSheet { get; private set; }

    public static bool HasDedicatedMutantSheet { get; private set; }

    public static void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        Pixel = new Texture2D(graphicsDevice, 1, 1);
        Pixel.SetData(new[] { Color.White });
        Font = content.Load<SpriteFont>("Fonts/DefaultFont");

        PlayerSheet = TryLoadSprite(graphicsDevice, "player_sheet.png");
        EnemySheet  = TryLoadSprite(graphicsDevice, "enemy_sheet.png");

        var mutantDedicated = TryLoadSprite(graphicsDevice, "mutant_sheet.png");
        HasDedicatedMutantSheet = mutantDedicated != null;
        MutantSheet = mutantDedicated ?? EnemySheet;

        if (!HasDedicatedMutantSheet && EnemySheet != null)
            Console.WriteLine("[Sprites] mutant_sheet.png not found — reusing enemy_sheet for mutant");
    }

    public static void DrawRect(SpriteBatch sb, Rectangle rect, Color color)
        => sb.Draw(Pixel, rect, color);

    public static void DrawRect(SpriteBatch sb, int x, int y, int w, int h, Color color)
        => sb.Draw(Pixel, new Rectangle(x, y, w, h), color);

    private static Texture2D TryLoadSprite(GraphicsDevice gd, string fileName)
    {
        string[] candidates =
        {
            Path.Combine(AppContext.BaseDirectory, "Content", "Sprites", fileName),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Content", "Sprites", fileName)),
        };
        foreach (var path in candidates)
        {
            if (!File.Exists(path)) continue;
            try
            {
                using var stream = File.OpenRead(path);
                Console.WriteLine($"[Sprites] Loaded {fileName}");
                return Texture2D.FromStream(gd, stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Sprites] Failed to load {path}: {ex.Message}");
            }
        }
        Console.WriteLine($"[Sprites] Not found: {fileName} — rectangle fallback active");
        return null;
    }
}
