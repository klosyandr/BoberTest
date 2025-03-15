using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxCount = 5;
    [SerializeField] private int _stacableMaxCount = 3;
    [Space]
    [SerializeField] private List<InventoryItem> _inventory = new List<InventoryItem>();
    [SerializeField] private int _selectedIndex = -1;

    public int Length => _inventory.Count;
    public int SelectedIndex => _selectedIndex;
    public InventoryItem[] Items => _inventory.ToArray();
    private bool _hasFreeSlot => _inventory.Count < _maxCount;
    public InventoryItem Get(string id) => GetItemToRemove(id);

    public Action OnChanged;
    public Action<int, int> OnIndexChanged;
    public Action OnInventoryFull;
    
    public bool TryAdd(string id, TakeComponent go)
    {
        if (go == null) return false;

        var itemDef = DefsFacade.I.Items.Get(id);
        if (itemDef.IsVoid) return false;

        InventoryItem item = GetItemToAdd(id, itemDef.IsStacable);
        
        if (item == null || !itemDef.IsStacable)
        {
            if (!_hasFreeSlot)
            {
                OnInventoryFull?.Invoke();
                return false;
            }
            item = new InventoryItem(id);
            _inventory.Add(item);
        }
        item.objectPool.Add(go);
        OnChanged?.Invoke();
        CalculateIndex();
        return true;
    }

    public void Remove(string id, TakeComponent go)
    {
        var itemDef = DefsFacade.I.Items.Get(id);
        if (itemDef.IsVoid) return;
        
        var item = GetItemToRemove(id, itemDef.IsStacable ? null : go);
        if (item == null) return;

        item.objectPool.Remove(go);
        var count = item.objectPool.Count;
        if (count == 0)
        {
            _inventory.Remove(item);
        }
        OnChanged?.Invoke();
        CalculateIndex();
    }

    private InventoryItem GetItemToRemove(string id, TakeComponent go = null)
    {
        foreach (var item in _inventory)
        {
            if (string.Equals(item.Id, id))
            {
                if (go == null || item.objectPool.Contains(go))
                {
                    return item;
                }
            }
        }
        return null;
    }

    private InventoryItem GetItemToAdd(string id, bool IsStacable)
    {
        foreach (var item in _inventory)
        {
            if (string.Equals(item.Id, id))
            {
                if (IsStacable && item.objectPool.Count < _stacableMaxCount)
                {
                    return item;
                }
            }
        }
        return null;
    }

    private void CalculateIndex()
    {
        var oldIndex = _selectedIndex;
        _selectedIndex = _inventory.Count == 0 ? -1 :
            Mathf.Clamp(_selectedIndex, 0, _inventory.Count - 1);
        OnIndexChanged?.Invoke(_selectedIndex, oldIndex);
    }

    public void SetNextItem(int value)
    {
        if (_inventory.Count <= 1) return;

        var oldIndex = _selectedIndex;
        _selectedIndex = (int) Mathf.Repeat(_selectedIndex + value, _inventory.Count);
        OnIndexChanged?.Invoke(_selectedIndex, oldIndex);
    }
    
    public void SelectIndex(int index)
    {
        index--;
        if (index > _inventory.Count - 1 || index < 0) return;
        
        var oldIndex = _selectedIndex;
        _selectedIndex = index;
        OnIndexChanged?.Invoke(_selectedIndex, oldIndex);
    }
    
}

[Serializable]
public class InventoryItem
{
    public readonly string Id;
    public List<TakeComponent> objectPool;

    public InventoryItem(string id)
    {
        Id = id;
        objectPool = new List<TakeComponent>();
    }
}