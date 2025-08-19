using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class BeatChecker : MonoBehaviour
{
    [SerializeField] private EventReference musicEvent;

    [Header("Beat Settings")]
    [SerializeField] private float bpm = 120f;           // beats per minute
    [SerializeField] private int beatsPerBar = 4;        // 4 = standard 4/4
    [SerializeField] private float songLength = 120f;    // in seconds
    [SerializeField] private float startOffset = 0f;     // time before first beat (sec)
    [SerializeField] private float beatWindow = 0.2f;    // tolerance (sec)

    [Header("Manual Extra Beats (sec)")]
    [SerializeField] private List<float> manualBeats = new List<float>();

    [Header("Events")]
    public UnityEvent<string> OnBeatInput;
    public UnityEvent<string> OffBeatInput;

    private EventInstance musicInstance;
    private float musicStartTime;
    private List<float> allBeats = new List<float>();

    private void Start()
    {
        // build beat map
        GenerateBeatMap();
        MergeManualBeats();

        // start FMOD music
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
        musicInstance.release();

        musicStartTime = Time.time;
    }

    private void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame) HandleInput("Up");
        if (Keyboard.current.downArrowKey.wasPressedThisFrame) HandleInput("Down");
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame) HandleInput("Left");
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame) HandleInput("Right");
    }

    private void HandleInput(string inputName)
    {
        if (IsOnBeat(out float closestBeat))
        {
            Debug.Log($"❌ {inputName} OFF-beat (closest: {closestBeat:F2}s)");
            OffBeatInput?.Invoke(inputName);
        }
        else
        {
            Debug.Log($"✅ {inputName} ON-beat (closest: {closestBeat:F2}s)");
            OnBeatInput?.Invoke(inputName);
        }
    }

    private void GenerateBeatMap()
    {
        allBeats.Clear();

        float interval = 60f / bpm; // seconds per beat
        for (float t = startOffset; t <= songLength; t += interval)
        {
            allBeats.Add(t);
        }
    }

    private void MergeManualBeats()
    {
        foreach (var beat in manualBeats)
        {
            if (!allBeats.Contains(beat)) allBeats.Add(beat);
        }

        allBeats.Sort();
    }

    public bool IsOnBeat(out float closestBeat)
    {
        float songTime = Time.time - musicStartTime;
        closestBeat = -1f;

        if (allBeats.Count == 0) return false;

        float bestDiff = float.MaxValue;

        foreach (float beat in allBeats)
        {
            float diff = Mathf.Abs(songTime - beat);
            if (diff < bestDiff)
            {
                bestDiff = diff;
                closestBeat = beat;
            }
        }

        return bestDiff <= beatWindow;
    }
}