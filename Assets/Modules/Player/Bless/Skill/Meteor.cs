using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Cardinals;
using Cardinals.Enums;
using DG.Tweening;
using UnityEngine;

namespace Cardinals
{
    public class Meteor : MonoBehaviour
    {
        public void Start()
        {
            transform.position = new Vector3(4.5f, 7, 10);
            
            StartCoroutine(MeteorCoroutine());
        }

        private IEnumerator MeteorCoroutine()
        {
            GameManager.I.Sound.MeteorDrop();
            GameManager.I.LightController.SetLightNight();
            yield return new WaitForSeconds(1f);
            transform.DOMove(new Vector3(1.5f, 2, -1.5f), 2f)
                .SetEase(Ease.InExpo)
                .OnComplete(Execute);
            yield return new WaitForSeconds(1.3f);
            GameManager.I.LightController.SetLightDay();
        }

        void Execute()
        {
            if (GameManager.I.CurrentEnemies.Any())
            {
                var enemies = GameManager.I.CurrentEnemies.ToList();
            
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    GameManager.I.Sound.MeteorBomb();
                    enemies[i].Hit(10, TileMagicType.Fire);
                }
            }

            GameManager.I.CameraController.ShakeCamera(1f, 5f, 1f);
        
            Destroy(gameObject, .5f);
        }
    }
}