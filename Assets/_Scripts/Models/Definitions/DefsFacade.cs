using UnityEngine;

[CreateAssetMenu(menuName ="Defs/DefsFacade", fileName = "DefsFacade")]
public class DefsFacade : ScriptableObject
{
    [SerializeField] private ItemsRepository _items;

    private static DefsFacade _instance;

    public ItemsRepository Items => _items;
    
    public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

    private static DefsFacade LoadDefs()
    {
        return _instance = Resources.Load<DefsFacade>("DefsFacade");
    }
}
