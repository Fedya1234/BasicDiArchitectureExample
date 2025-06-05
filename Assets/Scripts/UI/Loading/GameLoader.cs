using System.Collections;
using UI.Gates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Loading
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private float _minLoadingTime = 2f;
        private void Start()
        {
            StartCoroutine(LoadGame());
        }
        

        private IEnumerator LoadGame()
        {
            var startTime = Time.time;
            while (Time.time < startTime + _minLoadingTime)
            {
                var timeProgress = (Time.time - startTime) / _minLoadingTime;
                _slider.value = timeProgress * 0.5f;
                yield return null;
            }

            var sceneToLoad = (int) SceneId.MainMenu;
            
            var asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
            while (asyncLoad != null && !asyncLoad.isDone)
            {
                _slider.value = 0.5f + asyncLoad.progress * 0.5f;

                yield return null;
            }
        }
        
    }
}
