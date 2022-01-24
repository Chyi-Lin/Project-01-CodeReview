using UnityEngine;
using UnityEngine.UI;

public class SelectPage : MonoBehaviour
{
    [SerializeField]
    private Pages _pageIndex;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private PassPageEvent OnSelectPageEvent;

    private void OnEnable()
    {
        if (_button)
            _button.onClick.AddListener(OnSelectListener);
    }

    private void OnDisable()
    {
        if (_button)
            _button.onClick.RemoveListener(OnSelectListener);
    }

    private void OnSelectListener()
    {
        OnSelectPageEvent?.Invoke(_pageIndex);
    }

}
