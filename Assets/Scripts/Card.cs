using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private CardData Data;

    [Space]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;

    [SerializeField] private Image _iconImage;

    [SerializeField] private TMP_Text _manaText;
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _healthText;

    [Space]
    [SerializeField] private Image _outlineImage; 

    [Space]
    [SerializeField] private float _changeTimeout = 0.1f;
    [Min(1)]
    [SerializeField] private float _animationSpeed = 2f;

    public event Action<Card> OnHpZeroEvent;

    public event Action<Card> OnDragEvent;
    public event Action<Card> OnBeginDragEvent;
    public event Action<Card> OnEndDragEvent;
    public event Action OnDropEvent;

    public RectTransform RectTransform { get; private set; }

    public string Name => Data.Name;

    private Vector2 _startPivot;
    private Vector2 _startPosition;

    #region Parameter properties

    private int _mana;
    public int Mana
    {
        get { return _mana; }
        set
        {
            StartCoroutine(UpdateTextCor(_mana, value, _manaText));
            _mana = value;
        }
    }

    private int _attack;
    public int Attack
    {
        get { return _attack; }
        set
        {
            StartCoroutine(UpdateTextCor(_attack, value, _attackText));
            _attack = value;           
        }
    }

    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            StartCoroutine(UpdateTextCor(_health, value, _healthText, true));
            _health = value;
        }
    }

    #endregion

    private void Awake()
    {
        Assert.IsNotNull(Data);

        _mana = Data.Mana;
        _attack = Data.Attack;
        _health = Data.Health;

        RectTransform = GetComponent<RectTransform>();

        _startPivot = RectTransform.pivot;
        _startPosition = RectTransform.position;
    }

    private void OnValidate()
    {
        if (Data != null)
            UpdateUI(Data);
    }

    public void UpdateIconImage(Sprite sprite)
    {
        _iconImage.sprite = sprite;
    }

    public void Rotate(float angle)
    {
        StartCoroutine(RotateCor(angle));
    }

    public void RestorePivotAndPosition()
    {
        RectTransform.pivot = _startPivot;
        RectTransform.position = _startPosition;
    }

    private void UpdateUI(CardData card)
    {
        _nameText.text = card.Name;
        _descriptionText.text = card.Description;

        _manaText.text = card.Mana.ToString();
        _attackText.text = card.Attack.ToString();
        _healthText.text = card.Health.ToString();
    }

    private IEnumerator UpdateTextCor(int oldValue, int value, TMP_Text text, bool checkLessZero = false)
    {
        var delta = value > oldValue ? 1 : -1;
        while (oldValue != value)
        {
            oldValue += delta;
            text.text = oldValue.ToString();
            yield return new WaitForSeconds(_changeTimeout);
        }

        if (checkLessZero && value <= 0)
            OnHpZeroEvent?.Invoke(this);
    }   

    private IEnumerator RotateCor(float angle)
    {
        var time = 0f;
        var startAngle = RectTransform.localEulerAngles.z;
        
        while (time <= 1f)
        {
            time += _animationSpeed * Time.deltaTime;
            var angleProcess = Mathf.LerpAngle(startAngle, -angle, time);            
            RectTransform.localRotation = Quaternion.Euler(0, 0, angleProcess);

            yield return null;
        }
    }  

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        _outlineImage.enabled = true;

        OnBeginDragEvent?.Invoke(this);       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        _outlineImage.enabled = false;
        OnEndDragEvent?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropEvent?.Invoke();
    }
}
