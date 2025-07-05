using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchCheckMove : MonoBehaviour
{
    [SerializeField] MoveModeFunc _mode;
    List<int> rowClear = new();
    
    public bool CheckMatch(int r1, int c1, int r2, int c2)
    {
        int aValue = _mode.slot[r1,c1];
        int bValue = _mode.slot[r2,c2];
        if (_mode.isComplete[r1, c1] || _mode.isComplete[r2, c2]) return false;
        if (!(aValue == bValue || aValue + bValue == 10)) return false;
        var type  = GetDirectionType(r1,c1,r2,c2);
        switch (type)
        {
            case PosType.vertical:
                return CheckVertical(r1, c1, r2, c2);
            case PosType.hoirzontal:
                return CheckHorizontal(r1, c1, r2, c2);
            case PosType.leftDiagonal:
                return CheckDia(r1, c1, r2, c2);
            case PosType.rightDiagonal:
                return CheckDia(r1,c1, r2, c2); 
        }
        return false;
    }
    PosType GetDirectionType(int r1, int c1, int r2, int c2)
    {
        int dr = r2 - r1;
        int dc = c2 - c1;

        if (dr == 0 && dc == 0) return PosType.invalid;

        if (dc == 0) return PosType.vertical;
        if (dr == 0) return PosType.hoirzontal;
        if (Mathf.Abs(dr) == Mathf.Abs(dc))
        {
            if (dr * dc > 0) return PosType.leftDiagonal;
            else if (dr * dc < 0) return PosType.rightDiagonal;
        }
            

        return PosType.invalid;
    }
    bool CheckVertical(int r1,int c1,int r2, int c2)
    {
        int dr = Math.Sign(r2 - r1);
        int col = c1;
        int rowCheck = r1 + dr;
        while(rowCheck != r2)
        {
            if (!_mode.isComplete[rowCheck, col]) return false;
            rowCheck += dr;
        }
        return true;
    }
    bool CheckHorizontal(int r1,int c1,int r2, int c2)
    {
        int row = r1;
        int dc = Math.Sign(c2 - c1);
        int colCheck = c1 + dc;
        while(colCheck != c2)
        {
            if (!_mode.isComplete[row, colCheck]) return false;
            colCheck += dc;
        }
        return true;
    }
    bool CheckDia(int r1, int c1, int r2, int c2)
    {
        int dr = Math.Sign(r2 - r1);
        int dc = Math.Sign(c2 - c1);
        int rowCheck = r1 + dr;
        int colCheck = c1 + dc;
        while (rowCheck != r2)
        {
            if (!_mode.isComplete[rowCheck, colCheck]) return false;
            rowCheck += dr;
            colCheck += dc;
        }
        return true;
    }

    public bool CheckRow(int row)
    {
        for(int i = 0; i< ValueConstant.cols; i++)
        {
            if (!_mode.isComplete[row, i]) return false;
        }
        rowClear.Add(row);
        return true;
    }
    public void ClearRow()
    {
        
        int cols = ValueConstant.cols;
        int rows = _mode.qty/ cols;
        rowClear = rowClear.Distinct().OrderByDescending(r => r).ToList();

        foreach (int row in rowClear)
        {
            for (int r = row ; r < rows - 1; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    _mode.slot[r, c] = _mode.slot[r + 1, c];
                    _mode.isComplete[r, c] = _mode.isComplete[r + 1, c];
                }
            }

        }
        // Làm rỗng các dòng cuối cùng tương ứng số dòng đã clear
        for (int c = 0; c < cols; c++)
        {
            _mode.slot[rows - 1, c] = 0;
            _mode.isComplete[rows - 1, c] = true;
        }
        rowClear.Clear();
    }
}
