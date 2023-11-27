using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Cardinals.Board {
    [CreateAssetMenu(fileName = "TileSymbols", menuName = "Cardinals/TileSymbols")]
    public class TileSymbolsSO : ScriptableObject {
        public Sprite this[TileType type, int level] {
            get {
                switch (type) {
                    case TileType.Attack:
                        return _attackSymbols[level];
                    case TileType.Defence:
                        return _defenseSymbols[level];
                    default:
                        return null;
                }
            }
        }

        [SerializeField] private Sprite[] _attackSymbols;
        [SerializeField] private Sprite[] _defenseSymbols;
    }

}