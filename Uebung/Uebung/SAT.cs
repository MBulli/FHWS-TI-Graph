using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uebung6 {
    public class SAT {
        public List<Klausel> Klauseln;
        public List<Literal> Literale;

        public bool IsTrue(Dictionary<Literal, bool> belegung) {
            foreach (Klausel k in Klauseln) {
                if (!k.IsTrue(belegung)) {
                    return false;
                }
            }
            return true;
        }

        public int CountTrueLiterals(Dictionary<Literal, bool> belegung) {
            int count = 0;

            foreach (Klausel k in Klauseln) {
                //count += k.CountTrueLiterals(belegung);
                if (k.IsTrue(belegung)) {
                    count++;
                }
            }
            return count;
        }
    }
}
