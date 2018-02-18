﻿using UnityEngine;
using UnityEngine.UI;

namespace LunchGame01 {
    [RequireComponent(typeof(Text))]
    public class ScoreLabel : MonoBehaviour {

        [SerializeField]
        private IntAsset score;

        private Text     label;

        private void OnEnable() {
            label = GetComponent<Text>();
            score.OnValueChanged(OnScoreChanged);
        }

        private void OnScoreChanged(int value) {
            label.text = value.ToString("### ### ##0");
        }
    }
}
