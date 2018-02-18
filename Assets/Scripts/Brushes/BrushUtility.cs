#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LunchGame01 {
    public static class BrushUtility {

        public static Transform GetLayer(string layerName) {
            Grid grid = GetRootGrid();
            if (grid == null) {
                return null;
            }

            Transform layer = grid.transform.Find(layerName);
            if (layer != null) {
                return layer;
            }

            GameObject newLayer = new GameObject(layerName);
            #if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(newLayer, $"Create {layerName}");
            #endif
            layer = newLayer.transform;
            layer.SetParent(grid.transform);
            return layer;
        }

        // Finds all objects under the layer,
        // and returns the first object with a component of
        // type T.
        public static T GetObject<T>(GridLayout grid, Transform layer, Vector3Int position) {
            List<GameObject> children = new List<GameObject>();

            for (int i = 0; i < layer.childCount; i++) {
                Transform child = layer.GetChild(i);
                if (grid.WorldToCell(child.position) == position) {
                    children.Add(child.gameObject);
                }
            }

            for (int i = 0; i < children.Count; ++i) {
                T obj = children[i].GetComponent<T>();
                if (obj != null) {
                    return obj;
                }
            }

            return default(T);
        }

        public static T Instantiate<T>(GameObject prefab, Vector3 position, Transform layer) where T : Component {
            Assert.IsNotNull(prefab, $"Prefab {prefab.name} is null.");
            Assert.IsNotNull(prefab.GetComponent<T>(),
                             $"Prefab {prefab.name} doesn't contain component {typeof(T)}");

            #if UNITY_EDITOR
            GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            go.transform.SetPositionAndRotation(position, Quaternion.identity);
            go.transform.SetParent(layer);
            Undo.RegisterCreatedObjectUndo(go, $"Instantiate {go.name}");
            return go.GetComponent<T>();

            #else
            T obj = Object.Instantiate(prefab).GetComponent<T>();
            obj.transform.SetPositionAndRotation(position, Quaternion.identity);
            obj.transform.SetParent(layer);
            return obj;

            #endif
        }

        public static void Select<T>(T obj) where T : Component {
            #if UNITY_EDITOR
            Selection.activeGameObject = obj.gameObject;
            // If not in editor, record a command object for selecting
            // the object
            #endif
        }

        public static void RegisterUndo(Object go, string label) {
            #if UNITY_EDITOR
            Undo.RecordObject(go, label);
            // if not in editor, record a command object in an undo stack,
            // and manually handle undo
            #endif
        }

        public static void Destroy(GameObject go) {
            #if UNITY_EDITOR
            Undo.DestroyObjectImmediate(go);
            #else
            Object.Destroy(go);
            #endif
        }

        public static Vector3 GetWorldPos(GridLayout grid, Vector3 cellPos) {
            return grid.LocalToWorld(grid.CellToLocalInterpolated(cellPos));
        }

        public static GameObject GetSelection() {
            #if UNITY_EDITOR
            return Selection.activeGameObject;
            #else
            return null;
            #endif
        }

        public static Grid CreateRootGrid(string gridName) {
            return new GameObject(gridName).AddComponent<Grid>();
        }

        public static Grid GetRootGrid() {
            Grid grid = GetSelection()?.GetComponentInParent<Grid>();
            if (grid != null) {
                return grid;
            }

            Grid[] grids = Object.FindObjectsOfType<Grid>();
            Assert.IsTrue(grids.Length == 1,
                          grids.Length == 0
                              ? $"Couldn't find any Grid components in any of the open scenes"
                              : $"There are more than one Grid component in the open scenes. Returning the first found.");

            return grids.Length > 0 ? grids[0] : null;
        }

    }
}
