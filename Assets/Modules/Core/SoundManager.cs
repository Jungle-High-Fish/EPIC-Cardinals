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

        private ComponentGetter<AudioSource> _audioSource
            = new ComponentGetter<AudioSource>(TypeOfGetter.This);

        public void ButtonClick() {
            _audioSource.Get(gameObject).PlayOneShot(_buttonClick);
        }

        public void CardClick() {
            _audioSource.Get(gameObject).PlayOneShot(_cardClick);
        }

        public void CardUse() {
            _audioSource.Get(gameObject).PlayOneShot(_cardDraw);
        } 
    }
}