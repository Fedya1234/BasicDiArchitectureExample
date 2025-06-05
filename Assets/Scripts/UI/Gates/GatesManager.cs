using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Gates
{
  public class GatesManager : MonoBehaviour
  {
    private const int LagCompensationFramesDelay = 5;
    private static GatesManager _instance;

    [SerializeField] private GameObject _gates;
    [SerializeField] private Transform _toMove;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _duration = 0.3f;

    private bool _state;
    private Vector3 _defaultPosition;

    public static void LoadScene(SceneId id) => LoadScene((int) id);
    public static void LoadSceneAsync(SceneId id) => LoadSceneAsync((int) id);

    public static void LoadScene(int index)
    {
      if (_instance == null)
      {
        SceneManager.LoadScene(index);
        return;
      }

      _instance.LoadSceneByIndex(index).Forget();
    }

    public static void LoadSceneAsync(int index)
    {
      if (_instance == null)
      {
        SceneManager.LoadSceneAsync(index);
        return;
      }

      _instance.LoadSceneByIndex(index).Forget();
    }

    private void Awake()
    {
      if (_instance == null)
      {
        _instance = this;
        DontDestroyOnLoad(gameObject);
      }
      else
        Destroy(gameObject);
    }

    private void Start()
    {
      _gates.SetActive(true);
      _defaultPosition = _toMove.position;
      _gates.SetActive(false);
    }

    private void OnDestroy()
    {
      if (_instance == this)
        _instance = null;
    }

    private async UniTask LoadSceneByIndex(int sceneToLoad)
    {
      CloseGates();
      
      await UniTask.Delay(Mathf.RoundToInt(_duration * 1000));

      await SceneManager.LoadSceneAsync(sceneToLoad);

      await UniTask.DelayFrame(LagCompensationFramesDelay);

      OpenGates();
    }

    private void CloseGates()
    {
      _toMove.position = _startPoint.position;
      _gates.SetActive(true);

      _toMove.DOMove(_defaultPosition, _duration)
        .SetUpdate(true)
        .SetEase(Ease.OutSine);
    }

    private void OpenGates()
    {
      _toMove.DOMove(_endPoint.position, _duration)
        .SetEase(Ease.InSine)
        .SetUpdate(true)
        .OnComplete(() => _gates.SetActive(false));
    }
  }
}