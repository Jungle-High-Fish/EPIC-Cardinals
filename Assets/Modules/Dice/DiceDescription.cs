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
        [SerializeField] private GameObject _rerollPanel;
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

        public void UpdateDiceDescription(Dice dice)
        {
            List<int> numbers = dice.DiceNumbers;
            DiceType type = dice.DiceType;

            for (int i = 0; i < _diceDescription.transform.childCount; i++)
            {
                Image image = _diceDescription.transform.GetChild(i).GetComponent<Image>();
                string path = "Dice/Dice_" + type.ToString() + "_" + numbers[i].ToString();
                image.sprite = ResourceLoader.LoadSprite(path);

            }
        }
        public void SetDescriptionUIHovered(int index)
        {
            ResetOutline();
            _diceDescription.transform.GetChild(index).GetComponent<Outline>().enabled = true;
            _diceDescription.SetActive(true);
            _rerollPanel.SetActive(true);
        }

        public void SetDescriptionUIRestored()
        {
            _diceDescription.SetActive(false);
            _rerollPanel.SetActive(false);
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
