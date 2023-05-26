using Unity.Entities;
using Unity.Collections;
using ProjectM.Network;
using ProjectM;
using Wetstone.API;

namespace AluArena.Helpers
{
    public static class PlayerHelpers
    {
        public static bool FindPlayer(string name, bool mustOnline, out Entity characterEntity, out Entity userEntity)
        {
            EntityManager entityManager = Plugin.EntityManager;
            characterEntity = default;
            userEntity = default;

            foreach (var currentUserEntity in entityManager.CreateEntityQuery(ComponentType.ReadOnly<User>()).ToEntityArray(Allocator.Temp))
            {
                var target_component = entityManager.GetComponentData<User>(currentUserEntity);
                if (mustOnline && !target_component.IsConnected)
                    continue;

                string CharName = target_component.CharacterName.ToString();
                if (CharName.Equals(name))
                {
                    userEntity = currentUserEntity;
                    characterEntity = target_component.LocalCharacter._Entity;
                    return true;
                }
            }

            return false;
        }

        public static void RenamePlayer(Entity userEntity, Entity charEntity, FixedString64 newName)
        {
            var des = VWorld.Server.GetExistingSystem<DebugEventsSystem>();
            var networkId = Plugin.EntityManager.GetComponentData<NetworkId>(userEntity);

            var renameEvent = new RenameUserDebugEvent
            {
                NewName = newName,
                Target = networkId
            };

            var fromCharacter = new FromCharacter
            {
                User = userEntity,
                Character = charEntity
            };

            des.RenameUser(fromCharacter, renameEvent);
        }
    }
}
