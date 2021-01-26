using LightCheat.Utils;

namespace LightCheat.Data
{
    // Game data retrieved from process
    public class GameData :
        ThreadedComponent
    {
        #region // Storage

        // <inheritdoc />
        protected override string ThreadName => nameof(GameData);

        // <inheritdoc cref="GameProcess"/>
        private GameProcess GameProcess { get; set; }

        // <inheritdoc cref="Player"/>
        public Player Player { get; set; }

        #endregion

        #region // Constructor

        public GameData(GameProcess gameProcess)
        {
            GameProcess = gameProcess;
            Player = new Player();
        }

        public override void Dispose()
        {
            base.Dispose();

            Player = default;
            GameProcess = default;
        }

        #endregion

        protected override void FrameAction()
        {
            if (!GameProcess.IsValid)
            {
                return;
            }

            Player.Update(GameProcess);
        }
    }
}