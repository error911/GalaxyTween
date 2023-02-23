using System;
using UnityEngine;

namespace GGTeam.GalaxyTween
{
    public class ProcessTweenColor : ProcessTween
    {
        private Func<Color> m_GetStartValue;
        private Func<Color> m_GetTargetValue;
        private Action<Color> m_UpdateValue;
        private Color m_StartValue;
        private Color m_TargetValue;

        protected override float GetValue(bool isEnd, int valueIndex)
        {
            return (isEnd ? m_GetTargetValue()[valueIndex] : m_GetStartValue()[valueIndex]);
        }

        protected override int ValueLength()
        {
            return 4;
        }

        public void Initialize(Action<Color> updateValue, Func<Color> startValue, Func<Color> targetValue, float duration, float delay, TweenType tweenType, Action callback, AnimationCurve animationCurve, bool scaledTime, int id)
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

            Color value = new Color
            {
                r = BaseTween.Estimate(m_TweenType, m_StartValue.r, m_TargetValue.r, m_DeltaTime, m_Duration, m_CustomCurve),
                g = BaseTween.Estimate(m_TweenType, m_StartValue.g, m_TargetValue.g, m_DeltaTime, m_Duration, m_CustomCurve),
                b = BaseTween.Estimate(m_TweenType, m_StartValue.b, m_TargetValue.b, m_DeltaTime, m_Duration, m_CustomCurve),
                a = BaseTween.Estimate(m_TweenType, m_StartValue.a, m_TargetValue.a, m_DeltaTime, m_Duration, m_CustomCurve)
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