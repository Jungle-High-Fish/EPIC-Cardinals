using Cardinals.Enums;
using System.Collections;
using UnityEngine;
using Util;

namespace Cardinals.Board.Curse
{
    public class ThunderBolt : TileCurseData
    {
        public ThunderBolt() : base(TileCurseType.ThunderBolt)
        {
            Action = SpawnThunderBolt();
        }

        IEnumerator SpawnThunderBolt()
        {
            GameManager.I.Sound.ThunderBolt();
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Enemy_Spawn_ThunderBolt);
            var tb = GameObject.Instantiate(prefab).GetComponent<Enemy.Summon.ThunderBolt>();
           
            yield return tb.Init();
        }
    }
}