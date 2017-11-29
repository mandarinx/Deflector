using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mandarin {
    public partial class GO {

        static public T FindInParents<T>(GameObject go) where T : Component {
            if (go == null) {
                return null;
            }

            T comp = go.GetComponent<T>();
            if (comp != null) {
                return comp;
            }

            Transform t = go.transform.parent;
            while (t != null && comp == null) {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
            return comp;
        }

        static public T[] FindInChildren<T>(Transform parent) {
            List<T> children = new List<T>();
            Crawl<T>(parent, ref children);
            return children.ToArray();
        }

        static public T[] FindInChildren<T>(Transform parent, Predicate<T> validator) {
            List<T> children = new List<T>();
            Crawl<T>(parent, ref children, validator);
            return children.ToArray();
        }

        static private void Crawl<T>(Transform parent, ref List<T> list, Predicate<T> validator = null) {
            foreach (Transform child in parent) {
                T comp = child.GetComponent<T>();
                if (comp != null) {
                    if (validator == null) {
                        list.Add(comp);
                    } else if (validator(comp)) {
                        list.Add(comp);
                    }
                }
                if (child.childCount > 0) {
                    Crawl<T>(child, ref list);
                }
            }
        }

    }
}