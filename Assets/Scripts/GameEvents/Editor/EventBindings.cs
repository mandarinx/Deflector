using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameEvents {

    public class EventBindings<TListener, TEvent> where TListener : Component
                                                  where TEvent : Object {

        private readonly Object[]    dispatchers;
        private readonly TListener[] listeners;

        public EventBindings(TEvent gameEvent) {
            dispatchers = FindDispatchers<TListener>(gameEvent);
            listeners = FindListeners<TListener>(gameEvent);
        }

        public void OnInspectorGUI() {
            GUI.enabled = !GUI.enabled;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Dispatched by", EditorStyles.boldLabel);
            if (dispatchers.Length == 0) {
                EditorGUILayout.LabelField("none");
            } else {
                foreach (Object dispatcher in dispatchers) {
                    EditorGUILayout.ObjectField(dispatcher.name, dispatcher, typeof(Object), true);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Listeners", EditorStyles.boldLabel);
            if (listeners.Length == 0) {
                EditorGUILayout.LabelField("none");
            } else {
                foreach (TListener listener in listeners) {
                    EditorGUILayout.ObjectField(listener.name, listener, typeof(Object), true);
                }
            }
        }

        private static T[] FindListeners<T>(Object target) where T : Component {
            List<T> listeners = new List<T>();
            T[] allListeners = Object.FindObjectsOfType<T>();

            for (int j = 0; j < allListeners.Length; ++j) {
                T listener = allListeners[j];
                SerializedObject so = new SerializedObject(listener);
                SerializedProperty sp = so.GetIterator();

                while (sp.NextVisible(true)) {
                    if (sp.propertyType != SerializedPropertyType.ObjectReference) {
                        continue;
                    }

                    if (sp.objectReferenceValue != target) {
                        continue;
                    }

                    listeners.Add(listener);
                }
            }

            return listeners.ToArray();
        }

        private static Object[] FindDispatchers<T>(Object target) where T : Component {
            List<Object> dispatchers = new List<Object>();
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();

            for (int j = 0; j < allObjects.Length; ++j) {
                GameObject go = allObjects[j];
                Component[] components = go.GetComponents<Component>();

                for (int i = 0; i < components.Length; ++i) {
                    Component c = components[i];
                    if (c == null) {
                        continue;
                    }

                    if (c.GetType() == typeof(T)) {
                        continue;
                    }

                    SerializedObject so = new SerializedObject(c);
                    SerializedProperty sp = so.GetIterator();

                    while (sp.NextVisible(true)) {
                        if (sp.propertyType != SerializedPropertyType.ObjectReference) {
                            continue;
                        }

                        if (sp.objectReferenceValue != target) {
                            continue;
                        }

                        dispatchers.Add(c.gameObject);
                    }
                }
            }

            return dispatchers.ToArray();
        }
    }
}
