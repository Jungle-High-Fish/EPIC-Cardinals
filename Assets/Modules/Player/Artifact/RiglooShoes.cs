using Cardinals.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiglooShoes : Artifact
{
    public RiglooShoes(int money, string name, ArtifactType type)
    {
        Money = money;
        Name = name;
        Type = type;
    }
}
