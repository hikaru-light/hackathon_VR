// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using UnityEditor;
using System.Collections;

/// A custom editor for properties on the GvrAudioListener script. This appears in the Inspector
/// window of a GvrAudioListener object.
[CustomEditor(typeof(GvrAudioListener))]
public class GvrAudioListenerEditor : Editor {
  private SerializedProperty globalGainDb = null;
  private SerializedProperty occlusionMask = null;
  private SerializedProperty quality = null;
  private SerializedProperty worldScale = null;

  private GUIContent globalGainLabel = new GUIContent("Global Gain (dB)",
     "Sets the global gain of the system. Can be used to adjust the overall output volume.");
  private GUIContent occlusionMaskLabel = new GUIContent("Occlusion Mask",
     "Sets the global layer mask for occlusion detection.");
  private GUIContent qualityLabel = new GUIContent("Quality",
     "Sets the quality mode in which the spatial audio will be rendered. " +
     "Higher quality modes allow for increased fidelity at the cost of greater CPU usage.");
  private GUIContent worldScaleLabel = new GUIContent("World Scale",
     "Sets the ratio between game units and real world units (meters).");

  void OnEnable () {
    globalGainDb = serializedObject.FindProperty("globalGainDb");
    occlusionMask = serializedObject.FindProperty("occlusionMask");
    quality = serializedObject.FindProperty("quality");
    worldScale = serializedObject.FindProperty("worldScale");
  }

  /// @cond
  public override void OnInspectorGUI () {
    serializedObject.Update();

    // Rendering quality can only be modified through the Inspector in Edit mode.
    GUI.enabled = !EditorApplication.isPlaying;
    EditorGUILayout.PropertyField(quality, qualityLabel);
    GUI.enabled = true;

    EditorGUILayout.Separator();

    EditorGUILayout.Slider(globalGainDb, GvrAudio.minGainDb, GvrAudio.maxGainDb, globalGainLabel);
    EditorGUILayout.Slider(worldScale, GvrAudio.minWorldScale, GvrAudio.maxWorldScale,
                           worldScaleLabel);

    EditorGUILayout.Separator();

    EditorGUILayout.PropertyField(occlusionMask, occlusionMaskLabel);

    serializedObject.ApplyModifiedProperties();
  }
  /// @endcond
}
