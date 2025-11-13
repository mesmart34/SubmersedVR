using System;
using System.Globalization;
using UnityEngine;

namespace SubmersedVR.Common;

public struct TransformOffset
{
    public TransformOffset(Vector3 pos, Vector3 angles)
    {
        Pos = pos;
        Angles = angles;
    }

    public TransformOffset(Transform transform) : this()
    {
        Pos = transform.localPosition;
        Angles = transform.localEulerAngles;
    }

    public Vector3 Pos { get; }
    public Vector3 Angles { get; }
    public override string ToString() => $"TransformOffset(Pos=({Pos.x:f3}, {Pos.y:f3}, {Pos.z:f3}), Angles=({Angles.x:f3}, {Angles.y:f3}, {Angles.z:f3}))";
    internal string SwitchString(string type)
    {
        FormattableString str = $"case {type} _: return new TransformOffset(new Vector3({Pos.x:f3}f, {Pos.y:f3}f, {Pos.z:f3}f), new Vector3({Angles.x:f3}f, {Angles.y:f3}f, {Angles.z:f3}f));";
        return str.ToString(CultureInfo.InvariantCulture);
    }

    public void Apply(Transform tf)
    {
        tf.localPosition = Pos;
        tf.localEulerAngles = Angles;
    }

}