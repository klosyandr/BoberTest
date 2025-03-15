using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemWidget : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _selected;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private int _index;

    public void SetData(InventoryItem item, int index)
    {
        Select(false);
        _index = index;
        var def = DefsFacade.I.Items.Get(item.Id);
        _icon.sprite = def.Icon;
        _icon.enabled = true;
        _value.text = item.objectPool.Count > 1 ? $"x{item.objectPool.Count.ToString()}" : string.Empty;
    }

    public void Hide()
    {
        Select(false);
        _icon.enabled = false;
        _value.text = string.Empty;
    }

    public void Select(bool value)
    {
        _selected.SetActive(value);
    }

    public void OnIndexChanged(int newValue, int _)
    {
        _selected.SetActive(_index == newValue && newValue != -1);
    }
}