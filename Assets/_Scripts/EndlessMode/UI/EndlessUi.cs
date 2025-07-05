using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessUi : MonoBehaviour
{
    
    public void OpenPanel(CanvasGroup _cv)
    {
        if (GameState.Instance.state != State.play) return;
        SFXManager.instance.PlayBtnFX();
        GameState.Instance.SelectState(State.impossibleInput);
        _cv.alpha = 0;
        _cv.DOFade(1f, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _cv.interactable = true;
                _cv.blocksRaycasts = true;
            });
    }
    public void ClosePanel(CanvasGroup _cv)
    {
        SFXManager.instance.PlayBtnFX();
        _cv.interactable = false;
        _cv.blocksRaycasts = false;
        _cv.alpha = 1;
        _cv.DOFade(0f, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                GameState.Instance.SelectState(State.play);
            });
    }
}
