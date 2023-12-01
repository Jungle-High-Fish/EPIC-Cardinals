using Cardinals.UI;
using Cardinals.UI.Description;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] private Transform _descriptionTr;
        
        public void Init(BaseBuff baseBuff)
        {
            BaseBuff = baseBuff;

            _iconImg.sprite = baseBuff.Data.sprite;
            UpdateBuffCount(baseBuff.Count);
            
            BaseBuff.RemoveEvent += Destroy;
            BaseBuff.UpdateBuffCountEvent += UpdateBuffCount;
            
            transform.AddComponent<BuffDescription>().Init(BaseBuff.Type);
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
    }
}