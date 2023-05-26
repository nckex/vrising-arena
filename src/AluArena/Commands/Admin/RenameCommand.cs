using AluArena.Helpers;
using VampireCommandFramework;
using Unity.Collections;

namespace AluArena.Commands.Admin
{
    public static class RenameCommand
    {
        [Command("rename", adminOnly: true)]
        public static void OnRenameCommand(ChatCommandContext ctx, string playerName, string newName)
        {
            if (!PlayerHelpers.FindPlayer(playerName, false, out var currCharacterEntity, out var currUserEntity))
            {
                ctx.Reply($"Cannot find player: {playerName}");
                return;
            }

            FixedString64 newFixedName = newName;
            if (newFixedName.utf8LengthInBytes > 20)
            {
                ctx.Reply("New name is too long");
                return;
            }

            PlayerHelpers.RenamePlayer(currUserEntity, currCharacterEntity, newFixedName);

            ctx.Reply($"Player {playerName} has name changed to: {newName}");
        }
    }
}
