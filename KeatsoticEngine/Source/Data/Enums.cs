public enum ComponentType
{
	Transform,
	SpriteRenderer,
	PlayerController,
	Animation,
	Collision
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
	Special,
	Duck,
	DuckAttack,
	WallJump
}