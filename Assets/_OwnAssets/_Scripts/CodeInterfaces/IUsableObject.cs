﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IUsableObject
{
    void UseObject(NetworkInstanceId networkInstanceId);
}