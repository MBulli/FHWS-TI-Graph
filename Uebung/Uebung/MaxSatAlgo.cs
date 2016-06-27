using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uebung6 {
    public class MaxSatAlgo {
        private SAT sat;
        private Random random = new Random();
        public MaxSatAlgo(SAT sat) {
            this.sat = sat;
        }

        public int ComputeProbabilistic() {
            Dictionary<Literal, bool> belegung = new Dictionary<Literal, bool>();
            foreach (var lit in sat.Literale) {
                bool boolValue = random.Next(0,2) == 0 ? false : true;
                belegung.Add(lit, boolValue);
            }
            return sat.CountTrueLiterals(belegung);
        }
        //public int ComputeRandomizedRounded() {

        //}
    }
}
