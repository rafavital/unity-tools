//---------------------------------------------------
// <copyright file="TransformRecordObject.cs" company="Rafael Vital Lacerda Alves"
//       Author: Rafael Vital Lacerda Alves
//       Copyright (c) 2021 Rafael Vital Lacerda Alves
// </copyright>
//---------------------------------------------------

using UnityEngine;

public class TransformRecordObject 
{
    public TransformAnimationCurve[] curves;
    public Transform recordedTransform;
    public string relativePath = "";

    public TransformRecordObject (string _path, Transform transform)
    {
        relativePath = _path;
        recordedTransform = transform;

        curves = new TransformAnimationCurve[10];

        curves[0] = new TransformAnimationCurve("localPosition.x");
        curves[1] = new TransformAnimationCurve("localPosition.y");
        curves[2] = new TransformAnimationCurve("localPosition.z");

        curves[3] = new TransformAnimationCurve("localRotation.x");
        curves[4] = new TransformAnimationCurve("localRotation.y");
        curves[5] = new TransformAnimationCurve("localRotation.z");
        curves[6] = new TransformAnimationCurve("localRotation.w");

        curves[7] = new TransformAnimationCurve("localScale.x");
        curves[8] = new TransformAnimationCurve("localScale.y");
        curves[9] = new TransformAnimationCurve("localScale.z");

    }

    public void AddFrame (float time)
    {
        curves[0].AddFrame(time, recordedTransform.localPosition.x);
        curves[1].AddFrame(time, recordedTransform.localPosition.y);
        curves[2].AddFrame(time, recordedTransform.localPosition.z);

        curves[3].AddFrame(time, recordedTransform.localRotation.x);
        curves[4].AddFrame(time, recordedTransform.localRotation.y);
        curves[5].AddFrame(time, recordedTransform.localRotation.z);
        curves[6].AddFrame(time, recordedTransform.localRotation.w);

        curves[7].AddFrame(time, recordedTransform.localScale.x);
        curves[8].AddFrame(time, recordedTransform.localScale.y);
        curves[9].AddFrame(time, recordedTransform.localScale.z);
    }
}

public class TransformAnimationCurve 
{
    public string property = "";
    public AnimationCurve curve;

    public TransformAnimationCurve(string property)
    {
        this.property = property;
        curve = new AnimationCurve ();
    }

    public void AddFrame (float time, float value)
    {
        var key = new Keyframe (time, value, 0.0f, 0.0f);
        curve.AddKey(key);
    }
}