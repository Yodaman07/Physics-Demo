using System;
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
                Dictionary<float, Directions> distances = new Dictionary<float,Directions>();
                for (int i = 0; i < 4; i++) //Loop to grab all distances
                {
                    KeyValuePair<Directions, Vector3> currentElement = Values.ElementAt(i);
                    distances.Add(Vector2.Distance(collisionPos, Values.ElementAt(i).Value),currentElement.Key);
                }
                
                float minValue = distances.Keys.Min();
                // print(distances[minValue]);
                if (distances[minValue] == enterDirection)
                {
                    return true;
                }
                
                return false;
            }
            index++;
        }

        return false;
    }

    public static Vector3 GetCorrectVelocity(Directions exitDirection, Directions enterDirection, Vector3 currentVelocity)
    {
        Vector3 directionVelocityCorrected = Values.Values.ElementAt((int)exitDirection);
        for (int i = 0; i <= 2; i++)
        {
            if (directionVelocityCorrected[i] == 0.0f)
            {
                directionVelocityCorrected[i] = 1;
            }
        } //Corrects the exit pos
        
        Vector3 newVelocity = CorrectVelocity(Vector3.Scale(currentVelocity, directionVelocityCorrected)/2, enterDirection, exitDirection);

        // print(directionVelocityCorrected);
        // print(currentVelocity);
        // print(Vector3.Scale(currentVelocity, directionVelocityCorrected)/2);
        // print(newVelocity);
        return newVelocity;
    }

    private static Vector3 CorrectVelocity(Vector3 scale,Directions enterDirection, Directions exitDirection) //This method corrects the velocities from the Scale above
    {
        //right has all positives
        //left is negatives
        //up is positive flipped
        //down is negative flipped

        if (exitDirection is Directions.Down or Directions.Up) //This code flips the directions
        {
            float currentX = scale.x;
            float currentY = scale.y;
            scale.x = currentY;
            scale.y = currentX;
        }
        
        if (enterDirection is Directions.Down or Directions.Up)
        {
            float currentX = scale.x;
            float currentY = scale.y;
            scale.x = currentY;
            scale.y = currentX;
        }
        
        if (exitDirection is Directions.Right or Directions.Up) //Up & right is all positive while down and left is all negative.
        {
            for (int i = 0; i < 2; i++)
            {
                scale[i] = Math.Abs(scale[i]);    
            }
        }

        if (exitDirection is Directions.Left or Directions.Down)
        {
            for (int i = 0; i < 2; i++)
            {
                scale[i] = Math.Abs(scale[i]);
                scale[i] *= -1;
            }
        }

        return scale;
    }

    public static Vector3 GetOutDirection(Directions exitDirection)
    {
        Vector3 offset = OutDirection.Values.ElementAt((int)exitDirection);
        return offset;
    }
    
    
    private static readonly Dictionary<Directions, Vector3> Values = new Dictionary<Directions, Vector3>()
    {
        {Directions.Up, up},
        {Directions.Down, down},
        {Directions.Left, left},
        {Directions.Right, right},
    };

    private static readonly Dictionary<string, Vector3> OutDirection = new Dictionary<string, Vector3>()
    {
        { "Up",  new Vector3(0,1.25f,0)},
        { "Down", new Vector3(0,-1.25f,0)},
        { "Left", new Vector3(-1.25f,0,0) },
        { "Right", new Vector3(1.25f,0,0) },
    };

}

public enum Directions { 
    Up,
    Down,
    Left,
    Right
}
