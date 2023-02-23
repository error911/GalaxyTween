using System;
using UnityEngine;

namespace GGTeam.GalaxyTween
{
    public class ProcessTweenFloat : ProcessTween
    {
        private Func<float> m_GetStartValue;
        private Func<float> m_GetTargetValue;
        private Action<float> m_UpdateValue;
        private float m_StartValue;
        private float m_TargetValue;

        protected override float GetValue(bool isEnd, int valueIndex)
        {
            return (isEnd ? m_GetTargetValue() : m_GetStartValue());
        }

        protected override int ValueLength()
        {
            return 1;
        }

        public void Initialize(Action<float> updateValue, Func<float> startValue, Func<float> targetValue, float duration, float delay, TweenType tweenType, Action callback, AnimationCurve animationCurve, bool scaledTime, int id)
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

            m_UpdateValue(BaseTween.Estimate(m_TweenType, m_StartValue, m_TargetValue, m_DeltaTime, m_Duration,
                m_CustomCurve));
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