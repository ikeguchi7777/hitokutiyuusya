namespace pointcache.Minimap {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MinimapObject : MonoBehaviour {


        [SerializeField]
        private Config m_config = new Config();

        [SerializeField]
        Sprite[] playerSprites;

        public Config config
        {
            get { return m_config; }
        }

        [System.Serializable]
        public class Config {

            public Sprite icon;
            public Color color = Color.white;

        }

        protected virtual void OnEnable() {
            RegisterIcon();
        }

        protected virtual void OnDisable() {
            foreach (var system in MinimapSystem.MinimapSystems)
            {
                system.UnRegisterMMObject(this);
            }
        }

        public void SetIcon(int id)
        {
            m_config.icon = playerSprites[id];
            RegisterIcon();
        }

        void RegisterIcon()
        {
            if (config.icon == null)
                return;
            foreach (var system in MinimapSystem.MinimapSystems)
            {
                system.RegisterMMObject(this);
            }
        }
    }

}