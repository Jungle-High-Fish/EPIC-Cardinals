using System;
using Cardinals.BoardEvent.Card;
using Cardinals.BoardEvent.Roulette;
using Cardinals.BoardEvent.Shop;
using Cardinals.BoardEvent.Tile;
using Cardinals.Enemy;
using Cardinals.Game;
using Cardinals.UI;
using Cardinals.UI.Description;
using Cardinals.Tutorial;
using Unity.VisualScripting;
using UnityEngine;
using Util;
using System.Collections.Generic;
using System.Linq;

namespace Cardinals
{
    public class SoundManager: MonoBehaviour
    {   
        [Header("BGM")]
        [SerializeField] private AudioClip _bgm;

        [Header("Common UI")]
        [SerializeField] private AudioClip _buttonClick;

        [Header("Card")]
        [SerializeField] private AudioClip _cardClick;
        [SerializeField] private AudioClip _cardDraw;
        [SerializeField] private AudioClip _cardDiscard;
        [SerializeField] private AudioClip _cardUse;

        [Header("Player")]
        [SerializeField] private AudioClip _playerMove;

        [Header("Setting")]
        [SerializeField, Range(0f, 1f)] private float _bgmVolume = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _effectVolume = 0.5f;

        private ComponentGetter<AudioSource> _effectAudioSource1
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 1");
        private ComponentGetter<AudioSource> _effectAudioSource2
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 2");
        private ComponentGetter<AudioSource> _effectAudioSource3
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 3");
        private ComponentGetter<AudioSource> _effectAudioSource4
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 4");

        private Queue<AudioClip> _effectAudioClipQueue = new Queue<AudioClip>();
        private List<AudioSource> _effectAudioSourceList = new List<AudioSource>();

        #region Common UI
        public void ButtonClick() {
            if (_buttonClick == null) return;

            _effectAudioClipQueue.Enqueue(_buttonClick);
        }
        #endregion

        #region Card
        public void CardClick() {
            if (_cardClick == null) return;

            _effectAudioClipQueue.Enqueue(_cardClick);
        }

        public void CardUse() {
            if (_cardUse == null) return;

            _effectAudioClipQueue.Enqueue(_cardUse);
        }

        public void CardDraw() {
            if (_cardDraw == null) return;

            _effectAudioClipQueue.Enqueue(_cardDraw);
        }

        public void CardDiscard() {
            if (_cardDiscard == null) return;

            _effectAudioClipQueue.Enqueue(_cardDiscard);
        }
        #endregion

        #region Player
        public void PlayerMove() {
            if (_playerMove == null) return;

            _effectAudioClipQueue.Enqueue(_playerMove);
        }

        private void Awake() {
            _effectAudioSourceList.Add(_effectAudioSource1.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource2.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource3.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource4.Get(gameObject));
        }

        private void Update() {
            if (_effectAudioClipQueue.Count > 0) {
                var targetAudioSource = _effectAudioSourceList.FirstOrDefault(x => !x.isPlaying);
                if (targetAudioSource != null) {
                    targetAudioSource.volume = _effectVolume;
                    targetAudioSource.PlayOneShot(_effectAudioClipQueue.Dequeue());
                }
            }
        }
        #endregion
    }
}