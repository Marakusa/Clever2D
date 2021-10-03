using Gtk;

namespace Clever2D.Input
{
    /// <summary>
    /// Manages all the input.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// Initializes the InputManager.
        /// </summary>
        public void Initialize()
        {
            //window.KeyPressEvent += MainForm_KeyDown;
            //window.KeyReleaseEvent += MainForm_KeyUp;
        }

        /// <summary>
        /// Player presses a key.
        /// </summary>
        private void MainForm_KeyDown(object sender, KeyPressEventArgs e)
        {
            Input.KeyPressed(e.Event.Key.ToString());
        }
        /// <summary>
        /// Player releases a key.
        /// </summary>
        private void MainForm_KeyUp(object sender, KeyReleaseEventArgs e)
        {
            Input.KeyReleased(e.Event.Key.ToString());
        }
    }
}
