using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] Button endlessBtn, moveBtn;
    private void Start()
    {
        endlessBtn.onClick.AddListener(() => LoadScene(SceneConstant.endless));
        moveBtn.onClick.AddListener(() => LoadScene(SceneConstant.move));
    }

    void LoadScene(string sceneName)
    {
        SFXManager.instance.PlayBtnFX();
        SceneManager.LoadScene(sceneName);
    }
}
