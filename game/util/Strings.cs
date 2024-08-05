using System;

using Godot;

namespace Game;

public static class Actions
{
    public static readonly StringName UP = "up";
    public static readonly StringName DOWN = "down";
    public static readonly StringName LEFT = "left";
    public static readonly StringName RIGHT = "right";
    public static readonly StringName ATTACK = "attack";
    public static readonly StringName JUMP = "jump";
}

public static class Groups
{
    public static readonly StringName PLAYERS = "players";
    public static readonly StringName ENEMIES = "enemies";
}

enum CollisionLayerIndex
{
    None = 0,
    Ground = 1,
    Player = 2,
    Enemies = 3,
    Attacks = 4,
    Sight = 5,
    Collectables = 6,
}

[Flags]
enum CollisionLayers
{
    None = 0,
    Ground = 1 << 0,
    Player = 1 << 1,
    Enemies = 1 << 2,
    Attacks = 1 << 3,
    Sight = 1 << 4,
    Collectables = 1 << 5,
}