using System;
using UnityEngine;

namespace GGTeam.GalaxyTween
{
    public class ProcessTweenVector3 : ProcessTween
    {
        private Func<Vector3> m_GetStartValue;
        private Func<Vector3> m_GetTargetValue;
        private Action<Vector3> m_UpdateValue;
        private Vector3 m_StartValue;
        private Vector3 m_TargetValue;

        protected override float GetValue(bool isEnd, int valueIndex)
        {
            return (isEnd ? m_GetTargetValue()[valueIndex] : m_GetStartValue()[valueIndex]);
        }

        protected override int ValueLength()
        {
            return 3;
        }

        public void Initialize(Action<Vector3> updateValue, Func<Vector3> startValue, Func<Vector3> targetValue, float duration, float delay, TweenType tweenType, Action callback, AnimationCurve animationCurve, bool scaledTime, int id)
        {
            m_GetStartValue = startValue;
            m_UpdateValue = updateValue;
            m_GetTargetValue = targetValue;

            base.Initialize(duration, delay, tweenType, callback, animationCurve, scaledTime, id);
        }

        protected override void StartTween()
        {
            if (m_UpdateValue == null)
            {
                StopTween(false);
                return;
            }

            base.StartTween();

            try
            {
                m_StartValue = m_GetStartValue();
                m_TargetValue = m_GetTargetValue();
            }
            catch (Exception)
            {
                StopTween(false);
            }
        }

        protected override void OnUpdateValue()
        {
            if (m_UpdateValue == null)
            {
                StopTween(false);
                return;
            }

            Vector3 value = new Vector3
            {
                x = BaseTween.Estimate(m_TweenType, m_StartValue.x, m_TargetValue.x, m_DeltaTime, m_Duration, m_CustomCurve),
                y = BaseTween.Estimate(m_TweenType, m_StartValue.y, m_TargetValue.y, m_DeltaTime, m_Duration, m_CustomCurve),
                z = BaseTween.Estimate(m_TweenType, m_StartValue.z, m_TargetValue.z, m_DeltaTime, m_Duration, m_CustomCurve)
            };
            m_UpdateValue(value);
        }

        protected override void OnFinalUpdateValue()
        {
            if (m_UpdateValue == null)
            {
                return;
            }

            m_UpdateValue(m_TargetValue);
        }
    }
}