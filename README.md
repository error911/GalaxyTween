# GGTween

<<<<<<< Updated upstream
â•”â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•—
â•‘ Free license: CC BY Murnik Roman       â•‘
â•‘ Ð¡Ð²Ð¾Ð±Ð¾Ð´Ð½Ð°Ñ Ð»Ð¸Ñ†ÐµÐ½Ð·Ð¸Ñ: CC BY Murnik Roman â•‘
â• â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•£
â•‘ Tested in Unity 2019.2.X +	         â•‘
â•šâ•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
=======
ã========================================¬
¦ Free license: CC BY Murnik Roman       ¦
¦ Ñâîáîäíàÿ ëèöåíçèÿ: CC BY Murnik Roman ¦
¦========================================¦
¦ Tested in Unity 2019.2.X +	         ¦
L========================================-
>>>>>>> Stashed changes


ã=====================¬
¦ Support types       ¦
¦ Ïîääåðæèâàåìûå òèïû ¦
L=====================-
- Generic (any type of the following)
- Float
- Int
- Vector2
- Vector3
- Vector4
- Color
- Quaternion

ã======================¬
¦ Usage example        ¦
¦ Ïðèìåð èñïîëüçîâàíèÿ ¦
L======================-
		
		using GGTools.GGTween;
		void Sample_1()
		{
				var startPos = new Vector3(0,0,0);
				var endPos = new Vector3(3,6,0);
				Tween.TweenVector3((pos) => transform.position = pos, startPos, endPos, 1);
		}

ã=====================================¬
¦ Animation Interruption Example      ¦
¦ Ïðèìåð ïðåðûâàíèÿ ïðîöåññà àíèìàöèè ¦
L=====================================-
  
		void Sample_2()
		{
				int t = Tween.TweenFloat((x)=>Debug.Log(x), 0.0f, 1.0f, 5);
				Tween.EndTween(t);
		}