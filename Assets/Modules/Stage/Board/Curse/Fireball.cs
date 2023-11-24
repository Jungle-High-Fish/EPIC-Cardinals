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
            GameObject.Instantiate(prefab, BaseTile.transform);
            
            // [TODO] Board Manager ? Stage Manager에 소환수 관리 리스트 만들어서, 소환물 공용 생성 처리하도록 구현 필요
        }
    }
}