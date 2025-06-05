using System;
using UnityEngine;

namespace UI.Tools
{
  [RequireComponent(typeof(RectTransform))]
  public class TopSafeAreaOffset : MonoBehaviour
  {
    [SerializeField] private bool _isTopOnly;
    private const int CorrectionOffset = -10;
    private RectTransform _panel;

    private void Start()
    {
      TryGetComponent(out _panel);
      var offset = 0f;

      if (Screen.safeArea.y > 0)
        offset = Screen.safeArea.y - CorrectionOffset;
      else if (Math.Abs(Screen.height - Screen.safeArea.height) > float.Epsilon)
        offset = Screen.height - Screen.safeArea.height - CorrectionOffset;

      var oMax = _panel.offsetMax;
      
      if (_isTopOnly)
      {
        _panel.offsetMax = new Vector2(oMax.x, oMax.y - offset);
        return;
      }

      if (oMax == Vector2.zero && _panel.offsetMin == Vector2.zero)
      {
        _panel.offsetMax = new Vector2(oMax.x, oMax.y - offset);
      }
      else
      {
        var panel = _panel.transform;
        var localPosition = panel.localPosition;
        localPosition = new Vector3(localPosition.x, localPosition.y - offset, localPosition.z);
        panel.localPosition = localPosition;
      }
    }
  }
}