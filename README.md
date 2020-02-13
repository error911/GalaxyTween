# GGTween

г========================================¬
¦ Free license: CC BY Murnik Roman       ¦
¦ Свободная лицензия: CC BY Murnik Roman ¦
¦========================================¦
¦ Tested in Unity 2019.2.X +	         ¦
L========================================-


г=====================¬
¦ Support types       ¦
¦ Поддерживаемые типы ¦
L=====================-
- Generic (any type of the following)
- Float
- Int
- Vector2
- Vector3
- Vector4
- Color
- Quaternion

г======================¬
¦ Usage example        ¦
¦ Пример использования ¦
L======================-
		
		using GGTools.GGTween;
		void Sample_1()
		{
				var startPos = new Vector3(0,0,0);
				var endPos = new Vector3(3,6,0);
				Tween.TweenVector3((pos) => transform.position = pos, startPos, endPos, 1);
		}

г=====================================¬
¦ Animation Interruption Example      ¦
¦ Пример прерывания процесса анимации ¦
L=====================================-
  
		void Sample_2()
		{
				int t = Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 5);
				Tween.EndTween(t);
		}