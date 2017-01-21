using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusWaveNode
{
    public int ID { get; set; }
    public int score { get; set; }
    public Vector3 position { get; set; }

    public SinusWaveNode(int id, int score, Vector3 position)
    {
        this.ID = id;
        this.score = score;
        this.position = position;
    }
}