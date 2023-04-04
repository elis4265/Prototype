using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimal
{
    string name { get; }
    float hunger { get; }
    float health { get; }

    public void GetDemaged(float value);
    public void Die();
    public void MoveTo();
}
