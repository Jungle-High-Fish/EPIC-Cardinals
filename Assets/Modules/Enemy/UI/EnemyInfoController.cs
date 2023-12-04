// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Cardinals.Enemy;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Cardinals
// {
//     public class EnemyInfoController : MonoBehaviour
//     {
//         private GridLayoutGroup _gridLayoutGroup;
//
//         public void Awake()
//         {
//             var rect = transform.AddComponent<RectTransform>();
//             rect.anchorMin = new Vector2(.5f, 1);
//             rect.anchorMax = new Vector2(.5f, 1);
//             rect.anchoredPosition = new Vector3(0, -150, 0);
//             rect.localScale = Vector3.one; 
//             
//             _gridLayoutGroup = transform.AddComponent<GridLayoutGroup>();
//             _gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
//             _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
//             
//             _gridLayoutGroup.cellSize = new Vector2(600, 100);
//         }
//
//         public void Init(int enemyCount = 1)
//         {
//             _gridLayoutGroup.enabled = true;
//             _gridLayoutGroup.constraintCount = enemyCount;
//
//             _gridLayoutGroup.enabled = false;
//             _gridLayoutGroup.enabled = true;
//
//             if (enemyCount == 2)
//             {
//                 for (int idx = 0; idx < enemyCount; idx++)
//                 {
//                     var child = transform.GetChild(idx).GetComponent<RectTransform>();
//                     var pos = GameManager.I.Stage.Enemies[idx].transform.localPosition;
//                     
//                     child.anchoredPosition = Camera.main.WorldToScreenPoint(pos);
//                     
//                     child.GetComponent<UIEnemyInfo>().SetBubbleDirection(idx == 0);
//                     Debug.Log( child.anchoredPosition);
//                 }
//             }
//             
//             Invoke(nameof(DisableGrid), .1f);
//         }
//
//         private void DisableGrid()
//         {
//             _gridLayoutGroup.enabled = false;
//         }
//     }
// }
