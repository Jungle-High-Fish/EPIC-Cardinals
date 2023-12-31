using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cardinals.Game {
    public class SceneChanger: MonoBehaviour {
        public void ChangeScene(string sceneName, GameManager gameManager) {
            StartCoroutine(SceneLoad(sceneName, gameManager));
        }

        private IEnumerator SceneLoad(string sceneName, GameManager gameManager) {
            var loadScene = SceneManager.LoadSceneAsync(sceneName);
            Destroy(gameManager);

            while (!loadScene.isDone) {
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
