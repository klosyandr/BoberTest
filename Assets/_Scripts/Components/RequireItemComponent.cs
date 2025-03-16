using UnityEngine;
using UnityEngine.Events;

public class RequireItemComponent : IInteractableComponent
{
    [InventoryId] [SerializeField] private string _required;
    [SerializeField] private Message message;
    
    [SerializeField] private UnityEvent _onSuccess;        
    [SerializeField] private UnityEvent _onFail;

    public override void Interact(Player player)
    {
       /* if (string.Equals(player.SelectedObject?.Tag, _required))
        {
            message.SetText(MessagesData.WIN);
            _onSuccess?.Invoke();
            return;
        }
        
        message.SetText(
            player.Inventory.Get(_required) != null ?
                MessagesData.NO_REQUIRE_HAND(_required, player.SelectedObject.Tag) :
                MessagesData.NO_REQUIRE_INVENTORY(_required)
        );
        _onFail?.Invoke();*/
    }
}