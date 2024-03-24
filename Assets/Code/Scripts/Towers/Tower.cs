using System;
using UnityEngine;

[Serializable]
public class Tower {

    public string name;
    public int cost;
    public GameObject prefab;
    public bool isMelee;

    public Tower (string _name, int _cost, GameObject _prefab, bool _isMelee)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
        isMelee = _isMelee;
    }

}


