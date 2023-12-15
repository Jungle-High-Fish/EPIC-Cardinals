using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.UI.NewDice
{
    /// <summary>
    /// 선택된 다이스 정보를 New Dice Panel에 전달하는 클래스
    /// </summary>
    public class SelectedDice : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<SelectedEffect>().OnSelectedEvent += SendSelectedDiceInfo;
        }

        /// <summary>
        /// 선택된 다이스 정보를 New Dice Panel에 전달하는 함수
        /// </summary>
        /// <param name="state"></param>
        void SendSelectedDiceInfo(bool state)
        {
            var uiNewDice = GetComponentInParent<UINewDicePanel>();
            UIDice dice = null;
            
            if (state)
            {
                dice = GetComponent<UIDice>();
            }
            
            uiNewDice.SelectedItem(dice);
        }
    }

}