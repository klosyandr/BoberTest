using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemWidget[] _widgets;
    
    private void Start()
    {
        _inventory.OnChanged += Rebuild;

        foreach (var widget in _widgets)
        {
            _inventory.OnIndexChanged += widget.OnIndexChanged;
        }
        Rebuild();  
    }

    private void Rebuild()
    {
        var inventory = _inventory.Slots;

        if (inventory.Length != 0)
        {
            //update data and activate
            for(var i = 0; i < inventory.Length; i++)
            {
                _widgets[i].SetData(inventory[i], i);
            }
        }

        // hide unused items
        for(var i = inventory.Length; i < _widgets.Length; i++)
        {
            _widgets[i].Hide();
        }
    }

    private void OnDestroy()
    {
        _inventory.OnChanged -= Rebuild;
        foreach (var widget in _widgets)
        {
            _inventory.OnIndexChanged -= widget.OnIndexChanged;
        }
    }
}