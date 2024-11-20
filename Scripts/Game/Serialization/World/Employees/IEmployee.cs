using Game.DataBase;

namespace Game.Serialization.World
{
    public interface IEmployee
    {
        public int Id { get; }
        public int Salary { get; }
        public int SkillLevel { get; }
        public HumanInfo HumanInfo { get; }
    }
}