using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public SceneReference newGameScene;
    public SceneReference titleScreen;

    public void StartGame() {
        SceneLoadManager.Instance.LoadScene(newGameScene, () => {
            G.UI.mainUITye = MainUITye.InGame;
            G.UI.MarkDirty();
        });
    }
    
    public void OpenSetting() {
        G.UI.overlayUIType = OverlayUIType.TitleSetting;
        G.UI.MarkDirty();
    }
    public void EndGame() {
        Application.Quit();
    }
    
}
