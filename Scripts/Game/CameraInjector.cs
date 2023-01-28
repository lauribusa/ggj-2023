using Godot;

namespace Game
{
    public partial class CameraInjector : Camera3D
    {
        [Export]
        public Camera3DVariable Camera3DVariable;

        public override void _Ready()
        {
            Camera3DVariable.value = this;
        }
    }
}
