using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveUi : MonoBehaviour
{
    public void BackMenu()
    {
        SceneManager.LoadScene(SceneConstant.menu);
    }
}
