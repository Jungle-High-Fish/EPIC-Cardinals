using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cardinals.Entity.UI;
using Cardinals.Enums;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

namespace Cardinals.UI
{
    public class UIEntityStatus : MonoBehaviour
    {
        protected BaseEntity _entity;
        
        [Header("Status")]
        [SerializeField] protected RectTransform _statusTr;
        
        [Header("HP")]
        [SerializeField] private TextMeshProUGUI _hpTMP;
        [SerializeField] protected RectTransform _maxHPRect;
        [SerializeField] private RectTransform _orangeHPRect;
        [SerializeField] private RectTransform _curHPRect;
        
        [SerializeField] private TextMeshProUGUI _damageTextTMP;

        [Header("Defense")]
        [SerializeField] private GameObject _defenseObj;
        [SerializeField] private TextMeshProUGUI _defenseTMP;

        [Header("Buff")]
        [SerializeField] private GameObject _buffPrefab;
        [SerializeField] protected Transform _buffListArea;
        [SerializeField] private Transform _addBuffDescriptionTr;
        
        public virtual void Init(BaseEntity entity)
        {
            _entity = entity;
            
            _entity.UpdateHpEvent += UpdateHp;
            _entity.AddNewBuffEvent += AddNewBuff;
            _entity.UpdateDefenseEvent += UpdateDefense;
            _entity.AddBuffEvent += AddBuff;
            _entity.ExecuteBuffEvent += ExecuteBuff;
            _entity.SuccessDefenseEvent += SuccessDefense;
            _entity.BrokenDefenseEvent += BrokenDefense;
            _entity.ValidChangedHPEvent = PrintValueRelatedHp;
            
            UpdateHp(_entity.Hp, _entity.MaxHp);
        }
        
        public void Update()
        {
            if (_entity != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(_entity.transform.position) ;
            }
        }
        
        [Button]
        private void UpdateHp(int hp, int maxHp)
        {
            Vector3 vector3 = _curHPRect.localScale;
            vector3.x = (float) hp / maxHp;
            _curHPRect.localScale = vector3;
            _orangeHPRect.DOScaleX(vector3.x, .3f).SetDelay(0.3f).SetEase(Ease.InOutQuint);
            
            _hpTMP.text = $"{hp}/{maxHp}";
            _maxHPRect.DOPunchScale(new Vector3(.5f, .5f, 1), .1f, 1)
                      .OnComplete(() => {_maxHPRect.localScale = Vector3.one; });
        }

        private void UpdateDefense(int defense)
        {
            if (defense != 0)
            {
                _defenseObj.SetActive(true);
                _defenseObj.transform.DOPunchScale(new Vector3(.5f, .5f, 1), .3f, 2)
                                     .OnComplete(() => {_defenseObj.transform.localScale = Vector3.one; });
                _defenseTMP.text = defense.ToString();
            }
            else
            {
                if (_defenseObj.activeSelf)
                {
                    _defenseObj.transform.DOScale(0, .5f)
                        .SetEase(Ease.InOutElastic)
                        .OnComplete(() =>
                        {
                            _defenseObj.transform.localScale = Vector3.one;
                            _defenseObj.SetActive(false);
                        });
                }
            }
        }
        
        private void AddNewBuff(BaseBuff baseBuff)
        {
            Instantiate(_buffPrefab, _buffListArea).GetComponent<UIBuff>().Init(baseBuff);
        }
        
        private void AddBuff(BaseBuff baseBuff)
        {
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Entity_AddBuffDescription);
            Instantiate(prefab, _addBuffDescriptionTr).GetComponent<AddBuffDescription>().Init(baseBuff.Data);
        }

        private void ExecuteBuff(BaseBuff baseBuff)
        {
            if (baseBuff.Data.effectSprite != null)
            {
                var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Entity_ExecuteBuffPrefab);
                Instantiate(prefab, _addBuffDescriptionTr).GetComponent<ExecuteBuff>().Init(baseBuff.Data.effectSprite);
            }
        }

        private void SuccessDefense()
        {
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Entity_ExecuteBuffPrefab);
            var sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprite_UI_Entity_SuccessDefense);
            Instantiate(prefab, _addBuffDescriptionTr).GetComponent<ExecuteBuff>().Init(sprite);
        }
        
        private void BrokenDefense()
        {
            var prefab = ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_Entity_ExecuteBuffPrefab);
            var sprite = ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprite_UI_Entity_BrokenDefense);
            Instantiate(prefab, _addBuffDescriptionTr).GetComponent<ExecuteBuff>().Init(sprite);
        }

        public void PrintValueRelatedHp(int value, Color color)
        {
            var obj = Instantiate(_damageTextTMP.gameObject, _damageTextTMP.transform.parent as RectTransform);
            var rect = obj.transform as RectTransform; 
            rect.position += new Vector3(Random.Range(-50, 50f), Random.Range(-50, 20f));
            obj.SetActive(true);
            
            var tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = $"{(value > 0 ? "+" : "")}{value}";
            tmp.color = color;

            rect.DOMoveY(100, 1.5f).SetRelative()
                .OnComplete(() => Destroy(tmp.gameObject));
        }
    }
}