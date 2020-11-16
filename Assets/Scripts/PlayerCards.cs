using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCards : MonoBehaviour
{
    [SerializeField] private DropArea _table;
    [SerializeField] private DropArea _outsideTable;
    
    [Space]
    [SerializeField] private float _rotationDelta = 5;

    [SerializeField] private List<Card> _cards;

    public List<Card> Cards => _cards;

    private Card _draggedCard;   

    private void Start()
    {
        PlaceCards();
    }

    private void OnEnable()
    {
        foreach (var card in _cards)
        {
            card.OnHpZeroEvent += DestroyCard;

            card.OnDragEvent += Drag;
            card.OnBeginDragEvent += BeginDrag;
            card.OnEndDragEvent += EndDrag;
            card.OnDropEvent += DropOutsideTable;           
        }
        _table.OnDropEvent += DropOnTable;
        _outsideTable.OnDropEvent += DropOutsideTable;
    }

    private void OnDisable()
    {
        foreach (var card in _cards)
        {
            card.OnHpZeroEvent -= DestroyCard;

            card.OnDragEvent -= Drag;
            card.OnBeginDragEvent -= BeginDrag;
            card.OnEndDragEvent -= EndDrag;
            card.OnDropEvent -= DropOutsideTable;           
        }
        _table.OnDropEvent -= DropOnTable;
        _outsideTable.OnDropEvent -= DropOutsideTable;
    }

    public void PlaceCards()
    {
        var count = _cards.Count;

        float half = count / 2;
        var isEven = count % 2 == 0;
        if (isEven)
            half = (count - 1) / 2f;

        var leftRotation = -half * _rotationDelta;
        for (int i = 0; i < count; i++)
        {  
            var card = _cards[i];

            var angle = leftRotation + i * _rotationDelta;
            // print(angle);
            card.Rotate(angle);
        }
    }

    private void DestroyCard(Card card)
    {
        _cards.Remove(card);
        Destroy(card.gameObject);

        PlaceCards();
    }

    private void BeginDrag(Card card)
    {       
        _draggedCard = card;
       
        card.RectTransform.pivot = new Vector2(0.5f, 0.5f);
        card.RectTransform.localRotation = Quaternion.identity;
        card.RectTransform.position = Input.mousePosition;              

        RemoveCard(card);
    }

    private void RemoveCard(Card card)
    {       
        Cards.Remove(card);
        PlaceCards();
    }

    private void EndDrag(Card card)
    {
        _draggedCard = null;
    }

    private void Drag(Card card)
    {        
        _draggedCard.RectTransform.position = Input.mousePosition;        
    }   

    private void DropOnTable()
    {       
        if (_draggedCard == null)        
            return;
        
        _draggedCard.transform.parent = _table.transform;
    }

    private void DropOutsideTable()
    {      
        if (_draggedCard == null)
            return;

        _draggedCard.transform.parent = transform;

        _draggedCard.RestorePivotAndPosition();
        Cards.Insert(Cards.Count / 2 , _draggedCard);
        PlaceCards();
    }
}
