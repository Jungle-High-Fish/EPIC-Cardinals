using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Constants.GameSetting;
using UnityEngine;
using Cardinals.Buff;
using UnityEngine.UI;
using Cardinals.Game;
using Util;

namespace Cardinals.Board {

	public class TileMagic: MonoBehaviour {
		private TileMagicType _type;
		private int _level;
		private int _exp;
		private bool _isLevelUp;

		public TileMagicType Type 
		{ get => _type; private set => _type = value; } 
		public int Level => _level;
		public int Exp
		{
			get => _exp;
			set => _exp = value;
		}

		public static TileMagicDataSO Data(TileMagicType tileMagicType) {
            return ResourceLoader.LoadSO<TileMagicDataSO>(
                Constants.FilePath.Resources.SO_MagicData + tileMagicType
            );
        }

		public void Init() {
			_type = TileMagicType.None;
			_level = 0;
			_exp = 0;
		}

		public void SetType(TileMagicType type)
		{
			Type = type;
		}

		public void OnAction(int value, BaseEntity target) {
			GainExp(1);
			MagicAction(value, target);
		}

		public void GainExp(int exp) {
			_exp += exp;

			if (_level < Constants.GameSetting.Tile.MaxLevel && 
				_exp >= Constants.GameSetting.Tile.LevelUpExp[_level]) {
				_isLevelUp = true;
			}

			while (
				_level < Constants.GameSetting.Tile.MaxLevel && 
				_exp >= Constants.GameSetting.Tile.LevelUpExp[_level]
			) {
				_exp -= Constants.GameSetting.Tile.LevelUpExp[_level];
				
				// TODO: 밟았을 때만 실행되도록 변경 필요
				StartCoroutine(MagicLevelUp());
			}
		}

		public void GainExpToNextLevel() {
			GainExp(Constants.GameSetting.Tile.LevelUpExp[_level]);
		}

		public void ApplyLevelUp() {
			if (_isLevelUp == false) {
				return;
			}

			StartCoroutine(MagicLevelUp());
		}

		private IEnumerator MagicLevelUp() {
			while (
				_level < Constants.GameSetting.Tile.MaxLevel && 
				_exp >= Constants.GameSetting.Tile.LevelUpExp[_level]
			) {
				_exp -= Constants.GameSetting.Tile.LevelUpExp[_level];
				
				// TODO: 밟았을 때만 실행되도록 변경 필요
				yield return MagicLevelUp();
			}
		}

		private IEnumerator LevelUpUI() {
			var levelUpRequest = GameManager.I.Stage.Board.RequestTileLevelUp(_type, _level);
			yield return levelUpRequest.Requester();
			var (newMagic, newLevel) = levelUpRequest.Result();

			// TODO: 마법 적용 애니메이션 구현 필요
			_type = newMagic;
			_level = newLevel;
		}

		private void MagicAction(int value, BaseEntity target) {
			switch (_type) {
				case TileMagicType.None:
					break;
				case TileMagicType.Fire:
					MagicActionFireMain(target);
					MagicActionFireSub(value, target);
					break;
				case TileMagicType.Water:
					MagicActionWaterMain();
					MagicActionWaterSub(value, target);
					break;
				case TileMagicType.Earth:
					MagicActionEarthMain();
					MagicActionEarthSub();
					break;
			}
		}

		// 행동마다 해당 적에게 2/4/6의 데미지를 줍니다.
		private void MagicActionFireMain(BaseEntity target) {
			target.Hit(Constants.GameSetting.Tile.FireMagicMainDamage[_level - 1]);
		}

		// 3 이상의 행동에서 적에게 화상 효과를 부여합니다.
		private void MagicActionFireSub(int value, BaseEntity target) {
			if (value >= 3) {
				target.AddBuff(new Burn(1));
			}
		}

		// 행동마다 플레이어의 체력을 1/2/3만큼 회복합니다.
		private void MagicActionWaterMain() {
			GameManager.I.Stage.Player.Heal(
				Constants.GameSetting.Tile.WaterMagicMainCure[_level - 1]
			);
		}

		// 3 이상의 행동에서 적에게 젖음 효과를 부여합니다.
		private void MagicActionWaterSub(int value, BaseEntity target) {
			if (value >= 3) {
				target.AddBuff(new Weak(1));
			}
		}

		// 행동마다 방어력을 2/4/6만큼 얻습니다.
		private void MagicActionEarthMain() {
			GameManager.I.Stage.Player.DefenseCount 
				+= Constants.GameSetting.Tile.EarthMagicMainDefense[_level - 1];
		}

		private void MagicActionEarthSub() {
			// 없음
		}
	}

}
