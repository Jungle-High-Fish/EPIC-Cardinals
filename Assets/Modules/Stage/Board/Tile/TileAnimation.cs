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
		private ComponentGetter<MMF_Player> _MMFPlayer
			= new ComponentGetter<MMF_Player>(TypeOfGetter.This);

		private ComponentGetter<Transform> _transform
			= new ComponentGetter<Transform>(TypeOfGetter.This);

		private ComponentGetter<Rigidbody> _rigidbody
			= new ComponentGetter<Rigidbody>(TypeOfGetter.This);

		private ComponentGetter<Tile> _tile
			= new ComponentGetter<Tile>(TypeOfGetter.This);

		private Dictionary<TileAnimationType, (Sequence anim, int playNum)> _animationDict
			= new Dictionary<TileAnimationType, (Sequence, int)>();

		public void Init() {
			InitAnimations();
		}

		public void StopAll() {
			List<TileAnimationType> animationTypes = new List<TileAnimationType>(_animationDict.Keys);
			foreach (var animationType in animationTypes) {
				_animationDict[animationType] = (_animationDict[animationType].anim, 0);
				if (_animationDict[animationType].anim.IsPlaying()) {
					_animationDict[animationType].anim.Complete(false);
				}
			}
		}

		public void Play(TileAnimationType animationType, bool isLoop = false) {
			if (_animationDict[animationType].anim.IsPlaying()) {
				_animationDict[animationType] = (
					_animationDict[animationType].anim, 
					_animationDict[animationType].playNum + 1
				);

				return;
			}

			_animationDict[animationType] = (
				_animationDict[animationType].anim, 
				isLoop ? -1 : 1
			);
			
			ResetTile();
			_animationDict[animationType].anim.Restart();
		}

		private void ResetTile() {
			_rigidbody.Get(gameObject).velocity = Vector3.zero;
			_rigidbody.Get(gameObject).angularVelocity = Vector3.zero;
		}

		private void InitAnimations() {
			InitShakeAnimation();
			InitJumpAnimation();
		}

		private Sequence InitShakeAnimation() {
			Sequence shakeAnimation = DOTween.Sequence();
			shakeAnimation.Append(
				_rigidbody.Get(gameObject).DOJump(_tile.Get(gameObject).TilePositionOnGround, 1.5f, 1, 1f)
			).Insert(0.1f,
				_transform.Get(gameObject).DOShakeRotation(0.3f, 20f, 10, 90f, false)
			).AppendInterval(0.5f)
			.OnComplete(AnimationComplete(TileAnimationType.Shake))
			.SetAutoKill(false).Pause();

			_animationDict.Add(TileAnimationType.Shake, (shakeAnimation, 0));
			return shakeAnimation;
		}

		private Sequence InitJumpAnimation() {
			Sequence jumpAnimation = DOTween.Sequence();
			jumpAnimation.Append(
				_rigidbody.Get(gameObject).DOJump(_tile.Get(gameObject).TilePositionOnGround, 0.5f, 1, 0.5f)
			).AppendInterval(0.5f)
			.OnComplete(AnimationComplete(TileAnimationType.Jump))
			.SetAutoKill(false).Pause();
			
			_animationDict.Add(TileAnimationType.Jump, (jumpAnimation, 0));
			return jumpAnimation;
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
