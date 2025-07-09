
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnNumb : MonoBehaviour
{
    [SerializeField] RectTransform _board, _canvas;
    [SerializeField] GridLayoutGroup _grid;
    [SerializeField] List<SlotData> _slots = new();
    public List<SlotData> Slots => _slots;
    [SerializeField] SlotData _prefab;
    [SerializeField] Transform _parent;
    [SerializeField] Sprite[] numbSprites = new Sprite[9];
    public Sprite[] NumbSprites => numbSprites;
    public int _stage;
    int _firstAppear = 27;
    [SerializeField] MatchCheck _check;
    [SerializeField] ResultCheck _resultCheck;
    private void Start()
    {
        _stage = 1;
        Init();
        GameState.Instance.SelectState(State.play);
    }
    void Init()
    {
        // thiết lập kích thước board tùy theo kích thước màn hình đang sử dụng
        var boardHeight = _canvas.rect.height / 2f;
        float slotSize = boardHeight / 10.5f;
        _grid.cellSize = new Vector2(slotSize, slotSize);
        _board.sizeDelta = new Vector2(slotSize * 9f, boardHeight);

        StartCreateBoard();
    }
    void StartCreateBoard()
    {
        // tạo boarđ khi bắt đầu game
        for (int i = 0; i < 99; i++)
        {
            var newSlot = Instantiate(_prefab, _parent);
            newSlot.SetInit(i,_resultCheck);
            _slots.Add(newSlot);
        }
        _check.AddToDiction(_slots);
        SelectFirstStep();
        SpawnAllFirstStep();
    }
    int FirstSelect()
    {
        // thay đổi số lượng bước ban đầu theo stage
        switch (_stage)
        {
            case 1:
                return 3;
            case 2:
                return 2;
            default:
                return 1;
        }
    }
    // kiểm tra số lượng các số đang có trên board
    bool CheckQty(int value)
    {
        var ListValue = Slots.Where(slot => slot._isData && slot._info.value == value).ToList();
        return ListValue.Count +1 < 5;
    }
    public void SpawnAllFirstStep()
    {
        for (int i = 0; i < _firstAppear; i++)
        {
            if (_slots[i]._isData) continue;
            List<int> candiates = new();
            for (int v = 1; v <= 9; v++)
            {
                var temp = new SlotInfo(i);
                temp.value = v;
                if (!_check.CheckCreate(temp) && CheckQty(v))
                {
                    candiates.Add(v);
                }
            }
            int selected = 0;
            if(candiates.Count > 0)
            {
                selected = candiates[Random.Range(0,candiates.Count)];
            }
            else
            {
                Debug.LogWarning("eo co roi em oi");
                selected = Random.Range(1, 10);
            }
                _slots[i].SetValue(selected, numbSprites[selected - 1]);           
        }
    }
    public void SelectFirstStep()
    {
        // tạo sẵn các bước chơi ban dầu
        int step = FirstSelect();
        for (int i =0; i< step; i++)
        {
            var coup = Getvalue();
            int rd = 0;
            // nếu ô randam trước đó đã có số thì lặp lại
            do
            {
                rd = Random.Range(0, _firstAppear);
            } while (_slots[rd]._isData);
            Debug.Log(rd + "...");
            _slots[rd].SetValue(coup.x, numbSprites[coup.x - 1]);
            // tạo ô matching với ô đã tạo
            var step2 = MatchStep(rd);
            _slots[step2].SetValue(coup.y, numbSprites[coup.y - 1]);
        }
    }
    int MatchStep(int _first)
    {
        // tạo ô matching
        var info = new SlotInfo(_first);
        int minRow = Mathf.Max(0, info._row - 1);
        int maxRow = Mathf.Min(ValueConstant.rows - 1, info._row + 1);
        int minCol = Mathf.Max(0, info._column - 1);
        int maxCol = Mathf.Min(ValueConstant.cols -1, info._column + 1);
        int index = -1;
        do
        {
            var newRow = Random.Range(minRow, maxRow );
            var newCol = Random.Range(minCol, maxCol + 1);
            index = newRow * ValueConstant.cols + newCol;
        } while (_slots[index]._isData);
        return index;
    }

    Vector2Int Getvalue()
    {
        // tạo sẵn các gặp số tổng hoặc giống nhau
        int a, b;
        bool isSame = Random.value < 0.25f;
        if (isSame)
        {
            a = Random.Range(1, 10);
            b = a;
        }
        else
        {
            do
            {
                a = Random.Range(1, 10);
            } while (a == 5);
            b = 10 - a;
        }
        return new Vector2Int(a, b);
    }
    public void AddMoreSlot(int newSlot)
    {
        // hàm xử lí khi thêm ô ở dấu +
        var newslots = _slots.Count + newSlot;
        for(int i = _slots.Count;  i < newslots; i++)
        {
            var newS = Instantiate(_prefab, _parent);
            newS.SetInit(i, _resultCheck);
            _slots.Add(newS);
        }
        _check.AddToDiction(_slots);
    }
}
