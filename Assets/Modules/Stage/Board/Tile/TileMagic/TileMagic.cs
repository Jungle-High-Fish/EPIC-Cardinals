using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Constants.GameSetting;
using UnityEngine;

namespace Cardinals.Board {

	public class TileMagic: MonoBehaviour {
		private TileMagicType _type;
		private int _level;
		private int _exp;

		public void Init() {
			_type = TileMagicType.None;
			_level = 0;
			_exp = 0;
		}

		public void OnAction(int value) {
			GainExp(1);
			MagicAction();
		}

		private void MagicLevelUp() {

		}

		private void GainExp(int exp) {
			_exp += exp;
			while (
				_level < Constants.GameSetting.Tile.MaxLevel && 
				_exp >= Constants.GameSetting.Tile.LevelUpExp[_level]
			) {
				_exp -= Constants.GameSetting.Tile.LevelUpExp[_level];
				MagicLevelUp();
			}
		}

		private void MagicAction() {
			switch (_type) {
				case TileMagicType.None:
					break;
			}
		}
	}

}
