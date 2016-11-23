using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generator {
    class generator {

        static int CalculateNuberOfPossibleEqn(bool add, bool sub, bool mult, bool div) {//Calculate number of chousen operations
            
            // Зачем тут нужна своя функция? А хз

            int num = 0;
            if (add)
                num++;
            if (sub)
                num++;
            if (mult)
                num++;
            if (div)
                num++;
            return num;
        }

        public static int ChooseOperation(bool add, bool sub, bool mult, bool dev) {//Chose operation randomly

            // Зви**ец говнокод конечно, но явно быстрее чем отбрасывать неподходящие
            // и начинать генерацию сначала

            Random rnd = new Random(DateTime.Now.Millisecond + Cursor.Position.X);
            int num = CalculateNuberOfPossibleEqn(add, sub, mult, dev);
            int opr = rnd.Next(0, num), final = 0;
            switch (num) {
                case 4:         // It all four operation selected. (Самое простое)
                    final = opr;
                    break;

                case 3:         // If 3 operations selected
                    if (!dev) {
                        final = opr;
                    } else if (!add) {
                        final = opr + 1;
                    } else if (!sub) {
                        if (opr == 0) {
                            final = opr;
                        } else {
                            final = opr + 1;
                        }
                    } else if (!mult) {
                        if (opr == 2) {
                            final = opr + 1;
                        } else {
                            final = opr;
                        }
                    }
                    break;

                case 2:         // If 2 operations selected
                    if (!dev) {
                        if (!mult) {
                            final = opr;
                        } else if (!sub) {
                            if (opr == 1) {
                                final = 2;
                            } else {
                                final = 0;
                            }
                        } else if (!add) {
                            final = opr + 1;
                        }
                    } else if (!mult) {
                        if (!sub) {
                            if (opr == 1) {
                                final = 3;
                            } else {
                                final = 0;
                            }
                        } else if (!add) {
                            if (opr == 1) {
                                final = 3;
                            } else {
                                final = 1;
                            }
                        }
                    } else if (!sub) {
                        if (!add) {
                            final = opr + 2;
                        }
                    }
                    break;

                case 1:         // If one operation is selected
                    if (add) {
                        final = 0;
                    } else if (sub) {
                        final = 1;
                    } else if (dev) {
                        final = 3;
                    } else {
                        final = 2;
                    }
                    break;
            }

            return final; // Our final operation. Horey
        }
    }
}
