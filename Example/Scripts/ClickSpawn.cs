﻿using Clever2D.Engine;
using Clever2D.Input;
using Clever2D.Threading;
using System;

namespace Example
{
    public class ClickSpawn : CleverScript
    {
        public override void Start()
        {
            Console.WriteLine("Start");
        }
        public override void FixedUpdate()
        {
            Vector2 move = Vector2.zero;

            if (Input.GetKey("W"))
            {
                move += Vector2.up;
            }
            if (Input.GetKey("A"))
            {
                move += Vector2.left;
            }
            if (Input.GetKey("S"))
            {
                move += Vector2.down;
            }
            if (Input.GetKey("D"))
            {
                move += Vector2.right;
            }

            transform.position += move.Normalized;

            if (move != Vector2.zero)
            {
                SceneManager.Instance.LoadedScene.Draw();
            }
        }
    }
}
