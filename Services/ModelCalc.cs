using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using GenshinVybyu.Exceptions;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class ModelCalc : IModelCalc
    {
        private readonly IModelsCollection _models;
        private readonly BotConfiguration _conf;

        public ModelCalc(IModelsCollection models, IOptions<BotConfiguration> conf)
        {
            _models = models;
            _conf = conf.Value;
        }

        private int RollsFromPrimogems(int primogems) => primogems / _conf.RollPrimogemsCost;

        private static Polynome? GetPolynome(ModelData modelData, int rolls)
        {
            var isInRange = (Polynome pol) => pol.Start <= rolls && pol.End > rolls;

            Polynome? polynome = modelData.Polynomes.FirstOrDefault(isInRange);
            return polynome;
        }

        private static double CalcPolynome(Polynome polynome, int rolls)
        {
            double val = 0;
            double[] coefs = polynome.Coef;
            int pow = coefs.Length - 1;
            for (int i = pow; i >= 0; i--) 
                val = rolls * val + coefs[i];

            return val;
        }

        private static double CalcProb(ModelData modelData, int rolls)
        {
            Polynome? polynome = GetPolynome(modelData, rolls);

            double prob = 0.0;
            if (polynome is not null)
                prob = CalcPolynome(polynome, rolls);

            else
                prob = 1.0;

            return prob;
        }

        public async Task<double> GetProbability(int rolls, bool fiftyfifty, int consts, int refines, int primogems=0)
        {
            ModelData modelData = await _models.GetModelData(fiftyfifty, consts, refines);

            int rollsInPrimogems = RollsFromPrimogems(primogems);
            int totalRolls = rolls + rollsInPrimogems;

            double probability = CalcProb(modelData, totalRolls);

            return probability;
        }

        public async Task<int> GetRolls(double prob, bool fiftyfifty, int consts, int refines)
        {
            if (prob > 1 || prob < 0) 
                throw new VybyuBotException("Probability value is greater than 1 or negative");

            ModelData modelData = await _models.GetModelData(fiftyfifty, consts, refines);

            int start = 0;
            int end = modelData.Polynomes.Last().End;

            Console.WriteLine($"Start {start}, End {end}");
            while (start < end)
            {
                int pos = start + (end - start) / 2;
                double posProb = CalcProb(modelData, pos);

                if (posProb < prob) 
                    start = pos + 1;

                else 
                    end = pos;
            }

            return end;
        }
    }
}
