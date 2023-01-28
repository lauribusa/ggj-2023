using Godot;
using System;

namespace Game
{
    public partial class DragDropManager : Node3D
    {
        [Signal] public delegate void DragDropEventHandler();

        public override void _Ready()
        {
            DragDrop += OnDragDrop;
        }

        private void OnDragDrop()
        {
            throw new NotImplementedException();
        }
    }
}
