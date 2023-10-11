using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MetroidvaniaTools;

public class TargetGroupCameraControl : MonoBehaviour {
    public Transform Player;
    public Transform Boss;
    public CinemachineTargetGroup targetGroup;

    private void Start() {
        List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>();
        targets.Add(new CinemachineTargetGroup.Target{target = Player, radius = 4, weight = 1});
        targetGroup.m_Targets = targets.ToArray();
        LSleep.SleepEvent += AddBossToTargetGroup;
    }

    public void AddBossToTargetGroup() {
        List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>();
        targets.Add(new CinemachineTargetGroup.Target{target = Player, radius = 3, weight = 3});
        targets.Add(new CinemachineTargetGroup.Target{target = Boss, radius = 3, weight = 1});
        targetGroup.m_Targets = targets.ToArray();
    }
}
