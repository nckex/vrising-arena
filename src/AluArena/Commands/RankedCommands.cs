using ProjectM;
using ProjectM.Network;
using System;
using System.Collections.Generic;
using System.Text;
using VampireCommandFramework;

namespace AluArena.Commands
{
    public static class RankedCommands
    {
        [Command("ranked", usage: ".ranked", description: "Join ranked queue")]
        public static void OnRankedCommand(ChatCommandContext ctx)
        {
            ctx.Reply("Joined the ranked queue");
        }
    }
}
