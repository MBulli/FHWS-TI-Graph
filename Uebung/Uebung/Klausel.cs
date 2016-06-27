using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uebung6 {
    public class Klausel {
        public Dictionary<Literal, bool> Literale;

        public bool IsTrue(Dictionary<Literal, bool> belegung) {
            foreach (KeyValuePair<Literal, bool> kv in Literale) {
                bool value;
                belegung.TryGetValue(kv.Key, out value);
                if (value && !kv.Value) { //literal ist nicht negiert und belegung ist true => Klausel ist true
                    return true;
                } else if (!value && kv.Value) { //literal ist negiert und belegung ist false => Klausel ist true
                    return true;
                }
            }
            return false;
        }
        public int CountTrueLiterals(Dictionary<Literal, bool> belegung) {
            int count = 0;
            foreach (KeyValuePair<Literal, bool> kv in Literale) {
                bool value;
                belegung.TryGetValue(kv.Key, out value);
                if (!kv.Value && value) { //literal ist nicht negiert und belegung ist true => Klausel ist true
                    count++;
                } else if (kv.Value && !value) { //literal ist negiert und belegung ist false => Klausel ist true
                    count++;
                }
            }
            return count;
        }
    }
}
