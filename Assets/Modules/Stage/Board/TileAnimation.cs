using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using DG.Tweening;
using Util;

namespace Cardinals.Board {

	public class TileAnimation : MonoBehaviour {
		private ComponentGetter<MMF_Player> _MMFPlayer
			= new ComponentGetter<MMF_Player>(TypeOfGetter.This);

		private ComponentGetter<Transform> _transform
			= new ComponentGetter<Transform>(TypeOfGetter.This);

		private ComponentGetter<Rigidbody> _rigidbody
			= new ComponentGetter<Rigidbody>(TypeOfGetter.This);

		private Sequence _shakeSequence;

		public void Awake() {
			_shakeSequence = DOTween.Sequence();
			_shakeSequence.Append(
				_rigidbody.Get(gameObject).DOJump(_transform.Get(gameObject).position, 0.3f, 1, 0.5f)
				//_transform.Get(gameObject).DOShakeRotation(0.5f, 10f, 10, 90f, false)
			).Insert(0.1f,
				_transform.Get(gameObject).DOShakeRotation(0.3f, 10f, 10, 90f, false)
			).SetAutoKill(false).Pause();
		}

		public void Shake() {
			_rigidbody.Get(gameObject).velocity = Vector3.zero;
			_rigidbody.Get(gameObject).angularVelocity = Vector3.zero;
			//_MMFPlayer.Get(gameObject).PlayFeedbacks();
			//_transform.Get(gameObject).DOShakeRotation(0.5f, 10f, 10, 90f, false);

			_shakeSequence.Restart();
		}
	}

}
