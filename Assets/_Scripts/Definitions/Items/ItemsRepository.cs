using System;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")]
public class ItemsRepository : ScriptableObject
{
    [SerializeField] private ItemDef[] _items;

#if UNITY_EDITOR
    public ItemDef[] ItemsForEditor => _items;
#endif

    public ItemDef Get(string id)
    {
        if (string.IsNullOrEmpty(id)) 
            return default;
        
        foreach (var itemDef in _items)
        {
            if (itemDef.Id == id)
            {
                return itemDef;
            }
        }
        return default;
    }
}

[Serializable]
public struct ItemDef
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private bool _isStacable;
    [ShowIf("_isStacable")] [AllowNesting]
    [SerializeField] private int _maxStackCount;

    public string Id => _id;
    public Sprite Icon => _icon;
    public bool IsStacable => _isStacable;
    public int MaxStackCount => _maxStackCount;
    
    public bool IsVoid => string.IsNullOrEmpty(_id);
}