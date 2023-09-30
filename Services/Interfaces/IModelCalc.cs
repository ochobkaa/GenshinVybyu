namespace GenshinVybyu.Services.Interfaces
{
    public interface IModelCalc : IService
    {
        public Task<double> GetProbability(int rolls, bool fiftyfifty, int consts, int refines, int primogems=0);
        public Task<int> GetRolls(double prob, bool fiftyfifty, int consts, int refines);
    }
}
