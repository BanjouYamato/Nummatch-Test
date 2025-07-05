using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchCheck : MonoBehaviour
{
    public Dictionary<(int, int), SlotData> slots = new();

    // chuyển hóa list thành dictionary giả lập thay cho mảng 2 chiều (em dùng dictionary thay mảng vì trường hợp thay đổi kích thước)
    public void AddToDiction(List<SlotData> datas)
    {
        slots = datas.ToDictionary(s => (s._info._row, s._info._column), s => s);
    }
    // dùng để check khi tạo bảng để đảm bảo ngoài trừ các step có thể match tạo sẵn, các số được tạo còn lại không tạo thành cặp match
    public bool CheckCreate(SlotInfo _slot)
    {
        int minRow = Mathf.Max(0, _slot._row - 1);
        int minCol = Mathf.Max(0, _slot._column - 1);
        int maxCol = Mathf.Min(9, _slot._column + 1);
        for (int i = minRow; i < _slot._row + 1 + 1; i++)
        {
            for (int j = minCol; j < maxCol + 1; j++)
            {
                slots.TryGetValue((i, j), out var slot);
                if (slot != null && slot._isData)
                {
                    if ((slot._info.value == _slot.value || slot._info.value == 10 - _slot.value))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    // check matching theo hướng
    public bool CheckMatch(SlotData a, SlotData b)
    {
        int aValue = a._info.value;
        int bValue = b._info.value;
        if (!(aValue == bValue || aValue + bValue == 10)) return false;
        var type = CheckPos(a, b);
        switch (type)
        {
            case PosType.vertical:
                return HandleCheckVertical(a, b);
            case PosType.hoirzontal:
                return HandleCheckHorizontal(a, b);
            case PosType.leftDiagonal:
                return HandleCheckLeftDiagonal(a, b);
            case PosType.rightDiagonal:
                return HandleCheckRightDiagonal(a, b);
            default:
                return false;
        }
    }
    // xác định xem 2 slot nằm ở hướng nào của nhau
    public PosType? CheckPos(SlotData a, SlotData b)
    {
        int dRow = a._info._row - b._info._row;
        int dCol = a._info._column - b._info._column;
        int dirRow = Math.Sign(dRow);
        int dirCol = Math.Sign(dCol);
        if (dirRow == 1 && dirCol == 0) return PosType.vertical;
        else if (dirRow == -1 && dirCol == 0) return PosType.vertical;
        else if (dirRow == 0 && dirCol == 1) return PosType.hoirzontal;
        else if (dirRow == 0 && dirCol == -1) return PosType.hoirzontal;
        else if (dirRow == -1 && dirCol == -1) return PosType.leftDiagonal;
        else if (dirRow == -1 && dirCol == 1) return PosType.rightDiagonal;
        else if (dirRow == 1 && dirCol == -1) return PosType.rightDiagonal;
        else if (dirRow == 1 && dirCol == 1) return PosType.leftDiagonal;
        return PosType.invalid;
    }
    // xử lí khi nằm dọc
    bool HandleCheckVertical(SlotData a, SlotData b)
    {
        int col = a._info._column;
        int minRow = Mathf.Min(a._info._row, b._info._row);
        int maxRow = Mathf.Max(a._info._row,b._info._row);
        int distance = maxRow - minRow;
        if (distance == 1) return true;
        for(int i = minRow + 1; i < maxRow; i++)
        {
            slots.TryGetValue((i,col),out var slot);
            if (slot != null && (!slot._isComplete && slot._isData)) return false;
        }
        return true;
    }
    // xử lí khi nằm ngang
    bool HandleCheckHorizontal(SlotData a, SlotData b)
    {
        int row = a._info._row;
        int minCol = Mathf.Min(a._info._column, b._info._column);
        int maxCol = Mathf.Max(a._info._column, b._info._column);
        int distance = maxCol - minCol;
        if (distance == 1) return true;
        for (int i = minCol + 1; i < maxCol; i++)
        {
            slots.TryGetValue((row, i), out var slot);
            if (slot != null && (!slot._isComplete && slot._isData)) return false;
        }
        return true;
    }
    // xử lí khi chéo trái xuống
    bool HandleCheckLeftDiagonal(SlotData a, SlotData b)
    {
        if (Mathf.Abs(a._info._row - b._info._row) != Mathf.Abs(a._info._column - b._info._column))
            return false;
        int minRow = Mathf.Min(a._info._row, b._info._row);
        int maxRow = Mathf.Max(a._info._row, b._info._row);
        int minCol = Mathf.Min(a._info._column,b._info._column);
        int maxCol = Mathf.Max(a._info._column, b._info._column);
        if (maxRow - minRow == 1 && maxCol - minCol == 1) return true;
        for(int i = minRow +1, j = minCol + 1;i<maxRow && j < maxCol; i++, j++)
        {
            slots.TryGetValue((i, j), out var slot);
            if (slot != null && (!slot._isComplete && slot._isData)) return false;
        }
        return true;
    }
    // xử lí khi chéo phải xuống
    bool HandleCheckRightDiagonal(SlotData a, SlotData b)
    {
        if (Mathf.Abs(a._info._row - b._info._row) != Mathf.Abs(a._info._column - b._info._column))
            return false;
        int minRow = Mathf.Min(a._info._row, b._info._row);
        int maxRow = Mathf.Max(a._info._row, b._info._row);
        int minCol = Mathf.Min(a._info._column, b._info._column);
        int maxCol = Mathf.Max(a._info._column, b._info._column);
        if (maxRow - minRow == 1 && maxCol - minCol == 1) return true;
        for(int i = minRow + 1,j = maxCol -1; i <maxRow && j > minCol; i++, j--)
        {
            slots.TryGetValue((i, j), out var slot);
            if (slot != null && (!slot._isComplete && slot._isData)) return false;
        }
        return true;
    }


    // lúc sau khi làm move mode em nghĩ ra cách để code ngắn hơn nên có đổi lai khi check ở move mode
}
public enum PosType
{
    vertical,
    hoirzontal,
    leftDiagonal,
    rightDiagonal,
    invalid
}
