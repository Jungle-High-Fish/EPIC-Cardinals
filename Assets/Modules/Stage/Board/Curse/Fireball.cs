using System;
using Cardinals.Enums;
using System.Collections;
using UnityEngine;
using Util;

namespace Cardinals.Board.Curse
{
    public class Fireball : TileCurseData
    {
        public Fireball() : base(TileCurseType.Fireball)
        {
            Action = SpawnFireball();
        }

        IEnumerator SpawnFireball()
        {
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_BoardEventObject);
            var obj = MonoBehaviour.Instantiate(prefab);
            obj.AddComponent<BoardObject.Summon.Fireball>().Init(BaseTile, NewBoardObjectType.Fireball.ToString());

            yield return null;
        }
    }
}