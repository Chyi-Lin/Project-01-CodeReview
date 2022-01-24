using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField]
    private int _id;

    [SerializeField]
    private Button _button;

    [SerializeField]
    private TMP_Text _text;

    public void SetId(int id)
    {
        _id = id;
    }

    public void SetText(string text)
    {
        _text.SetText(text);
    }

    public void SetOnClickEvent(UnityAction<int> listener)
    {
        _button.onClick.AddListener(()=> 
        {
            listener?.Invoke(_id);
        });
    }

    public bool SetInteractable
    {
        set 
        {
            _button.interactable = value;
        }
    }
}
