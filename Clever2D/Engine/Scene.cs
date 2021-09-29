using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    public class Scene
    {
        private int nextId = -2147483648;
        private List<int> availableIds = new List<int>();

        public Dictionary<int, GameObject> gameObjects = new Dictionary<int, GameObject>();

        public void InstantiateGameObject(GameObject gameObject)
        {

        }
    }
}
