
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int _score;
    public int Score => _score;
    [SerializeField] TextMeshProUGUI _scoreText;
    public List<GameObject> eff = new();
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform _parent;

    private void Start()
    {
        SetScore(0);
        Observer.Instance.AddToObser<int>(ObserverConstant.score, PlusScore);
    }
    private void OnDisable()
    {
        Observer.Instance.RemoveObser<int>(ObserverConstant.score, PlusScore);
    }
    void SetScore(int newScore)
    {
        _score = newScore;
        _scoreText.text = _score.ToString();
    }
    void PlusScore(int scorePlus)
    {
        _score += scorePlus;
        _scoreText.text = _score.ToString();
    }
    public GameObject ClonePrefab(int score)
    {
        foreach(var vfx in eff)
        {
            if (!vfx.activeInHierarchy)
            {
                var tex = vfx.GetComponentInChildren<TextMeshProUGUI>();
                tex.text = $"+{score}";
                vfx.SetActive(true);
                return vfx;
            }
        }
        var clone = Instantiate(_prefab, _parent);
        var text = clone.GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"+{score}";
        eff.Add(clone);
        clone.SetActive(true);
        return clone;
    }
}
