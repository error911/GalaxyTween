/* 
 * https://github.com/error911/GGTween
 * Free license: CC BY Murnik Roman
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGTools   //.GGTween
{
    //[ExecuteInEditMode]
    public class Tween : MonoBehaviour
    {
        #region Доступ

        [Serializable]
        private class TweenQueue<T> where T : ProcessTween, new()
        {
            [SerializeField]
            private Queue<T> m_Tweens = new Queue<T>();
            [SerializeField]
            public Queue<T> tweens
            {
                get { return m_Tweens; }
            }

            [SerializeField]
            public T GetTween()
            {
                if (m_Tweens.Count > 0)
                {
                    return m_Tweens.Dequeue();
                }
                else
                {
                    return new T();
                }
            }
        }

        [SerializeField]
        private static Tween m_Instance;

        private static Tween instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("GGTweenModule").AddComponent<Tween>();
                }
                return m_Instance;
            }
        }

        public int totalTweenCount
        {
            get { return activeTweenCount + dormantTweenCount; }
        }

        public int activeTweenCount
        {
            get { return m_ActiveTweens.Count; }
        }

        public int dormantTweenCount
        {
            get
            {
                int count = 0;
                count += m_TweenIntQueue.tweens.Count;
                count += m_TweenFloatQueue.tweens.Count;
                count += m_TweenVector2Queue.tweens.Count;
                count += m_TweenVector3Queue.tweens.Count;
                count += m_TweenVector4Queue.tweens.Count;
                count += m_TweenColorQueue.tweens.Count;
                count += m_TweenQuaternionQueue.tweens.Count;
                return count;
            }
        }

        private bool m_ReadyToKill;

        public void OnApplicationQuit()
        {
            m_ReadyToKill = true;
        }

        [SerializeField]
        private List<ProcessTween> m_ActiveTweens = new List<ProcessTween>();

        private int m_TweenIdCount = 1;

        private bool m_FirstFrame = true;


#if UNITY_EDITOR
        private void Start()
        {
            if (!Application.isPlaying)
            {
                DestroyImmediate(gameObject);
            }
        }
#endif

        private void Update()
        {
            if (m_FirstFrame)
            {
                m_FirstFrame = false;
                return;
            }

            for (int i = 0; i < m_ActiveTweens.Count; i++)
            {
                m_ActiveTweens[i].UpdateTween();
            }

            if (m_ReadyToKill)
            {
                Destroy(gameObject);
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(gameObject);
            }
#endif
        }

        public static void Release(ProcessTween tween)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            instance.m_ActiveTweens.Remove(tween);

            if (tween.GetType() == typeof(ProcessTweenFloat))
            {
                instance.m_TweenFloatQueue.tweens.Enqueue((ProcessTweenFloat)tween);
            }
        }

        public static bool TweenIsActive(int id)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return false;
#endif
            for (int i = 0; i < instance.m_ActiveTweens.Count; i++)
            {
                if (instance.m_ActiveTweens[i].tweenId == id)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Остановить твин
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callCallback"></param>
        public static void StopTween(int id, bool callCallback = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            for (int i = 0; i < instance.m_ActiveTweens.Count; i++)
            {
                ProcessTween tween = instance.m_ActiveTweens[i];

                if (tween.tweenId == id)
                {
                    tween.StopTween(callCallback);
                }
            }
        }

        #endregion

        #region ===== Тип Generic =====

        public static int TweenValue<T>(Action<T> updateValue, T startValue, T targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenValue<T>(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime, tweenType);
        }

        public static int TweenValue<T>(Action<T> updateValue, Func<T> startValue, T targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenValue<T>(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime, tweenType);
        }

        public static int TweenValue<T>(Action<T> updateValue, Func<T> startValue, Func<T> targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            if (typeof(T) == typeof(float))
            {
                return TweenFloat(updateValue as Action<float>, startValue as Func<float>, targetValue as Func<float>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(int))
            {
                return TweenInt(updateValue as Action<int>, startValue as Func<int>, targetValue as Func<int>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(Vector2))
            {
                return TweenVector2(updateValue as Action<Vector2>, startValue as Func<Vector2>, targetValue as Func<Vector2>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return TweenVector3(updateValue as Action<Vector3>, startValue as Func<Vector3>, targetValue as Func<Vector3>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(Vector4))
            {
                return TweenVector4(updateValue as Action<Vector4>, startValue as Func<Vector4>, targetValue as Func<Vector4>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(Color))
            {
                return TweenColor(updateValue as Action<Color>, startValue as Func<Color>, targetValue as Func<Color>, duration, delay, callback, scaledTime, tweenType);
            }
            else
            {
                Debug.LogWarning("Этот тип значения не поддерживается модулем");
                return 0;
            }
        }

        public static int TweenTweenValueCustom<T>(Action<T> updateValue, T startValue, T targetValue, float duration, AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenValueCustom<T>(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenValueCustom<T>(Action<T> updateValue, Func<T> startValue, T targetValue, float duration, AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenValueCustom<T>(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenValueCustom<T>(Action<T> updateValue, Func<T> startValue, Func<T> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
            if (typeof(T) == typeof(float))
            {
                return TweenFloatCustom(updateValue as Action<float>, startValue as Func<float>, targetValue as Func<float>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(int))
            {
                return TweenIntCustom(updateValue as Action<int>, startValue as Func<int>, targetValue as Func<int>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Vector2))
            {
                return TweenVector2Custom(updateValue as Action<Vector2>, startValue as Func<Vector2>, targetValue as Func<Vector2>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return TweenVector3Custom(updateValue as Action<Vector3>, startValue as Func<Vector3>, targetValue as Func<Vector3>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Vector4))
            {
                return TweenVector4Custom(updateValue as Action<Vector4>, startValue as Func<Vector4>, targetValue as Func<Vector4>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Color))
            {
                return TweenColorCustom(updateValue as Action<Color>, startValue as Func<Color>, targetValue as Func<Color>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                return TweenQuaternionCustom(updateValue as Action<Quaternion>, startValue as Func<Quaternion>, targetValue as Func<Quaternion>, duration, animationCurve, delay, callback, scaledTime);
            }
            else
            {
                Debug.LogWarning("Этот тип значения не поддерживается модулем");
                return 0;
            }
        }

        #endregion

        #region ===== Тип Float =====

        [SerializeField]
        private TweenQueue<ProcessTweenFloat> m_TweenFloatQueue = new TweenQueue<ProcessTweenFloat>();

        public static int TweenFloat(Action<float> updateValue, float startValue, float targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenFloat(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime, tweenType);
        }

        public static int TweenFloat(Action<float> updateValue, Func<float> startValue, float targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenFloat(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime, tweenType);
        }

        public static int TweenFloat(Action<float> updateValue, Func<float> startValue, Func<float> targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenFloat tween = instance.m_TweenFloatQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenFloatCustom(Action<float> updateValue, float startValue, float targetValue, float duration, AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenFloatCustom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenFloatCustom(Action<float> updateValue, Func<float> startValue, float targetValue, float duration, AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenFloatCustom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenFloatCustom(Action<float> updateValue, Func<float> startValue, Func<float> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenFloat tween = instance.m_TweenFloatQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region ===== Тип Int =====

        [SerializeField]
        private TweenQueue<ProcessTweenInt> m_TweenIntQueue = new TweenQueue<ProcessTweenInt>();

        public static int TweenInt(Action<int> updateValue, int startValue, int targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenInt(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenInt(Action<int> updateValue, Func<int> startValue, int targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenInt(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenInt(Action<int> updateValue, Func<int> startValue, Func<int> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenInt tween = instance.m_TweenIntQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenIntCustom(Action<int> updateValue, int startValue, int targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenIntCustom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenIntCustom(Action<int> updateValue, Func<int> startValue, int targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenIntCustom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenIntCustom(Action<int> updateValue, Func<int> startValue, Func<int> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenInt tween = instance.m_TweenIntQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region ===== Тип Vector2 =====

        [SerializeField]
        private TweenQueue<ProcessTweenVector2> m_TweenVector2Queue = new TweenQueue<ProcessTweenVector2>();

        public static int TweenVector2(Action<Vector2> updateValue, Vector2 startValue, Vector2 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenVector2(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector2(Action<Vector2> updateValue, Func<Vector2> startValue, Vector2 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenVector2(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector2(Action<Vector2> updateValue, Func<Vector2> startValue, Func<Vector2> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenVector2 tween = instance.m_TweenVector2Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenVector2Custom(Action<Vector2> updateValue, Vector2 startValue, Vector2 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector2Custom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector2Custom(Action<Vector2> updateValue, Func<Vector2> startValue, Vector2 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector2Custom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector2Custom(Action<Vector2> updateValue, Func<Vector2> startValue, Func<Vector2> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenVector2 tween = instance.m_TweenVector2Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region ===== Тип Vector3 =====

        [SerializeField]
        private TweenQueue<ProcessTweenVector3> m_TweenVector3Queue = new TweenQueue<ProcessTweenVector3>();

        public static int TweenVector3(Action<Vector3> updateValue, Vector3 startValue, Vector3 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenVector3(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector3(Action<Vector3> updateValue, Func<Vector3> startValue, Vector3 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenVector3(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector3(Action<Vector3> updateValue, Func<Vector3> startValue, Func<Vector3> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenVector3 tween = instance.m_TweenVector3Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenVector3Custom(Action<Vector3> updateValue, Vector3 startValue, Vector3 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector3Custom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector3Custom(Action<Vector3> updateValue, Func<Vector3> startValue, Vector3 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector3Custom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector3Custom(Action<Vector3> updateValue, Func<Vector3> startValue, Func<Vector3> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenVector3 tween = instance.m_TweenVector3Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region ===== Тип Vector4 =====

        [SerializeField]
        private TweenQueue<ProcessTweenVector4> m_TweenVector4Queue = new TweenQueue<ProcessTweenVector4>();

        public static int TweenVector4(Action<Vector4> updateValue, Vector4 startValue, Vector4 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenVector4(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector4(Action<Vector4> updateValue, Func<Vector4> startValue, Vector4 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenVector4(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector4(Action<Vector4> updateValue, Func<Vector4> startValue, Func<Vector4> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenVector4 tween = instance.m_TweenVector4Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenVector4Custom(Action<Vector4> updateValue, Vector4 startValue, Vector4 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector4Custom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector4Custom(Action<Vector4> updateValue, Func<Vector4> startValue, Vector4 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector4Custom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector4Custom(Action<Vector4> updateValue, Func<Vector4> startValue, Func<Vector4> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenVector4 tween = instance.m_TweenVector4Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region ===== Тип Color =====

        [SerializeField]
        private TweenQueue<ProcessTweenColor> m_TweenColorQueue = new TweenQueue<ProcessTweenColor>();

        public static int TweenColor(Action<Color> updateValue, Color startValue, Color targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenColor(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenColor(Action<Color> updateValue, Func<Color> startValue, Color targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenColor(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenColor(Action<Color> updateValue, Func<Color> startValue, Func<Color> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenColor tween = instance.m_TweenColorQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenColorCustom(Action<Color> updateValue, Color startValue, Color targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenColorCustom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenColorCustom(Action<Color> updateValue, Func<Color> startValue, Color targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenColorCustom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenColorCustom(Action<Color> updateValue, Func<Color> startValue, Func<Color> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenColor tween = instance.m_TweenColorQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region ===== Тип Quaternion =====

        [SerializeField]
        private TweenQueue<ProcessTweenQuaternion> m_TweenQuaternionQueue = new TweenQueue<ProcessTweenQuaternion>();

        public static int TweenQuaternion(Action<Quaternion> updateValue, Quaternion startValue, Quaternion targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenQuaternion(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenQuaternion(Action<Quaternion> updateValue, Func<Quaternion> startValue, Quaternion targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
            return TweenQuaternion(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenQuaternion(Action<Quaternion> updateValue, Func<Quaternion> startValue, Func<Quaternion> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, TweenType tweenType = TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenQuaternion tween = instance.m_TweenQuaternionQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenQuaternionCustom(Action<Quaternion> updateValue, Quaternion startValue, Quaternion targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenQuaternionCustom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenQuaternionCustom(Action<Quaternion> updateValue, Func<Quaternion> startValue, Quaternion targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenQuaternionCustom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenQuaternionCustom(Action<Quaternion> updateValue, Func<Quaternion> startValue, Func<Quaternion> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            ProcessTweenQuaternion tween = instance.m_TweenQuaternionQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

    }

    public static class ActionExtension
    {
        public static void InvokeIfNotNull(this Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }

        public static void InvokeIfNotNull<T>(this Action<T> action, T parameter)
        {
            if (action != null)
            {
                action.Invoke(parameter);
            }
        }
    }

}