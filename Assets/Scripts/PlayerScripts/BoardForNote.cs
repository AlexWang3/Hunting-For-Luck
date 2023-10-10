using System;
using System.Collections;
using System.Collections.Generic;
using MetroidvaniaTools;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class BoardForNote : MonoBehaviour {
    [Multiline] private string content;

    public void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out PlayerHealth player)) {
            G.UI.uiNoteBoardDisplayState.displayTarget = this.gameObject;
            G.UI.uiNoteBoardDisplayState.content = content;
            G.UI.uiNoteBoardDisplayState.MarkDirty();
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.TryGetComponent(out PlayerHealth player)) {
            G.UI.uiNoteBoardDisplayState.Reset();
        }
    }
}
