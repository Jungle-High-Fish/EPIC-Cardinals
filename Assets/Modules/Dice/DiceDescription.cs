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

                string path = "Dice/Dice_" + type.ToString() + "_" + numbers[i].ToString();
                surface.GetComponent<Image>().sprite = ResourceLoader.LoadSprite(path);

            }
        }

        public void SetDescriptionUIHovered(int index)
        {
            ResetOutline();
            _diceDescription.transform.GetChild(index).GetComponent<Outline>().enabled = true;
            _diceDescription.SetActive(true);
        }

        public void SetDescriptionUIRestored()
        {
            _diceDescription.SetActive(false);
        }

        private void ResetOutline()
        {
            foreach(Transform t in _diceDescription.transform)
            {
                t.GetComponent<Outline>().enabled = false;
            }
        }
    }
}
