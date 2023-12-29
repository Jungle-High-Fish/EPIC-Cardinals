using Cardinals.Board;
using Cardinals.Enums;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Cardinals.Title
{
    public class TileMaker : MonoBehaviour
    {
        private GameObject _tilePrefab;

        [SerializeField] private List<Transform> _tileList = new();
        
        [SerializeField] int startPosX;
        [SerializeField] int intervalX;
        [SerializeField] private int tileCount;

        [Button]
        public void Init()
        {
            _tilePrefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Tile);
            
            // 기존 항목 제거
            for (int i = _tileList.Count - 1; i >= 0; i--)
            {
                Destroy(_tileList[i].gameObject);
                _tileList.Remove(_tileList[i]);
            }
            
            // 생성
            for (int i = 0; i < tileCount; i++)
            {
                var obj = Instantiate(_tilePrefab, transform);
                _tileList.Add(obj.transform);

                var pos = new Vector3(startPosX + intervalX * i, 0, 0);
                obj.transform.position = pos;
                var tile = obj.GetComponent<Tile>();
                tile.Init(
                    GetRandomTileData(),
                    (e) => { },
                    pos,
                    Vector3.zero
                );
                tile.UITile.Init(tile);
                tile.UITile.SetMaterial();
                tile.TileMagic.SetType(GetRandomMagicType());
                tile.TileMagic.SetLevel(Random.Range(1, 4));
                
                obj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
        }

        private TileData GetRandomTileData()
        {
            return new TileData()
            {
                type = TileType.Defence,
                magicType = GetRandomMagicType(),
                level = Random.Range(1, 3)
            };
        }

        private TileMagicType GetRandomMagicType()
        {
            return (TileMagicType)Random.Range((int)TileMagicType.Fire, (int)TileMagicType.Defence + 1);
        }

        [Button]
        public void Move()
        {
            StartCoroutine(MoveFlow(.6f));
        }

        public IEnumerator MoveFlow(float time)
        {
            //bool next = false;
            _tileList.Add(_tileList[0]);
            _tileList[0].transform.position = _tileList[tileCount - 1].transform.position + new Vector3(intervalX, 0 ,0);
            SetRandomTile(_tileList[tileCount].GetComponent<Tile>()) ;
            _tileList.RemoveAt(0);
            
            for (int i = 0; i < _tileList.Count; i++)
            {
                _tileList[i].DOMoveX(startPosX + (intervalX * i), time);
            }

            yield return new WaitForSeconds(time);
        }
        
        private void SetRandomTile(Tile tile)
        {
            tile.TileMagic.SetType(GetRandomMagicType());
            tile.TileMagic.SetLevel(Random.Range(1, 4));
        }
    }

}