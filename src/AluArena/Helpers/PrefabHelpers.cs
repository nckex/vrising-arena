using ProjectM;
using Unity.Entities;

namespace AluArena.Helpers
{
    public static class PrefabHelpers
    {
        public static PrefabGUID GetPrefabGUID(Entity entity)
        {
            var entityManager = Plugin.EntityManager;
            
            PrefabGUID guid;
            try
            {
                guid = entityManager.GetComponentData<PrefabGUID>(entity);
            }
            catch
            {
                guid.GuidHash = 0;
            }

            return guid;
        }
    }
}
