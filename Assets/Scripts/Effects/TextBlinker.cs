using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Deflector.Effects {

    [AddComponentMenu("Effects/Text Blinker")]
    public class TextBlinker : BaseMeshEffect {

        [SerializeField]
        private float       interval;
        [SerializeField]
        private UnityEvent  onBlink;

        private bool        show;
        private Text        compText;
        private IEnumerator blinker;

        protected override void Awake() {
            base.Awake();
            compText = GetComponent<Text>();
            blinker = BlinkText();
        }

        [UsedImplicitly]
        public void Blink() {
            show = true;
            StartCoroutine(blinker);
        }

        public void Stop() {
            StopCoroutine(blinker);
        }

        private IEnumerator BlinkText() {
            while (true) {
                onBlink.Invoke();
                compText.SetVerticesDirty();
                show = !show;
                yield return new WaitForSeconds(interval);
            }
        }

        public override void ModifyMesh(VertexHelper vh) {
            if (!IsActive()) {
                return;
            }

            List<UIVertex> verts = new List<UIVertex>();
            vh.GetUIVertexStream(verts);

            int l = verts.Count / 6;
            for (int i = 0; i < l; ++i) {

                int idx1 = i * 6 + 0;
                int idx2 = i * 6 + 1;
                int idx3 = i * 6 + 2;
                int idx4 = i * 6 + 3;
                int idx5 = i * 6 + 4;
                int idx6 = i * 6 + 5;

                verts[idx1] = SetUIVertexPos(verts[idx1], show);
                verts[idx2] = SetUIVertexPos(verts[idx2], show);
                verts[idx3] = SetUIVertexPos(verts[idx3], show);
                verts[idx4] = SetUIVertexPos(verts[idx4], show);
                verts[idx5] = SetUIVertexPos(verts[idx5], show);
                verts[idx6] = SetUIVertexPos(verts[idx6], show);
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
        }

        private static UIVertex SetUIVertexPos(UIVertex vert, bool show) {
            vert.position = show ? vert.position : Vector3.zero;
            return vert;
        }
    }
}
