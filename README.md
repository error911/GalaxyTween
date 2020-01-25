# GGTween

Free license: CC BY Murnik Roman

===== Support types =====
- Generic (any type of the following)
- Float
- Int
- Vector2
- Vector3
- Vector4
- Color
- Quaternion

===== Usage example =====
		
		using GGTools.GGTween;
		void Sample1()
		{
				var startPos = new Vector3(0,0,0);
				var endPos = new Vector3(3,6,0);
				Tween.TweenVector3((pos) => transform.position = pos, startPos, endPos, 1);
		}

===== Animation Interruption Example =====
  
		void Sample2()
		{
				int t = Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 5);
				Tween.EndTween(t);
		}




Свободная лицензия: CC BY Murnik Roman

===== Поддерживаемые типы =====
- Generic (любой тип из перечисленных ниже)
- Float
- Int
- Vector2
- Vector3
- Vector4
- Color
- Quaternion

===== Пример использования =====
	
	using GGTools.GGTween;
		void Sample1()
		{
				var startPos = new Vector3(0,0,0);
				var endPos = new Vector3(3,6,0);
				Tween.TweenVector3((pos) => transform.position = pos, startPos, endPos, 1);
		}

===== Пример прерывания процесса анимации =====
		
		void Sample2()
		{
				int t = Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 5);
				Tween.EndTween(t);
		}
