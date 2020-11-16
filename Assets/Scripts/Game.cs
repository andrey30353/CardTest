using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerCards _playerCards;

    [Space]
    [SerializeField] private int _minCardCount = 2;
    [SerializeField] private int _maxCardCount = 9;

    [Space]
    [SerializeField] private int _minChangeValue = -2;
    [SerializeField] private int _maxChangeValue = 9;

    [Space]    
    [SerializeField] private float _nextCardTimeout = 0.2f;

    public UnityEvent OnRandomChangesEndEvent;

    public void RandomChanges()
    {
        if(_playerCards.Cards.Count > 0)
            StartCoroutine(RandomChangesCor());
    }

    private IEnumerator RandomChangesCor()
    {
        var changesCount = Random.Range(_minCardCount, _maxCardCount + 1);
        print($"{nameof(changesCount)} = {changesCount}");

        for (int i = 0; i < changesCount; i++)
        {
            if (_playerCards.Cards.Count <= 0)
                break;

            var index = i;
            if (index >= _playerCards.Cards.Count)
                index = 0;

            var card = _playerCards.Cards[index];
            if (card == null)
                continue;

            ChangeRandomParameter(card);            

            yield return new WaitForSeconds(_nextCardTimeout);
        }

        OnRandomChangesEndEvent?.Invoke();
    }

    private void ChangeRandomParameter(Card card)
    {
        var parameter = (CardParameter)Random.Range(0, 3);
        var newValue = Random.Range(_minChangeValue, _maxChangeValue + 1);

        print($"{card.Name}: {parameter} => {newValue}");

        switch (parameter)
        {
            case CardParameter.Mana:
                card.Mana = newValue;
                break;

            case CardParameter.Attack:
                card.Attack = newValue;
                break;

            case CardParameter.Health:
                card.Health = newValue;
                break;

            default:
                throw new System.Exception($"Unknown {nameof(parameter)} = {parameter}");
        }
    }
}
