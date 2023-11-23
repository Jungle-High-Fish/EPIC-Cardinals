using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cardinals.UI
{
    public class UIDescription : MonoBehaviour
    {
        private IDescription _baseDescription;
        public IDescription BaseDesc => _baseDescription;
        
        [Header("Component")]
        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _nameTMP;
        [SerializeField] private TextMeshProUGUI _descriptionTMP;
        
        public void Init(IDescription description)
        {
            _baseDescription = description; 
            
            _nameTMP.text = description.Name;
            _descriptionTMP.text = description.Description;
            
            Sprite icon = description.IconSprite;
            _iconImg.sprite = icon;
            _iconImg.gameObject.SetActive(icon != null);
        }
    }
}