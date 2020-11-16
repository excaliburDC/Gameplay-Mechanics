using System;
using UnityEngine;

namespace TPP
{
    public interface IInput
    {
        Action<Vector3> OnMovementDirectionInput { get; set; }
        Action<float,float> OnMovementInput { get; set; }
    }
}