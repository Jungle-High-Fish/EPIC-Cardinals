using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Util;
using Sirenix.OdinInspector;
using Cardinals.Enums;
using Cardinals.UI;
using Cardinals.Game;
using UnityEngine.UI;

namespace Cardinals
{
    public class UIPlayerInfo : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [Header("HP")]
        [SerializeField] private TextMeshProUGUI _hpTMP;
        [SerializeField] private RectTransform _maxHPRect;
        [SerializeField] private RectTransform _curHPRect;
        [SerializeField] private float _curHPEndPosX;

        [Header("Defense")]
        [SerializeField] private TextMeshProUGUI _defenseTMP;

        [Header("Buff")]
        [SerializeField] private GameObject _buffPrefab;

        [Header("Description")]
        [SerializeField] private Transform _descriptionInstTr;
        public Transform DescriptionInstTr => _descriptionInstTr;

        private ComponentGetter<Button> _detailInfoOpenButton = new ComponentGetter<Button>(
            TypeOfGetter.ChildByName,
            "Player Status/Detail Info Open Button"
        );

        private ComponentGetter<Transform> _buffListArea = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName,
            "Player Status/Buff List Area/"
        );

        private ComponentGetter<UIPlayerDetailInfo> _detailInfoPanel = new ComponentGetter<UIPlayerDetailInfo>(
            TypeOfGetter.ChildByName,
            "Player Status/Detail Info Panel"
        );

        private ComponentGetter<Transform> _alertTr = new ComponentGetter<Transform>(
            TypeOfGetter.ChildByName,
            "Player Status/Alert Image"
        );

        [Header("Panel")]
        [SerializeField] private GameObject _playerInfoPanel;
        [SerializeField] private float _panelMoveDistance;

        [Header("Bless")]
        [SerializeField] private Transform _blessTr;
        
        [Header("Potion")]
        [SerializeField] private Transform _potionTr;

        [Header("Artifact")]
        [SerializeField] private Transform _artifactTr;
        private bool _isPanelOpen;
        
        public void Init()
        {
            _player = GameManager.I.Player;
            _player.UpdateHpEvent += UpdateHp;
            _player.AddNewBuffEvent += AddBuff;
            _player.UpdateDefenseEvent += UpdateDefense;

            _player.PlayerInfo.AddPotionEvent += (_, __) => { ShowAlertImage(); };
            _player.PlayerInfo.AddArtifactEvent += (_) => { ShowAlertImage(); };
            _player.PlayerInfo.AddBlessEvent += (_) => { ShowAlertImage(); };

            _detailInfoOpenButton.Get(gameObject).onClick.AddListener(OpenPanel);
            _detailInfoPanel.Get(gameObject).Init();
            _detailInfoPanel.Get(gameObject).gameObject.SetActive(false);

            _alertTr.Get(gameObject).gameObject.SetActive(false);
            
            // init
            UpdateHp(_player.Hp, _player.MaxHp);
        }

        private void ShowAlertImage() {
            if (_isPanelOpen) {
                return;
            }

            _alertTr.Get(gameObject).gameObject.SetActive(true);
            _alertTr.Get(gameObject).transform.localScale = Vector3.one * 1.5f;
            _alertTr.Get(gameObject).transform
                .DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
        }

        private void HideAlertImage() {
            if (_alertTr.Get(gameObject).gameObject.activeSelf == false) {
                return;
            }

            _alertTr.Get(gameObject).transform.localScale = Vector3.one;
            _alertTr.Get(gameObject).transform
                .DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutCubic).OnComplete(() => {
                    _alertTr.Get(gameObject).gameObject.SetActive(false);
                });
        }

        private void UpdateHp(int hp, int maxHp)
        {
            float curHPPosX = Mathf.Lerp(_curHPEndPosX, 0, (float)hp / maxHp);
            _curHPRect.localPosition = new Vector3(curHPPosX, 0, 0);
            _hpTMP.text = $"{hp}/{maxHp}";
        }

        private void UpdateDefense(int defense)
        {
            _defenseTMP.text = defense.ToString();
        }
        private void AddBuff(BaseBuff baseBuff)
        {
            Instantiate(_buffPrefab, _buffListArea.Get(gameObject)).GetComponent<UIBuff>().Init(baseBuff);
        }
        
        public void OpenPanel()
        {
            if (_isPanelOpen)
            {
                _detailInfoPanel.Get(gameObject).transform.localScale = Vector3.one;
                _detailInfoPanel.Get(gameObject).transform
                    .DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutCubic).OnComplete(() =>
                    {
                        _detailInfoPanel.Get(gameObject).Deactivate();
                    });

                _isPanelOpen = false;
                Debug.Log("close");
            }
            else
            {
                _detailInfoPanel.Get(gameObject).transform.localScale = Vector3.zero;
                _detailInfoPanel.Get(gameObject).Activate();
                _detailInfoPanel.Get(gameObject).transform
                    .DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutCubic);
                _isPanelOpen = true;

                HideAlertImage();
                Debug.Log("open");
            }
        }

        [Button]
        public void TestPotion(PotionType potionType)
        {
            _player.PlayerInfo.AddPotion(potionType);
        }

        [Button]
        public void TestArtifact(ArtifactType artifactType)
        {
            _player.PlayerInfo.AddArtifact(artifactType);
        }

        [Button]
        public void DebugArtifactList()
        {
            _player.PlayerInfo.DebugArtifactList();
        }
    }
}

