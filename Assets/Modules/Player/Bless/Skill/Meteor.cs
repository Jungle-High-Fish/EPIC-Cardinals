using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Cardinals;
using DG.Tweening;
using UnityEngine;

namespace Cardinals
{
    public class Meteor : MonoBehaviour
    {
        public void Start()
        {
            transform.position = new Vector3(4.5f, 7, 10);
            transform.DOMove(new Vector3(1.5f, 2, -1.5f), 2f)
                .SetEase(Ease.InExpo)
                .OnComplete(Execute);
        }

        void Execute()
        {
            if (GameManager.I.CurrentEnemies.Any())
            {
                var enemies = GameManager.I.CurrentEnemies.ToList();
            
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    enemies[i].Hit(20);
                }
            }
        
            Destroy(gameObject, .5f);
        }
    }
}