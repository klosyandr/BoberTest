using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [ReadOnly] [SerializeField] private Player _player;

    [SerializeField] private int _maxCount = 5;
    [SerializeField] private List<InventorySlotData> _collection;

    private int _selectedIndex = -1;
    private bool _hasFreeSlot => _collection.Count < _maxCount;
    private int Count => _collection.Count;

    public bool HasItem(string id) => _collection.Find(item => item.Id == id) != null;
    public InventorySlotData[] Slots => _collection.ToArray();
    public InventorySlotData SelectedSlot => _collection[_selectedIndex];
    public TakeComponent SelectedItem => _selectedIndex == -1 ? null : _collection[_selectedIndex].GetItem();

    public int SelectedIndex
    {
        set
        {
            _selectedIndex = value;
            OnIndexChanged?.Invoke(value);
        }
        get { return _selectedIndex; }
    }

    public Action OnInventoryFull;
    public Action OnChanged;
    public Action<int> OnIndexChanged;

    private void Awake()
    {
        _collection = new List<InventorySlotData>();

        _player ??= GetComponent<Player>();
        _player.Input.OnSelectNum += SelectSlot;
        _player.Input.OnScroll += SetNextItem;
    }

    public bool TryAdd(TakeComponent item)
    {
        var itemDef = DefsFacade.I.Items.Get(item.Tag);
        if (itemDef.IsVoid) return false;

        InventorySlotData slot = GetSlot(item.Tag);
        
        if (slot == null)
        {
            if (!_hasFreeSlot)
            {
                OnInventoryFull?.Invoke();
                return false;
            }
            slot = new InventorySlotData(itemDef);
            _collection.Add(slot);
        }
        slot.AddItem(item);

        OnChanged?.Invoke();
        CalculateIndex();
        return true;
    }

    public void RemoveSelected()
    {
        if (_selectedIndex == -1) return;

        if (_selectedIndex >= _collection.Count) return;

        var slot = _collection[_selectedIndex];
        slot.RemoveItem();
        if (slot.IsEmpty)
        {
            _collection.RemoveAt(_selectedIndex);
        }
        OnChanged?.Invoke();
        CalculateIndex();
    }

    private InventorySlotData GetSlot(string id)
    {
        foreach (var item in _collection)
        {
            if (string.Equals(item.Id, id) && !item.IsFull)
            {
                return item;
            }
        }
        return null;
    }

    public void SelectSlot(int index)
    {
        if (index > Count - 1 || index < 0) return;
        
        SelectedIndex = index;
    }

    public void SetNextItem(int value)
    {
        if (Count <= 1) return;

        SelectedIndex = (int) Mathf.Repeat(_selectedIndex + value, Count);
    }

    private void CalculateIndex()
    {
        SelectedIndex = Count == 0 ? -1 :
            Mathf.Clamp(_selectedIndex, 0, Count - 1);
    }

    private void OnDestroy()
    {        
        _player.Input.OnSelectNum -= SelectSlot;
        _player.Input.OnScroll -= SetNextItem;

    }
}

[Serializable]
public class InventorySlotData
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private int _maxStackCount;
    [SerializeField] private List<TakeComponent> _pool;

    public string Id => _id;
    public int Count => _pool.Count;
    public bool IsFull => Count == _maxStackCount;
    public bool IsEmpty => Count == 0;

    public InventorySlotData(ItemDef itemDef)
    {
        _id = itemDef.Id;
        _maxStackCount = itemDef.IsStacable ? itemDef.MaxStackCount : 1;
        _pool = new List<TakeComponent>();
    }

    public void AddItem(TakeComponent item)
    {
        _pool.Add(item);
        item.gameObject.SetActive(false);
    }

    public void RemoveItem()
    {
        _pool.RemoveAt(0);
    }
    
    public TakeComponent GetItem()
    {
        return _pool[0];
    }
}