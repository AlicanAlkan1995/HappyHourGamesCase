using System.Collections.Generic;

namespace HappyHourGames.Scripts.Services
{
    public interface IUpdatable
    {
        void Update();
    }

    public interface IFixedUpdatable
    {
        void FixedUpdate();
    }

    public interface ILateUpdatable
    {
        void LateUpdate();
    }

    public interface IUpdateService
    {
        void RegisterUpdatable(IUpdatable updatable);
        void UnregisterUpdatable(IUpdatable updatable);

        void RegisterFixedUpdatable(IFixedUpdatable fixedUpdatable);
        void UnregisterFixedUpdatable(IFixedUpdatable fixedUpdatable);

        void RegisterLateUpdatable(ILateUpdatable lateUpdatable);
        void UnregisterLateUpdatable(ILateUpdatable lateUpdatable);

        void UpdateAll();
        void FixedUpdateAll();
        void LateUpdateAll();
    }

    public class UpdateService : IUpdateService
    {
        private readonly List<IUpdatable> _updatableObjects = new();
        private readonly List<IFixedUpdatable> _fixedUpdatableObjects = new();
        private readonly List<ILateUpdatable> _lateUpdatableObjects = new();

        public void RegisterUpdatable(IUpdatable updatable)
        {
            if (!_updatableObjects.Contains(updatable))
                _updatableObjects.Add(updatable);
        }

        public void UnregisterUpdatable(IUpdatable updatable)
        {
            if (_updatableObjects.Contains(updatable))
                _updatableObjects.Remove(updatable);
        }

        public void RegisterFixedUpdatable(IFixedUpdatable fixedUpdatable)
        {
            if (!_fixedUpdatableObjects.Contains(fixedUpdatable))
                _fixedUpdatableObjects.Add(fixedUpdatable);
        }

        public void UnregisterFixedUpdatable(IFixedUpdatable fixedUpdatable)
        {
            if (_fixedUpdatableObjects.Contains(fixedUpdatable))
                _fixedUpdatableObjects.Remove(fixedUpdatable);
        }

        public void RegisterLateUpdatable(ILateUpdatable lateUpdatable)
        {
            if (!_lateUpdatableObjects.Contains(lateUpdatable))
                _lateUpdatableObjects.Add(lateUpdatable);
        }

        public void UnregisterLateUpdatable(ILateUpdatable lateUpdatable)
        {
            if (_lateUpdatableObjects.Contains(lateUpdatable))
                _lateUpdatableObjects.Remove(lateUpdatable);
        }

        public void UpdateAll()
        {
            foreach (var updatable in _updatableObjects)
                updatable.Update();
        }

        public void FixedUpdateAll()
        {
            foreach (var fixedUpdatable in _fixedUpdatableObjects)
                fixedUpdatable.FixedUpdate();
        }

        public void LateUpdateAll()
        {
            foreach (var lateUpdatable in _lateUpdatableObjects)
                lateUpdatable.LateUpdate();
        }
    }
}