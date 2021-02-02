using System.Linq;
using LightCheat.Data.Internal;
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

        public Entity[] Entities { get; private set; }

        #endregion

        #region // Constructor

        public GameData(GameProcess gameProcess)
        {
            GameProcess = gameProcess;
            Player = new Player();
            Entities = Enumerable.Range(0, 64).Select(index => new Entity(index)).ToArray();
        }

        public override void Dispose()
        {
            base.Dispose();

            Entities = default;
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
            foreach (var entity in Entities)
            {
                entity.Update(GameProcess);
            }
        }
    }
}