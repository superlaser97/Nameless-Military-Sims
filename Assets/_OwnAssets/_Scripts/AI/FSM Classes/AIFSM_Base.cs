﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SealTeam4
{
    public class AIFSM_Base
    {
        protected Transform aiTransform;

        // Stores various state of this AI
        protected AIState aiState;
        // Stores various stats of this AI
        protected AIStats aiStats;
        // Schedules this NPC has
        protected List<NPCSchedule> npcSchedules;
        // Parent AI Script
        protected AIController aiController;
        // Animation Controller for the AI
        protected AIAnimationController aiAnimController;

        public void InitializeFSM(
            AIController aiController,
            Transform aiTransform,
            AIState aiState,
            AIStats aiStats,
            AIAnimationController aiAnimController,
            List<NPCSchedule> npcSchedules
            )
        {
            this.aiController = aiController;
            this.aiTransform = aiTransform;
            this.aiState = aiState;
            this.aiStats = aiStats;
            this.aiAnimController = aiAnimController;
            this.npcSchedules = npcSchedules;
        }
    }
}