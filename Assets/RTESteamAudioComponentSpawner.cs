﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SteamAudio;

namespace SealTeam4
{
    public class RTESteamAudioComponentSpawner : MonoBehaviour
    {
        private void Update()
        {
            if(!GameManager.instance.IsInLevelEditMode())
            {
                if(GetComponent<AudioSource>())
                {
                    SteamAudioSource sas = gameObject.AddComponent<SteamAudioSource>();
                    AudioSource audioS = GetComponent<AudioSource>();

                    audioS.spatialize = true;
                    audioS.spatializePostEffects = true;
                    audioS.dopplerLevel = 0;
                    audioS.rolloffMode = AudioRolloffMode.Linear;
                    audioS.spatialBlend = 1;

                    sas.physicsBasedAttenuation = true;
                    sas.directBinaural = true;
                    sas.avoidSilenceDuringInit = true;
                    sas.reflections = true;
                    sas.indirectBinaural = true;
                    sas.occlusionMode = OcclusionMode.OcclusionWithFrequencyDependentTransmission;
                    sas.occlusionMethod = OcclusionMethod.Raycast;
                }
                else
                {
                    gameObject.AddComponent<SteamAudioGeometry>();
                    gameObject.AddComponent<SteamAudioMaterial>();
                }
                Destroy(this);
            }
        }
    }
}

