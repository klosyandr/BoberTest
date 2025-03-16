using UnityEngine;
using NaughtyAttributes;

public class PlayerInteraction : MonoBehaviour
{
    [ReadOnly] [SerializeField] private Player _player;
    [Header("Interaction Settings")]
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _distance = 3f;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.Input.OnInteract += OnInteract;
    }

    private void OnInteract(Vector2 vector)
    {
        Ray ray = new Ray(_player.Camera.transform.position, _player.Camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _distance);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, _distance, _layer))
        {
            var target = hit.collider.gameObject.GetComponent<IInteractableComponent>();
            if (target != null)
            {
                target.Interact(_player);
            }
        }
    }

    private void OnDestroy()
    {
        _player.Input.OnInteract -= OnInteract;
    }
}