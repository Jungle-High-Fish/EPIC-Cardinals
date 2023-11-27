using System.Collections;
using System.Collections.Generic;
using Cardinals.UI;
using UnityEngine;

public class DescriptionInstTrInfo : MonoBehaviour, IDescriptionInstTrInfo
{
    [SerializeField] Transform _descriptionInstTr;
    public Transform DescriptionInstTr => _descriptionInstTr;
}
