using System;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public Action OnMove;
    public void Place(Vector3 position)
    {
        transform.position = position;
        OnMove?.Invoke();
    }

    internal void Activate()
    {
        gameObject.SetActive(true);
    }

    internal void Deactivate()
    {
        gameObject.SetActive(false);
    }
}