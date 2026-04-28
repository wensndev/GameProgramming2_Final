using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;
using Game.Data;

namespace Game.World;

public class TileMap
{
    private readonly bool[,] _solid;
    public int Cols { get; }
    public int Rows { get; }
    public int TileSize => GameConstants.TileSize;

    public TileMap(string[] mapData)
    {
        Rows = mapData.Length;
        Cols = mapData[0].Length;
        _solid = new bool[Rows, Cols];
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < mapData[r].Length && c < Cols; c++)
                _solid[r, c] = mapData[r][c] == '#';
    }

    public bool IsSolid(int col, int row)
    {
        if (col < 0 || col >= Cols || row < 0 || row >= Rows) return true;
        return _solid[row, col];
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Cols; c++)
                if (_solid[r, c])
                    AssetManager.DrawRect(spriteBatch,
                        c * TileSize, r * TileSize, TileSize, TileSize,
                        Color.SlateGray);
    }
}
