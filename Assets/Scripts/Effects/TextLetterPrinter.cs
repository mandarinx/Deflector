using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Deflector.Effects {
    [AddComponentMenu("Effects/Text Letter Printer")]
    public class TextLetterPrinter : BaseMeshEffect {

        [SerializeField]
        private float       letterDuration;
        [SerializeField]
        private UnityEvent  onPrintDone;

        private int         curLetter;
        private int         textLength;
        private Text        compText;

        protected override void Awake() {
            base.Awake();
            compText = GetComponent<Text>();
        }

        public void PrintText(string text) {
            curLetter = 0;
            compText.text = text;
            textLength = text.Length;
            StartCoroutine(TypeText(letterDuration));
        }

        private IEnumerator TypeText(float delay) {
            while (curLetter < textLength) {
                ++curLetter;
                compText.SetVerticesDirty();
                yield return new WaitForSeconds(delay);
            }
            onPrintDone.Invoke();
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

                verts[idx1] = SetUIVertexPos(verts[idx1], i, curLetter);
                verts[idx2] = SetUIVertexPos(verts[idx2], i, curLetter);
                verts[idx3] = SetUIVertexPos(verts[idx3], i, curLetter);
                verts[idx4] = SetUIVertexPos(verts[idx4], i, curLetter);
                verts[idx5] = SetUIVertexPos(verts[idx5], i, curLetter);
                verts[idx6] = SetUIVertexPos(verts[idx6], i, curLetter);
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
        }

        private static UIVertex SetUIVertexPos(UIVertex vert, int letter, int curLetter) {
            vert.position = letter < curLetter ? vert.position : Vector3.zero;
            return vert;
        }
    }
}
