using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using DG.Tweening;
using Util;
using Cardinals.Enums;
using System;

namespace Cardinals.Board {

	public class TileAnimation : MonoBehaviour {
		public Transform RendererTransform => _rendererTransform.Get(gameObject);

		private ComponentGetter<MMF_Player> _MMFPlayer
			= new ComponentGetter<MMF_Player>(TypeOfGetter.This);

		private ComponentGetter<Transform> _transform
			= new ComponentGetter<Transform>(TypeOfGetter.This);

		private ComponentGetter<Transform> _rendererTransform
			= new ComponentGetter<Transform>(TypeOfGetter.ChildByName, "Renderer");

		private ComponentGetter<Rigidbody> _rigidbody
			= new ComponentGetter<Rigidbody>(TypeOfGetter.This);

		private ComponentGetter<Tile> _tile
			= new ComponentGetter<Tile>(TypeOfGetter.This);

		private Dictionary<TileAnimationType, (Sequence anim, float time, int playNum)> _animationDict
			= new Dictionary<TileAnimationType, (Sequence, float, int)>();

		public void Init() {
			InitAnimations();
		}

		public void StopAll() {
			List<TileAnimationType> animationTypes = new List<TileAnimationType>(_animationDict.Keys);
			foreach (var animationType in animationTypes) {
				_animationDict[animationType] = (_animationDict[animationType].anim, _animationDict[animationType].time, 0);
				if (_animationDict[animationType].anim.IsPlaying()) {
					_animationDict[animationType].anim.Complete(false);
				}
			}
		}

		public void Stop(TileAnimationType animationType) {
			_animationDict[animationType] = (_animationDict[animationType].anim, _animationDict[animationType].time, 0);
			if (_animationDict[animationType].anim.IsPlaying()) {
				_animationDict[animationType].anim.Complete(false);
			}
		}

		public float Play(TileAnimationType animationType, bool isLoop = false) {
			if (_animationDict[animationType].anim.IsPlaying()) {
				_animationDict[animationType] = (
					_animationDict[animationType].anim,
					_animationDict[animationType].time,
					_animationDict[animationType].playNum + 1
				);

				return _animationDict[animationType].time * (_animationDict[animationType].playNum - 1);
			}

			_animationDict[animationType] = (
				_animationDict[animationType].anim,
				_animationDict[animationType].time,
				isLoop ? -1 : 1
			);
			
			ResetTile();
			_animationDict[animationType].anim.Restart();

			return _animationDict[animationType].time;
		}

		private void ResetTile() {
			_rigidbody.Get(gameObject).velocity = Vector3.zero;
			_rigidbody.Get(gameObject).angularVelocity = Vector3.zero;
		}

		private void InitAnimations() {
			InitShakeAnimation();
			
			InitJumpAnimation();
			
			InitFlipAnimation(true);
			InitFlipAnimation(false);
			
			InitFloatAnimation(true);
			InitFloatAnimation(false);
			InitFloatLittleAnimation();

			InitAttackAnimation();
			InitDefenceAnimation();
			InitHealAnimation();

			InitRotate360Animation();

			InitShockAnimation();
		}

		private Sequence InitShakeAnimation() {
			Sequence shakeAnimation = DOTween.Sequence();
			shakeAnimation.Append(
				_rigidbody.Get(gameObject).DOJump(_tile.Get(gameObject).TilePositionOnGround, 1f, 1, 1f)
			).Insert(
				0.1f, _transform.Get(gameObject).DOShakeRotation(0.3f, 10f, 10, 90f, false)
			).AppendInterval(0.5f)
			.OnComplete(AnimationComplete(TileAnimationType.Shake))
			.SetAutoKill(false).Pause();

			_animationDict.Add(TileAnimationType.Shake, (shakeAnimation, 1f, 0));
			return shakeAnimation;
		}

		private Sequence InitJumpAnimation() {
			Sequence jumpAnimation = DOTween.Sequence();
			jumpAnimation.Append(
				_rigidbody.Get(gameObject).DOJump(_tile.Get(gameObject).TilePositionOnGround, 0.5f, 1, 0.5f)
			).AppendInterval(0.5f)
			.OnComplete(AnimationComplete(TileAnimationType.Jump))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(TileAnimationType.Jump, (jumpAnimation, 1f, 0));
			return jumpAnimation;
		}

		private Sequence InitFlipAnimation(bool isBackWard=false) {
			Sequence flipAnimation = DOTween.Sequence();
			flipAnimation.Append(
				transform.DOPunchPosition(Vector3.up, 1.5f, 1, 0f)
				//_rigidbody.Get(gameObject).DOJump(_tile.Get(gameObject).TilePositionOnGround, 1.5f, 1, 1f)
			).Insert(
				0.1f, _rendererTransform.Get(gameObject).DORotate(
					new Vector3(0, _tile.Get(gameObject).TileRotation.y, isBackWard ? 180 : 0), 
					0.5f
				)
			).AppendInterval(0.5f)
			.OnComplete(AnimationComplete(isBackWard ? TileAnimationType.Flip : TileAnimationType.FlipBack))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(isBackWard ? TileAnimationType.Flip : TileAnimationType.FlipBack, (flipAnimation, 1.5f, 0));
			return flipAnimation;
		}

