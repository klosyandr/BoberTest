using DG.Tweening;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private AudioSource _sfx;
    [Space]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _hidePosition;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private float _forceDrop;

    private TakeComponent _selectedObject;

    private void Awake()
    {
        _player ??= GetComponent<Player>();
        _player.Input.OnDrop += OnDrop;
        _player.Inventory.OnIndexChanged += (value) => ChangeSelectedObject();
    }

    private void ChangeSelectedObject()
    {
        if (_selectedObject != null && _selectedObject == _player.Inventory.SelectedItem) return;

        if(_selectedObject != null) Hide(_selectedObject);
        _selectedObject = _player.Inventory.SelectedItem;
        if(_selectedObject != null) Show(_selectedObject);
    }

    public void Take(TakeComponent item, AudioClip clip)
    {
        if (!_player.Inventory.TryAdd(item)) return;

        item.Take(_targetPosition);
        _sfx.clip = clip;
        _sfx?.Play();
    }

    private void Hide(TakeComponent target)
    {
        target.DOKill(true);
        target.transform.DOLocalMove(_hidePosition.localPosition, _duration).OnKill(() => target.gameObject.SetActive(false));
    }

    private void Show(TakeComponent target)
    {
        target.DOKill(true);
        target.transform.localPosition = _hidePosition.localPosition;
        target.gameObject.SetActive(true);
        target.gameObject.transform.DOLocalMove(target.LocalOffset, _duration).OnKill(() => target.gameObject.SetActive(true));
    }

    private void OnDrop()
    {
        if (_selectedObject == null) return;
        
        var dropable = _selectedObject;
        _selectedObject = null;
        _player.Inventory.RemoveSelected();
        dropable.Drop(_orientation.forward * _forceDrop);
    }

    private void OnDestroy()
    {
        _player.Input.OnDrop -= OnDrop;
    }
}