using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputСontroller _input;
    [SerializeField] private CameraController _camera;
    [Header("Components")]
    [SerializeField] private PlayerInteraction _interaction;
    [SerializeField] private PlayerMovement _move;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private HandController _hand;

    public PlayerInputСontroller Input => _input;
    public PlayerInteraction Interaction => _interaction;
    public PlayerMovement Move => _move;
    public Inventory Inventory => _inventory;
    public HandController Hand => _hand;
    public CameraController Camera => _camera;

    private void Reset()
    {
        _interaction = GetComponent<PlayerInteraction>();
        _move = GetComponent<PlayerMovement>();
        _hand = GetComponent<HandController>();
        _inventory = GetComponent<Inventory>();
    }
}