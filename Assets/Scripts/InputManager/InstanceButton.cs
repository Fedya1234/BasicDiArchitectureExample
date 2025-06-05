using System;
using UnityEngine.UI;

namespace InputManager
{
  public class InstanceButton : Button
  {
    public event Action<InstanceButton> EventClick;

    protected override void OnEnable()
    {
      base.OnEnable();
      onClick.AddListener(OnClick);
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
      EventClick?.Invoke(this);
    }
  }
}