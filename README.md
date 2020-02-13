# GGTween

Free license: CC BY Murnik Roman    
Tested in Unity 2019.2.X +
____

 ## Support types (Поддерживаемые типы)    

- [X] Generic (any type of the following)
- [X] Float
- [X] Int
- [X] Vector2
- [X] Vector3
- [X] Vector4
- [X] Color
- [X] Quaternion


 ## Usage example (Пимер использования)

```C#	
using GGTools.GGTween;
void Sample_1()
{
	var startPos = new Vector3(0,0,0);
	var endPos = new Vector3(3,6,0);
	Tween.TweenVector3((pos) => transform.position = pos, startPos, endPos, 1);
}

```

 ## Animation Interruption Example (Пример прерывания процесса анимации)

```C#  
void Sample_2()
{
	int t = Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 5);
	Tween.EndTween(t);
}
```
