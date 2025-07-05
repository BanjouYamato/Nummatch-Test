using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveModeFunc : MonoBehaviour
{
    [SerializeField] TMP_InputField _field;
    [SerializeField] List<int> _numbList = new();
    public int required;
    public int[,] slot;
    public bool[,] isComplete;
    [SerializeField] MatchCheckMove _check;
    int[,] backupSlot;
    bool[,] backupComplete;
    int backupCollected;
    int collected;
    List<Move> currentPath;
    [SerializeField] List<List<Move>> Solutions;
    public int qty;
    [SerializeField] float test;
    public void OnCheck()
    {
        StartCoroutine(FuncFlow());
    }
    IEnumerator FuncFlow()
    {
        ConvertToList();
        CreateBoard();
        yield return null;
        StartSolve();
    }
    void CreateBoard()
    {
        if(_numbList.Count > 0)
        {
            int rowVolume = _numbList.Count / ValueConstant.cols;
            int colVolume = ValueConstant.cols;
            slot = new int[rowVolume, colVolume];
            isComplete = new bool[rowVolume, colVolume];
            for (int i = 0; i < _numbList.Count; i++) 
        {
                int row = i / colVolume;
                int col = i % colVolume;
                slot[row, col] = _numbList[i];
            }
}
    }
    void ConvertToList()
    {
        _numbList.Clear();
        string input = _field.text;
        int numb5 = 0;
        foreach(char c in input)
        {
            if (char.IsDigit(c))
            {
                int numb = (int)char.GetNumericValue(c);
                _numbList.Add(numb);
                if (numb == 5) numb5++;
            }
            else continue;
        }
        Debug.Log(numb5);
        required = numb5 / 2;
        qty = input.Length;
        test = numb5 / 2;
    }
    List<Move> GetAllValidMove()
    {
        List<Move> moveList = new();
        int cols = ValueConstant.cols;
        int rows = qty / cols;
        for (int r1 = 0; r1 < rows; r1++)
        {
            for(int c1 = 0; c1 < cols; c1++)
            {
                if (isComplete[r1, c1]) continue;
                for(int r2 =0; r2 < rows; r2++)
                {
                    for(int c2 = 0;c2 < cols; c2++)
                    {
                        if (r1 == r2 && c1 == c2) continue;
                        if (isComplete[r2, c2]) continue;
                        if (_check.CheckMatch(r1, c1, r2, c2))
                        {
                            moveList.Add(new Move(r1,c1,r2,c2));
                        }
                    }
                }
            }
        }
        return moveList;
    }
    void StartSolve()
    {
        currentPath = new();
        Solutions = new();
        collected = 0;
        Solve();
        var best10 = Solutions.OrderBy(solution => solution.Count)
            .Take(10).ToList();
        string fileName = $"output.txt";
        string path = Path.Combine(ValueConstant.path, fileName);
        using (StreamWriter writer = new StreamWriter(path, false)) // path là biến bạn đã có
        {
            int index = 1;
            foreach (var solution in best10)
            {
                writer.WriteLine($"--- Solution {index} | Moves: {solution.Count} ---");
                foreach (var move in solution)
                {
                    writer.WriteLine(move.ToString());
                }
                writer.WriteLine();
                index++;
            }
        }

        Debug.Log("Đã ghi kết quả ra file: " + ValueConstant.path);
    }
    void Solve()
    {
        if(collected >= required)
        {
            Debug.Log("nhung oi anh yeu em");
            Solutions.Add(new List<Move>(currentPath));
            return;
        }
        var moves = GetAllValidMove();
        if (moves.Count == 0) return;
        foreach( var move in moves )
        {
            SaveState();
            currentPath.Add(move);
            ApplyMove(move.r1, move.c1, move.r2, move.c2);
            Solve();
            currentPath.RemoveAt(currentPath.Count - 1);
            RestoreState();
        }
    }
    void ApplyMove(int r1,int c1, int r2, int c2)
    {
        int v1 = slot[r1, c1];
        int v2 = slot[r2, c2];
        isComplete[r1, c1] = true;
        isComplete[r2, c2] = true;
        bool was5_1 = (v1 == 5);
        bool was5_2 = (v2 == 5);

        if (was5_1 && was5_2) collected++;
        var isRowClearA = _check.CheckRow(r1);
        var isRowClearB = _check.CheckRow(r2);
        if (isRowClearA || isRowClearB) _check.ClearRow();
    }
    void SaveState()
    {
        backupSlot = (int[,])slot.Clone();
        backupComplete = (bool[,])isComplete.Clone();
        backupCollected = collected;
    }
    void RestoreState()
    {
        int cols = ValueConstant.cols;
        int rows = qty / cols;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                slot[r, c] = backupSlot[r, c];
                isComplete[r, c] = backupComplete[r, c];
            }
        }
        collected = backupCollected;
    }
}
public struct Move
{
    public int r1,c1,r2,c2;
    public Move(int r1, int  c1, int r2, int c2)
    {
        this.r1 = r1; this.c1 = c1; 
        this.r2 = r2; this.c2 = c2;
    }
    public override string ToString()
    {
        return $"{r1},{c1},{r2},{c2}";
    }
}

