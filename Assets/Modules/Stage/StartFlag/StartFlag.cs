using System.Collections;
using Cardinals.Board;
using DG.Tweening;
using UnityEngine;

namespace Cardinals.Game {
    public class StartFlag : MonoBehaviour
    {
        public void Init() {
            gameObject.SetActive(false);
        }

        public IEnumerator Show(Tile tile) {
            gameObject.SetActive(true);

            Vector3 targetPos = tile.RendererTransform.localPosition;
            targetPos.z -= Constants.GameSetting.Board.TileHeight / 2 + 0.5f;

            targetPos = tile.RendererTransform.TransformPoint(targetPos) + new Vector3(0, 0.3f, 0);
            transform.position = targetPos + new Vector3(0, 5f, 0);

            transform.DOMove(targetPos, 0.5f).SetEase(Ease.InQuint);
            yield return new WaitForSeconds(0.5f);
        }

        public IEnumerator Hide() {
            transform.DOMove(transform.position + new Vector3(0, 5f, 0), 0.5f).SetEase(Ease.InQuint);
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }

        public void HideImmediate() {
            gameObject.SetActive(false);
        }
    }
}