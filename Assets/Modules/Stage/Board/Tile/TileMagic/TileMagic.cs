using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Constants.GameSetting;
using UnityEngine;
using Cardinals.Buff;
using UnityEngine.UI;
using Cardinals.Game;
using Util;
using System.Linq;
using DG.Tweening;

namespace Cardinals.Board {

	public class TileMagic: MonoBehaviour {
		private TileMagicType _type;
		private int _level;
		private int _exp;
		private bool _isLevelUp;

		private bool IsLevelUp
		{
			get => _isLevelUp;
			set
			{
				_isLevelUp = value;
				if (_isLevelUp) _tile.Get(gameObject).CanLevelUpTwinkleMMF.PlayFeedbacks();
				else _tile.Get(gameObject).CanLevelUpTwinkleMMF.StopFeedbacks();
			}
		}

		private ComponentGetter<Tile> _tile
			= new ComponentGetter<Tile>(TypeOfGetter.This);

		private ComponentGetter<Rigidbody> _rigidbody
			= new ComponentGetter<Rigidbody>(TypeOfGetter.This);

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

		public void Init(TileMagicType type) {
			_type = type;
			_level = 0;
			_exp = 0;
		}

		public void SetType(TileMagicType type)
		{
			Type = type;
			_tile.Get(gameObject).UITile.SetMaterial();
		}

        public void SetLevel(int level)
        {
            _level = level;
        }

		public void OnAction(int value, BaseEntity target) {
			GainExp(1);
			MagicAction(value, target);
		}

		public void GainExp(int exp) {
			_exp += exp;

			if (_level < Constants.GameSetting.Tile.MaxLevel && 
				_exp >= Constants.GameSetting.Tile.LevelUpExp[_level]) {
				IsLevelUp = true;

				if (GameManager.I.Stage.Player.OnTile == _tile.Get(gameObject)) {
					ApplyLevelUp();
				}
			}
		}

		public void GainExpToNextLevel() {
			GainExp(Constants.GameSetting.Tile.LevelUpExp[_level]);
		}

		public void ApplyLevelUp() {
			if (IsLevelUp == false) {
				return;
			}

			StartCoroutine(MagicLevelUp());
		}

		public IEnumerator SetSaveData(TileMagicType type, int level, int exp) {
			if (type == TileMagicType.None || type == TileMagicType.Attack || type == TileMagicType.Defence) {
				Type = type;
				_level = level;
				_exp = exp;
				yield break;
			}
			
			yield return MagicChangeApplyAnimation(type, level, exp);
		}

		private IEnumerator MagicLevelUp() {
			while (
				_level < Constants.GameSetting.Tile.MaxLevel && 
				_exp >= Constants.GameSetting.Tile.LevelUpExp[_level]
			) {
				int newExp = _exp - Constants.GameSetting.Tile.LevelUpExp[_level];
				
				// TODO: 밟았을 때만 실행되도록 변경 필요
				yield return LevelUpUI(newExp);
			}

			IsLevelUp = false;
		}

		private IEnumerator LevelUpUI(int newExp) {
			var levelUpRequest = GameManager.I.Stage.Board.RequestTileLevelUp(_type, _level);
			yield return levelUpRequest.Requester();
			var (newMagic, newLevel) = levelUpRequest.Result();

			yield return MagicChangeApplyAnimation(newMagic, newLevel, newExp);
		}

		private IEnumerator MagicChangeApplyAnimation(TileMagicType newMagic, int newLevel, int newExp) {
			// TODO: 마법 적용 애니메이션 구현 필요
			//_rigidbody.Get(gameObject).isKinematic = false;
			float animTime = _tile.Get(gameObject).Animation.Play(TileAnimationType.Rotate360);

			ParticleSystem.MainModule mainP = _tile.Get(gameObject).ParticleSystem.main;
            ParticleSystem.MinMaxGradient startColor = new ParticleSystem.MinMaxGradient(
                Data(newMagic).particleColor1,
                Data(newMagic).particleColor2
            )
            {
                mode = ParticleSystemGradientMode.TwoColors
            };
            mainP.startColor = startColor;
			yield return new WaitForSeconds(0.3f);
			_tile.Get(gameObject).ParticleSystem.Play();
			
			yield return new WaitForSeconds(0.1f);

			Type = newMagic;
			_level = newLevel;
			_exp = newExp;
			_tile.Get(gameObject).UITile.SetMaterial();

			yield return new WaitForSeconds(animTime - 0.4f);
			//_rigidbody.Get(gameObject).isKinematic = true;
			//_tile.Get(gameObject).Animation.Play(TileAnimationType.Float);
		}

		private void MagicAction(int value, BaseEntity target) {
			switch (_type) {
				case TileMagicType.None:
					break;
				case TileMagicType.Attack:
					MagicActionAttack(value, target);
					break;
				case TileMagicType.Defence:
					MagicActionDefence(value);
					break;
				case TileMagicType.Fire:
					MagicActionFireMain(value, target);
					MagicActionFireSub(value, target);
					break;
				case TileMagicType.Water:
					MagicActionWaterMain(value);
					MagicActionWaterSub(value, target);
					break;
				case TileMagicType.Earth:
					MagicActionEarthMain(value);
					MagicActionEarthSub();
					break;
			}
		}

		private void MagicActionAttack(int value, BaseEntity target) {
			GameManager.I.Player.Attack(target, value);
		}

		private void MagicActionDefence(int value) {
			GameManager.I.Player.Defense(value);
		}

		// 행동마다 해당 적에게 2/4/6 + 주사위 숫자의 데미지를 줍니다.
		private void MagicActionFireMain(int value, BaseEntity target)
		{
            GameManager.I.Sound.FlyFireBall();
			var targetPos = target.transform.position + new Vector3(0, -1, 0);
			var obj = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Particle_FireAttack));
			obj.transform.position = transform.position;
            
			obj.transform.DOJump(targetPos, 3, 1, .8f).SetEase(Ease.Linear)
				.OnComplete(() =>
				{
                    GameManager.I.Sound.BombFireBall();
                    target.Hit(value + Constants.GameSetting.Tile.FireMagicMainDamage[_level - 1], TileMagicType.Fire); // 실제 데미지 입히는 영역

					var explosion = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_Particle_Explosion));
					explosion.transform.position = targetPos;
                    
					Destroy(obj);
				});
		}

		// 3 이상의 행동에서 적에게 화상 효과를 부여합니다.
		private void MagicActionFireSub(int value, BaseEntity target) {
			
		}

		// 행동마다 플레이어의 체력을 최대 2/4/6 만큼 회복합니다.
		private void MagicActionWaterMain(int value) {
            GameManager.I.Sound.WaterHeal();
			int left = GameManager.I.Stage.Player.Heal(
				Mathf.Min(Constants.GameSetting.Tile.WaterMagicMainCure[_level - 1], value)
			);
		}

		// 3 이상의 행동에서 적에게 젖음 효과를 부여합니다.
		private void MagicActionWaterSub(int value, BaseEntity target) {
			
		}

		// 행동마다 방어력을 2/4/6 + 주사위 숫자만큼 얻습니다.
		private void MagicActionEarthMain(int value) {
            GameManager.I.Sound.EarthDefense();
			int defenseValue = Constants.GameSetting.Tile.EarthMagicMainDefense[_level - 1];
			GameManager.I.Stage.Player.AddDefenseCount(defenseValue + value);
		}

		private void MagicActionEarthSub() {
			// 없음
		}
	}

}
