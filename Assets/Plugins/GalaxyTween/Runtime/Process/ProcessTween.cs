/* 
 * https://github.com/error911/GGTween
 * Free license: CC BY Murnik Roman
 */

using System;
using UnityEngine;

namespace GGTeam.GalaxyTween
{
    public abstract class ProcessTween
    {
        protected int m_TweenId = -1;
        public int tweenId
        {
            get { return m_TweenId; }
        }

        protected bool m_Active;
        protected bool m_WaitingForDelay;

        protected Action m_Callback;

        protected float m_StartTime;
        protected float m_DeltaTime;
        protected float m_Duration;
        protected float m_Delay;
        protected bool m_ScaledTime;

        private AnimationCurve[] m_AnimationCurves;
        private Keyframe[][] m_Keyframes;

        protected TweenType m_TweenType;
        protected AnimationCurve m_CustomCurve;

        protected abstract int ValueLength();
        protected abstract void OnUpdateValue();
        protected abstract void OnFinalUpdateValue();
        protected abstract float GetValue(bool isEnd, int valueIndex);

        public void Initialize(float duration, float delay, TweenType tweenType, Action callback, AnimationCurve animationCurve, bool scaledTime, int id)
        {
            m_Duration = duration;
            m_Delay = delay;
            m_TweenType = tweenType;
            m_Callback = callback;
            m_CustomCurve = animationCurve;
            m_ScaledTime = scaledTime;
            m_TweenId = id;

            if (m_Delay > 0)
            {
                m_WaitingForDelay = true;
            }
            else
            {
                m_WaitingForDelay = false;
                StartTween();
            }

            m_Active = true;
        }

        protected virtual void StartTween()
        {
            SetupCurves();

            m_StartTime = m_ScaledTime ? Time.time : Time.unscaledTime;
        }

        public virtual void UpdateTween()
        {
            if (!m_Active) return;

            if (m_WaitingForDelay)
            {
                m_Delay -= m_ScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;

                if (m_Delay <= 0)
                {
                    StartTween();
                    m_WaitingForDelay = false;
                }
            }
            else
            {
                m_DeltaTime = m_ScaledTime ? Time.time : Time.unscaledTime - m_StartTime;

                if (m_DeltaTime < m_Duration)
                {
                    try
                    {
                        OnUpdateValue();
                    }
                    catch (Exception)
                    {
                        StopTween(false);
                    }
                }
                else
                {
                    try
                    {
                        OnFinalUpdateValue();
                    }
                    catch
                    {
                        //  ignored
                    }

                    m_Active = false;
                    StopTween(true);
                }
            }
        }

        public void StopTween(bool callback)
        {
            if (callback)
            {
                m_Callback.InvokeIfNotNull();
            }

            m_TweenId = -1;
            Tween.Release(this);
        }

        private void SetupCurves()
        {
            m_AnimationCurves = new AnimationCurve[ValueLength()];
            m_Keyframes = new Keyframe[ValueLength()][];

            if (m_TweenType == TweenType.Custom)
            {
                for (int i = 0; i < ValueLength(); i++)
                {
                    m_Keyframes[i] = m_CustomCurve.keys;
                }
            }
            else
            {
                GetKeys(BaseTween.GetAnimCurveKeys(m_TweenType));
            }

            for (int i = 0; i < ValueLength(); i++)
            {
                for (int j = 0; j < m_Keyframes[i].Length; j++)
                {
                    m_Keyframes[i][j].value *= GetValue(true, i) - GetValue(false, i);
                    m_Keyframes[i][j].value += GetValue(false, i);
                    m_Keyframes[i][j].time *= m_Duration;
                }

                m_AnimationCurves[i] = new AnimationCurve(m_Keyframes[i]);

                if (m_CustomCurve == null)
                {
                    for (int j = 0; j < m_Keyframes[i].Length; j++)
                    {
                        m_AnimationCurves[i].SmoothTangents(j, 0f);
                    }
                }
            }
        }

        private void GetKeys(float[][] source)
        {
            for (int i = 0; i < ValueLength(); i++)
            {
                m_Keyframes[i] = new Keyframe[source.Length];

                for (int j = 0; j < m_Keyframes[i].Length; j++)
                {
                    m_Keyframes[i][j].time = source[j][0];
                    m_Keyframes[i][j].value = source[j][1];
                }
            }
        }
    }
}