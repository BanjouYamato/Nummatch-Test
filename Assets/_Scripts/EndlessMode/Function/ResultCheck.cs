using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultCheck : MonoBehaviour
{
    [SerializeField] SpawnNumb _spawn;
    [SerializeField] MatchCheck _check;
    [SerializeField]List<SlotData> selectedSlot = new();
    [SerializeField] List<int> clearSlot;
    [SerializeField] AddNumb _add;
    [SerializeField] StateEffect _eff;
    [SerializeField] AudioClip _clearRowSFx, _matchSFX;
    private void Update()
    {
        CheckResult();
    }
    //hàm check các điều kiện khi select 2 ô
    void CheckResult()
    {
        if (selectedSlot.Count != 2) return; // nếu select 2 ô thì mới check
        var result = OnCheck(selectedSlot[0], selectedSlot[1]);
        if (!result)
        {
            // nếu không match thì chạy hiệu ứng sai
            var min = Mathf.Min(selectedSlot[0]._info._index, selectedSlot[1]._info._index);
            var max = Mathf.Max(selectedSlot[0]._info._index, selectedSlot[1]._info._index);
            for (int i = min +1; i < max; i++)
            {
                if (!_spawn.Slots[i]._isComplete && _spawn.Slots[i]._isData)
                {
                    _spawn.Slots[i].WrongEffect();
                }
            }
        }
        else
        {
            // nếu 2 ô matching với nhau
            _eff.ScoreEff(1, selectedSlot[1].transform);
            SFXManager.instance.PlaySFX(_matchSFX);
            foreach(var slot in selectedSlot)
            {
                slot.OnComplete(); // làm mở và đánh dấu hoàn thành
                CheckRow(slot); // check em row của 2 slot vừa match đã clear chưa
            }
            clearSlot = clearSlot.Distinct().OrderByDescending(r => r).ToList();// sắp xếp các row đã clear theo thứ tự giảm dần
            if(clearSlot.Count >0) StartCoroutine(ClearAllRow(clearSlot)); // nếu trong list có row cần clear thì chạy hiệu ứng
        }
            DeSelect();
        CheckClearStage();
        if (_add.Numb <= 0) CheckLose(); // nếu số lượt add =  thì bắt đầu check thua
    }
    bool OnCheck(SlotData a, SlotData b)
    {
        return _check.CheckMatch(a, b);
    }
    // thêm vào list select
    public void AddSelected(SlotData a)
    {
        selectedSlot.Add(a);
    }
    public void DeSelect()
    {
        foreach(SlotData a in selectedSlot)
        {
            a.DeSelect();
        }
        selectedSlot.Clear();
    }

    void CheckRow(SlotData slot)
    {
        int row = slot._info._row;
        var result = CheckRowComplete(slot);
        if (result == true)
        {
            clearSlot.Add(row);
        }
    }  
    bool CheckRowComplete(SlotData _slot)
    {
        int row = _slot._info._row;
        for (int i = 0; i < ValueConstant.cols; i++)
        {
            _check.slots.TryGetValue((row, i), out SlotData slot);
            if (slot != null && slot._isData && !slot._isComplete) return false;
        }
        return true;
    }
    IEnumerator ClearRow(int row)
    {
        SFXManager.instance.PlaySFX(_clearRowSFx);
        for (int i = 0; i < ValueConstant.cols; i++)
        {
            _check.slots.TryGetValue((row, i), out SlotData slot);
            if (slot != null && slot._isData) slot.ResetToEmpty();
            yield return new WaitForSeconds(0.05f);
        }
        for (int col = 0; col < ValueConstant.cols; col++)
        {
            // Tạo danh sách các row phía dưới hàng bị clear trong cùng cột
            List<SlotData> downSlots = new List<SlotData>();
            for (int r = row + 1; r < ValueConstant.rows; r++)
            {
                var index = r * ValueConstant.cols + col;
                var slot = _spawn.Slots[index];
                if (slot._isData)
                {
                    downSlots.Add(slot);
                }
            }

            // Gán dữ liệu từ dưới lên
            for (int r = row, j = 0; j < downSlots.Count; r++, j++)
            {
                var fromSlot = downSlots[j];
                var toIndex = r * ValueConstant.cols + col;
                var toSlot = _spawn.Slots[toIndex];
                toSlot.SetSlot(fromSlot);
            }

            // Reset các slot còn lại phía dưới
            for (int r = row + downSlots.Count; r < ValueConstant.rows; r++)
            {
                var index = r * ValueConstant.cols + col;
                _spawn.Slots[index].ResetToEmpty();
            }
        }
    }
    IEnumerator ClearAllRow(List<int> rows)
    {
        
        foreach (int row in rows)
        {
            yield return StartCoroutine(ClearRow(row));
        }
        clearSlot.Clear();
    }
    // check hiện tại còn ô nào match được không
    bool CheckAnyValidMatch()
    {
        var dataSlots = _spawn.Slots.Where(x => x._isData && !x._isComplete).ToList();

        for (int i = 0; i < dataSlots.Count; i++)
        {
            for (int j = 0; j < dataSlots.Count; j++)
            {
                if (dataSlots[j] == dataSlots[i]) continue;
                var a = dataSlots[i];
                var b = dataSlots[j];

                if (_check.CheckMatch(a, b))
                    return true;
            }
        }

        return false;
    }
    void CheckLose()
    {
        if (GameState.Instance.state != State.play) return;
        var canLose = CheckAnyValidMatch();
        if (!canLose) GameState.Instance.SelectState(State.lose);
    }
    bool CheckClear()
    {
        return !_spawn.Slots.Any(slot => slot._isData && !slot._isComplete);
    }
    void CheckClearStage()
    {
        var clear = CheckClear();
        if(clear) GameState.Instance.SelectState(State.stageClear);
    }
}
