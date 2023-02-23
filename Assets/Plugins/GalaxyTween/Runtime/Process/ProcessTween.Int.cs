using System;
using UnityEngine;

namespace GGTeam.GalaxyTween
{
    public class ProcessTweenInt : ProcessTween
    {
        private Func<int> m_GetStartValue;
        private Func<int> m_GetTargetValue;
        private Action<int> m_UpdateValue;
        private int m_StartValue;
        private int m_TargetValue;

        protected override float GetValue(bool isEnd, int valueIndex)
        {
            return (isEnd ? m_GetTargetValue() : m_GetStartValue());
        }

        protected override int ValueLength()
        {
            return 1;
        }

        public void Initialize(Action<int> updateValue, Func<int> startValue, Func<int> targetValue, float duration, float delay, TweenType tweenType, Action callback, AnimationCurve animationCurve, bool scaledTime, int id)
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

            m_StartValue = m_GetStartValue();
            m_TargetValue = m_GetTargetValue();
        }

        protected override void OnUpdateValue()
        {
            if (m_UpdateValue == null)
            {
                StopTween(false);
                return;
            }
            m_UpdateValue(
                Mathf.RoundToInt(BaseTween.Estimate(m_TweenType, m_StartValue, m_TargetValue, m_DeltaTime, m_Duration,
                    m_CustomCurve)));
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