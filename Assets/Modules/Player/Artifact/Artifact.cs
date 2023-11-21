using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardinals.Enums;

public class Artifact
{
    private int _money;
    private string _name;
    private ArtifactType _type;

    public int Money { get; set; }
    public string Name { get; set; }
    public ArtifactType Type
    {
        get => _type;
        set
        {
            _type = value;
        }
    }
}
