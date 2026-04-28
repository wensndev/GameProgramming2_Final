using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Data;

namespace Game.World;

public class Level
{
    public TileMap TileMap { get; private set; }
    public Vector2 PlayerSpawn { get; private set; }
    public Vector2 DoorPosition { get; private set; }
    public List<(Vector2 Position, bool IsMutant)> EnemySpawns { get; } = new();
    public List<Vector2> NpcSpawns { get; } = new();
    public List<(Vector2 Position, int NoteIndex)> NoteSpawns { get; } = new();

    public int WorldWidth => TileMap.Cols * GameConstants.TileSize;
    public int WorldHeight => TileMap.Rows * GameConstants.TileSize;

    private static readonly string[] RawMap =
    {
        "########################################",
        "#P..E.......T..........................#",
        "#......................................#",
        "#......................................#",
        "#############...########...............#",
        "#......................................#",
        "#.....E.........M......T...............#",
        "#......................................#",
        "########.......########................#",
        "#......................................#",
        "#.....E.........E......................#",
        "#......................................#",
        "####........####.........T.............#",
        "#......................................#",
        "#.....N..............................D.#",
        "#......................................#",
        "########################################",
    };

    public Level()
    {
        int maxLen = 0;
        foreach (var line in RawMap) maxLen = Math.Max(maxLen, line.Length);

        var cleanedMap = new string[RawMap.Length];
        for (int r = 0; r < RawMap.Length; r++)
        {
            var chars = RawMap[r].PadRight(maxLen, '.').ToCharArray();
            for (int c = 0; c < chars.Length; c++)
            {
                int ts = GameConstants.TileSize;
                switch (chars[c])
                {
                    case 'P':
                        PlayerSpawn = new Vector2(c * ts, r * ts);
                        chars[c] = '.';
                        break;
                    case 'E':
                        EnemySpawns.Add((new Vector2(c * ts, r * ts), false));
                        chars[c] = '.';
                        break;
                    case 'M':
                        EnemySpawns.Add((new Vector2(c * ts, r * ts), true));
                        chars[c] = '.';
                        break;
                    case 'N':
                        NpcSpawns.Add(new Vector2(c * ts, r * ts));
                        chars[c] = '.';
                        break;
                    case 'T':
                        NoteSpawns.Add((new Vector2(c * ts, r * ts), NoteSpawns.Count));
                        chars[c] = '.';
                        break;
                    case 'D':
                        DoorPosition = new Vector2(c * ts, (r - 1) * ts);
                        chars[c] = '.';
                        break;
                }
            }
            cleanedMap[r] = new string(chars);
        }

        TileMap = new TileMap(cleanedMap);
    }

    public void Draw(SpriteBatch spriteBatch) => TileMap.Draw(spriteBatch);
}
