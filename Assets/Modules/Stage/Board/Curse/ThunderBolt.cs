using Cardinals.Enums;
using UnityEngine;
using Util;

namespace Cardinals.Board.Curse
{
    public class ThunderBolt : TileCurseData
    {
        public ThunderBolt() : base(TileCurseType.ThunderBolt)
        {
            Action = SpawnThunderBolt;
        }

        void SpawnThunderBolt()
        {
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Enemy_Spawn_ThunderBolt);
            GameObject.Instantiate(prefab, BaseTile.transform);
        }
    }
}