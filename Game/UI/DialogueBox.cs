using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;
using Game.Data;
using Game.Systems;

namespace Game.UI;

public class DialogueBox
{
    private readonly DialogueSystem _dialogue;
    private const int BoxHeight = 100;

    public DialogueBox(DialogueSystem dialogue) => _dialogue = dialogue;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_dialogue.IsActive) return;

        int y = GameConstants.WindowHeight - BoxHeight - 85;
        AssetManager.DrawRect(spriteBatch, 20, y, GameConstants.WindowWidth - 40, BoxHeight, new Color(0, 0, 0, 200));
        spriteBatch.DrawString(AssetManager.Font, _dialogue.CurrentLine,
            new Vector2(30, y + 10), Color.White);
        spriteBatch.DrawString(AssetManager.Font, "[Enter: Continue]",
            new Vector2(GameConstants.WindowWidth - 200, y + BoxHeight - 25), Color.Gray);
    }
}
