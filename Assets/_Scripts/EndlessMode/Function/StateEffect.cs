
using UnityEngine;
using DG.Tweening;
using TMPro;

public class StateEffect : MonoBehaviour
{
    [SerializeField] CanvasGroup gov, _clear;
    [SerializeField] TextMeshProUGUI _scoreText, _stageText,st;
    [SerializeField] ScoreManager _score;
    [SerializeField] SpawnNumb _spawn;
    [SerializeField] Transform scoreText;
    [SerializeField] AddNumb _add;
    private void Start()
    {
        GameState.OnGOV += SetGOVEffect;
        GameState.OnClear += SetStageClearEff;
    }
    void SetGOVEffect()
    {
        // hiệu ứng khi thua
        _scoreText.text = _score.Score.ToString();
        DOVirtual.DelayedCall(0.5f, () =>
        {
            gov.DOFade(1f, 2f).
            SetEase(Ease.InOutElastic)
            .OnComplete(() =>
            {
                gov.interactable = true;
                gov.blocksRaycasts = true;
            });
            
        }); 
    }
    void SetStageClearEff()
    {
        // hiệu ứng khi clear stage
        _stageText.text = $"Stage {_spawn._stage.ToString()} clear!!!";
        _spawn._stage++;
        _clear.alpha = 1f;
        _clear.transform.localScale = Vector3.zero;
        _clear.transform.DOScale(1f, 3f)
            .SetEase(Ease.InOutBack)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                foreach(var slot in _spawn.Slots)
                {
                    if(slot._isData) slot.ResetToEmpty();                   
                }
                _spawn.SelectFirstStep();
                _spawn.SpawnAllFirstStep();
                st.text = $"Stage {_spawn._stage}";
                _add.SetNumb(6);
                GameState.Instance.SelectState(State.play);
            });           
    }
    public void ScoreEff(int score, Transform pos)
    {
        // hiệu ứng tăng điểm khi matching 1 cặp sôs
        var scoreVF = _score.ClonePrefab(score);
        scoreVF.transform.position = pos.position;
        scoreVF.transform.DOMove(scoreText.position, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                scoreVF.SetActive(false);
                scoreText.DOScale(1.3f, 0.3f)
                .SetEase(Ease.InOutBounce)
                .SetLoops(2, LoopType.Yoyo);
                Observer.Instance.TriggerAction(ObserverConstant.score, score);
            });
    }
    private void OnDisable()
    {
        transform.DOKill();
        GameState.OnGOV -= SetGOVEffect;
        GameState.OnClear -= SetStageClearEff;
    }
}
