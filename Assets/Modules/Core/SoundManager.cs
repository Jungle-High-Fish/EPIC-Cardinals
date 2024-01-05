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
    public class SoundManager : MonoBehaviour
    {
        [Header("BGM")]
        [SerializeField] private AudioClip _bgm;

        [Header("Common UI")]
        [SerializeField] private AudioClip _buttonClick;
        [SerializeField] private AudioClip _titlebuttonClick;
        [SerializeField] private AudioClip _getCoin;
        [SerializeField] private AudioClip _getPotion;
        [SerializeField] private AudioClip _turnAlert;
        [SerializeField] private AudioClip _blessDrop;
        [SerializeField] private AudioClip _thunderBolt;
        [SerializeField] private AudioClip _meteorDrop;
        [SerializeField] private AudioClip _meteorBomb;
        [SerializeField] private AudioClip _tutorialClear;


        [Header("Card")]
        [SerializeField] private AudioClip _cardClick;
        [SerializeField] private AudioClip _cardDraw;
        [SerializeField] private AudioClip _cardDiscard;
        [SerializeField] private AudioClip _cardUse;
        [SerializeField] private AudioClip _cardReroll1;
        [SerializeField] private AudioClip _cardReroll2;
        [SerializeField] private AudioClip _cardReroll3;
        [SerializeField] private AudioClip _cardHover;
        [SerializeField] private AudioClip _getDice;
        [SerializeField] private AudioClip _diceDeter;
        [SerializeField] private AudioClip _diceMove;


        [Header("Player")]
        [SerializeField] private AudioClip _playerMove;
        [SerializeField] private AudioClip _playerHit;
        [SerializeField] private AudioClip _playerDefenseHit;
        [SerializeField] private AudioClip _playerHeal;

        [Header("TileEffect")]
        [SerializeField] private AudioClip _normalBall1;
        [SerializeField] private AudioClip _getDefense;
        [SerializeField] private AudioClip _fireBall1;
        [SerializeField] private AudioClip _fireBall2;
        [SerializeField] private AudioClip _waterHeal;
        [SerializeField] private AudioClip _earthDefense;
        [SerializeField] private AudioClip _tile;

        private ComponentGetter<AudioSource> _effectAudioSource1
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 1");
        private ComponentGetter<AudioSource> _effectAudioSource2
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 2");
        private ComponentGetter<AudioSource> _effectAudioSource3
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 3");
        private ComponentGetter<AudioSource> _effectAudioSource4
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 4");
        private ComponentGetter<AudioSource> _effectAudioSource5
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 5");
        private ComponentGetter<AudioSource> _effectAudioSource6
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 6");
        private ComponentGetter<AudioSource> _effectAudioSource7
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 7");
        private ComponentGetter<AudioSource> _effectAudioSource8
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 8");
        private ComponentGetter<AudioSource> _effectAudioSource9
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 9");
        private ComponentGetter<AudioSource> _effectAudioSource10
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 10");
        private ComponentGetter<AudioSource> _effectAudioSource11
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 11");
        private ComponentGetter<AudioSource> _effectAudioSource12
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "Effect Sound Source 12");

        private ComponentGetter<AudioSource> _bgmAudioSource
            = new ComponentGetter<AudioSource>(TypeOfGetter.ChildByName, "BGM Sound Source 1");

        private Queue<AudioClip> _effectAudioClipQueue = new Queue<AudioClip>();
        private List<AudioSource> _effectAudioSourceList = new List<AudioSource>();

        #region Common UI
        public void ButtonClick()
        {
            if (_buttonClick == null) return;

            _effectAudioClipQueue.Enqueue(_buttonClick);
        }

        public void TitleButtonClick()
        {
            if (_titlebuttonClick == null) return;

            _effectAudioClipQueue.Enqueue(_titlebuttonClick);
        }

        public void GetCoin()
        {
            if (_getCoin == null) return;

            _effectAudioClipQueue.Enqueue(_getCoin);
        }

        public void TurnAlert()
        {
            if (_turnAlert == null) return;

            _effectAudioClipQueue.Enqueue(_turnAlert);
        }

        public void BlessDrop()
        {
            if (_blessDrop == null) return;

            _effectAudioClipQueue.Enqueue(_blessDrop);
        }

        public void GetPotion()
        {
            if (_getPotion == null) return;

            _effectAudioClipQueue.Enqueue(_getPotion);
        }

        public void ThunderBolt()
        {
            if (_thunderBolt == null) return;

            _effectAudioClipQueue.Enqueue(_thunderBolt);
        }


        public void MeteorDrop()
        {
            if (_meteorDrop == null) return;

            _effectAudioClipQueue.Enqueue(_meteorDrop);
        }

        public void MeteorBomb()
        {
            if (_meteorBomb == null) return;

            _effectAudioClipQueue.Enqueue(_meteorBomb);
        }

        public void TutorialClear()
        {
            if (_tutorialClear == null) return;

            _effectAudioClipQueue.Enqueue(_tutorialClear);
        }

        #endregion

        #region Card
        public void CardClick()
        {
            if (_cardClick == null) return;

            _effectAudioClipQueue.Enqueue(_cardClick);
        }

        public void CardUse()
        {
            if (_cardUse == null) return;

            _effectAudioClipQueue.Enqueue(_cardUse);
        }

        public void CardDraw()
        {
            if (_cardDraw == null) return;

            _effectAudioClipQueue.Enqueue(_cardDraw);
        }

        public void CardDiscard()
        {
            if (_cardDiscard == null) return;

            _effectAudioClipQueue.Enqueue(_cardDiscard);
        }

        public void DiceHover()
        {
            if (_cardHover == null) return;

            _effectAudioClipQueue.Enqueue(_cardHover);
        }

        public void DiceDetermine()
        {
            if (_diceDeter == null) return;

            _effectAudioClipQueue.Enqueue(_diceDeter);
        }

        public void DiceMove()
        {
            if (_diceMove == null) return;

            _effectAudioClipQueue.Enqueue(_diceMove);
        }
        public void CardReroll()
        {
            if (_cardReroll1 == null) return;


            AudioClip[] cardRerollOptions = { _cardReroll1, _cardReroll2, _cardReroll3 };

            int randomIndex = UnityEngine.Random.Range(0, cardRerollOptions.Length);

            _effectAudioClipQueue.Enqueue(cardRerollOptions[randomIndex]);
        }

        public void GetDice()
        {
            if (_getDice == null) return;

            _effectAudioClipQueue.Enqueue(_getDice);
        }
        #endregion

        #region Player
        public void PlayerMove()
        {
            if (_playerMove == null) return;

            _effectAudioClipQueue.Enqueue(_playerMove);
        }

        public void PlayerHit()
        {
            if (_playerHit == null) return;

            _effectAudioClipQueue.Enqueue(_playerHit);

        }

        public void PlayerHeal()
        {
            if (_playerHeal == null) return;

            _effectAudioClipQueue.Enqueue(_playerHeal);

        }

        public void PlayerDefenseHit()
        {
            if (_playerDefenseHit == null) return;

            _effectAudioClipQueue.Enqueue(_playerDefenseHit);

        }

        public void PlayBGM()
        {
            if (_bgm == null) return;

            _bgmAudioSource.Get(gameObject).clip = _bgm;
            _bgmAudioSource.Get(gameObject).volume = GameManager.I.GameSetting.BgmVolume / 100f;
            _bgmAudioSource.Get(gameObject).loop = true;
            _bgmAudioSource.Get(gameObject).Play();
        }

        private void Awake()
        {
            _effectAudioSourceList.Add(_effectAudioSource1.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource2.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource3.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource4.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource5.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource6.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource7.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource8.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource9.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource10.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource11.Get(gameObject));
            _effectAudioSourceList.Add(_effectAudioSource12.Get(gameObject));

            GameManager.I.GameSetting.OnSoundSettingChanged -= OnSoundSettingChange;
            GameManager.I.GameSetting.OnSoundSettingChanged += OnSoundSettingChange;
        }

        private void Update()
        {
            if (_effectAudioClipQueue.Count > 0)
            {
                var targetAudioSource = _effectAudioSourceList.FirstOrDefault(x => !x.isPlaying);
                if (targetAudioSource != null)
                {
                    targetAudioSource.volume = GameManager.I.GameSetting.SfxVolume / 100f;
                    targetAudioSource.PlayOneShot(_effectAudioClipQueue.Dequeue());
                }
            }
        }

        private void OnDisable()
        {
            if (GameManager.I != null)
            {
                GameManager.I.GameSetting.OnSoundSettingChanged -= OnSoundSettingChange;
            }
        }

        private void OnSoundSettingChange()
        {
            _bgmAudioSource.Get(gameObject).volume = GameManager.I.GameSetting.BgmVolume / 100f;
            _effectAudioSourceList.ForEach(x => x.volume = GameManager.I.GameSetting.SfxVolume / 100f);
        }
        #endregion

        #region TileMagic
        public void FlyFireBall()
        {
            if (_fireBall1 == null) return;

            _effectAudioClipQueue.Enqueue(_fireBall1);
        }

        public void BombFireBall()
        {
            if (_fireBall2 == null) return;

            _effectAudioClipQueue.Enqueue(_fireBall2);
        }

        public void BombNormalBall()
        {
            if (_normalBall1 == null) return;

            _effectAudioClipQueue.Enqueue(_normalBall1);
        }

        public void GetDefense()
        {
            if (_getDefense == null) return;

            _effectAudioClipQueue.Enqueue(_getDefense);
        }

        public void WaterHeal()
        {
            if (_waterHeal == null) return;

            _effectAudioClipQueue.Enqueue(_waterHeal);
        }

        public void EarthDefense()
        {
            if (_earthDefense == null) return;

            _effectAudioClipQueue.Enqueue(_earthDefense);
        }

        public void Tile()
        {
            if (_tile == null) return;

            _effectAudioClipQueue.Enqueue(_tile);
        }
        #endregion


    }
}