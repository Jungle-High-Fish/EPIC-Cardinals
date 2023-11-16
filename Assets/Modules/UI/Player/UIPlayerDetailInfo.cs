using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.UI
{
    public class UIPlayerDetailInfo : MonoBehaviour, IDescriptionInstTrInfo
    {
        /*
         * 추가 가능 여부는 실제 플레이어 쪽에서 검사를 할 것!!!
         * (포션, 유물, 축복 최대 갯수 같은..)
         * 아예 고정된 설명창으로 변경 할수도?
         */
        [Header("설명 생성 위치")]
        [SerializeField] private Transform _descriptionTr;
        public Transform DescriptionInstTr => _descriptionTr;
    
        [Header("축복")]
        [SerializeField] private Transform _blessTr;

        [Header("유물")]
        [SerializeField] private Transform _artifactTr;
    
        [Header("포션")]
        [SerializeField] private Transform _potionTr;


        public void Init() // [TODO] 플레이어 등록
        {
        
        }

        void AddBless()
        {
        
        }
    
        void AddArtifact()
        {
        
        }

        void AddPotion()
        {
        
            // [TODO] 포션 사용 시, 해당 오브젝트 제거하도록 명령 추가 필요
        }
    }

}