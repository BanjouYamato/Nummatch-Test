using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AddNumb : MonoBehaviour
{
    [SerializeField] SpawnNumb _spawn;
    int _numbPosible;
    public int Numb => _numbPosible;
    [SerializeField] TextMeshProUGUI _numbText;


    private void Start()
    {
        SetNumb(6);
    }
    // logic add numb
    public void AddNumbFunc()
    {
        if (_numbPosible == 0 || GameState.Instance.state != State.play) return;
        SFXManager.instance.PlayBtnFX();
        _numbPosible--;
        SetNumb(_numbPosible);
        AddNumbAnim();
    }
    // cài dặt lại số lượt add
    public void SetNumb(int _newNumb)
    {
        _numbPosible = _newNumb;
        _numbText.text = _numbPosible.ToString();
    }
    // hiệu ứng adđ numb
    void AddNumbAnim()
    {
        var newNumbList = _spawn.Slots.Where(x => x._isData && !x._isComplete).ToList();
        var TotalData = _spawn.Slots.Where(x => x._isData== true).ToList();
        if(newNumbList.Count + TotalData.Count  > _spawn.Slots.Count)
        {
            int extra = newNumbList.Count + TotalData.Count - _spawn.Slots.Count;
            int extraRow = Mathf.CeilToInt((float)extra/ValueConstant.cols);
            int newSlotAdd = extraRow * ValueConstant.cols;
            _spawn.AddMoreSlot(newSlotAdd);
        }
        StartCoroutine(AddEffect(TotalData.Count, newNumbList.Count + TotalData.Count, newNumbList));
    }
    // hiệu ứng add từng số
    IEnumerator AddEffect(int start, int end, List<SlotData> values)
    {
        int valueIndex = 0;
        for(int i=start; i < end; i++)
        {
            var targetSlot = _spawn.Slots[i];
            var value = values[valueIndex]._info.value;
            targetSlot.SetValue(value, _spawn.NumbSprites[value - 1]);
            yield return new WaitForSeconds(0.1f);
            valueIndex++;
        }
    }
}
