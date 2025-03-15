using UnityEngine;

public abstract class IInteractableComponent : MonoBehaviour
{
    public abstract void Interact(Player player);
}