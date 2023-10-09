using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using BehaviorTree;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class SceneLoadManager : MonoBehaviour {
    public static SceneLoadManager Instance;

    public void Awake() {
        Instance = this;
    }

    public void LoadScene(string sceneName,Action callBack = null, LoadSceneMode loadMode = LoadSceneMode.Additive,bool useLoadScreen = true) {
        if (useLoadScreen) {
            G.UI.mainUITye = MainUITye.LoadingScreen;
            G.UI.MarkDirty();
            StartCoroutine(LoadSceneAsync(sceneName, loadMode, callBack));
        } else {
            SceneManager.LoadScene(sceneName, loadMode);
            if (callBack != null) {
                callBack();
            }
        }
    }

    public void UnLoadScene(string name) {
        SceneManager.UnloadSceneAsync(name);
    }

    private IEnumerator LoadSceneAsync(string sceneName,LoadSceneMode loadMode, Action callBack = null) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadMode);

        while (!asyncLoad.isDone) {
            G.UI.uiLoadScreenState.progress = asyncLoad.progress;
            G.UI.uiLoadScreenState.MarkDirty();
            yield return null;
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Insert(0,G.I.UIMain.LoadScreen.TopBlackTransit.DOFade(1, 0.5f));
        sequence.Insert(0.5f,G.I.UIMain.LoadScreen.TopBlackTransit.DOFade(0, 0f));

        if (callBack != null) {
            sequence.InsertCallback(0.5f, () => callBack());
        }
        sequence.Play();
    }
}
