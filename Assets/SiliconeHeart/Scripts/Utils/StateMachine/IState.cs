namespace Utils.StateMachine
{
    public interface IState
    {
        public int ID { get; }
        public void Enter();
        public void Update();
        public void FixedUpdate();
        public void Exit();

    }
}
