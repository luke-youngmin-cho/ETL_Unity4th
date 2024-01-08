using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Idle,
    StartLoading,
    OnLoading,
    EndLoading,
}

[System.Serializable]
public class Sample : MonoBehaviour
{
    public int Num { get; set; }
    private int _num;

    public States state { get; set; }
    private States _state;

    public bool dataVisualization { get; set; }
    private bool _dataVisualization;

    public Data data { get; set; }
    private Data _data;

    [Serializable]
    public struct Data
    {
        public string key;
        public string value;
    }
}
