using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElHome : MonoBehaviour
{
    public void BackToMenu()
    {
        SFXManager.instance.PlayBtnFX();
        SceneManager.LoadScene(SceneConstant.menu);
    }
    public void Restart()
    {
        SFXManager.instance.PlayBtnFX();
        SceneManager.LoadScene(SceneConstant.endless);
    }

}
