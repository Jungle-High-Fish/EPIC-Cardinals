using System;
using Cardinals.Enums;
using UnityEngine;
using Util;

namespace Cardinals.Board.Curse
{
    public class Fireball : TileCurseData
    {
        public Fireball() : base(TileCurseType.Fireball)
        {
            Action = SpawnFireball;
        }

        void SpawnFireball()
        {
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Enemy_Spawn_Fireball);
            var fireball = GameObject.Instantiate(prefab).GetComponent<Enemy.Summon.Fireball>();
            fireball.Init(BaseTile);
        }
    }
}