		private Sequence InitFloatAnimation(bool isUp = false) {
			Sequence floatAnimation = DOTween.Sequence();
			floatAnimation.Append(
				transform.DOMoveY(
					_tile.Get(gameObject).TilePositionOnGround.y + (isUp ? 0.5f : 0), 
					0.5f
				).SetEase(Ease.InOutSine)
			).OnComplete(AnimationComplete(isUp ? TileAnimationType.Float : TileAnimationType.FloatDown))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(isUp ? TileAnimationType.Float : TileAnimationType.FloatDown, (floatAnimation, 0.5f, 0));
			return floatAnimation;
		}

		private Sequence InitFloatLittleAnimation() {
			Sequence floatAnimation = DOTween.Sequence();
			floatAnimation.Append(
				transform.DOMoveY(
					_tile.Get(gameObject).TilePositionOnGround.y + 0.1f, 
					0.3f
				).SetEase(Ease.InOutSine)
			).OnComplete(AnimationComplete(TileAnimationType.FloatLittle))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(TileAnimationType.FloatLittle, (floatAnimation, .3f, 0));
			return floatAnimation;
		}

		private Sequence InitAttackAnimation() {
			Sequence attackAnimation = DOTween.Sequence();
			attackAnimation.Append(
				transform.DOPunchPosition((transform.rotation * Vector3.forward).normalized * 0.5f, 0.3f, 1, 0.1f).SetEase(Ease.InOutSine)
			)
			.OnComplete(AnimationComplete(TileAnimationType.Attack))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(TileAnimationType.Attack, (attackAnimation, 0.4f, 0));
			return attackAnimation;
		}
		
		private Sequence InitDefenceAnimation() {
			Sequence defenceAnimation = DOTween.Sequence();
			defenceAnimation.Append(
				_rendererTransform.Get(gameObject)
				.DODynamicLookAt(_tile.Get(gameObject).TilePositionOnGround + Vector3.up, 0.7f)
				.SetEase(Ease.InOutSine)
			).Join(
				_rendererTransform.Get(gameObject)
					.DOPunchRotation(new Vector3(-80, 0, 0), 0.7f, 1, 0)
					.SetEase(Ease.InOutSine)
			).Join(
				_rendererTransform.Get(gameObject)
					.DOPunchPosition(Vector3.forward * 0.7f, 0.7f, 1, 0.1f)
					.SetEase(Ease.InOutSine)
			)
			.AppendInterval(0.5f)
			.OnComplete(AnimationComplete(TileAnimationType.Defence))
			.SetAutoKill(false).Pause();

			_animationDict.Add(TileAnimationType.Defence, (defenceAnimation, 0.7f, 0));
			return defenceAnimation;
		}

		private Sequence InitHealAnimation() {
			Sequence healAnimation = DOTween.Sequence();
			healAnimation.Append(
				transform.DOPunchPosition(Vector3.up * 0.5f, 1.2f, 1, 0f)
			)
			.OnComplete(AnimationComplete(TileAnimationType.Heal))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(TileAnimationType.Heal, (healAnimation, 1.2f, 0));
			return healAnimation;
		}

		private Sequence InitRotate360Animation() {
			Sequence rotate360Animation = DOTween.Sequence();
			rotate360Animation.Append(
				transform.DOPunchPosition(Vector3.up, 1.5f, 1, 0f)
				//_rigidbody.Get(gameObject).DOJump(_tile.Get(gameObject).TilePositionOnGround, 1.5f, 1, 1f)
			).Insert(
				0.2f, _rendererTransform.Get(gameObject).DORotate(
					new Vector3(0, 0, 360), 
					0.5f,
					RotateMode.LocalAxisAdd
				)
			).AppendInterval(0.5f)
			.OnComplete(AnimationComplete(TileAnimationType.Rotate360))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(TileAnimationType.Rotate360, (rotate360Animation, 1.5f, 0));
			return rotate360Animation;
		}

		private Sequence InitShockAnimation() {
			Sequence shockAnimation = DOTween.Sequence();
			shockAnimation.Append(
				transform.DOPunchPosition(Vector3.up * 1.2f, .7f, 1, 0f)
			).AppendInterval(0.1f)
			.OnComplete(AnimationComplete(TileAnimationType.Shock))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(TileAnimationType.Shock, (shockAnimation, 1f, 0));
			return shockAnimation;
		}

		private TweenCallback AnimationComplete(TileAnimationType animationType) {
			return () => {
				if (_animationDict[animationType].playNum == 0) return;
				if (_animationDict[animationType].playNum == -1) {
					_animationDict[animationType].anim.Restart();
					return;
				}

				_animationDict[animationType] = (
					_animationDict[animationType].anim, 
					_animationDict[animationType].time,
					_animationDict[animationType].playNum - 1
				);

				if (_animationDict[animationType].playNum == 0) {
					_animationDict[animationType].anim.Pause();
				} else {
					_animationDict[animationType].anim.Restart();
				}
			};
		}
	}

}
