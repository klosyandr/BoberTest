using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemWidget[] _items;
    
    private void Start()
    {
        _inventory.OnChanged += Rebuild;

        foreach (var widget in _items)
        {
            _inventory.OnIndexChanged += widget.OnIndexChanged;
        }
        Rebuild();  
    }

    private void Rebuild()
    {
        var inventory = _inventory.Items;

        if (_inventory.Length != 0)
        {
            //update data and activate
            for(var i = 0; i < inventory.Length; i++)
            {
                _items[i].SetData(inventory[i], i);
            }
        }

        // hide unused items
        for(var i = inventory.Length; i < _items.Length; i++)
        {
            _items[i].Hide();
        }
    }

    private void OnDestroy()
    {
        _inventory.OnChanged -= Rebuild;
        foreach (var widget in _items)
        {
            _inventory.OnIndexChanged -= widget.OnIndexChanged;
        }
    }
}