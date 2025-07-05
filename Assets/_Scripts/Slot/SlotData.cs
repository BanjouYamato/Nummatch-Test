using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SlotData : MonoBehaviour, IPointerClickHandler
{
    public bool _isData, _isComplete, _isSelected;
    [SerializeField] Image _numbSprite, _selected;
    public Image Selected => _selected;
    public SlotInfo _info;
    ResultCheck _checkR;
    [SerializeField] AudioClip _chooseClip;
    public void SetInit(int index, ResultCheck checkR)
    {
        _info = new SlotInfo(index);
        _checkR = checkR;
    }
    public void SetValue(int value, Sprite sprite)
    {
        _info.value = value;
        _numbSprite.sprite = sprite;
        Color color = _numbSprite.color;
        color.a = 1;
        _numbSprite.color = color;
        _isData = true;
        _isComplete = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameState.Instance.state != State.play) return;
        if (!_isData || _isComplete) return;
        if (!_isSelected)
        {
            SFXManager.instance.PlaySFX(_chooseClip);
            _selected.gameObject.SetActive(true);
            _isSelected = true;
            _checkR.AddSelected(this);
        }
        else
        {
            _selected.gameObject.SetActive(false);
            _isSelected = false;
            _checkR.DeSelect();
        }
    }
    public void OnComplete()
    {
        Color color = _numbSprite.color;
        color.a = 0.2f;
        _numbSprite.color = color;
        _isComplete = true;


    }
    public void DeSelect()
    {
        _isSelected = false;
        _selected.gameObject.SetActive(false);
    }
    public void WrongEffect()
    {
        Vector2 trans = _numbSprite.transform.position;
        _numbSprite.transform.DOMoveX(0.15f, 0.1f)
            .SetRelative(true)
            .SetLoops(3, LoopType.Yoyo)
            .SetEase(Ease.InOutBounce).
            OnComplete(() => _numbSprite.transform.position = trans);

    }
    public void ResetToEmpty()
    {
        DeSelect();
        Color color = _numbSprite.color;
        color.a = 0f;
        _numbSprite.color = color;
        _isComplete = false;
        _info.value = 0;
        _isData = false;
    }
    public void SetSlot(SlotData _slot)
    {
        _info.value = _slot._info.value;
        _numbSprite.sprite = _slot._numbSprite.sprite;
        _numbSprite.color = _slot._numbSprite.color;
        _isData = _slot._isData;
    }
    private void OnDisable()
    {
        transform.DOKill();
    }
}
[System.Serializable]
public struct SlotInfo
{
    public int _row;
    public int _column;
    public int _index;
    public int value;
    public SlotInfo(int index)
    {
        _index = index;
        _row = _index / ValueConstant.cols;
        _column = _index % ValueConstant.cols;
        value = 0;
    }
}
