using VampireCommandFramework;
using ProjectM.Network;
using ProjectM;
using AluArena.Common.Prefabs;
using Wetstone.API;

namespace AluArena.Commands
{
    public static class BloodCommands
    {
        [Command("b", usage: ".b <blood type>", description: "Available types is brute, warrior, rogue, scholar, creature, worker")]
        public static void ChangeBloodTypeCommand(ChatCommandContext ctx, string bloodTypeName)
        {
            var bloodTypePrefab = bloodTypeName switch
            {
                "brute" => BloodTypePrefabs.Brute,
                "warrior" => BloodTypePrefabs.Warrior,
                "rogue" => BloodTypePrefabs.Rogue,
                "scholar" => BloodTypePrefabs.Scholar,
                "creature" => BloodTypePrefabs.Creature,
                "worker" => BloodTypePrefabs.Worker,
                _ => default
            };

            if (bloodTypePrefab == default)
            {
                ctx.Reply("<color=#ffff00>Invalid blood type.</color>");
                return;
            }

            var bloodEvent = new ChangeBloodDebugEvent()
            {
                Amount = 100,
                Quality = 100f,
                Source = bloodTypePrefab
            };

            VWorld.Server.GetExistingSystem<DebugEventsSystem>().ChangeBloodEvent(ctx.Event.User.Index, ref bloodEvent);

            ctx.Reply($"Blood type changed to <color=#ffff00>{bloodTypeName}</color>.");
        }
    }
}
