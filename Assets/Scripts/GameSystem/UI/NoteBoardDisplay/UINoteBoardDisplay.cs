using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINoteBoardDisplayState : UIState {
    public string content;
    public GameObject displayTarget;
    public void Reset() {
        content = "";
        displayTarget = null;
        MarkDirty();
    }
}

public class UINoteBoardDisplay : UIBase<UINoteBoardDisplayState> {
    public TMP_Text textArea;
    public override void ApplyNewStateInternal() {
        if (!state.displayTarget || state.content == "") {
            gameObject.SetActive(false);
            return;
        }
        textArea.text = state.content;
        gameObject.SetActive(true);
    }

    private void LateUpdate() {
        if (Camera.main) {
            transform.position =  Camera.main.WorldToScreenPoint(state.displayTarget.transform.position + Vector3.up);
        }
        
    }
}
