using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Cardinals.Game {
    public class SceneChanger: MonoBehaviour {

        public void ChangeScene(string sceneName, GameManager gameManager) {
            StartCoroutine(SceneLoad(sceneName, gameManager));
        }

        public void ChangeSceneCredit(string sceneName, GameManager gameManager)
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(SceneLoadCredit(sceneName, gameManager));
        }
        private IEnumerator SceneLoad(string sceneName, GameManager gameManager)
        {
            var loadScene = SceneManager.LoadSceneAsync(sceneName);
            Destroy(gameManager);

            while (!loadScene.isDone)
            {
                yield return null;
            }
            
            Destroy(gameObject);
        }

        private IEnumerator SceneLoadCredit(string sceneName, GameManager gameManager) {
            GameObject prefab = Instantiate(ResourceLoader.LoadPrefab(Constants.FilePath.Resources.Prefabs_UI_FadePanel));
            yield return prefab.GetComponent<UIFade>().FadeIn();
            var loadScene = SceneManager.LoadSceneAsync(sceneName);
            Destroy(gameManager);

            while (!loadScene.isDone) {
                yield return null;
            }

            GameObject.Find("TitleManager")?.GetComponent<TitleManager>().OnCredit();
            yield return prefab.GetComponent<UIFade>().FadeOut();
            
            Destroy(gameObject);
        }
    }
}
