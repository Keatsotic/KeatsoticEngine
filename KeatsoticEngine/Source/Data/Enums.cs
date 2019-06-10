﻿public enum ComponentType
{
	Transform,
	SpriteRenderer,
	PlayerController,
	Animation,
	Collision,
	Damage,

	Equipment,
	Shuriken,

	Health,
	EfxGenerator,

	MovementAI,
	SpawnController
}

public enum Input
{
	Left,
	Right,
	Up, 
	Down,
	Attack,
	Jump,
	Special,
	None
}

public enum Direction
{ 
	Left,
	Right,
	None,
	Up,
	Down
}

public enum State
{
	Idle,
	Walk,
	Attack,
	Jump,

	Hurt, 
	Dead,

	//player only states:
	Fall,
	WallAttack,
	Special,
	Duck,
	DuckAttack,
	WallJump
}

public enum PatrolType
{
	WallPatrol,
	PlatformPatrol,
	Follow,
	SlowFollow
}