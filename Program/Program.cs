using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program {
    class Program {
        class Szótár<K, T> {
            class KulcsÉrték {
                public K kulcs = default(K);
                public T tartalom = default(T);
                public bool üres = true;
                public bool törölve = false;
            }

            private int count = 0;
            private int méret;
            private Func<K, int> hasítófüggvény;
            private KulcsÉrték[] tábla;

            private Szótár() { }

            public Szótár(int méret, Func<K, int> hasítófüggvény) {
                this.méret = méret;
                this.hasítófüggvény = hasítófüggvény;
                this.tábla = new KulcsÉrték[méret];
                for (int i = 0; i < méret; i++) {
                    tábla[i] = new KulcsÉrték();
                }
            }

            public int Count {
                get => this.count;
            }

            public T this[K k] {
                get {
                    int index = Helye(k);
                    if (this.tábla[index].üres || !k.Equals(this.tábla[index].kulcs)) {
                        throw new Exception("Nincs ilyen kulcs!");
                    }
                    return this.tábla[index].tartalom;
                }
                set {
                    int index = Helye(k);
                    if (k.Equals(this.tábla[index].kulcs)) {
                        this.tábla[index].tartalom = value;
                    }
                    else if (this.count == this.méret) {
                        throw new Exception("Nincs hely a szótárban!");
                    }
                    else {
                        this.tábla[index].kulcs = k;
                        this.tábla[index].tartalom = value;
                        this.tábla[index].üres = false;
                        this.tábla[index].törölve = false;
                        ++this.count;
                    }
                }
            }

            private int Helye(K k) {
                int kezdet = hasítófüggvény(k);
                if (k.Equals(this.tábla[kezdet].kulcs)) {
                    return kezdet;
                }
                int hely = kezdet;
                for (int cím = kezdet + 1; cím != kezdet; cím = (cím + 1) % méret) {
                    if (this.tábla[cím].üres && !this.tábla[hely].üres) {
                        hely = cím;
                    }
                    if (k.Equals(this.tábla[cím].kulcs)) {
                        return cím;
                    }
                }
                return hely;
            }

            public bool ContainsKey(K k) => k.Equals(tábla[Helye(k)].kulcs);

            public bool FindValue(T t, out K k) {
                foreach (KulcsÉrték elem in this.tábla) {
                    if (t.Equals(elem.tartalom)) {
                        k = elem.kulcs;
                        return true;
                    }
                }
                k = default(K);
                return false;
            }

            public void Remove(K k) {
                int index = Helye(k);
                if (!k.Equals(this.tábla[index].kulcs)) {
                    throw new Exception("Nincs ilyen kulcs!");
                }
                this.tábla[index] = new KulcsÉrték();
                this.tábla[index].törölve = true;
                --this.count;
            }

            public K[] Keys() {
                K[] keys = new K[this.count];
                int index = 0;
                for (int i = 0; i < this.méret; i++) {
                    if (!this.tábla[i].üres) {
                        keys[index++] = this.tábla[i].kulcs;
                    }
                }
                return keys;
            }

            public T[] Values() {
                T[] values = new T[this.count];
                int index = 0;
                for (int i = 0; i < this.méret; i++) {
                    if (!this.tábla[i].üres) {
                        values[index++] = this.tábla[i].tartalom;
                    }
                }
                return values;
            }

            public void Kiír() {
                foreach (KulcsÉrték elem in this.tábla) {
                    if (!elem.üres) {
                        Console.WriteLine($"{elem.kulcs.ToString()} : {elem.tartalom.ToString()}");
                    }
                }
            }

            public void Diagnosztika() {
                for (int i = 0; i < méret; i++) {
                    Console.WriteLine($"[{i}] {(tábla[i].üres ? tábla[i].törölve ? "TÖRÖLVE" : "ÜRES" : $"{tábla[i].kulcs.ToString()} : {tábla[i].tartalom.ToString()}")}");
                }
            }
        }

        static void Main(string[] args) {
            Szótár<string, char> szótár = new Szótár<string, char>(5, (string s) => (s[s.Length - 1] - '0') % 5);
            szótár["06301234567"] = 'F';
            szótár["06201234576"] = 'L';
            szótár["06201111111"] = 'L';
            szótár["06701234123"] = 'F';
            szótár.Kiír();
            szótár.Remove("06201234576");
            szótár.Diagnosztika();
        }
    }
}
