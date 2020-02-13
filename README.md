# GGTween
Allows you to easily create a transition, animation, lerp or timer.    
Позволяет легко создать переход, анимацию, лерп или таймер.

Free license: CC BY Murnik Roman    
Tested in Unity 2019.2.X +
____

 ## Support types (Поддерживаемые типы)    

- [X] Generic (any type of the following)
- [X] Int
- [X] Float
- [X] Vector2
- [X] Vector3
- [X] Vector4
- [X] Color
- [X] Quaternion


 ## Usage example (Пимер использования)

```C#	
using GGTools;
void Sample_1()
{
	var startPos = new Vector3(0,0,0);
	var endPos = new Vector3(3,6,0);
	Tween.TweenVector3((pos) => transform.position = pos, startPos, endPos, 1);
}
```

 ## Animation Interruption Example (Пример прерывания процесса анимации)

```C#  
using GGTools;
void Sample_2()
{
	int t = Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 5);
	// ...some code
	Tween.StopTween(t);
}
```

## Animation Interruption Example (Задать нелинейность интерполяции)

```C#  
using GGTools;
void Sample_3()
{
	int t = Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 10, 0, null, false, TweenType.Bounce);
}
```
## Animation completion events Example (События при завершении анимации)

```C#  
using GGTools;
void Sample_4()
{
	Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 10, 0, TweenIsEnded, false, TweenType.Bounce);
	void TweenIsEnded()
        {
            Debug.Log("Completed");
        }
}
```

