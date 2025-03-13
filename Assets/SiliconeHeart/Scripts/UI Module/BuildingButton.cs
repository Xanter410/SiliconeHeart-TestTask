using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Image _icon;

    [SerializeField] private Sprite _ButtonBaseImage;
    [SerializeField] private Sprite _ButtonSelectedImage;

    private Button _button;

    public void Initialize(BuildingData data, System.Action<BuildingData, BuildingButton> callback)
    {
        _icon.sprite = data.Icon;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => callback?.Invoke(data, this));

        SetSelected(false);
    }

    public void SetSelected(bool state)
    {
        if (state == true)
        {
            _buttonImage.sprite = _ButtonSelectedImage;
        }
        else
        {
            _buttonImage.sprite = _ButtonBaseImage;
        }
    }
}