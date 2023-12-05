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

        private ComponentGetter<AudioSource> _audioSource
            = new ComponentGetter<AudioSource>(TypeOfGetter.This);

        #region Common UI
        public void ButtonClick() {
            if (_buttonClick == null) return;

            _audioSource.Get(gameObject).PlayOneShot(_buttonClick);
        }
        #endregion

        #region Card
        public void CardClick() {
            if (_cardClick == null) return;

            _audioSource.Get(gameObject).PlayOneShot(_cardClick);
        }

        public void CardUse() {
            if (_cardUse == null) return;

            _audioSource.Get(gameObject).PlayOneShot(_cardUse);
        }

        public void CardDraw() {
            if (_cardDraw == null) return;

            _audioSource.Get(gameObject).PlayOneShot(_cardDraw);
        }

        public void CardDiscard() {
            if (_cardDiscard == null) return;

            _audioSource.Get(gameObject).PlayOneShot(_cardDiscard);
        }
        #endregion

        #region Player
        public void PlayerMove() {
            if (_playerMove == null) return;

            _audioSource.Get(gameObject).PlayOneShot(_playerMove);
        }
        #endregion
    }
}