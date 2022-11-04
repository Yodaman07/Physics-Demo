using System.Collections.Generic;
using System.Linq;
using static UnityEngine.Vector3;
using UnityEngine;

public class GetDirection : MonoBehaviour
{
    public static bool CheckHitDirection(Vector2 collisionPos, Directions enterDirection)
    {
        //print(new Vector2(collisionPos.x, collisionPos.y));
        int index = 0;
        float colX = 0.0f;
        float colY = 0.0f;
        foreach (var direction in Values)
        {
            if (collisionPos.x >= 0.5 || collisionPos.x <= -0.5) //Determines if the collision is valid or not
            {
                colX = Mathf.Sign(collisionPos.x); //Sets to 1 or -1 else keeps it at 0
            }
            
            if (collisionPos.y >= 0.5 || collisionPos.y <= -0.5)
            {
                colY = Mathf.Sign(collisionPos.y);
            }

            if (new Vector3(colX, colY) == direction.Value)
            {
                Directions currentDirection = (Directions)index;
                if (currentDirection == enterDirection)
                {
                    return true;
                }
            }
            else if(colX != 0 && colY != 0)
            {
                //X and Y are either 1 or -1
                //True using fix
                return true;
            }
            index++;
        }

        return false;
    }

    public static Vector3 GetCorrectVelocity(Directions exitDirection, Directions enterDirection, Vector3 currentVelocity)
    {
        Vector3 exitDirectionPos = Values.Values.ElementAt((int)exitDirection);
        for (int i = 0; i <= 2; i++)
        {
            if (exitDirectionPos[i] == 0.0f)
            {
                exitDirectionPos[i] = 1;
            }
        } //Corrects the exit pos
        
        if (exitDirection is Directions.Down or Directions.Up || enterDirection is Directions.Up or Directions.Down)
        {
            float currentX = currentVelocity.x;
            float currentY = currentVelocity.y;
            currentVelocity.x = currentY;
            currentVelocity.y = currentX;
            //Reassigns the exit pos values
        }
        
        if (exitDirection is Directions.Down or Directions.Up && enterDirection is Directions.Up or Directions.Down)
        {
            float currentX = currentVelocity.x;
            float currentY = currentVelocity.y;
            currentVelocity.x = currentY;
            currentVelocity.y = currentX;
        }
        
        print(currentVelocity);
        print(exitDirectionPos);
        print(Vector3.Scale(currentVelocity, exitDirectionPos));
        return Vector3.Scale(currentVelocity, exitDirectionPos)/2;
    }
    
    private static readonly Dictionary<string, Vector3> Values = new Dictionary<string, Vector3>()
    {
        {"Up", up},
        {"Down", down},
        {"Left", left},
        {"Right", right},
    };
}

public enum Directions { 
    Up,
    Down,
    Left,
    Right
}
