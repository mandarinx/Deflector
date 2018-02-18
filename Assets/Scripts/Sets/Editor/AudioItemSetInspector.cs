using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LunchGame01 {

    [CustomEditor(typeof(AudioItemSet))]
	public class AudioEventInspector : Editor {

        private ReorderableList list;

        private void OnEnable() {
            list = new ReorderableList(serializedObject:    serializedObject,
                                       elements:            serializedObject.FindProperty("audioItems"),
                                       draggable:           false,
                                       displayHeader:       true,
                                       displayAddButton:    true,
                                       displayRemoveButton: true) {

                drawHeaderCallback = (rect) => { EditorGUI.LabelField(rect, "Audio Items"); },

                drawElementCallback = (rect, index, active, focused) => {
                    SerializedProperty elm = list.serializedProperty.GetArrayElementAtIndex(index);

                    rect.y += 2f;
                    if (ButtonPlay(new Rect(rect.width * 0.4f, rect.y, 30, EditorGUIUtility.singleLineHeight))) {
                        Play(elm);
                    }
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                                          elm.FindPropertyRelative("clip"));

                    rect.y += EditorGUIUtility.singleLineHeight + 2f;
                    float w = EditorGUI.Slider(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                                               "Weight",
                                               elm.FindPropertyRelative("weight").floatValue,
                                               0f,
                                               1f);
                    elm.FindPropertyRelative("weight").floatValue = w;

                    float weightSum = AudioItemSet.ComputeWeightSum(serializedObject.targetObject as AudioItemSet);
                    serializedObject.FindProperty("weightSum").floatValue = weightSum;

                    rect.y += EditorGUIUtility.singleLineHeight + 2f;
                    Vector2 pr = elm.FindPropertyRelative("pitchRange").vector2Value;
                    EditorGUI.MinMaxSlider(new Rect(rect.x, rect.y, rect.width - 120f, EditorGUIUtility.singleLineHeight),
                                           "Pitch Range",
                                           ref pr.x,
                                           ref pr.y,
                                           -3f,
                                           3f);
                    pr.x = EditorGUI.FloatField(new Rect(rect.width - 90f, rect.y, 50f, EditorGUIUtility.singleLineHeight), pr.x);
                    pr.y = EditorGUI.FloatField(new Rect(rect.width - 30f, rect.y, 50f, EditorGUIUtility.singleLineHeight), pr.y);
                    elm.FindPropertyRelative("pitchRange").vector2Value = pr;
                },

                onAddCallback = rlist => {
                    int index = rlist.serializedProperty.arraySize;
			        rlist.serializedProperty.arraySize++;
			        rlist.index = index;
			        SerializedProperty audioItem = rlist.serializedProperty.GetArrayElementAtIndex(index);
                    audioItem.FindPropertyRelative("clip").objectReferenceValue = null;
                    audioItem.FindPropertyRelative("weight").floatValue = AudioItem.GetDefaultWeight();
                    audioItem.FindPropertyRelative("pitchRange").vector2Value = AudioItem.GetDefaultPitchRange();
                },

                onRemoveCallback = rlist => {
                    ReorderableList.defaultBehaviours.DoRemoveButton(rlist);
                    AudioItemSet.ComputeWeightSum(serializedObject.targetObject as AudioItemSet);
                },

                elementHeight = (EditorGUIUtility.singleLineHeight + 2) * 3f + 2
            };
        }

        public override void OnInspectorGUI() {
			AudioItemSet set = (AudioItemSet)target;
			SerializedProperty items = serializedObject.FindProperty("audioItems");

			if (GUILayout.Button("Play Random Clip")) {
				if (items.arraySize <= 0) {
					Debug.LogError("AudioItemSet has no AudioItems");
					return;
				}
                Play(set.GetRandom());
			}

			if (GUILayout.Button("Play Next Clip")) {
				if (items.arraySize <= 0) {
					Debug.LogError("AudioItemSet has no AudioItems");
					return;
				}
                Play(set.GetNext());
			}

			serializedObject.Update();
            list.DoLayoutList();

            GUI.enabled = false;
            EditorGUILayout.LabelField("Weight Sum", serializedObject.FindProperty("weightSum").floatValue.ToString("F"));
            GUI.enabled = true;

			serializedObject.ApplyModifiedProperties();
		}

        private static bool ButtonPlay(Rect rect) {
            Color c = GUI.color;
            GUI.color = Color.black;
            bool click = GUI.Button(rect, "", new GUIStyle("Foldout"));
            GUI.color = c;
            return click;
        }

        private static void Play(AudioItem audioItem) {
            Play(audioItem.AudioClip, audioItem.PitchRange.x, audioItem.PitchRange.y);
        }

        private static void Play(SerializedProperty audioItem) {
            AudioClip clip = audioItem.FindPropertyRelative("clip").objectReferenceValue as AudioClip;
            Vector2 pitch = audioItem.FindPropertyRelative("pitchRange").vector2Value;
            Play(clip, pitch.x, pitch.y);
        }

        private static void Play(AudioClip clip, float pitchLow, float pitchHigh) {
            GameObject dummy = new GameObject("Audio Dummy") {
                hideFlags    = HideFlags.HideAndDontSave
            };
            AudioSource source = dummy.AddComponent<AudioSource>();
            source.clip = clip;
            source.pitch = Random.Range(pitchLow, pitchHigh);
            source.Play();
        }
    }
}
