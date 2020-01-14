using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsHolder 
{
    static UnitsHolder _instance;

    List<Unit> _units;

    public static UnitsHolder instance
    {
        get => _instance;
    }

    public static List<Unit> units
    {
        get => instance._units;
    }

    static UnitsHolder()
    {
        _instance = new UnitsHolder();
        _instance._units = new List<Unit>();      
    }

}
