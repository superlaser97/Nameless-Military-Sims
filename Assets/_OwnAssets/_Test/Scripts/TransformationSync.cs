﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TransformationSync : NetworkBehaviour
{
    [SerializeField] private float sendRatePerSecond = 45;
    private float counter = float.MinValue;

    private void FixedUpdate()
    {
        if (isServer)
        {
            if (IsCounterReady(ref counter, 1 / sendRatePerSecond))
            {
                RpcSyncTransform(transform.position, transform.eulerAngles);
            }
        }
    }

    private bool IsCounterReady(ref float timeCounter, float downTime)
    {
        if (Time.time > timeCounter)
        {
            timeCounter = Time.time + downTime;
            return true;
        }

        return false;
    }

    [ClientRpc]
    private void RpcSyncTransform(Vector3 position, Vector3 eulerAngle)
    {
        // Smooth the transform
        transform.position = Vector3.Lerp(transform.position, position, 0.5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(eulerAngle), 0.5f);
    }
}