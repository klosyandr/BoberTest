using DG.Tweening;
using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    [SerializeField] private Transform _root;
    [SerializeField] private Vector3 _endRotation;
    [SerializeField] private float _duration = 0.2f;

    public void Open()
    {
        _root.DOLocalRotate(_endRotation, _duration);
    }
    
    public void Close()
    {
        _root.DOLocalRotate(Vector3.zero, _duration);
    }
}
