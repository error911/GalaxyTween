# GGTween

Free license: CC BY Murnik Roman
Tested in Unity 2019.2.X +

____
 ## Support types (Поддерживаемые типы)    
____
- Generic (any type of the following)
- Float
- Int
- Vector2
- Vector3
- Vector4
- Color
- Quaternion

____  
 ## Usage example (Пимер использования)
____   
	
```using GGTools.GGTween;
void Sample_1()
{
	var startPos = new Vector3(0,0,0);
	var endPos = new Vector3(3,6,0);
	Tween.TweenVector3((pos) => transform.position = pos, startPos, endPos, 1);
}
```
____   
 ## Animation Interruption Example (Пример прерывания процесса анимации)
____  
  
	void Sample_2()
	{
		int t = Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 5);
		Tween.EndTween(t);
	}
