using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Deflector {

    /// <summary>
    /// Attach to a camera. It relies on the OnPostRender method,
    /// which is never called unless the script is attached to a
    /// camera.
    ///
    /// Usage:
    /// Press the record key at runtime to record a set of frames
    /// at the defined framerate. The frames are encoded as png's
    /// and put in a folder. The folder names changes for each
    /// recording, and it uses a unix time stamp for naming, to
    /// make sure there aren't any naming conflicts.
    ///
    /// Check the enableTransparency boolean to replace any color
    /// with a transparent one. Set the color you want to treat as
    /// transparent via the transparentColor field.
    /// </summary>
    public class ScreenRecorder : MonoBehaviour {

        [SerializeField]
        private string      outputPath;
        [SerializeField]
        private int         frameRate;
        [SerializeField]
        private int         maxFrames;
        [SerializeField]
        private bool        enableTransparency;
        [SerializeField]
        private Color32     transparentColor;
        [SerializeField]
        private KeyCode     recordKey;

        private bool        recording;
        private Rect        captureRect;
        private int         vSyncCount;
        private int         recTime;
        private int         fileCount;
        private int         defaultFrameRate;
        private string      recDir;

        private void Start() {
            captureRect = new Rect(0, 0, Screen.width, Screen.height);
        }

        private void Update() {
            if (!Input.GetKeyUp(recordKey)) {
                return;
            }

            recording = !recording;

            if (recording) {
                StartRecording();
            }

            if (!recording) {
                StopRecording();
            }
        }

        private void StartRecording() {
            vSyncCount = QualitySettings.vSyncCount;
            QualitySettings.vSyncCount = 0;
            defaultFrameRate = Application.targetFrameRate;
            Application.targetFrameRate = frameRate;
            Time.captureFramerate = frameRate;

            recTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Debug.Log($"Start recording at {Application.targetFrameRate} fps");

            recDir = $"{outputPath}/{recTime}";
            if (!Directory.Exists(recDir)) {
                Directory.CreateDirectory(recDir);
            }

            fileCount = 0;
        }

        private void StopRecording() {
            Debug.Log($"Stop recording ({fileCount})");
            Application.targetFrameRate = defaultFrameRate;
            Time.captureFramerate = defaultFrameRate;
            QualitySettings.vSyncCount = vSyncCount;
        }

        private IEnumerator OnPostRender() {
            yield return new WaitForEndOfFrame();
            if (recording) {
                Texture2D capture = new Texture2D(Screen.width,
                                                  Screen.height,
                                                  enableTransparency
                                                      ? TextureFormat.ARGB32
                                                      : TextureFormat.RGB24,
                                                  mipmap: false);
                capture.ReadPixels(captureRect, 0, 0);
                capture.Apply();

                if (enableTransparency) {
                    Color32[] raw = capture.GetPixels32();
                    Color[] colors = new Color[raw.Length];
                    for (int i = 0; i < raw.Length; ++i) {
                        if (raw[i].r == transparentColor.r &&
                            raw[i].g == transparentColor.g &&
                            raw[i].b == transparentColor.b) {
                            colors[i] = new Color32(0,0,0,0);
                        } else {
                            colors[i] = raw[i];
                        }
                    }
                    capture.SetPixels(colors);
                    capture.Apply();
                }

                File.WriteAllBytes($"{recDir}/{fileCount:0000}.png", capture.EncodeToPNG());

                ++fileCount;
                if (fileCount == maxFrames) {
                    recording = false;
                    StopRecording();
                }
            }
        }
    }
}
