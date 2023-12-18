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

        private ComponentGetter<AudioSource> _effectAudioSource1
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 1");
        private ComponentGetter<AudioSource> _effectAudioSource2
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 2");
        private ComponentGetter<AudioSource> _effectAudioSource3
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 3");
        private ComponentGetter<AudioSource> _effectAudioSource4
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 4");

        private ComponentGetter<AudioSource> _bgmAudioSource
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "BGM Sound Source 1");

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

        public void PlayBGM() {
            if (_bgm == null) return;

            _bgmAudioSource.Get(gameObject).clip = _bgm;
            _bgmAudioSource.Get(gameObject).volume = GameManager.I.GameSetting.BgmVolume / 100f;
            _bgmAudioSource.Get(gameObject).loop = true;
            _bgmAudioSource.Get(gameObject).Play();
        }

        private void Awake() {
            _effectAudioSourceList.Add(_effectAudioSource1.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource2.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource3.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource4.Get(gameObject));
            
            GameManager.I.GameSetting.OnSoundSettingChanged -= OnSoundSettingChange;
            GameManager.I.GameSetting.OnSoundSettingChanged += OnSoundSettingChange;
        }

        private void Update() {
            if (_effectAudioClipQueue.Count > 0) {
                var targetAudioSource = _effectAudioSourceList.FirstOrDefault(x => !x.isPlaying);
                if (targetAudioSource != null) {
                    targetAudioSource.volume = GameManager.I.GameSetting.SfxVolume / 100f;
                    targetAudioSource.PlayOneShot(_effectAudioClipQueue.Dequeue());
                }
            }
        }

        private void OnDisable() {
            GameManager.I.GameSetting.OnSoundSettingChanged -= OnSoundSettingChange;
        }

        private void OnSoundSettingChange() {
            _bgmAudioSource.Get(gameObject).volume = GameManager.I.GameSetting.BgmVolume / 100f;
            _effectAudioSourceList.ForEach(x => x.volume = GameManager.I.GameSetting.SfxVolume / 100f);
        }
        #endregion
    }
}