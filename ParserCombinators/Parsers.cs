using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCombinators {
    public static class Parsers {

        public static Parser<T, T> Any<T>() {
            return new Parser<T, T>(context => {
                if (context) {
                    return new ParseResult<T, T>(
                        context.Value,
                        context.Next);
                }
                else {
                    throw new ParseException<T>("Unexpected end of input", context, context);
                }
            });
        }

        public static Parser<T, T> Any<T>(Func<T, bool> f) {
            return Any<T>().Where(f);
        }
    }
}
