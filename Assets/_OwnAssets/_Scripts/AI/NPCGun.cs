﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SealTeam4
{
    public class NPCGun : MonoBehaviour
    {
        [SerializeField] private Transform firingPt;
        [SerializeField] private float firingRate = 0.5f;
        [SerializeField] private GameObject hitEffect_Prefab;
        private float timeToNextShot = 0;

        public void FireGun()
        {
            Ray ray = new Ray(firingPt.position, firingPt.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                IDamageable iDamagable = hitInfo.transform.root.GetComponent<IDamageable>();

                if (iDamagable != null)
                    iDamagable.OnHit(hitInfo.collider);

                //Debug.Log(hitInfo.transform.name + " | " + hitInfo.transform.root.name);

            }
        }
    }
}