using Cardinals.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals
{
    public class UIBuff : MonoBehaviour, IDescription
    {
        private BaseBuff BaseBuff { get; set; }

        [Header("Component")] 
        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _buffCountTMP;
        [SerializeField] private Transform _descriptionTr;
        
        public void Init(BaseBuff baseBuff)
        {
            BaseBuff = baseBuff;

            _iconImg.sprite = baseBuff.Data.sprite;
            UpdateBuffCount(baseBuff.Count);
            
            BaseBuff.RemoveEvent += Destroy;
            BaseBuff.UpdateBuffCountEvent += UpdateBuffCount;
        }

        private void UpdateBuffCount(int buffCount)
        {
            _buffCountTMP.text = buffCount == 0 ? string.Empty : buffCount.ToString();
        }

        private void Destroy()
        {
            BaseBuff = null;
            Destroy(gameObject);
        }

        #region IDescription

        public string Name => BaseBuff.Data.buffName;
        public string Description => BaseBuff.Data.description;
        public Sprite IconSprite => BaseBuff.Data.sprite;
        public Transform InstTr => _descriptionTr;

        #endregion
    }
}