using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    public event Action OnDropEvent;

    public void OnDrop(PointerEventData eventData)
    {
        print(name);
        OnDropEvent?.Invoke();
    }    
}
