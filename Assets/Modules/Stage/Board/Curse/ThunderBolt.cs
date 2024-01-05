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
            GameManager.I.Sound.ThunderBolt();
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Enemy_Spawn_ThunderBolt);
            var tb = GameObject.Instantiate(prefab).GetComponent<Enemy.Summon.ThunderBolt>();
            tb.Init();
        }
    }
}