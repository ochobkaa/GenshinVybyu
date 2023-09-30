using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using GenshinVybyu.Exceptions;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class ModelCalc : IModelCalc
    {
        private readonly ILoadModel _loader;
        private readonly BotConfiguration _conf;

        public ModelCalc(ILoadModel loader, IOptions<BotConfiguration> conf)
        {
            _loader = loader;
            _conf = conf.Value;
        }

        private int RollsFromPrimogems(int primogems) => primogems / _conf.RollPrimogemsCost;

        private static Polynome GetPolynome(ModelData modelData, int rolls)
        {
            var isInRange = (Polynome pol) => pol.Start <= rolls && pol.End > rolls;

            Polynome polynome = modelData.Polynomes.First(isInRange);
            return polynome;
        }

        private static double CalcPolynome(Polynome polynome, int rolls)
        {
            double val = 0;
            foreach (double coef in polynome.Coef) val = rolls * val + coef;

            return val;
        }

        private static double CalcProb(ModelData modelData, int rolls)
        {
            Polynome polynome = GetPolynome(modelData, rolls);
            double prob = CalcPolynome(polynome, rolls);

            return prob;
        }

        public async Task<double> GetProbability(int rolls, bool fiftyfifty, int consts, int refines, int primogems=0)
        {
            ModelData modelData = await _loader.Load(fiftyfifty, consts, refines);

            int rollsInPrimogems = RollsFromPrimogems(primogems);
            int totalRolls = rolls + rollsInPrimogems;

            double probability = CalcProb(modelData, totalRolls);

            return probability;
        }

        public async Task<int> GetRolls(double prob, bool fiftyfifty, int consts, int refines)
        {
            if (prob > 1 && prob < 0) 
                throw new VybyuBotException("Probability value is greater than 1 or negative");

            ModelData modelData = await _loader.Load(fiftyfifty, consts, refines);

            int start = 0;
            int end = modelData.Polynomes.Last().End;
            while (end - start > 1)
            {
                int pos = (end + start) / 2;
                double posProb = CalcProb(modelData, pos);

                if (posProb < prob) start = pos;

                else end = pos;
            }

            return end;
        }
    }
}
