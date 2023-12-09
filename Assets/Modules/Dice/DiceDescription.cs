using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cardinals.Enums;
using Util;
using TMPro;

namespace Cardinals
{
    public class DiceDescription : MonoBehaviour
    {
        [SerializeField] private GameObject _diceDescription;
        [SerializeField] private GameObject _rerollDescription;
        public void Init(List<int> numbers, DiceType type)
        {
            for(int i = 0; i < numbers.Count; i++)
            {
                GameObject surface = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_DiceSurface),_diceDescription.transform);
                surface.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = numbers[i].ToString();
                switch (type)
                {
                    case DiceType.Normal:
                        surface.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
                        break;
                    case DiceType.Fire:
                        surface.GetComponent<Image>().color = new Color(0.9245283f, 0.51689120f, 0.4404593f);
                        break;
                    case DiceType.Water:
                        surface.GetComponent<Image>().color = new Color(0.4332948f, 0.6019194f, 0.8584906f);
                        break;
                    case DiceType.Earth:
                        surface.GetComponent<Image>().color = new Color(0.6320754f, 0.5570136f, 0.5277234f);
                        break;
                }
            }
        }

        public void SetDescriptionUIHovered(int index)
        {
            _diceDescription.transform.GetChild(index).GetComponent<Outline>().enabled = true;
            _diceDescription.SetActive(true);
        }

        public void SetDescriptionUIRestored(int index)
        {
            _diceDescription.transform.GetChild(index).GetComponent<Outline>().enabled = false;
            _diceDescription.SetActive(false);
        }
    }
}
