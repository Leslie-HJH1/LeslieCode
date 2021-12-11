using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarAttribute : Attribute
{
    public float MaxHealth;

    public HealthBarAttribute(float maxHealth)
    {
        this.MaxHealth = maxHealth;
    }
    
}
