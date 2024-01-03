using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using Util;

namespace Cardinals.UI.Description
{
    public class UIPotion : MonoBehaviour
    {
        private Potion _curPotion;
        private Button _button;
        private int _index;

        private DG.Tweening.Sequence _shakeSeq;

        private ObjectGetter _usePanel = new ObjectGetter(TypeOfGetter.ChildByName, "Canvas");
        //private ComponentGetter<RectTransform> _mouseArea = new (TypeOfGetter.ChildByName, "Canvas/CanDeletePanel/MouseArea");
        private ComponentGetter<Button> _useBTN = new (TypeOfGetter.ChildByName, "Canvas/CanDeletePanel/UseBTN");
        private ComponentGetter<Button> _deleteBTN = new (TypeOfGetter.ChildByName, "Canvas/CanDeletePanel/DeleteBTN");
        
        void Start()
        {
            // 버튼 상호 작용
            _button = transform.AddComponent<Button>();
            _button.onClick.AddListener(Click);
            _useBTN.Get(gameObject).onClick.AddListener(Use);
            _deleteBTN.Get(gameObject).onClick.AddListener(Delete);
            _usePanel.Get(gameObject).SetActive(false);

            _useBTN.Get(gameObject).GetComponentInChildren<TextMeshProUGUI>().text
                = GameManager.I.Localization[LocalizationEnum.UI_ITEM_USE];
            _deleteBTN.Get(gameObject).GetComponentInChildren<TextMeshProUGUI>().text 
                = GameManager.I.Localization[LocalizationEnum.UI_ITEM_REMOVE];
            
            // Use Panel 마우스 이벤트 설정
            var trigger = transform.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                transform.AddComponent<EventTrigger>();
            }
            
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener( (eventData) => { SetUsePanelActive(false); } );
            trigger.triggers.Add(entry);

            // skakeSeq
            _shakeSeq = DOTween.Sequence();
            _shakeSeq.Append(
                (transform as RectTransform).DOShakeAnchorPos(0.3f, 5f)
            ).SetAutoKill(false).Pause();
        }

        public void Init(int index)
        {
            _index = index;
            GetComponent<UIIcon>().Init(null, new Color(0.88f, 0.88f, 0.88f, 1f));
        }
        

        public void Set(Potion potion)
        {
            _curPotion = potion;
            transform.AddComponent<PotionDescription>().Init(potion.PotionType);
            GetComponent<UIIcon>().Init(potion.Sprite);
        }

        void Click()
        {
            if (_curPotion != null)
            {
                GetComponent<DescriptionConnector>().OffPanel();
                SetUsePanelActive(true);
            }
        }
        
        void Use()
        {
            if (_curPotion != null)
            {
                if ( GameManager.I.Stage.IsPlayerTurn &&
                     GameManager.I.Player.PlayerInfo.UsePotion(_index))
                {
                    GetComponent<UIIcon>().Init(null, new Color(0.88f, 0.88f, 0.88f, 1f));

                    var descs = GetComponents<PotionDescription>();
                    for (int i = descs.Length - 1; i >= 0; i--)
                    {
                        Destroy(descs[i]);
                    }
                    _curPotion = null;

                    GetComponent<DescriptionConnector>().OffPanel();
                } else
                {  
                    if (_shakeSeq.IsPlaying()) _shakeSeq.Restart();
                    else _shakeSeq.Play();
                    GameManager.I.Player.Bubble.SetBubble(GameManager.I.Localization.Get(LocalizationEnum.PLAYER_SCRIPT_POTION));
                }

                SetUsePanelActive(false);
            }
        }

        void Delete()
        {
            if (_curPotion != null)
            {
                GameManager.I.Player.PlayerInfo.DeletePotion(_index);
                GetComponent<UIIcon>().Init(null, new Color(0.88f, 0.88f, 0.88f, 1f));
                var descs = GetComponents<PotionDescription>();
                for (int i = descs.Length - 1; i >= 0; i--)
                {
                    Destroy(descs[i]);
                }
                
                _curPotion = null;
                SetUsePanelActive(false);
            }
        }

        void SetUsePanelActive(bool state)
        {
            _usePanel.Get(gameObject).SetActive(state);
        }
    }
}
