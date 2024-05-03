using System;
using UnityEngine;

public static class DirectionEnum {
    public enum Direction {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }

    public static Direction ConvertVector2ToDirectionDiagonals(Vector2 vector2) {
        return vector2 switch
        {
            Vector2 v when v.x == 0 && v.y > 0 => Direction.Up,
            Vector2 v when v.x > 0 && v.y > 0 => Direction.UpRight,
            Vector2 v when v.x > 0 && v.y == 0 => Direction.Right,
            Vector2 v when v.x > 0 && v.y < 0 => Direction.DownRight,
            Vector2 v when v.x == 0 && v.y <= 0 => Direction.Down,
            Vector2 v when v.x < 0 && v.y < 0 => Direction.DownLeft,
            Vector2 v when v.x < 0 && v.y == 0 => Direction.Left,
            Vector2 v when v.x < 0 && v.y > 0 => Direction.UpLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(vector2), vector2, null),
        };
    }

    public static Direction ConvertVector2ToDirectionNoDiagonals(Vector2 v) {
        if(Mathf.Abs(v.x) >= MathF.Abs(v.y))
        {
            return v.x >= 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return v.y > 0 ? Direction.Up : Direction.Down;
        }
    }

    public static Vector2 ConvertDirectionToVector2(Direction direction) {
        return direction switch
        {
            Direction.Up => Vector2.up,
            Direction.UpRight => new Vector2(1, 1),
            Direction.Right => Vector2.right,
            Direction.DownRight => new Vector2(1, -1),
            Direction.Down => Vector2.down,
            Direction.DownLeft => new Vector2(-1, -1),
            Direction.Left => Vector2.left,
            Direction.UpLeft => new Vector2(-1, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };
    }
}