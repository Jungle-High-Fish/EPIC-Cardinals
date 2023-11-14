using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals
{
    public class UIBuff : MonoBehaviour
    {
        private BaseBuff BaseBuff { get; set; }

        [Header("Component")] 
        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _buffCountTMP;

        public void Init(BaseBuff baseBuff)
        {
            BaseBuff = baseBuff;

            _iconImg.sprite = null; // [TODO] 버프 아이콘 업데이트 필요
            UpdateBuffCount(baseBuff.Count);
            
            BaseBuff.RemoveEvent += Destroy;
            BaseBuff.UpdateBuffCountEvent += UpdateBuffCount;
        }

        private void UpdateBuffCount(int buffCount)
        {
            _buffCountTMP.text = $"{buffCount}";
        }

        private void Destroy()
        {
            BaseBuff = null;
            Destroy(gameObject);
        }
    }
}