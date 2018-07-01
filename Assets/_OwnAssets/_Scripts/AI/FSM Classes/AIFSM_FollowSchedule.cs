﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SealTeam4
{
    public class AIFSM_FollowSchedule : AIFSM_Base
    {
        // Schedules this NPC has
        protected List<NPCSchedule> npcSchedules;

        public void InitializeFSM(
            AIController aiController,
            Transform aiTransform,
            AIState aiState,
            AIStats aiStats,
            AIAnimationController aiAnimController,
            AIAnimEventReciever aiAnimEventReciever,
            List<NPCSchedule> npcSchedules
            )
        {
            this.aiController = aiController;
            this.aiTransform = aiTransform;
            this.aiState = aiState;
            this.aiStats = aiStats;
            this.aiAnimController = aiAnimController;
            this.aiAnimEventReciever = aiAnimEventReciever;
            this.npcSchedules = npcSchedules;
        }

        public void FSM_Update()
        {
            // if all schedule is finished or NPC not active
            if (aiState.general.currSchedule >= npcSchedules.Count)
                return;

            switch (npcSchedules[aiState.general.currSchedule].scheduleType)
            {
                case NPCSchedule.SCHEDULE_TYPE.MOVE_TO_POS:
                    ScheduleProcess_MoveToWaypoint();
                    break;

                case NPCSchedule.SCHEDULE_TYPE.MOVE_TO_POS_WITH_ROT:
                    ScheduleProcess_MoveToWaypoint_And_Rotate();
                    break;

                case NPCSchedule.SCHEDULE_TYPE.IDLE:
                    ScheduleProcess_Idle();
                    break;

                case NPCSchedule.SCHEDULE_TYPE.SIT_IN_AREA:
                    ScheduleProcess_SitDownInArea();
                    break;

                case NPCSchedule.SCHEDULE_TYPE.TALK_TO_OTHER_NPC:
                    ScheduleProcess_TalkToOtherNPCs();
                    break;
            }

        }
        #region Schedule Processes Methods
        /// <summary>
        /// Run delay timer schedule
        /// </summary>
        private void ScheduleProcess_Idle()
        {
            switch (aiState.general.currSubschedule)
            {
                case 0:
                    Idle_Setup();
                    break;
                case 1:
                    Idle();
                    break;
                case 2:
                    Idle_Term();
                    break;
                case 3:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
                default:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
            }
        }

        /// <summary>
        /// Run move to position schedule
        /// </summary>
        private void ScheduleProcess_MoveToWaypoint()
        {
            switch (aiState.general.currSubschedule)
            {
                case 0:
                    LeaveSeat();
                    break;
                case 1:
                    Setup_MoveToWaypoint();
                    break;
                case 2:
                    MoveToWaypoint(aiState.general.currWaypointTarget.position, 0);
                    break;
                case 3:
                    Terminate_MoveToWaypoint();
                    break;
                case 4:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
                default:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
            }
        }

        /// <summary>
        /// Run move to position with rotation schedule
        /// </summary>
        private void ScheduleProcess_MoveToWaypoint_And_Rotate()
        {
            switch (aiState.general.currSubschedule)
            {
                case 0:
                    LeaveSeat();
                    break;
                case 1:
                    Setup_MoveToWaypoint();
                    break;
                case 2:
                    MoveToWaypoint(aiState.general.currWaypointTarget.position, 0);
                    break;
                case 3:
                    RotateToTargetRotation(aiState.general.currWaypointTarget, false);
                    break;
                case 4:
                    Terminate_MoveToWaypoint();
                    break;
                case 5:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
                default:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
            }
        }

        /// <summary>
        /// Sit down on a seat in the area
        /// </summary>
        private void ScheduleProcess_SitDownInArea()
        {
            switch (aiState.general.currSubschedule)
            {
                case 0:
                    LeaveSeat();
                    break;
                case 1:
                    aiController.SitDownInArea_Setup();
                    break;
                case 2:
                    MoveToWaypoint(aiState.general.currSeatTarget.transform.position, 0);
                    break;
                case 3:
                    RotateToTargetRotation(aiState.general.currSeatTarget.transform, false);
                    break;
                case 4:
                    aiController.SitDownOnSeat();
                    break;
                case 5:
                    aiController.SitDownInArea_Term();
                    break;
                case 6:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
                default:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
            }
        }

        /// <summary>
        /// Talking to ther othe NPC
        /// </summary>
        private void ScheduleProcess_TalkToOtherNPCs()
        {
            switch (aiState.general.currSubschedule)
            {
                case 0:
                    LeaveSeat();
                    break;
                case 1:
                    aiController.TalkToOtherNPC_Setup();
                    break;
                case 2:
                    MoveToWaypoint(aiState.general.currConvoNPCTarget.transform.position, aiStats.stopDist_Convo);
                    break;
                case 3:
                    Terminate_MoveToWaypoint();
                    break;
                case 4:
                    aiController.ConvoProcess_TalkToOtherNPC();
                    break;
                case 5:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
                default:
                    aiState.general.currSubschedule = 0;
                    aiState.general.currSchedule++;
                    break;
            }
        }
        #endregion

        public void Setup_MoveToWaypoint()
        {
            aiState.general.currWaypointTarget = aiController.GetTargetMarkerTransform();
            aiController.SetNMAgentDestination(aiState.general.currWaypointTarget.position);
            aiState.general.currSubschedule++;
        }

        public bool MoveToWaypoint(Vector3 waypointPos, float extraStoppingDistance)
        {
            aiController.SetNMAgentDestination(waypointPos);

            if (!aiController.ReachedNMAgentDestination(extraStoppingDistance))
            {
                aiController.MoveAITowardsNMAgentDestination();
                return false;
            }
            else
            {
                aiState.general.currSubschedule++;
                return true;
            }
        }

        public void Terminate_MoveToWaypoint()
        {
            aiController.StopAIMovement();
            aiState.general.currSubschedule++;
        }

        public void LeaveSeat()
        {
            bool leftSeat = aiController.LeaveSeat();

            if(leftSeat)
                aiState.general.seated = false;
                aiState.general.currSubschedule++;
        }

        public void RotateToTargetRotation(Transform targetRotation, bool reversedDirection)
        {
            bool rotateComplete = aiController.RotateTowardsTargetRotation(targetRotation, reversedDirection);

            if (rotateComplete)
            {
                aiState.general.currSubschedule++;
            }
        }

        public void Idle_Setup()
        {
            aiState.general.currTimerValue = float.Parse(npcSchedules[aiState.general.currSchedule].argument);
            aiState.general.currSubschedule++;
        }

        public void Idle()
        {
            if (aiState.general.currTimerValue > 0)
            {
                aiState.general.currTimerValue -= Time.deltaTime;
            }
            else
            {
                aiState.general.currSubschedule++;
            }
        }

        public void Idle_Term()
        {
            aiState.general.currSubschedule++;
        }
    }
}
