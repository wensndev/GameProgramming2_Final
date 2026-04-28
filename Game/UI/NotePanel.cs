using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;
using Game.Data;
using Game.Systems;

namespace Game.UI;

public class NotePanel
{
    private readonly NoteSystem _notes;

    public NotePanel(NoteSystem notes) => _notes = notes;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_notes.IsShowing) return;

        AssetManager.DrawRect(spriteBatch, 200, 100, GameConstants.WindowWidth - 400, 400, new Color(10, 10, 30, 230));
        spriteBatch.DrawString(AssetManager.Font, "NOTES", new Vector2(220, 115), Color.Yellow);

        for (int i = 0; i < _notes.Collected.Count; i++)
            spriteBatch.DrawString(AssetManager.Font, _notes.Collected[i], new Vector2(220, 145 + i * 24), Color.White);

        if (_notes.Collected.Count == 0)
            spriteBatch.DrawString(AssetManager.Font, "(No notes collected yet)", new Vector2(220, 145), Color.Gray);

        spriteBatch.DrawString(AssetManager.Font, "[Tab: Close]",
            new Vector2(220, 460), Color.DarkGray);
    }
}
