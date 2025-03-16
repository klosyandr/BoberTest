using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Header("Interaction Settings")]
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _distance = 3f;
    
    private GameObject _lastHighlighted;
    private Outline _lastOutline;

    private void Awake()
    {
        _player ??= GetComponent<Player>();
        _player.Input.OnInteract += OnInteract;
    }

    private void Update()
    {
        Ray ray = new Ray(_player.Camera.transform.position, _player.Camera.transform.forward);

#if UNITY_EDITOR
Debug.DrawRay(ray.origin, ray.direction * _distance);
#endif
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _distance, _layer))
        {
            var target = hit.collider.gameObject;

            if (_lastHighlighted != target)
            {
                ClearOutline();

                _lastHighlighted = target;
                _lastOutline = _lastHighlighted.GetComponent<Outline>();

                if (_lastOutline != null)
                {
                    _lastOutline.enabled = true;
                }
            }
        }
        else
        {
            ClearOutline();
        }
    }

    private void OnInteract(Vector2 vector)
    {
        var target = _lastHighlighted?.GetComponent<IInteractableComponent>();
        target?.Interact(_player);
    }

    private void ClearOutline()
    {
        if (_lastOutline != null)
        {
            _lastOutline.enabled = false;
        }

        _lastHighlighted = null;
        _lastOutline = null;
    }

    private void OnDestroy()
    {
        _player.Input.OnInteract -= OnInteract;
    }
}