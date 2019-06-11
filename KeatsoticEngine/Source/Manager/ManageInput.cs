﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KeatsoticEngine.Source.Manager;
using KeatsoticEngine.Source;
using KeatsoticEngine.Source.World.Components;

namespace KeatsoticEngine
{ 
    public static class ManageInput 
    {
		public static bool CanPressButtons { get; set; }
		public static bool GamePaused { get; private set; }
        private static KeyboardState keyboardState = Keyboard.GetState();
        private static KeyboardState lastKeyboardState;

        private static MouseState mouseState;
        private static MouseState lastMouseState;

		//player controls
		public static bool playerLeft;
		public static bool playerRight;
		public static bool playerUp;
		public static bool playerDown;
		public static bool playerJump;
		public static bool playerJumpCancel;
		public static bool playerAttack;
		public static bool playerMenu;
		public static bool playerSpecial;
		public static bool playerStart;
		public static bool playerSelect;


		public static void Update()
        {
			if (CanPressButtons)
			{
				lastKeyboardState = keyboardState;
				keyboardState = Keyboard.GetState();

				lastMouseState = mouseState;
				mouseState = Mouse.GetState();
				PlayerInputsUpdate();

				if (playerStart && PlayerController.Player != null)
				{
					GamePaused = GamePaused == true ? false : true;
				}
			}
        }

        /// <summary>
        /// Checks if key is currently pressed.
        /// </summary>
        public static bool IsKeyDown(Keys input)
        {
            return keyboardState.IsKeyDown(input);
        }

        /// <summary>
        /// Checks if key is currently up.
        /// </summary>
        public static bool IsKeyUp(Keys input)
        {
            return keyboardState.IsKeyUp(input);
        }

        /// <summary>
        /// Checks if key was just pressed.
        /// </summary>
        public static bool KeyPressed(Keys input)
        {
            if (keyboardState.IsKeyDown(input) == true && lastKeyboardState.IsKeyDown(input) == false)
                return true;
            else
                return false;
        }

		/// <summary>
		/// Checks if key was just released.
		/// </summary>
		public static bool KeyReleased(Keys input)
		{
			if (keyboardState.IsKeyUp(input) == true && lastKeyboardState.IsKeyDown(input) == true)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Returns whether or not the left mouse button is being pressed.
		/// </summary>
		public static bool MouseLeftDown()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is being pressed.
        /// </summary>
        public static bool MouseRightDown()
        {
            if (mouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the left mouse button was clicked.
        /// </summary>
        public static bool MouseLeftClicked()
        {
            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the right mouse button was clicked.
        /// </summary>
        public static bool MouseRightClicked()
        {
            if (mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets mouse coordinates adjusted for virtual resolution and camera position.
        /// </summary>
        public static Vector2 MousePositionCamera()
        {
            Vector2 mousePosition = Vector2.Zero;
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;

            return ScreenToWorld(mousePosition);
        }

        /// <summary>
        /// Gets the last mouse coordinates adjusted for virtual resolution and camera position.
        /// </summary>
        public static Vector2 LastMousePositionCamera()
        {
            Vector2 mousePosition = Vector2.Zero;
            mousePosition.X = lastMouseState.X;
            mousePosition.Y = lastMouseState.Y;

            return ScreenToWorld(mousePosition);
        }

        /// <summary>
        /// Takes screen coordinates (2D position like where the mouse is on screen) then converts it to world position (where we clicked at in the world). 
        /// </summary>
        private static Vector2 ScreenToWorld(Vector2 input)
        {
            input.X -= ManageResolution.VirtualViewportX;
            input.Y -= ManageResolution.VirtualViewportY;

            return Vector2.Transform(input, Matrix.Invert(Camera.GetTransformMatrix()));
        }

		private static void PlayerInputsUpdate()
		{
			// check for directional movement
			playerLeft = IsKeyDown(Keys.Left) == true || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed;
			playerRight = IsKeyDown(Keys.Right) == true || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed;

			playerUp = IsKeyDown(Keys.Up) == true || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed;
			playerDown = IsKeyDown(Keys.Down) == true || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed;


			//check for button presses
			playerAttack = KeyPressed(Keys.V) == true || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed;
			playerJump = KeyPressed(Keys.Space) == true || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed;
			playerJumpCancel = IsKeyUp(Keys.Space) == true || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed;

			playerSpecial = KeyPressed(Keys.B) == true || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed;
			playerMenu = KeyPressed(Keys.G) == true || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed;

			playerStart = KeyPressed(Keys.Enter) == true || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed;
			playerSelect = KeyPressed(Keys.Escape) == true || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;

		}
	}
}
